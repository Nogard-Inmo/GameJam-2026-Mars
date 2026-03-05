using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]    
public class Monster
{
    [SerializeField] MonsterBaseScript _base;
    [SerializeField] int level;
    public MonsterBaseScript Base {
        get
        {
            return _base;
        }
    }
    public int Level {
        get
        {
            return level;
        }
    }

    public int Hp { get; set; }
    public List<Ability> Abilities { get; set; }

    public void Init()
    {
        
        Hp = MaxHp;
            
        //Generate abilities
        Abilities = new List<Ability>();
            foreach (var learnableAbility in Base.learnableAbilities)
            {
                if (learnableAbility.Level <= Level)
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

    public DamageDetails TakeDamage(Ability ability, Monster attacker)
    {
        float critical = 1f;
        if (Random.value * 100 <= 6.9f)
            critical = 2f;


        float type = TypeChart.GetEffectiveness(ability.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(ability.Base.Type, this.Base.Type1);
             
        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted = false
            
        };



        float attack = (ability.Base.IsSpecial)? attacker.SpAttack : attacker.Attack;
        float defense = (ability.Base.IsSpecial)? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.level + 10) / 250f;
        float d = a * ability.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            damageDetails.Fainted = true;
        }
       
        return damageDetails;
        
    }

    public Ability GetRandomAbility()
    {         int r = Random.Range(0, Abilities.Count);
        return Abilities[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }
}
