using UnityEngine;

public class Monster
{

    public MonsterBaseScript Base; //https://youtu.be/zKRMkD28-xY?t=1065
    public int level;



    public Monster(MonsterBaseScript pBase, int plevel)
    {
        Base = pBase;
        level = plevel;
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
