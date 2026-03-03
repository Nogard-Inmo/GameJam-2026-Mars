using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalize) 
    {
        health.transform.localScale = new Vector3(hpNormalize, 1f);
    }
}
