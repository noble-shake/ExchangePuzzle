using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventPushObject : MonoBehaviour
{
    [SerializeField] PlayerScript TraceTarget;
    [SerializeField] Collider coll;
    [SerializeField] Rigidbody rigid;

    public PlayerScript Tracing { get { return TraceTarget; } set { TraceTarget = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TraceTarget != null)
        { 
            transform.position = TraceTarget.transform.position;
        }
    }
}
