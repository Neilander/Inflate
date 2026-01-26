using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointKind { FlagPosition, CameraLeft, CameraRight, CameraUp, CameraDown }

public class Point : MonoBehaviour
{
    public PointKind pointKind;
    void FixedUpdate()
    {
        switch (pointKind)
        {
            case PointKind.FlagPosition:
                Flag.flagPosition.x = transform.position.x;
                Flag.flagPosition.y = transform.position.y;
                break;
            case PointKind.CameraLeft:
                CameraManager.left = transform.position.x;
                break;
            case PointKind.CameraRight:
                CameraManager.right = transform.position.x;
                break;
            case PointKind.CameraUp:
                CameraManager.up = transform.position.y;
                break;
            case PointKind.CameraDown:
                CameraManager.down = transform.position.y;
                break;
        }
    }
}
