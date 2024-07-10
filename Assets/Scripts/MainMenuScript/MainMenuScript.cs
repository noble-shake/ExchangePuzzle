using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;
using System;


public enum EnumInterface
{
    LogoUI,
    StartUI,
    StageSelectUI,
}


public class MainMenuScript : MonoBehaviour
{
    [Header("Effector")]
    [SerializeField] BackgroundMoveEffect EffectSlider;
    [SerializeField] float EffectorTime;
    float curEffectorTime;

    [Header("UI Component")]
    [SerializeField] GameObject StartMenuUI;
    [SerializeField] GameObject StageSelectUI;
    [SerializeField] GameObject LogoUI;
    [SerializeField] GameObject CreditUI;

    [SerializeField] Button StartBtn;
    [SerializeField] Button CreditBtn;
    [SerializeField] Button EndBtn;
    [SerializeField] Button CreditBackBtn;
    [SerializeField] Button StageSelectBackBtn;
    [SerializeField] Button StageUnlockAllBtn;
    [SerializeField] Button StageResetAllBtn;

    [Header("External Setup")]
    [SerializeField] List<StageScript> StageObjects;

    private void Awake()
    {
        Button btnStartObject = StartBtn.GetComponent<Button>();
        Button btnEndObject = EndBtn.GetComponent<Button>();
        Button CreditBtnObject = CreditBtn.GetComponent<Button>();
        Button CreditBackBtnObject = CreditBackBtn.GetComponent<Button>();
        Button StageSelectBackBtnObject = StageSelectBackBtn.GetComponent<Button>();
        Button StageUnlockAllBtnObject = StageUnlockAllBtn.GetComponent<Button>();
        Button StageResetAllBtnObject = StageResetAllBtn.GetComponent<Button>();

        btnStartObject.onClick.AddListener(BtnStart);
        btnEndObject.onClick.AddListener(BtnGameEnd);
        CreditBtnObject.onClick.AddListener(BtnCredit);
        CreditBackBtnObject.onClick.AddListener(BtnCreditBack);
        StageSelectBackBtnObject.onClick.AddListener(BtnSelectBack);
        StageUnlockAllBtnObject.onClick.AddListener(BtnUnlockAll);
        StageResetAllBtnObject.onClick.AddListener(BtnResetAll);
    }

    // Start is called before the first frame update
    void Start()
    {
        // EffectSlider.BackSlide();
        LogoUI.SetActive(true);
        StartMenuUI.SetActive(false);
        StageSelectUI.SetActive(false);
        CreditUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LogoCheck();
    }

    public void LogoCheck()
    {
        curEffectorTime -= Time.deltaTime;
        if (curEffectorTime < 0)
        {
            curEffectorTime = 0;
        }

        if (Input.anyKeyDown && LogoUI.activeSelf && curEffectorTime == 0)
        {
            // Logo -> StartUI
            Color currentAlpha = new Color(1, 1, 1, (curEffectorTime) / 3f);
            LogoUI.GetComponentInChildren<Image>().color = currentAlpha;
            curEffectorTime = EffectorTime;
            
            
        }
        if (LogoUI.activeSelf && LogoUI.GetComponentInChildren<Image>().color.a == 0f)
        {
            StartOn();
        }
    }

    public void SystemReset()
    {
        LogoUI.SetActive(true);
        StartMenuUI.SetActive(false);
        StageSelectUI.SetActive(false);
        LogoUI.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
        EffectSlider.SlideReset();
    }

    public void StartOn()
    {
        LogoUI.SetActive(false);
        StartMenuUI.SetActive(true);
        StageSelectUI.SetActive(false);
        EffectSlider.SlideChange();
    }

    public void BtnStart()
    {
        LogoUI.SetActive(false);
        StartMenuUI.SetActive(false);
        StageSelectUI.SetActive(true);
        EffectSlider.SlideChange();
    }

    public void BtnCredit()
    {
        StartMenuUI.SetActive(false);
        CreditUI.SetActive(true);
        EffectSlider.SlideChange();
    }

    public void BtnCreditBack()
    {
        StartMenuUI.SetActive(true);
        CreditUI.SetActive(false);
        EffectSlider.SlideChange();
    }
    public void BtnSelectBack()
    {
        StartMenuUI.SetActive(true);
        StageSelectUI.SetActive(false);
        EffectSlider.SlideChange();
    }

    public void BtnGameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BtnUnlockAll()
    {
        foreach (StageScript target in StageObjects)
        {
            EnumStage targetStage = target.getStageID();
            string key = StageManageClass.GetStageInfo(targetStage);
            PlayerPrefs.SetInt(key, 1);
            target.StageStateInit();
        }
    }

    public void BtnResetAll()
    {
        foreach (StageScript target in StageObjects)
        {
            EnumStage targetStage = target.getStageID();
            string key = StageManageClass.GetStageInfo(targetStage);
            PlayerPrefs.SetInt(key, 0);
            target.StageStateInit();
        }
    }
}
