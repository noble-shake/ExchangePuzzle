using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] TutorialSequence1 seq;

    [Header("External Setup")]
    [SerializeField] List<GoalScript> goals;

    [Header("Game UI")]
    [SerializeField] GameObject StageStartUI;
    [SerializeField] GameObject StageEndUI;
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
    public GameObject WarpBlock1 { get { return WarpConnectBlock1; } set { WarpConnectBlock1 = value; } }
    public GameObject WarpBlock2 { get { return WarpConnectBlock2; } set { WarpConnectBlock2 = value; } }

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

        // btnNextStage.onClick.AddListener(BtnNextStage);

    }

    void Start()
    {
        PlayUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        PauseUI.SetActive(false);

        StageInit();
    }

    public void BtnRetry()
    {
        Debug.Log("Retry");
    }

    public void BtnMainMenu()
    {
        Debug.Log("MainMenu Load");
    }

    public void BtnContinue()
    {
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BtnNextStage()
    {
        Debug.Log("NextStage Load");
    }


    public void StageInit()
    {
        goals = new List<GoalScript>();
        findGoalObjects();

        
        seq.Stage1TutorialSequence1();
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

        resetGoalObjects();
        // Stage Load;
    }

    public void StageGameOver()
    {
        resetGoalObjects();
        // reload():
    }


    private void PauseGame() {
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseUI.activeSelf)
        {
            Time.timeScale = 0f;
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.activeSelf)
        {
            Time.timeScale = 1f;
            PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PlayerUIChange() {
        if (PlayUI.activeSelf)
        {
            PlayUI.SetActive(false);
            AimUI.SetActive(true);
        }
        else
        {
            PlayUI.SetActive(true);
            AimUI.SetActive(false);
        }
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
                if (WarpBlock2 != null && WarpBlock1 != WarpBlock2)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }
                if (WarpBlock1 == WarpBlock2 && WarpBlock1 != null && WarpBlock2 !=null)
                {
                    // WarpBlock2.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock2.GetComponent<BlockScript>().UnSyncronizeBlock();
                    WarpBlock2.GetComponent<BlockScript>().closeDimension();
                    WarpBlock2 = null;
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
                if (WarpBlock1 != null && WarpBlock1 != WarpBlock2)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }

                if (WarpBlock1 == WarpBlock2 && WarpBlock1 != null && WarpBlock2 != null)
                {
                    // WarpBlock1.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock1.GetComponent<BlockScript>().UnSyncronizeBlock();
                    WarpBlock1.GetComponent<BlockScript>().closeDimension();

                    WarpBlock1 = null;
                }
                break;
        }
    }

    public void StageStartSequence()
    {
        StageStartUI.SetActive(true);
    }

    public void StageEndSequence()
    {
        StageEndUI.SetActive(false);
    }

}
