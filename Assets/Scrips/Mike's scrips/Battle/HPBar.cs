using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewEmptyCSharpScript : MonoBehaviour
{
    [SerializeField] GameObject health;

    private void Start()
    {
        health.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
    }

}
