using UnityEngine;

public abstract class AsteroidScript : MonoBehaviour , IPooleableItem
{        
    #region Variables

    [Header("Managers")]
    protected BoardManager boardManager;

    [Header("States")]
    [HideInInspector]
    public bool readyToUse=true;

    [Header("Configuration")]
    public float speed;

    [Header("Private Use")]
    [HideInInspector]
    public Vector2 direction;

    [Header("References")]
    protected Collider2D ownCollider;
    protected SpriteRenderer ownRenderer;
    protected GameObject display;
    protected Rigidbody2D ownRigidBody;

    [Header("MemoryAllocation")]
    ShootScript currentShootScript;
    protected int randomNumber;

    public bool ReadyToUse { get => readyToUse; }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        boardManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardManager>();
        ownRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        ownRigidBody = GetComponent<Rigidbody2D>();
        ownRigidBody.Sleep();
        ownCollider = GetComponent<Collider2D>();
        display = transform.GetChild(0).gameObject;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shoot"))
        {
            currentShootScript = other.GetComponent<ShootScript>();
            currentShootScript.ShootHit();
            AsteroidDestroyed();
        }     
    }

    #endregion

    #region Asteroid Management

    //Method to Delete an Asteroid
    public void CleanAsteroid()
    {
        ownCollider.enabled = false;
        display.SetActive(false);
        ownRigidBody.velocity = Vector2.zero;
        ownRigidBody.Sleep();
        gameObject.SetActive(false);
        readyToUse = true;
    }
    
    public void InitializeAsteroid(Vector2 direction)
    {
        readyToUse = false;
        gameObject.SetActive(true);
        RandomizeSprite();
        this.direction = direction;
        ownCollider.enabled=true;
        display.SetActive(true);
        ownRigidBody.WakeUp();
        ownRigidBody.velocity = direction * speed * Time.fixedDeltaTime;
        randomNumber = Random.Range(-30, 31);
        ownRigidBody.AddTorque(randomNumber);
    }
    //Method to change some components configurations of the asteroids when they are destroyed
    internal void ConfigureDestroyedAsteroid()
    {
        ownCollider.enabled = false;
        display.SetActive(false);
        ownRigidBody.velocity = Vector2.zero;
        ownRigidBody.Sleep();
        gameObject.SetActive(false);
        readyToUse = true;
    }

    #endregion

    #region Abstract Methods

    //Method to delete an Asteroid when is destroyed by a shoot
    public abstract void AsteroidDestroyed();
    //Method to Randomize The display of the Asteroid
    internal abstract void RandomizeSprite();

    #endregion
}