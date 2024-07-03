using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [Header("External Setup")]
    [SerializeField] Animator anim;
    [SerializeField] PlayerTag ownerType;
    [SerializeField] GameObject HoldPortal;
    [SerializeField] float holdingTime;
    [SerializeField] bool isTriggered;
    public bool Trigger { get { return isTriggered; } }

    private void Start()
    {
        PaletteChange();
    }

    private void PaletteChange()
    {
        Color TargetColor = GateColor();
        foreach (MeshRenderer childMesh in GetComponentsInChildren<MeshRenderer>())
        {
            childMesh.material.color = TargetColor;
        }
    }

    private Color GateColor()
    {
        switch (ownerType)
        {
            case PlayerTag.Player1:
                return BlockColoring.GetBlockColor(BlockColorTag.Player1);
            case PlayerTag.Player2:
                return BlockColoring.GetBlockColor(BlockColorTag.Player2);
        }

        return Color.white;
    }

    void Update()
    {
        PlayerCheck();
    }

    private void PlayerCheck()
    {
        if (Physics.Raycast(HoldPortal.transform.position, HoldPortal.transform.up, out RaycastHit hit, 1.1f, LayerMask.GetMask("Player")) && hit.transform.gameObject.GetComponent<PlayerScript>().player == ownerType)
        {
            holdingTime -= Time.deltaTime;
            anim.SetBool("PortalOn", true);
        }
        else
        {
            holdingTime = 2f;
            isTriggered = false;
            anim.SetBool("PortalOn", false);
        }

        if (holdingTime < 0f)
        {
            holdingTime = 0f;
            isTriggered = true;

        }

    }

}
