using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Makes it so that you can create an ability in the editor and then use it as a reference for the abilities of the monsters-Liam
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
