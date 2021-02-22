using UnityEngine;

public class PlayerPhysicScript : MonoBehaviour
{
    //This is a simple script that put in the same Gameobject that have the player Trigger detects the asteoids and throw the playerDeadEvent

    public delegate void PlayerDeadAction(Vector2 position);
    public static event PlayerDeadAction OnPlayerDead;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            OnPlayerDead(transform.position);
        }
    }
}
