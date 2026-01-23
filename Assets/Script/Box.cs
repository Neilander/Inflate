using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Collider2D[] colliders;
    private static float depth = 0.1f;
    private bool result;

    public bool TestCanInflate()
    {
        result = true;
        colliders = Physics2D.OverlapBoxAll(transform.position, new Vector3(transform.lossyScale.x - depth, transform.lossyScale.y - depth),0, 1 << 7);
        foreach(Collider2D collider in colliders)
        {
            if (collider.transform.parent != transform.parent)
                 result = false;
        }
        return result;
    }
}
