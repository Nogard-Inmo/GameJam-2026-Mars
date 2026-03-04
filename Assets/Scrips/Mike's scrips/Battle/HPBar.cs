using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    // Cache the original local scale of the health bar so 1f means "full" relative to original size.
    Vector3 initialHealthScale = Vector3.one;

    void Awake()
    {
        if (health == null)
        {
            Debug.LogWarning("HPBar: health GameObject reference is null in Awake.");
            initialHealthScale = Vector3.one;
            return;
        }

        initialHealthScale = health.transform.localScale;
    }

    public void SetHP(float hpNormalize)
    {
        if (health == null)
        {
            Debug.LogWarning("HPBar: health GameObject reference is null.");
            return;
        }

        hpNormalize = Mathf.Clamp01(hpNormalize);

        // Scale X relative to the initial X scale so that hpNormalize == 1 restores the original size.
        health.transform.localScale = new Vector3(initialHealthScale.x * hpNormalize, initialHealthScale.y, initialHealthScale.z);
    }

}