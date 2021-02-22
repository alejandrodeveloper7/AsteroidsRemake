using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //this script manages all the UI movement and changes and have the score management and the flow derivated from it, like check the record in the end and check
    // the score to add lifes to the player

    #region Variables
    [Header("Managers")]
    private BoardManager boardManager;
    private DataManager dataManager;

    [Header("References")]
    public TextMeshProUGUI presToStartText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    [Space]
    public TextMeshProUGUI recordText;
    public TextMeshProUGUI recordIndicatorText;
    [Space]
    public Image[] lifesIcons;

    [Header("Private Use")]
    private int lifeIndex;
    private int bonusLifes;
    private int currentScore;

    #endregion
  
    #region Monobehaviour

    private void Awake()
    {
        dataManager = GetComponent<DataManager>();
        boardManager = GetComponent<BoardManager>();
    }

    #endregion

    #region Event Suscriptions

    private void OnEnable()
    {
        GameManager.OnFirstPhaseStart += EnterPressToStartText;
        GameManager.OnFirstPhaseStart += ExitGameOverText;
        GameManager.OnFirstPhaseStart += ExitScoreText;
        GameManager.OnFirstPhaseStart += ExitRecordIndicator;

        InputManager.OnGameStart += ExitPressToStartText;
        InputManager.OnGameStart += RestartLifes;
        InputManager.OnGameStart += RestartScore;
        InputManager.OnGameStart += EnterScoreText;

        BoardManager.OnPlayerGenerated += LifeUsed;

        SmallAsteroidScript.OnSmallAsteroidDestroyed += AddScoreSmallAsteroid;
        MediumAsteroidScript.OnMediumAsteroidDestroyed += AddScoreMediumAsteroid;
        BigAsteroidScript.OnBigAsteroidDestroyed += AddScoreBigAsteroid;

        BoardManager.OnGameOver += EnterGameOverText;
        BoardManager.OnGameOver += SendFinalScore;

        DataManager.OnNewRecord += NewHighScore;
    }
    private void OnDisable()
    {
        GameManager.OnFirstPhaseStart -= EnterPressToStartText;
        GameManager.OnFirstPhaseStart -= ExitGameOverText;
        GameManager.OnFirstPhaseStart -= ExitScoreText;
        GameManager.OnFirstPhaseStart -= ExitRecordIndicator;
        InputManager.OnGameStart -= ExitPressToStartText;
        InputManager.OnGameStart -= RestartLifes;
        InputManager.OnGameStart -= RestartScore;
        InputManager.OnGameStart -= EnterScoreText;
        BoardManager.OnPlayerGenerated -= LifeUsed;
        SmallAsteroidScript.OnSmallAsteroidDestroyed -= AddScoreSmallAsteroid;
        MediumAsteroidScript.OnMediumAsteroidDestroyed -= AddScoreMediumAsteroid;
        BigAsteroidScript.OnBigAsteroidDestroyed -= AddScoreBigAsteroid;
        BoardManager.OnGameOver -= EnterGameOverText;
        BoardManager.OnGameOver -= SendFinalScore;
        DataManager.OnNewRecord -= NewHighScore;

    }

    #endregion

    #region UI Elements Management 

    private void EnterPressToStartText()
    {
        presToStartText.enabled = true;
    }
    private void ExitPressToStartText()
    {
        presToStartText.enabled = false;
    }

    private void ExitRecordIndicator()
    {
        recordIndicatorText.enabled = false;
    }

    private void EnterGameOverText()
    {
        gameOverText.enabled = true;
    }
    private void ExitGameOverText()
    {
        gameOverText.enabled = false;
    }

    private void EnterScoreText()
    {
        scoreText.enabled = true;
    }
    private void ExitScoreText()
    {
        scoreText.enabled = false;
    }

    #endregion

    #region Score Management

    private void RestartScore()
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();

    }

    public void AddScoreSmallAsteroid(AsteroidScript asteroid)
    {
        currentScore += 3;
        scoreText.text = currentScore.ToString();
        BonusLifeCheck();

    }
    public void AddScoreMediumAsteroid(AsteroidScript asteroid)
    {
        currentScore += 2;
        scoreText.text = currentScore.ToString();
        BonusLifeCheck();

    }
    public void AddScoreBigAsteroid(AsteroidScript asteroid)
    {
        currentScore += 1;
        scoreText.text = currentScore.ToString();
        BonusLifeCheck();

    }

    #endregion

    #region Lifes Management

    private void BonusLifeCheck()
    {
        if (currentScore >= 2000 && bonusLifes == 0)
        {
            boardManager.currentLifes++;
            AddLife();
            bonusLifes++;
        }
        else if (currentScore >= 5000 && bonusLifes == 1)
        {
            boardManager.currentLifes++;
            bonusLifes++;

        }
        if (currentScore >= 10000 && bonusLifes == 2)
        {
            boardManager.currentLifes++;
            AddLife();
            bonusLifes++;
        }
    }

    private void RestartLifes()
    {
        lifesIcons[0].enabled = true;
        lifesIcons[1].enabled = true;
        lifesIcons[2].enabled = true;
        lifesIcons[3].enabled = false;
        lifesIcons[4].enabled = false;
        lifeIndex = 2;
        bonusLifes = 0;
    }

    private void AddLife()
    {
        lifeIndex++;
        lifesIcons[lifeIndex].enabled = true;
    }

    public void LifeUsed()
    {
        lifesIcons[lifeIndex].enabled = false;
        lifeIndex--;

    }

    #endregion

    #region Record Management

    private void SendFinalScore()
    {
        dataManager.CheckRecord(currentScore);
    }

    public void UpdateRecordText(int currentRecord)
    {
        recordText.text = currentRecord.ToString();
    }

    private void NewHighScore(int newRecord)
    {
        recordText.text = newRecord.ToString();
        recordIndicatorText.enabled = true;

    }

    #endregion

}