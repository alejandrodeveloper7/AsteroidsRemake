using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //This script controls The start of the application and some principals GameFlows, in this game there is only one Gameflow to control

    #region Variables

    [Header("Memory Allocation")]
    Coroutine currentGameEndedCoroutine;

    #endregion

    #region Events

    public delegate void FirstPhaseStartAction();
    public static event FirstPhaseStartAction OnFirstPhaseStart;

    public delegate void GameInitializationAction();
    public static event GameInitializationAction OnGameInitialization;

    #endregion

    #region EventSuscriptions

    private void OnEnable()
    {
        BoardManager.OnGameOver += GameEnded;
    }

    private void OnDisable()
    {
        BoardManager.OnGameOver -= GameEnded;

    }

    #endregion

    #region EventsCallbacks

    private void GameEnded()
    {
        if (currentGameEndedCoroutine != null)
        {
            StopCoroutine(currentGameEndedCoroutine);
        }
        currentGameEndedCoroutine = StartCoroutine(GameEndedCoroutine());
    }
    private IEnumerator GameEndedCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        OnFirstPhaseStart();
    }

    #endregion

    #region Monobehaviour

    private void Start()
    {
        InitialLogic();
    }

    #endregion

    #region GameFlow

    private void InitialLogic()
    {
        Application.targetFrameRate = 60;
        OnGameInitialization();
        OnFirstPhaseStart();
    }
 
    #endregion
}
