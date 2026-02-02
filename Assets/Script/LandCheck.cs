using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCheck : MonoBehaviour
{
    public bool onLand { get; private set; }
    public bool justLanded { get; private set; }

    private bool wasOnLand;

    void LateUpdate()
    {
        if(justLanded)
            AudioManager.PlaySFX("BoxLand");
        // justLanded 只保留一帧
        justLanded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 只要是 Default Layer 的东西
        if (collision.gameObject.layer != LayerMask.NameToLayer("Default"))
            return;

        // 判断是不是“被下面托住”
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                wasOnLand = onLand;
                onLand = true;
                justLanded = !wasOnLand;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            onLand = false;
        }
    }
}
