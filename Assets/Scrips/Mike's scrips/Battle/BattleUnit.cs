using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    [SerializeField] BattleHud hud;


    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public BattleHud Hud { get { return hud; } }
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
        
        hud.SetData(monster);

        image.color = originalColor;
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
     
    }
}

