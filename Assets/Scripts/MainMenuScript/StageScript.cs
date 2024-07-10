using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScript : MonoBehaviour
{
    [SerializeField] EnumStage StageID;
    [SerializeField] int StageCount;
    string StageName;
    [SerializeField] string StageDescription;
    [SerializeField] Button BtnStageIn;
    [SerializeField] TMP_Text StageNumber;
    bool isLocked;
    [SerializeField] GameObject Lock;
    public bool LockCheck { get { return isLocked; } set { isLocked = value; } }

    private void Awake()
    {
        Button btnStageIn = BtnStageIn.GetComponent<Button>();
        btnStageIn.onClick.AddListener(BtnStageIncount);
    }

    private void Start()
    {
        StageName = StageID.ToString();
        StageNumber.text = StageCount.ToString();

        StageStateInit();
    }

    public void StageStateInit()
    {
        if (!PlayerPrefs.HasKey(StageName))
        {
            PlayerPrefs.SetInt(StageName, 0);
        }

        if (PlayerPrefs.GetInt(StageName) == 0)
        {
            Lock.SetActive(true);
            isLocked = true;
        }
        else if (PlayerPrefs.GetInt(StageName) == 1)
        {
            Lock.SetActive(false);
            isLocked = false;
        }
    }

    public void BtnStageIncount()
    {
        if (isLocked) return;

        // Debug.Log("In");
        StageManager.instance.setCurrentStage(StageID);
        StageManager.instance.StageIn(StageID);
    }


    public EnumStage getStageID()
    {
        return StageID;
    }



}
