using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{ 
    Normal,
    Moving,
    NonChanged,
    Holding,
}


public enum BlockColorTag
{
    Normal,
    Player1,
    Player2,
    Etc,
}

public static class BlockColoring
{ 
    public static string GetBlockColorTag(BlockColorTag _value)
    {
        return _value.ToString();
    }

    public static Color GetBlockColor(BlockColorTag _value) 
    {
        switch (_value)
        {
            case BlockColorTag.Normal:
                return Color.white;
            case BlockColorTag.Player1:
                return new Color(80f / 255f, 188f / 255f, 223f / 255f, 1f);
            case BlockColorTag.Player2:
                return new Color(68f / 255f, 0f / 255f, 82f / 255f, 1f);
            case BlockColorTag.Etc:
                break;

        }

        return Color.white;
    }
}