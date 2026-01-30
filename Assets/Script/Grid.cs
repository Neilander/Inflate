using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<LineRenderer> horizontalLineRenderers;
    public List<LineRenderer> verticalLineRenderers;
    public Transform myCamera;
    public float width;
    private int x;
    private int y;

    void Start()
    {
        foreach (LineRenderer lineRenderer in horizontalLineRenderers)
        {
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
        }
        foreach (LineRenderer lineRenderer in verticalLineRenderers)
        {
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
        }
    }

    void FixedUpdate()
    {
        foreach (LineRenderer lineRenderer in horizontalLineRenderers)
        {
            y = (int)(lineRenderer.GetPosition(0).y);
            if (y > myCamera.position.y + 8f)
                y -= 15;
            else if (y < myCamera.position.y - 8f)
                y += 15;
            lineRenderer.SetPosition(0, new Vector3(myCamera.position.x - 18f, y, 0));
            lineRenderer.SetPosition(1, new Vector3(myCamera.position.x + 18f, y, 0));
        }
        foreach (LineRenderer lineRenderer in verticalLineRenderers)
        {
            x = (int)(lineRenderer.GetPosition(0).x);
            if (x > myCamera.position.x + 12)
                x -= 23;
            else if (x < myCamera.position.x - 12)
                x += 23;
            lineRenderer.SetPosition(0, new Vector3(x,myCamera.position.y - 10f, 0));
            lineRenderer.SetPosition(1, new Vector3(x,myCamera.position.y + 10f, 0));
        }
    }
}
