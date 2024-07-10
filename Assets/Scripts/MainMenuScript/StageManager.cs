using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("Stage Management")]
    [SerializeField] EnumStage CurrentStage;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(instance);
        }

        
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Stage1", 1);
    }

    public EnumStage getCurrentStage()
    {
        return CurrentStage;
    }

    public void setCurrentStage(EnumStage _stage)
    { 
        CurrentStage = _stage;
    }

    public void ResetStage()
    { 
        SceneManager.LoadSceneAsync(StageManageClass.GetStageInfo(CurrentStage));
    }

    public void StageIn(EnumStage _stage)
    {
         SceneManager.LoadSceneAsync(StageManageClass.GetStageInfo(_stage));
    }

    public void LoadMainMenu()
    {
        setCurrentStage(EnumStage.MainMenu);
        SceneManager.LoadSceneAsync(StageManageClass.GetStageInfo(EnumStage.MainMenu));
    }

}
