using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster
{

    public MonsterBaseScript Base; //https://youtu.be/zKRMkD28-xY?t=1065
    public int level;

    public int Hp { get; set; }
    public List<Ability> Abilities { get; set; }

    public Monster(MonsterBaseScript pBase, int plevel)
    {
        Base = pBase;
        level = plevel;
        Hp = MaxHp;

        //Generate abilities
        Abilities = new List<Ability>();
            foreach (var learnableAbility in Base.learnableAbilities)
            {
                if (learnableAbility.Level <= level)
                {
                    Abilities.Add(new Ability(learnableAbility.Base));
                }

                if (Abilities.Count >= 4)
                {
                    break;
                }
            }
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * level) / 100f) + 5; }
    }
    
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * level) / 100f) + 5; }
    }
    
    public int SpAttack
    {          
       get { return Mathf.FloorToInt((Base.SpAttack * level) / 100f) + 5; }
    }
   
    public int SpDefense
    {
       get { return Mathf.FloorToInt((Base.SpDefense * level) / 100f) + 5; }
    }
   
    public int Speed
    {
       get { return Mathf.FloorToInt((Base.Speed * level) / 100f) + 5; }
    }

}
