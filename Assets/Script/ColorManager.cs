using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorGroup
{
    public Color mainColor;
    public Color edgeColor;
    public Color inflatetEdgeColor;
    public Color lightColor;
}

[CreateAssetMenu(fileName = "Color Manager", menuName = "Color/Color Manager")]
public class ColorManager : ScriptableObject
{
    public List<ColorGroup> colorGroups;
    public static ColorManager Instance
    {
        get
        {
            ColorManager manager = Resources.Load("Color Manager") as ColorManager;
            return manager;
        }
    }

}
