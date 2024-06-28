using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Dialogs
{
    public int ID;
    public string CharacterName;
    public string Context;
    public string LeftSpriteName;
    public string RightSpriteName;
    public string Comment;
    public string SpriteSide;
    public string BarSide;
    
}


[System.Serializable]
public class DialogsRead
{
    public Dialogs info;
    public bool check_read;
}

[System.Serializable]
public class DialogCycle
{
    public List<DialogsRead> info = new List<DialogsRead>();
    public int cycle_index;
    public bool check_cycle_read;
}

public class SequenceManager : MonoBehaviour
{

    public static SequenceManager instance;


    // [SerializeField] InputActionControls sequenceAction;
    bool _interact;

    [Header("json serialize")]
    public TextAsset JsonFile;
    [SerializeField] Dialogs[] TypeWriter;
    [SerializeField] Dictionary<int, List<DialogsRead>> ScriptData;
    [SerializeField] DialogCycle contents;

    [Header("Reading Store")]
    public string temp_context;
    public string temp_left_spriteName;
    public string temp_right_spriteName;
    public EnumSpriteRect BarUISide;
    public Queue<string> text_seq = new Queue<string>();
    public Queue<string> left_sprite_seq = new Queue<string>();
    public Queue<string> right_sprite_seq = new Queue<string>();

    [Header("Dialog Target")]
    private TextMeshProUGUI textName;
    private TextMeshProUGUI textContext;
    private Image leftSprite;
    private Image RightSprite;
    public GameObject DialogBarUI; // DialogName, DialogText
    public GameObject DialogBarObject; // DialogName, DialogText
    public float delay;

    [Header("Coroutine Definition")]
    [SerializeField] IEnumerator SequanceIEnum;
    [SerializeField] IEnumerator SkipIEnum;


    public bool isEnd;
    public bool SequenceProcessing { get { return isEnd; } set { isEnd = value; } }

    public bool dialogEnd;
    public bool DialogProcessing { get { return dialogEnd; } set { dialogEnd = value; } }

    WaitForSecondsRealtime SequenceWaiting = new WaitForSecondsRealtime(0.3f);
    WaitForSecondsRealtime NextContextWait = new WaitForSecondsRealtime(0.8f);

