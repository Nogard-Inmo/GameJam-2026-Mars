using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    public Monster Monster { get; set; }    

    public void Setup(Monster monster) 
    { 
        Monster = monster;

        if (isPlayerUnit)
            GetComponent<Image>().sprite = monster.Base.BackSprite;

        else
            GetComponent<Image>().sprite = monster.Base.frontSprite;    
        
    }
}

