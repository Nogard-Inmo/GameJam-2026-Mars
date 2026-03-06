using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { FreeRoam, Battle }

public class GameController : MonoBehaviour
{
    [SerializeField] MovementV2 movementV2;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start()
    {
        //movementV2.OnEncountered += StartBattle;
        {
            state = GameState.Battle;
        };
    }  
    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
        //    movementV2.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }


}
