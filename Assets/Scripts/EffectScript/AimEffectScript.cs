using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum AimColorTag
{
    Blue,
    Yellow,
}

public class AimEffectScript : MonoBehaviour
{
    [SerializeField] Color colorContainer;
    [SerializeField] Image BlueAimEffect;
    [SerializeField] Image YellowAimEffect;
    [SerializeField] float InactiveColor = 50f;
    [SerializeField] float activeColor = 200f;
    public void AlphaChange(AimColorTag _tag, bool _active)
    {
        switch (_tag)
        { 
            case AimColorTag.Blue:
                if (_active)
                {
                    colorContainer = BlueAimEffect.color;
                    colorContainer.a = activeColor / 255f;
                    BlueAimEffect.color = colorContainer;
                }
                else
                {
                    colorContainer = BlueAimEffect.color;
                    colorContainer.a = InactiveColor / 255f;
                    BlueAimEffect.color = colorContainer;
                }

                break;
            case AimColorTag.Yellow:
                if (_active)
                {
                    colorContainer = YellowAimEffect.color;
                    colorContainer.a = activeColor / 255f;
                    YellowAimEffect.color = colorContainer;
                }
                else
                {
                    colorContainer = YellowAimEffect.color;
                    colorContainer.a = InactiveColor / 255f;
                    YellowAimEffect.color = colorContainer;
                }

                break;
        }

    
    }


}
