using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDimensionCollider : MonoBehaviour
{
    [Header("Collider Setup")]
    [SerializeField] BlockScript ParentObj;
    [SerializeField] BoxCollider coll;

    void Start()
    {
        ParentObj = GetComponentInParent<BlockScript>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            ParentObj.TriggerOnDimensionObject(other);
            //StartCoroutine(ParentObj.TriggerOnDimensionObject(other));
        }
        else if (other.CompareTag("block"))
        {
            ParentObj.BlockCheck = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("block"))
        {
            ParentObj.BlockCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("block"))
        {
            ParentObj.BlockCheck = false;
        }
    }

}
