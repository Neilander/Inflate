using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glue : MonoBehaviour
{
    private Collider2D[] gluedObjects;
    private InflateObject gluedConponent;

    void FixedUpdate()
    {
        gluedObjects = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.lossyScale.x + 0.05f, transform.lossyScale.y + 0.05f), 0);
        foreach(Collider2D gluedObject in gluedObjects)
        {
            if (gluedObject.transform.parent != transform.parent)
            {
                gluedConponent = gluedObject.transform.parent.GetComponent<InflateObject>();
                if (gluedConponent != null)
                    gluedConponent.glueMessage = true;
            }
        }
    }
}
