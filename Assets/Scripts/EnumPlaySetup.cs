using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumSpriteRect
{ 
    Left,
    Right,
    Both,
    OnlyLeft,
    OnlyRight,
    Empty,
}

public enum EnumStage
{ 
    MainMenu,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
}
public enum PlayerTag
{ 
    Player1,
    Player2,
}

public enum GateSwitchType
{ 
    Pressed,
    Hold,
}

public enum BlockType
{ 
    Normal,
    Moving,
    NonChanged,
    NonChangedMoving,
    Holding,
    Passed,
    Gate,
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

public static class StageManageClass
{
    public static string GetStageInfo(EnumStage _value)
    {
        return _value.ToString();
    }

    public static string CurrentStageInfo(EnumStage _value)
    {
        return GetStageInfo(_value);
    }

    public static string NextStageInfo(EnumStage _value)
    {
        return GetStageInfo(++_value);
    }
}

public static class SequenceSpriteManagerClass
{
    public static EnumSpriteRect GetEnumSide(string _sprite)
    {
        return (EnumSpriteRect)System.Enum.Parse(typeof(EnumSpriteRect), _sprite);
    }

    public static string GetStringSide(EnumSpriteRect _sprite)
    { 
        return _sprite.ToString();
    }
}