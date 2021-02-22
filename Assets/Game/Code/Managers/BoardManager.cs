using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //This Script manages the creation and the destruction of some Gameobjects in the game and the management of the player

    #region Variables

    [Header("Managers")]
    private InputManager inputManager;
    private PlayerManager playerManager;
    private UIManager uiManager;

    [Header("Pools")]
    public GameObject smallAsteroidPrefab;
    public GameObject mediumAsteroidPrefab;
    public GameObject bigAsteroidPrefab;
    public GameObject destructionParticleSystemPrefab;

    private InterfacePool smallAsteroidsPool;
    private InterfacePool mediumAsteroidsPool;
    private InterfacePool bigAsteroidsPool;
    public SimpleParticleSystemPool destructionParticleSystemPool;

    [Header("References")]
    public Sprite[] bigAsteroiSprites;
    public Sprite[] mediumAsteroiSprites;
    public Sprite[] smallAsteroiSprites;

    [Header("Configuration")]
    public float playerSafeAreaRadius;

    [Header("PrivateUse")]
    private LayerMask asteroidsLayer;
    private AsteroidScript[] currentAsteroidScripts= new AsteroidScript[0];
    [HideInInspector]
    public int currentLifes;
    private int currentRound;
    private float widthSpawnArea = 6.5f;
    private float heightSpawnArea = 4f;
    private float startWidth = 7.25f;
    private float startheight = 4.75f;

    [Header("MemoryAllocation")]
    Coroutine currentPlayerDeadCoroutine;
    Coroutine currentPlayerGenerationCoroutine;
    Collider2D detectedAsteroid;
    bool canGeneratePlayer;
    int randoNumber;
    int randomXAngle;
    int randomYAngle;
    float randomAxisPosition;
    Vector2 newSpawnPosition;
    Vector2 newDirection;
    GameObject newAsteroid;
    AsteroidScript newAsteroidScript;
    GameObject newDestructionParticleSystemGameObject;
    ParticleSystem newParticleSystem;
    Quaternion transformedDirection;
    Vector2 newDirection1;
    Vector2 newDirection2;
    #endregion
    
    #region Events

    public delegate void PlayerGeneratedAction();
    public static event PlayerGeneratedAction OnPlayerGenerated;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        asteroidsLayer = LayerMask.GetMask("Asteroids");
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        bigAsteroidsPool = new InterfacePool(bigAsteroidPrefab, null, 5, 2);
        mediumAsteroidsPool = new InterfacePool(mediumAsteroidPrefab, null, 10, 4);
        smallAsteroidsPool = new InterfacePool(smallAsteroidPrefab, null, 20, 8);
        destructionParticleSystemPool = new SimpleParticleSystemPool(destructionParticleSystemPrefab, null, 10, 2);
    }
    #endregion

    #region Event Suscriptions
    private void OnEnable()
    {
        GameManager.OnFirstPhaseStart += CleanCurrentAsteroids;
        GameManager.OnFirstPhaseStart += GenerateDecorationAsteroids;
        InputManager.OnGameStart += RestartValues;
        InputManager.OnGameStart += CleanCurrentAsteroids;
        InputManager.OnGameStart += GeneratePlayer;
        InputManager.OnGameStart += GenerateGameAsteroids;
        SmallAsteroidScript.OnSmallAsteroidDestroyed += SmallAsteroidDestroyed;
        MediumAsteroidScript.OnMediumAsteroidDestroyed += MediumAsteroidDestroyed;
        BigAsteroidScript.OnBigAsteroidDestroyed += BigAsteroidDestroyed;
        PlayerPhysicScript.OnPlayerDead += PlayerDead;
    }

    private void OnDisable()
    {
        GameManager.OnFirstPhaseStart -= CleanCurrentAsteroids;
        GameManager.OnFirstPhaseStart -= GenerateDecorationAsteroids;
        InputManager.OnGameStart -= RestartValues;
        InputManager.OnGameStart -= CleanCurrentAsteroids;
        InputManager.OnGameStart -= GeneratePlayer;
        InputManager.OnGameStart -= GenerateGameAsteroids;
        SmallAsteroidScript.OnSmallAsteroidDestroyed -= SmallAsteroidDestroyed;
        MediumAsteroidScript.OnMediumAsteroidDestroyed -= MediumAsteroidDestroyed;
        BigAsteroidScript.OnBigAsteroidDestroyed -= BigAsteroidDestroyed;
        PlayerPhysicScript.OnPlayerDead -= PlayerDead;

    }
    #endregion

    #region Asteroids Management

    private void GenerateDecorationAsteroids()
    {
        for (int i = 0; i < 4; i++)
        {
            GenerateBigAsteroid();
        }
    }
    private void GenerateGameAsteroids()
    {
        currentRound++;

        for (int i = 0; i < currentRound; i++)
        {
            GenerateBigAsteroid();
        }
    }

    private void CleanCurrentAsteroids()
    {
        for (int i = 0; i < currentAsteroidScripts.Length; i++)
        {
            currentAsteroidScripts[i].CleanAsteroid();
            newAsteroidScript = currentAsteroidScripts[i];
        }
        currentAsteroidScripts = new AsteroidScript[0];
    }
    
    private void AsteroidDestruction(AsteroidScript asteroidScript)
    {
        newAsteroidScript = asteroidScript;
        EraseAsteroidFromReferences();
        GenerateDestructionParticleSystem(newAsteroidScript.transform.position);
    }
    
    private void GenerateNewAsteroidsDirections()
    {
        newDirection1 = Quaternion.AngleAxis(30, Vector3.forward) * newAsteroidScript.direction;
        newDirection2 = Quaternion.AngleAxis(-30, Vector3.forward) * newAsteroidScript.direction;
    }

    private void BigAsteroidDestroyed(AsteroidScript asteroidScript)
    {
        AsteroidDestruction(asteroidScript);
        GenerateNewAsteroidsDirections();
        GenerateMediumAsteroid(asteroidScript.transform.position, newDirection1);
        GenerateMediumAsteroid(asteroidScript.transform.position, newDirection2);

    }
    private void MediumAsteroidDestroyed(AsteroidScript asteroidScript)
    {
        AsteroidDestruction(asteroidScript);
        GenerateNewAsteroidsDirections();
        GenerateSmallAsteroid(asteroidScript.transform.position, newDirection1);
        GenerateSmallAsteroid(asteroidScript.transform.position, newDirection2);
    }
    private void SmallAsteroidDestroyed(AsteroidScript asteroidScript)
    {
        AsteroidDestruction(asteroidScript);
        CheckRoundClear();
    }

    private void GenerateBigAsteroid()
    {
        randoNumber = UnityEngine.Random.Range(0, 4);

        switch (randoNumber)
        {
            //Left
            case 0:
                randomAxisPosition = UnityEngine.Random.Range(-heightSpawnArea, heightSpawnArea);
                newSpawnPosition = new Vector2(-startWidth, randomAxisPosition);
                randomXAngle = UnityEngine.Random.Range(0, 101);
                randomYAngle = UnityEngine.Random.Range(-100, 101);
                newDirection = new Vector2(randomXAngle, randomYAngle).normalized;
                break;
            //Right
            case 1:
                randomAxisPosition = UnityEngine.Random.Range(-heightSpawnArea, heightSpawnArea);
                newSpawnPosition = new Vector2(startWidth, randomAxisPosition);
                randomXAngle = UnityEngine.Random.Range(-100, 1);
                randomYAngle = UnityEngine.Random.Range(-100, 101);
                newDirection = new Vector2(randomXAngle, randomYAngle).normalized;
                break;
            //Up
            case 2:
                randomAxisPosition = UnityEngine.Random.Range(-widthSpawnArea, widthSpawnArea);
                newSpawnPosition = new Vector2(randomAxisPosition, startheight);
                randomXAngle = UnityEngine.Random.Range(-100, 101);
                randomYAngle = UnityEngine.Random.Range(-100, 1);
                newDirection = new Vector2(randomXAngle, randomYAngle).normalized;
                break;
            //Down
            case 3:
                randomAxisPosition = UnityEngine.Random.Range(-widthSpawnArea, widthSpawnArea);
                newSpawnPosition = new Vector2(randomAxisPosition, -startheight);
                randomXAngle = UnityEngine.Random.Range(-100, 101);
                randomYAngle = UnityEngine.Random.Range(0, 101);
                newDirection = new Vector2(randomXAngle, randomYAngle).normalized;
                break;
        }


        newAsteroid = bigAsteroidsPool.GetInstance();
        newAsteroid.transform.position = newSpawnPosition;

        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(newDirection);


        Array.Resize(ref currentAsteroidScripts, currentAsteroidScripts.Length + 1);
        currentAsteroidScripts[currentAsteroidScripts.Length - 1] = newAsteroidScript;

    }
    private void GenerateMediumAsteroid(Vector2 position, Vector2 direction)
    {
        newAsteroid = mediumAsteroidsPool.GetInstance();
        newAsteroid.transform.position = position;

        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(direction);

        Array.Resize(ref currentAsteroidScripts, currentAsteroidScripts.Length + 1);
        currentAsteroidScripts[currentAsteroidScripts.Length - 1] = newAsteroidScript;
    }
    private void GenerateSmallAsteroid(Vector2 position, Vector2 direction)
    {
        newAsteroid = smallAsteroidsPool.GetInstance();
        newAsteroid.transform.position = position;

        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(direction);

        Array.Resize(ref currentAsteroidScripts, currentAsteroidScripts.Length + 1);
        currentAsteroidScripts[currentAsteroidScripts.Length - 1] = newAsteroidScript;
    }

    private void EraseAsteroidFromReferences()
    {
        for (int i = 0; i < currentAsteroidScripts.Length; i++)
        {
            if (currentAsteroidScripts[i] == newAsteroidScript)
            {
                currentAsteroidScripts[i] = null;
                currentAsteroidScripts = currentAsteroidScripts.Where(x => x != null).ToArray();
                return;
            }
        }
    }

    #endregion

    #region Player Management   

    private void PlayerDead(Vector2 position)
    {
        GenerateDestructionParticleSystem(position);

        if (currentPlayerDeadCoroutine != null)
        {
            StopCoroutine(currentPlayerDeadCoroutine);
        }
        currentPlayerDeadCoroutine = StartCoroutine(PlayerDeadCoroutine());
    }
    private IEnumerator PlayerDeadCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        if (currentLifes > 0)
        {
            GeneratePlayer();
        }
        else
        {
            OnGameOver();
        }
    }

    public void GeneratePlayer()
    {
        if (currentPlayerGenerationCoroutine != null)
        {
            StopCoroutine(currentPlayerGenerationCoroutine);
        }
        currentPlayerGenerationCoroutine = StartCoroutine(PlayerGenerationCoroutine());
    }
    private IEnumerator PlayerGenerationCoroutine()
    {
        canGeneratePlayer = false;

        while (canGeneratePlayer == false)
        {
            detectedAsteroid = Physics2D.OverlapCircle(Vector2.zero, playerSafeAreaRadius, asteroidsLayer);

            if (detectedAsteroid == null)
            {
                canGeneratePlayer = true;
            }

            if (!canGeneratePlayer)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        currentLifes--;
        OnPlayerGenerated();

    }

    #endregion

    #region Particle Systems

    private void GenerateDestructionParticleSystem(Vector2 position)
    {
        newDestructionParticleSystemGameObject = destructionParticleSystemPool.GetInstance();
        newDestructionParticleSystemGameObject.transform.position = position;

        newParticleSystem = newDestructionParticleSystemGameObject.GetComponent<ParticleSystem>();
        newParticleSystem.Play();
    }

    #endregion

    #region GameFlows

    public void RestartValues()
    {
        currentLifes = 3;
        currentRound = 0;
    }

    private void CheckRoundClear()
    {
        if (currentAsteroidScripts.Length == 0)
        {
            GenerateGameAsteroids();
        }
    }
   
    #endregion
}
