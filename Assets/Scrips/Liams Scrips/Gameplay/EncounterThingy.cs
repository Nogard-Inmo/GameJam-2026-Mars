using UnityEngine;

public class EncounterThingy : MonoBehaviour, IPlayerTriggerable
{
    public void OnPLayerTriggered(MovementV2 player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
          //  character.Animator.IsMoving= false;
           // GameController.Instance.StartBattle();
        }
    }
}
