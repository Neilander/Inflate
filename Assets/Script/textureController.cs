using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Material targetMaterial;
    [SerializeField] private string propertyName = "_Blend";

    [Header("Height Control")]
    [SerializeField] private float lowY = 0f;      // 低于这个 y
    [SerializeField] private float highY = 10f;    // 高于这个 y

    [Header("Value Range")]
    [SerializeField] private float maxValue = 1f;  // 低处的值
    [SerializeField] private float minValue = 0f;  // 高处的值

    void Update()
    {
        float y = transform.position.y;
        float value;

        if (y <= lowY)
        {
            value = maxValue;
        }
        else if (y >= highY)
        {
            value = minValue;
        }
        else
        {
            // 0~1 插值因子
            float t = Mathf.InverseLerp(lowY, highY, y);
            value = Mathf.Lerp(maxValue, minValue, t);
        }

        targetMaterial.SetFloat(propertyName, value);
    }
}
