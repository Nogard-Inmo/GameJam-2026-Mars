using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void OnPLayerTriggered(MovementV2 player)
    {
        Debug.Log("Hello you are in a portal! :) ");
    }
}
