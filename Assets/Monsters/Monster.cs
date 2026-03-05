using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster
{

    public MonsterBaseScript Base { get; set; }
    public int level { get; set; }

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

    public bool TakeDamage(Ability ability, Monster attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.level + 10) / 250f;
        float d = a * ability.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            return true;
        }
       
        return false;
        
    }

    public Ability GetRandomAbility()
    {         int r = Random.Range(0, Abilities.Count);
        return Abilities[r];
    }
}
