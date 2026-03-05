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
        originalColor = image.color;
    }

    public void Setup(Monster monster) 
    {   
        monster = monster;
        if (isPlayerUnit)
            GetComponent<Image>().sprite = monster.Base.BackSprite;
        else
            GetComponent<Image>().sprite = monster.Base.frontSprite;    
        
        hud.SetData(monster);

        image.color = originalColor;
    }
}

