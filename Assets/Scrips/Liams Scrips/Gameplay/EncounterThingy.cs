using UnityEngine;
using System;

public class EncounterThingy : MonoBehaviour, IPlayerTriggerable
{

    

    Character character;
    public void OnPLayerTriggered(MovementV2 player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            
        }
    }
}
