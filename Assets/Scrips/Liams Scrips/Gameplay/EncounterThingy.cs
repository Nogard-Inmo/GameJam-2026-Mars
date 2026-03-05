using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EncounterThingy : MonoBehaviour, IPlayerTriggerable
{

    

    Character character;
    public void OnPLayerTriggered(MovementV2 player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {

            character.Animator.IsMoving = false;
            //GameController.Instance.StartBattle();

        }
    }
}
