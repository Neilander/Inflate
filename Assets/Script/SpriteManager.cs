using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Manager", menuName = "Sprite/Sprite Manager")]
public class SpriteManager : ScriptableObject
{
    public Sprite positionFixed;
    public Sprite banPush;
    public Sprite breakable;
    public Sprite glue;
    public static SpriteManager Instance
    {
        get
        {
            SpriteManager manager = Resources.Load("Sprite Manager") as SpriteManager;
            return manager;
        }
    }
}
