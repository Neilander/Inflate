using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Collider2D pressureCollider;
    private bool pressure;
    public Transform hero;
    public static Vector2 flagPosition;

    void FixedUpdate()
    {
        transform.position = new Vector3(flagPosition.x, flagPosition.y, 2);
        CheckPressure();
        if (pressure)
            Damage();
    }

    private void CheckPressure()
    {
        pressureCollider = Physics2D.OverlapBox(transform.position + new Vector3(0.4f, 0.75f, 0), new Vector2(0.8f, 0.5f), 0);
        if (pressureCollider != null)
        {
            if (pressureCollider.transform.parent != hero)
                pressure = true;
        }
    }

    private void Damage()
    {
        Destroy(gameObject);
    }
}
