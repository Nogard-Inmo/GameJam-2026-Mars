using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]
public class MonsterBaseScript : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    public Sprite frontSprite;
    public Sprite backSprite;

    [SerializeField] MonsterType type1;

    //Base stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] public List<LearnableAbility> learnableAbilities;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }   

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public MonsterType Type1
    {
        get { return type1; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableAbility> LearnableAbilities
    {
        get { return learnableAbilities; }
    }

}


[System.Serializable]

public class LearnableAbility
{
    [SerializeField] AbilityBase abilityBase;
    [SerializeField] int level;
    public AbilityBase Base
    {
        get { return abilityBase; }
    }
    public int Level
    {
        get { return level; }
    }
}


public enum MonsterType
{
    None,
    Fantasy,
    Science,
    Ancient
}
