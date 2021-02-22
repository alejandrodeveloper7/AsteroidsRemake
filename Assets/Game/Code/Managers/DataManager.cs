using UnityEngine;

public class DataManager : MonoBehaviour
{
    //This Scripts Controls the save and load of the data. In this case there is only one data, the record.

    #region Variables

    private UIManager uiManager;
    private int currentRecord;

    #endregion

    #region Events

    public delegate void NewRecordAction(int newRecord);
    public static event NewRecordAction OnNewRecord;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    #endregion

    #region Event Subscription

    private void OnEnable()
    {
        GameManager.OnGameInitialization += InitialRecordDataLoad;
    }

    private void OnDisable()
    {
        GameManager.OnGameInitialization -= InitialRecordDataLoad;
    }

    #endregion

    #region funcionality

    public void InitialRecordDataLoad()
    {
        if (PlayerPrefs.HasKey("Record"))
        {
            currentRecord = PlayerPrefs.GetInt("Record");
        }
        else
        {
            PlayerPrefs.SetInt("Record", currentRecord);
        }

        uiManager.UpdateRecordText(currentRecord);

    }

    public void CheckRecord(int score)
    {
        if (score > currentRecord)
        {
            currentRecord = score;
            PlayerPrefs.SetInt("Record", currentRecord);
            OnNewRecord(currentRecord);
        }
    }
    
    #endregion

}
