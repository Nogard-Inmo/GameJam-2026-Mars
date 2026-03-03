using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour 
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

   public void SetData(Monster monster) 
    {
        nameText.text = monster.Base.name;
        levelText.text = "Lvl " + monster.level;
        //hpBar.SetHP((float)monster.HP / monster.Base.MaxHp);
    }
     public void SetHP(float hpNormalize) 
    {
        hpBar.SetHP(hpNormalize);
    }
   
}