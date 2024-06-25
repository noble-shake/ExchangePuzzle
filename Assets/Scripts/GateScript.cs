using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [Header("Gate Setup")]
    [SerializeField] MeshRenderer m_Renderer;
    [SerializeField] GateSwitchType gateType;
    [SerializeField] Color gateColor;

    [Header("External Setup")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject EffectObject;
    [SerializeField] List<GameObject> GateBlocks;
    [SerializeField] bool isTriggered;
    [SerializeField] bool isProcessed;

    void Start()
    {
        // gateColor = GetComponent<MeshRenderer>().material.color;
        
        Material mat = Instantiate(m_Renderer.material);
        m_Renderer.material = mat;
        m_Renderer.material.color = gateColor;
        EffectObject.GetComponent<MeshRenderer>().material.color = gateColor;


        int count = GateBlocks.Count;
        for (int inum = 0; inum < count; inum++)
        {
            GateBlocks[inum].GetComponent<BlockScript>().changeColorFromSwitch(gateColor);
        }
    }

    void Update()
    {
        m_Renderer.material.color = gateColor;
        PressCheck();
    }

    private void PressCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 1.1f, LayerMask.GetMask("Player")) || Physics.Raycast(transform.position, transform.up, out hit, 1.1f, LayerMask.GetMask("Object")))
        {
            isTriggered = true;
            anim.SetBool("ButtonTriggered", false);
        }
        else
        {
            if (isTriggered && gateType == GateSwitchType.Hold)
            { 
                isTriggered = false;
                int count = GateBlocks.Count;
                for (int inum = 0; inum < count; inum++)
                {
                    if (!GateBlocks[inum].activeSelf)
                    {
                        GateBlocks[inum].SetActive(true);
                    }

                }
                anim.SetBool("ButtonTriggered", true);
            }
        }

        if (isTriggered)
        {
            if (isProcessed) return;
            int count = GateBlocks.Count;
            for (int inum = 0; inum < count; inum++)
            {
                if (GateBlocks[inum].activeSelf)
                {
                    GateBlocks[inum].SetActive(false);
                }

            }
            isProcessed = true;
        }
    }
}
