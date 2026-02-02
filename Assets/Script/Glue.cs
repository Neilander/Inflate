using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glue : MonoBehaviour
{
    private Collider2D[] gluedObjects;
    private InflateObject gluedConponent;

    private HashSet<InflateObject> gluedSet = new HashSet<InflateObject>();

    
    void FixedUpdate()
    {
        HashSet<InflateObject> touching = new HashSet<InflateObject>();
        
        gluedObjects = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.lossyScale.x + 0.05f, transform.lossyScale.y + 0.05f), 0);
        foreach(Collider2D gluedObject in gluedObjects)
        {
            if (gluedObject.transform.parent.parent != transform.parent)
            {
                gluedConponent = gluedObject.transform.parent.parent.GetComponent<InflateObject>();
                if (gluedConponent != null)
                {
                    touching.Add(gluedConponent);

                    if (!gluedSet.Contains(gluedConponent))
                    {
                        //gluedSet.Add(gluedConponent);
                        gluedConponent.glueMessage = true;
                    }
                }
            }
        }
        /*
        gluedSet.RemoveWhere(inflate =>
        {
            if (inflate == null)
                return true;

            if (!touching.Contains(inflate))
            {
                inflate.UnregisterGluer(this);
                return true;
            }

            return false;
        });*/
    }
}
