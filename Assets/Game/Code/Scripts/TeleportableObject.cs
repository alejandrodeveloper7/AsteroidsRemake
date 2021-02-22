using UnityEngine;

public class TeleportableObject : MonoBehaviour
{
    // Functionallity script that allows to the Gameobject that have it relocate in the game Area when go out from the viewport

    #region Variables

    [Header("Private Use")]
    private Camera camara;

    [Header("Memory Allocation")]
    Vector2 viewportPosition;
    Vector2 newPosition;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        camara = Camera.main;
    }
    private void Update()
    {
        TryRelocateObject();
    }

    #endregion
    
    #region Functionality

    private void TryRelocateObject()
    {
        //I use not exact values because the viewport position is based in the pivot of the object, increasing by 0.05f this values 
        //the object will move a little more out of the screen before teleport, not when the pivot go out of the screen

        //The positions are multiplied by 0.98 to decrease slightly the value and in the next frame dont detonate the other case

        viewportPosition = camara.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1.05f || viewportPosition.x < -0.05f)
        {
            newPosition = transform.position;
            newPosition.x *= -0.98f;
            transform.position = newPosition;
        }
        else if (viewportPosition.y > 1.05f || viewportPosition.y < -0.05f)
        {
            newPosition = transform.position;
            newPosition.y *= -0.98f;
            transform.position = newPosition;
        }
    }

    #endregion
     
}
