using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisappearEffect : MonoBehaviour
{
    [SerializeField] Animation anim;
    [SerializeField] GameObject OwnPlayer;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        OwnPlayer.SetActive(false);
        Invoke("DisappearPlayer", 2f);
    }

    public void DisappearPlayer()
    { 
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
