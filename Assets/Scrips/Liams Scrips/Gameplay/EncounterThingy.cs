using UnityEngine;
using System;

public class EncounterThingy : MonoBehaviour, IPlayerTriggerable
{

    public event Action OnEncountered;

    Character character;
    public void OnPLayerTriggered(MovementV2 player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            OnEncountered();
        }
    }
}
