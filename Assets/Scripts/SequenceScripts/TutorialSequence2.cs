using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TutorialSequence2 : Sequences
{
    [Header("inner setup")]
    [SerializeField] BoxCollider2D coll;
    [SerializeField] bool SequenceTrigger;
    [SerializeField] GameObject WallCam;

    [Header("external setup")]
    [SerializeField] GameObject KeyZ;
    [SerializeField] GameObject MouseClick;

    [SerializeField] List<IEnumerator> functions;
    [SerializeField] List<float> delays;

    public void Stage1TutorialSequence2()
    {
        functions = new List<IEnumerator>();
        delays = new List<float>();

        functions.Add(SeqDialPlay(4, gameObject));
        delays.Add(0.3f);

        functions.Add(SeqActive(KeyZ, true));   
        delays.Add(1f);

        functions.Add(SeqActive(MouseClick, true));
        delays.Add(1f);

        GenerateSequence(functions, delays);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerScript>().player == PlayerTag.Player2 && !SequenceTrigger)
        {
            SequenceTrigger = true;
            Stage1TutorialSequence2();
        }
    }


}
