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

    private void Awake()
    {
        battleSystem.gameObject.SetActive(false);
    }


    private void Start()
    {
        movementV2.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }  


    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = movementV2.GetComponent<MonsterParty>();
        var wildMonster = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildMonster();

        battleSystem.StartBattle(playerParty, wildMonster);
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        worldCamera.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            movementV2.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }


}
