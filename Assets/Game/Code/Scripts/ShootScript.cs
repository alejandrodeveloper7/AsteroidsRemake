using System.Collections;
using UnityEngine;

public class ShootScript : MonoBehaviour, IPooleableItem
{
    #region Variables

    [Header("Configuration")]
    public float shootDuration;
    public float shootSpeed;

    [Header("References")]
    private Collider2D ownCollider;
    private Rigidbody2D ownrigidBody;
    private GameObject display;

    [Header("States")]
    [HideInInspector]
    public bool readyToUse = true;
    public bool ReadyToUse { get => readyToUse; }

    [Header("Private Use")]
    private float deadTime;

    [Header("Memory Allocation")]
    Coroutine currentShootCoroutine;

    #endregion
    
    #region Monobehaviour

    private void Awake()
    {
        ownrigidBody = GetComponent<Rigidbody2D>();
        ownCollider = GetComponent<Collider2D>();
        display = transform.GetChild(0).gameObject;
        gameObject.SetActive(false);
    }

    #endregion

    #region ShootManagement

    public void InitializeShoot()
    {
        readyToUse = false;
        gameObject.SetActive(true);
        if (currentShootCoroutine != null)
        {
            StopCoroutine(currentShootCoroutine);
        }
        currentShootCoroutine = StartCoroutine(ShootCoroutine());
    }
    private IEnumerator ShootCoroutine()
    {
        ownrigidBody.WakeUp();
        ownCollider.enabled = true;
        display.SetActive(true);
        deadTime = Time.time + shootDuration;
        while (Time.time < deadTime)
        {
            transform.position += transform.up * shootSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        ownCollider.enabled = false;
        display.SetActive(false);
        ownrigidBody.Sleep();
        gameObject.SetActive(false);
        readyToUse = true;
    }

    public void ShootHit()
    {
        if (currentShootCoroutine != null)
        {
            StopCoroutine(currentShootCoroutine);
            currentShootCoroutine = null;
        }
        ownCollider.enabled = false;
        ownrigidBody.Sleep();
        display.SetActive(false);
        gameObject.SetActive(false);
        readyToUse = true;
    }

    #endregion
}
