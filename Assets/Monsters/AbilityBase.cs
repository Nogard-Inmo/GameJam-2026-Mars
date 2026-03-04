using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//Creates a scriptable object that can be used as a reference for the abilities of the monsters-Liam
[CreateAssetMenu(fileName = "Ability", menuName = "Monster/Create new ability")]
public class AbilityBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] MonsterType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int up;

    public string Name
    {
               get { return name; }
    }
    public string Description
    {
                get { return description; }
    }
    public MonsterType Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Accuracy
    {
        get { return accuracy; }
    }
    public int UP
    {
        get { return up; }
    }

}
