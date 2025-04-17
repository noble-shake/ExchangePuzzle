using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("External Setup")]
    [SerializeField] List<GoalScript> goals;
    [SerializeField] PlayerDisappearEffect disappearEffectPlayer1;
    [SerializeField] PlayerDisappearEffect disappearEffectPlayer2;

    [Header("Game UI")]
    [SerializeField] GameObject StageStartUI;
    [SerializeField] GameObject StageEndUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject PlayUI;
    [SerializeField] GameObject AimUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject WarpConnectBlock1;
    [SerializeField] GameObject WarpConnectBlock2;
    [SerializeField] Button btnPauseRetry;
    [SerializeField] Button btnPauseMainMenu;
    [SerializeField] Button btnPauseContinue;
    [SerializeField] Button btnEndNextStage;
    [SerializeField] Button btnEndMainMenu;
    [SerializeField] Button btnOverMainMenu;
    [SerializeField] Button btnOverRetry;
    [SerializeField] bool isPaused;
    bool isClear;
    public GameObject WarpBlock1 { get { return WarpConnectBlock1; } set { WarpConnectBlock1 = value; } }
    public GameObject WarpBlock2 { get { return WarpConnectBlock2; } set { WarpConnectBlock2 = value; } }

    public bool PauseEvent { get { return isPaused; } set { isPaused = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }

        Button btnObject1 = btnPauseRetry.GetComponent<Button>();
        btnObject1.onClick.AddListener(BtnRetry);

        Button btnObject2 = btnPauseMainMenu.GetComponent<Button>();
        btnObject2.onClick.AddListener(BtnMainMenu);

        Button btnObject3 = btnPauseContinue.GetComponent<Button>();
        btnObject3.onClick.AddListener(BtnContinue);

        Button btnEndObject1 = btnEndNextStage.GetComponent<Button>();
        btnEndObject1.onClick.AddListener(BtnNextStage);

        Button btnEndObject2 = btnEndMainMenu.GetComponent<Button>();
        btnEndObject2.onClick.AddListener(BtnMainMenu);

        Button btnOverRetryObject = btnOverMainMenu.GetComponent<Button>();
        btnOverRetryObject.onClick.AddListener(BtnMainMenu);

        Button btnOverMainMenuObject2 = btnOverRetry.GetComponent<Button>();
        btnOverMainMenuObject2.onClick.AddListener(BtnRetry);

        StageStartUI.SetActive(false);
        StageEndUI.SetActive(false);
        GameOverUI.SetActive(false);
        PlayUI.SetActive(false);
        AimUI.SetActive(false);
        PauseUI.SetActive(false);
    }

    void Start()
    {
        // PlayUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        PauseUI.SetActive(false);



        PlayerManager.instance.GeneratePlayers();
        StageInit();
    }

    public void BtnRetry()
    {
        Time.timeScale = 1f;
        StageManager.instance.ResetStage();
        
    }

    public void BtnMainMenu()
    {
        Time.timeScale = 1f;
        StageManager.instance.LoadMainMenu();
        
    }

    public void BtnContinue()
    {
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PauseEvent = false;
    }

    public void BtnNextStage()
    {
        Debug.Log("NextStage Load");
        Time.timeScale = 1f;
        StageManager.instance.setCurrentStage(StageManageClass.NextStageEnum(StageManager.instance.getCurrentStage()));
        StageManager.instance.StageIn(StageManager.instance.getCurrentStage());
        
    }

    public void StageInit()
    {
        goals = new List<GoalScript>();
        findGoalObjects();
    }


    public void findGoalObjects()
    {
        GameObject GoalObjectParent = GameObject.Find("GoalsObjects");
        GoalScript[] gObjs = GoalObjectParent.GetComponentsInChildren<GoalScript>();
        foreach (GoalScript target in gObjs)
        {
            goals.Add(target);
        }
    }

    public void resetGoalObjects()
    {
        goals = new List<GoalScript>();
    }

    void Update()
    {
        StageClearCheck();
        PauseGame();

    }


    public void StageClearCheck()
    {
        if (goals.Count == 0) return;

        foreach (GoalScript target in goals)
        {
            if (!target.Trigger) return;
        }

        if (isClear) return;

        isClear = true;
        // string nextPhase = StageManageClass.GetStageInfo(StageManager.instance.getCurrentStage());
        string nextPhase = StageManageClass.NextStageString(StageManager.instance.getCurrentStage());
        Debug.Log(nextPhase);
        if (nextPhase != "AllClear")
        {
            PlayerPrefs.SetInt(nextPhase, 1);
        }

        PlayerManager.instance.EventHappend = true;
        Cursor.lockState = CursorLockMode.Confined;
        StageEndUI.SetActive(true);
        // Time.timeScale = 0f;
    }

    public void StageGameOver()
    {
        PlayerManager.instance.EventHappend = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (!GameOverUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        GameOverUI.SetActive(true);
        
    }


    private void PauseGame() {
        if (SequenceManager.instance.SequenceProcessing) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !PauseUI.activeSelf && !StageEndUI.activeSelf && !GameOverUI.activeSelf && PlayerManager.instance.isAimUIActive == false)
        {
            Time.timeScale = 0f;
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            PauseEvent = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.activeSelf && !StageEndUI.activeSelf && !GameOverUI.activeSelf && PlayerManager.instance.isAimUIActive == false)
        {
            Time.timeScale = 1f;
            PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            PauseEvent = false;
        }
    }

    public void PlayerUIChange(bool isOn) {
            AimUI.SetActive(isOn);
    }

    public void RegistryBlock(GameObject _block, PlayerTag _playerID)
    {
        switch (_playerID)
        {
            case PlayerTag.Player1:
                if (WarpBlock1 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock1 = null;
                }
                WarpBlock1 = _block;
                WarpBlock1.GetComponent<BlockScript>().changeColorFromPlayer(PlayerTag.Player1);
                PlayerManager.instance.AimColorChange(_playerID, true);
                if (WarpBlock2 != null && WarpBlock1 != WarpBlock2)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }
                if (WarpBlock1 == WarpBlock2 && WarpBlock1 != null && WarpBlock2 !=null)
                {
                    WarpBlock2.GetComponent<BlockScript>().UnSyncronizeBlock();
                    WarpBlock2.GetComponent<BlockScript>().closeDimension();
                    WarpBlock2 = null;
                    PlayerManager.instance.AimColorChange(PlayerTag.Player2, false);
                }
                break;
            case PlayerTag.Player2:
                if (WarpBlock2 != null)
                {
                    WarpBlock2.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock2 = null;
                }
                WarpBlock2 = _block;
                WarpBlock2.GetComponent<BlockScript>().changeColorFromPlayer(PlayerTag.Player2);
                PlayerManager.instance.AimColorChange(_playerID, true);
                if (WarpBlock1 != null && WarpBlock1 != WarpBlock2)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }

                if (WarpBlock1 == WarpBlock2 && WarpBlock1 != null && WarpBlock2 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().UnSyncronizeBlock();
                    WarpBlock1.GetComponent<BlockScript>().closeDimension();

                    WarpBlock1 = null;
                    PlayerManager.instance.AimColorChange(PlayerTag.Player1, false);
                }
                break;
        }
    }
}
