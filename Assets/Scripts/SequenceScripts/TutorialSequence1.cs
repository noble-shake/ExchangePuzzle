using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TutorialSequence1 : Sequences
{
    [Header("Sequence Setup")]
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject Camera1;
    [SerializeField] GameObject Camera2;
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;


    [SerializeField] List<IEnumerator> functions = new List<IEnumerator>();
    [SerializeField] List<float> delays = new List<float>();

    public void makeSeqeunce()
    {
        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, true));
        delays.Add(1f);

        functions.Add(SeqActive(StartUI, false));
        delays.Add(1f);

        functions.Add(composite());
        delays.Add(1f);

        functions.Add(SeqActive(Player1, true));
        delays.Add(1f);

        functions.Add(SeqActive(Player2, true));
        delays.Add(1f);

        GenerateSequence(functions, delays);
    }

    protected IEnumerator composite()
    {
        Debug.Log(QueSeqeunce.Count);
        StartCoroutine(SeqActive(Camera1, false));
        StartCoroutine(SeqActive(Camera2, true));
        yield return null;
    }

}
