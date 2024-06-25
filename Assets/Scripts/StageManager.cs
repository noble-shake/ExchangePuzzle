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
        }
        else
        { 
            Destroy(instance);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
    }
    void Update()
    {

    }

    public void ResetStage()
    { 
        SceneManager.LoadSceneAsync(StageManageClass.GetStageInfo(CurrentStage));
    }

    public void NextStage()
    {
         SceneManager.LoadSceneAsync(StageManageClass.NextStageInfo(CurrentStage));
    }

}
