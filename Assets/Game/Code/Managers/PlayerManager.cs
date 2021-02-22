using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //This class manages the player´s ship and applys the inputs of the player

    #region Variablees

    [Header("Managers")]
    private SoundManager soundaManager;

    [Header("Configuration")]
    public float speed;
    public float rotationSpeed;
    public float BetweenShootsTime;

    [Header("References")]
    public GameObject shootPrefab;
    private Transform playerTransform;
    private Animator playerAnimator;
    private Collider2D ownCollider;
    private Rigidbody2D ownRigidBody;
    private Transform shootInstancePoint;

    [Header("Private Use")]
    float ShotTime;
    private InterfacePool shootPool;

    [Header("MemoryAllocation")]
    Coroutine currentFireCoroutine;
    ShootScript newShootScript;
    GameObject newShoot;
    GameObject player;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        soundaManager = GetComponent<SoundManager>();
        player=GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerAnimator = player.GetComponent<Animator>();
        shootInstancePoint = player.transform.GetChild(1);
        shootPool = new InterfacePool(shootPrefab, null, 5, 1);
        ownCollider = playerTransform.GetComponent<Collider2D>();
        ownRigidBody = playerTransform.GetComponent<Rigidbody2D>();
        player.SetActive(false);
    }
    
    #endregion

    #region Event suscriptions

    private void OnEnable()
    {
        BoardManager.OnPlayerGenerated += RestartPlayer;
        PlayerPhysicScript.OnPlayerDead += PlayerDead;
    }

    private void OnDisable()
    {
        BoardManager.OnPlayerGenerated -= RestartPlayer;
        PlayerPhysicScript.OnPlayerDead -= PlayerDead;

    }

    #endregion

    #region Event Callbacks

    public void RestartPlayer()
    {
        playerTransform.position = Vector3.zero;
        ownCollider.enabled = true;
        playerAnimator.Rebind();
        player.SetActive(true);
    }

    public void PlayerDead(Vector2 position)
    {
        playerAnimator.SetTrigger("Dead");
        ownCollider.enabled = false;
        ownRigidBody.velocity = Vector3.zero;
        StopFire();
    }

    #endregion

    #region Input Callbacks

    public void MoveForward()
    {
        ownRigidBody.AddForce(playerTransform.up * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
    public void RotateAround(float value)
    {
        playerTransform.Rotate(0, 0, -value * rotationSpeed * Time.fixedDeltaTime);

    }

    public void StartFire()
    {
        if (currentFireCoroutine != null)
        {
            StopCoroutine(currentFireCoroutine);
        }
        currentFireCoroutine = StartCoroutine(FireCoroutine());
    }
    public void StopFire()
    {
        if (currentFireCoroutine != null)
        {
            StopCoroutine(currentFireCoroutine);
            currentFireCoroutine = null;
        }
    }
    private IEnumerator FireCoroutine()
    {
        for (; ; )
        {
            if (Time.time >= ShotTime)
            {
                GenerateShoot();
                ShotTime = Time.time + BetweenShootsTime;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void GenerateShoot()
    {
        newShoot = shootPool.GetInstance();
        newShoot.transform.position = shootInstancePoint.position;
        newShoot.transform.rotation = shootInstancePoint.rotation;

        newShootScript = newShoot.GetComponent<ShootScript>();
        soundaManager.PlayShootSound();
        newShootScript.InitializeShoot();
    }

    #endregion
}
