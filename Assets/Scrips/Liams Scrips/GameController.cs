using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

       movementV2.OnEncountered += StartBattle;
      // battleSystem.OnBattleOver += EndBattle;

    }



    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            movementV2.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }




    public void StartBattle()
    {
        
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        
        var playerParty = movementV2.gameObject.GetComponent<MonsterParty>();
        var wildMonster = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildMonster();

       // var wildMonsterCopy = new Monster(wildMonster.Base, wildMonster.Level);

        battleSystem.StartBattle(playerParty, wildMonster);
        
    }

    void EndBattle()
    {

    }

}
