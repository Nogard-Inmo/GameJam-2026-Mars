using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] MonsterBaseScript _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    public Monster monster { get; set; }    

    public void Setup() 
    { 
       /* monster = new Monster(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = monster.Base.BackSprite;
        else
            GetComponent<Image>().sprite = monster.Base.FrontSprite;    
        */
    }
}

