using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerateEffect : MonoBehaviour
{
    [SerializeField] GameObject OwnPlayer;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        Invoke("GeneratePlayer", 2f);
    }

    public void GeneratePlayer()
    { 
        OwnPlayer.transform.position = transform.position;
        OwnPlayer.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