    public IEnumerator DialogSystemStart(int _id, GameObject _seqOwner)
    {
        textName = DialogBarUI.GetComponent<DialogBarScript>().dialogName;
        textContext = DialogBarUI.GetComponent<DialogBarScript>().dialogText;
        leftSprite = DialogBarUI.GetComponent<DialogBarScript>().dialogLeftSprite;
        RightSprite = DialogBarUI.GetComponent<DialogBarScript>().dialogRightSprite;

        DialogBarUI.SetActive(true);
        // Cutscene UI active
        for (int i = 0; i < ScriptData[_id].Count; i++)
        {
            ScriptData[_id][i].check_read = false;
        }

        contents = new DialogCycle() { cycle_index = 0, check_cycle_read = false, info = ScriptData[_id] };

        for (int i = 0; i < contents.info.Count; i++)
        {
            text_seq.Enqueue(contents.info[i].info.Context);
            left_sprite_seq.Enqueue(contents.info[i].info.LeftSpriteName);
            right_sprite_seq.Enqueue(contents.info[i].info.RightSpriteName);
        }

        for (int i = 0; i < contents.info.Count; i++)
        {
            textName.text = contents.info[i].info.CharacterName;

            string side = contents.info[i].info.SpriteSide;
            string barSide = contents.info[i].info.BarSide;

            EnumSpriteRect _spriteRect = SequenceSpriteManagerClass.GetEnumSide(side);
            EnumSpriteRect _barRect = SequenceSpriteManagerClass.GetEnumSide(barSide);

            switch (_barRect)
            {
                case EnumSpriteRect.Left:
                    DialogBarObject.transform.localScale = Vector3.one;
                    break;
                case EnumSpriteRect.Right:
                    Vector3 inverseVec = Vector3.one;
                    inverseVec.x = -1;
                    DialogBarObject.transform.localScale = inverseVec;
                    break;
            }

            switch (_spriteRect)
            { 
                case EnumSpriteRect.Left:
                    leftSprite.gameObject.SetActive(true);
                    RightSprite.gameObject.SetActive(true);
                    break;
                case EnumSpriteRect.Right:
                    leftSprite.gameObject.SetActive(true);
                    RightSprite.gameObject.SetActive(true);
                    break;
                case EnumSpriteRect.Both:
                    leftSprite.gameObject.SetActive(true);
                    RightSprite.gameObject.SetActive(true);
                    break;
                case EnumSpriteRect.OnlyLeft:
                    leftSprite.gameObject.SetActive(true);
                    RightSprite.gameObject.SetActive(false);
                    break;
                case EnumSpriteRect.OnlyRight:
                    leftSprite.gameObject.SetActive(false);
                    RightSprite.gameObject.SetActive(true);
                    break;
                case EnumSpriteRect.Empty:
                    leftSprite.gameObject.SetActive(false);
                    RightSprite.gameObject.SetActive(false);
                    break;
            }

            temp_context = text_seq.Dequeue();
            temp_left_spriteName = left_sprite_seq.Dequeue();
            temp_right_spriteName = right_sprite_seq.Dequeue();

            if (temp_left_spriteName != "-")
            {
                
                Sprite targetSpr = Resources.Load<Sprite>("SequenceSprites/" + temp_left_spriteName);
                leftSprite.sprite = targetSpr;
            }

            if (temp_right_spriteName != "-")
            {

                Sprite targetSpr = Resources.Load<Sprite>("SequenceSprites/" + temp_right_spriteName);
                RightSprite.sprite = targetSpr;
            }

            SequanceIEnum = SequanceCoroutine(i);
            StartCoroutine(SequanceIEnum);

            yield return new WaitUntil(() =>
            {
                if (contents.info[i].check_read)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

        }


        dialogEnd = true;
        // _seqOwner.GetComponent<Sequences>().QueTrigger = false;
        leftSprite.gameObject.SetActive(false);
        RightSprite.gameObject.SetActive(false);
        DialogBarUI.SetActive(false);
    }

    public IEnumerator SequanceCoroutine(int index)
    {
        SkipIEnum = SkipSequanceCoroutine(SequanceIEnum, index);
        StartCoroutine(SkipIEnum);
        textContext.text = "";

        bool SpriteTextCheck = false;

        foreach (char letter in temp_context.ToCharArray())
        {
            textContext.text += letter;
            if (!SpriteTextCheck && letter == '<')
            { 
                SpriteTextCheck = true;
            }
            if (SpriteTextCheck && letter == '>')
            {
                SpriteTextCheck = false;
            }

            if (SpriteTextCheck) continue;

            yield return new WaitForSeconds(delay);
        }

        StopCoroutine(SkipIEnum);
        IEnumerator next = nextContext(index);
        StartCoroutine(next);
    }

    public IEnumerator SkipSequanceCoroutine(IEnumerator _seq, int index)
    {
        yield return SequenceWaiting;
        // yield return new WaitUntil(() => _interact);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Z));
        StopCoroutine(_seq);
        textContext.text = temp_context;
        IEnumerator nextDialog = nextContext(index);
        StartCoroutine(nextDialog);
    }

    public IEnumerator nextContext(int index)
    {
        StopCoroutine(SequanceIEnum);
        StopCoroutine(SkipIEnum);
        yield return NextContextWait;
        // yield return new WaitUntil(() => _interact);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Z));
        DisplayNext(index);
    }

    public void DisplayNext(int index)
    {
        if (text_seq.Count == 0)
        {
            DialogBarUI.gameObject.SetActive(false);
        }
        StopCoroutine(SequanceIEnum);
        contents.info[index].check_read = true;
    }

    public bool dialogRead()
    {
        if (!contents.check_cycle_read)
        {
            return true;
        }

        return false;
    }

    public bool getIsEnd()
    {
        return isEnd;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ScriptData = new Dictionary<int, List<DialogsRead>>();
        JsonDeserialized();
    }

    private void HandleSequence(bool _bool)
    {
        _interact = _bool;
    }

    public void JsonDeserialized()
    {
        var result = JsonConvert.DeserializeObject<Dialogs[]>(JsonFile.ToString());
        foreach (var temp_dialog in result)
        {
            int matchKey = (int)temp_dialog.ID;
            // Debug.Log(matchKey);
            DialogsRead data = new DialogsRead() { info = temp_dialog, check_read = false };

            if (ScriptData.ContainsKey(matchKey))
            {

                ScriptData[matchKey].Add(data);
            }
            else
            {
                ScriptData.Add(matchKey, new List<DialogsRead>());
                ScriptData[matchKey].Add(data);
            }
        }
    }


}
