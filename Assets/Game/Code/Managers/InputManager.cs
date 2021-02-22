using UnityEngine;

public class InputManager : MonoBehaviour
{

    //This class is the entrance point of all the inputs.

    #region Variables

    [Header("Managers")]
    private PlayerManager playerManager;

    [Header("States")]
    [HideInInspector]
    public bool inFirstPhase;
    [HideInInspector]
    public bool isPlaying;

    [Header("KeyBoard Controls")]
    public KeyCode shootKey;
    public KeyCode moveForwardKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    [Header("memory Allocation")]
    float editorHorizontal;

    #endregion

    #region Events

    public delegate void StartAction();
    public static event StartAction OnGameStart;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (inFirstPhase)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
            {
                OnGameStart();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }


        }
        else if (isPlaying)
        {
            //The keyboard and mouse controls work simultanously

            //the rotations check if the keboard is being used, if it isnt, take the mouse xAxis movement and uses it , including if the value is 0
            if (Input.GetKey(leftKey))
            {
                playerManager.RotateAround(-2);
            }
            else if (Input.GetKey(rightKey))
            {
                playerManager.RotateAround(2);
            }
            else
            {
                editorHorizontal = Input.GetAxis("Mouse X");
                playerManager.RotateAround(editorHorizontal * 2);
            }


            if (Input.GetKey(moveForwardKey) || Input.GetKey(KeyCode.Mouse1))
            {
                playerManager.MoveForward();
            }

            //The fire System is working with the KeyDown and KeyUp and a corrutine in the player Script to avoid send the fire input to the player everyFrame
            if (Input.GetKeyDown(shootKey) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerManager.StartFire();
            }
            else if (Input.GetKeyUp(shootKey) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                playerManager.StopFire();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    #endregion

    #region Event Suscriptions

    private void OnEnable()
    {
        BoardManager.OnPlayerGenerated += StartGame;
        GameManager.OnFirstPhaseStart += FirstPhaseStart;
        PlayerPhysicScript.OnPlayerDead += TurnOffInteraction;
    }

    private void OnDisable()
    {
        BoardManager.OnPlayerGenerated -= StartGame;
        GameManager.OnFirstPhaseStart -= FirstPhaseStart;
        PlayerPhysicScript.OnPlayerDead -= TurnOffInteraction;
    }

    #endregion

    #region Event CallBacks

    public void FirstPhaseStart()
    {
        inFirstPhase = true;
    }

    public void StartGame()
    {
        inFirstPhase = false;
        isPlaying = true;
    }

    public void TurnOffInteraction(Vector2 position)
    {
        isPlaying = false;
    }
    #endregion

}
