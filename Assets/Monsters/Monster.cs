using UnityEngine;

public class Monster
{

    MonsterBaseScript _base; //https://youtu.be/zKRMkD28-xY?t=1065
    int level;



    public Monster(MonsterBaseScript pBase, int plevel)
    {
        _base = pBase;
        level = plevel;
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((_base.MaxHp * level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }
    
    public int Defense
    {
        get { return Mathf.FloorToInt((_base.Defense * level) / 100f) + 5; }
    }
    
    public int SpAttack
    {          
       get { return Mathf.FloorToInt((_base.SpAttack * level) / 100f) + 5; }
    }
   
    public int SpDefense
    {
       get { return Mathf.FloorToInt((_base.SpDefense * level) / 100f) + 5; }
    }
   
    public int Speed
    {
       get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
    }

}
