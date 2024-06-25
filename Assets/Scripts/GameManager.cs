using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
    }

    void Start()
    {
        PlayUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        PauseUI.SetActive(false);
        StageInit();
    }

    public void StageInit()
    {
        goals = new List<GoalScript>();
        findGoalObjects();

        
        seq.makeSeqeunce();
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
        // PauseGame();

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

    public void RegistryBlock(GameObject _block, int _playerID)
    {
        switch (_playerID)
        {
            case 0:
                if (WarpBlock1 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock1 = null;
                }
                WarpBlock1 = _block;
                WarpBlock1.GetComponent<BlockScript>().changeColorFromPlayer(0);
                if (WarpBlock2 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }
                if (WarpBlock1 == WarpBlock2)
                {
                    WarpBlock2.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock2 = null;
                }
                break;
            case 1:
                if (WarpBlock2 != null)
                {
                    WarpBlock2.GetComponent<BlockScript>().shutDownDimension();
                    WarpBlock2 = null;
                }
                WarpBlock2 = _block;
                WarpBlock2.GetComponent<BlockScript>().changeColorFromPlayer(1);
                if (WarpBlock1 != null)
                {
                    WarpBlock1.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock2.GetComponent<BlockScript>());
                    WarpBlock2.GetComponent<BlockScript>().SynchronizeBlock(WarpBlock1.GetComponent<BlockScript>());
                    WarpBlock1.GetComponent<BlockScript>().openDimenstion();
                    WarpBlock2.GetComponent<BlockScript>().openDimenstion();
                }

                if (WarpBlock1 == WarpBlock2)
                {
                    WarpBlock1.GetComponent<BlockScript>().shutDownDimension();
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
