using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public Monster monster { get; set; }    

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
    }

    public void Setup(Monster monster) 
    {   
        this.monster = monster;
        if (isPlayerUnit)
            image.sprite = monster.Base.BackSprite;
        else
            image.sprite = monster.Base.frontSprite;    
        
    }
}

