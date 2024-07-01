using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    None,
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
    Trap,
}


public enum BlockColorTag
{
    Normal,
    Player1,
    Player2,
    Etc,
}

public enum EnumDimension
{ 
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public static class DimensionClass
{
    public static EnumDimension GetFromFixedDimension(Vector3 _vec)
    {
        if (_vec == Vector3.up)
        {
            return EnumDimension.UP;
        }
        else if (_vec == Vector3.down)
        {
            return EnumDimension.DOWN;
        }
        else if (_vec == Vector3.left)
        {
            return EnumDimension.LEFT;
        }
        else if (_vec == Vector3.right)
        {
            return EnumDimension.RIGHT;
        }
        return EnumDimension.UP;
    }

    public static EnumDimension GetFromDynamicDimension(Vector3 _vec)
    {
        Vector3 dimVector;
        float targetDim = Mathf.Max(Mathf.Abs(_vec.x), Mathf.Abs(_vec.y)) == Mathf.Abs(_vec.x) ?  _vec.x : _vec.y;
        if (targetDim >= 0)
        {
            if (targetDim == _vec.x)
            {
                dimVector = Vector3.right;
            }
            else
            {
                dimVector = Vector3.up;
            }
        }
        else
        {
            if (targetDim == _vec.x)
            {
                dimVector = Vector3.left;
            }
            else
            {
                dimVector = Vector3.down;
            }
        }

        if (dimVector == Vector3.up)
        {
            return EnumDimension.UP;
        }
        else if (dimVector == Vector3.down)
        {
            return EnumDimension.DOWN;
        }
        else if (dimVector == Vector3.left)
        {
            return EnumDimension.LEFT;
        }
        else if (dimVector == Vector3.right)
        {
            return EnumDimension.RIGHT;
        }
        return EnumDimension.UP;
    }

    public static Vector3 GetFromDimensionDirection(EnumDimension _dim)
    {
        switch (_dim)
        { 
            case EnumDimension.UP:
                return Vector3.up;
            case EnumDimension.DOWN:
                return Vector3.down;
            case EnumDimension.LEFT:
                return Vector3.left;
            case EnumDimension.RIGHT:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }
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
                return new Color(255f / 255f, 255f / 255f, 0f, 1f);
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