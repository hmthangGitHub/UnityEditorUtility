using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class RectTransformExtension
{
    public enum RectValues
    {
        POSX,
        POSY,
        WIDTH,
        HEIGHT,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    public static List<RectValues> GetAllValues(RectTransform rect)
    {
        List<RectValues> values = new List<RectValues>();
        if (rect.anchorMax.x == rect.anchorMin.x)
        {
            values.Add(RectValues.POSX);
            values.Add(RectValues.WIDTH);
        }
        else
        {
            values.Add(RectValues.LEFT);
            values.Add(RectValues.RIGHT);
        }

        if (rect.anchorMax.y == rect.anchorMin.y)
        {
            values.Add(RectValues.POSY);
            values.Add(RectValues.HEIGHT);
        }
        else
        {
            values.Add(RectValues.TOP);
            values.Add(RectValues.BOTTOM);
        }
        return values;        
    }


    public static (string field, string value) GetModifiedValue(RectValues value)
    {
        switch (value)
        {
            case RectValues.POSX:
                return ("anchoredPosition", "x");
            case RectValues.POSY:
                return ("anchoredPosition", "y");
            case RectValues.WIDTH:
                return ("sizeDelta", "x");
            case RectValues.HEIGHT:
                return ("sizeDelta", "y");
            case RectValues.LEFT:
                return ("offsetMin", "x");
            case RectValues.RIGHT:
                return ("offsetMax", "x");
            case RectValues.TOP:
                return ("offsetMax", "y");
            case RectValues.BOTTOM:
                return ("offsetMin", "y");
            default:
                return ("", "");
        }
    }

    public static void RoundRectTransformValue(this RectTransform rect)
    {
        var values = GetAllValues(rect);
        foreach (var item in values)
        {
            var modifiedValue = GetModifiedValue(item);
            DoRound(rect, modifiedValue.field, modifiedValue.value);
        }
    }

    public static bool CheckIfRound(this RectTransform rect)
    {
        var values = GetAllValues(rect);
        foreach (var item in values)
        {
            var modifiedValue = GetModifiedValue(item);
            if (!CheckIfRound(rect, modifiedValue.field))
            {
                return false;
            }
        }
        return true;
    }

    private static void DoRound(Object instance, string field, string value)
    {
        var property = instance.GetType().GetProperty(field);
        var vector = (Vector2)property.GetValue(instance);
        var newX = value == "x" ? Mathf.Round(vector.x) : vector.x;
        var newY = value == "y" ? Mathf.Round(vector.y) : vector.y;
        property.SetValue(instance, new Vector2(newX, newY));
    }

    private static bool CheckIfRound(Object instance, string field)
    {
        var property = instance.GetType().GetProperty(field);
        var vector = (Vector2)property.GetValue(instance);
        return vector.x % 1 == 0 && vector.y % 1 == 0;
    }
}


