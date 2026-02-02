using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform hero;

    private float width;
    private Vector3 position;

    public static float left = -1024f;
    public static float right = 1024f;
    public static float up = 1024f ;
    public static float down = -5f;

    public Vector2 offset;

    void Start()
    {
        width = (float)Screen.width / (float)Screen.height;
    }

    void LateUpdate()
    {
        position = transform.position;

        Vector2 target = hero != null
            ? (Vector2)hero.position + offset
            : Vector2.zero;

        // ===== X 轴 =====
        if (right - left <= 10f * width)
        {
            position.x = (left + right) / 2;
        }
        else if (hero != null)
        {
            if (position.x - target.x > 2f)
                position.x = target.x + 2f;
            else if (position.x - target.x < -2f)
                position.x = target.x - 2f;

            if (position.x - left < 5f * width)
                position.x = left + 5f * width;
            else if (position.x - right > -5f * width)
                position.x = right - 5f * width;
        }

        // ===== Y 轴 =====
        if (up - down <= 10f)
        {
            position.y = (up + down) / 2;
        }
        else if (hero != null)
        {
            if (position.y - target.y > 3f)
                position.y = target.y + 3f;
            else if (position.y - target.y < -1f)
                position.y = target.y - 1f;

            if (position.y - up > -5f)
                position.y = up - 5f;
            else if (position.y - down < 5f)
                position.y = down + 5f;
        }

        transform.position = position;
    }
}
