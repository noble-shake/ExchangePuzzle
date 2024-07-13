using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiButtonTest : MonoBehaviour
{
    List<Image> listImg = new List<Image>();
    List<Color> listColor =new List<Color>();

    private void Awake()
    {
        listImg.AddRange(GetComponentsInChildren<Image>());
        foreach (Image _img in listImg)
        {
            
        }
    }

    public void OnClick()
    {
        
    }

    public void Release()
    {
        
    }
}
