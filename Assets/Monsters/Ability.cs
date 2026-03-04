using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability 
{
    public AbilityBase Base { get; set; }
    public int up { get; set; }

    public Ability(AbilityBase pBase)
    {
        Base = pBase;
        up = pBase.UP;
    }




}
