using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TutorialSequence3 : Sequences
{
    [Header("inner setup")]
    [SerializeField] BoxCollider coll;
    [SerializeField] bool SequenceTrigger;

    [SerializeField] List<IEnumerator> functions;
    [SerializeField] List<float> delays;

    private void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    public void Stage1TutorialSequence3()
    {
        functions = new List<IEnumerator>();
        delays = new List<float>();

        functions.Add(SeqDialPlay(5, gameObject));
        delays.Add(0.3f);

        GenerateSequence(functions, delays);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<PlayerScript>().player == PlayerTag.Player2 && !SequenceTrigger)
    //    {
    //        SequenceTrigger = true;
    //        Stage1TutorialSequence2();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            if (!SequenceTrigger)
            {
                SequenceTrigger = true;
                Stage1TutorialSequence3();
            }
        }
    }
}
