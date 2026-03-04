using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalize)
    {
        if (health == null)
        {
            Debug.LogWarning("HPBar: health GameObject reference is null.");
            return;
        }

        hpNormalize = Mathf.Clamp01(hpNormalize);
        // Ensure 3D vector (x, y, z)
        health.transform.localScale = new Vector3(hpNormalize, 1f, 1f);
    }

}