using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections;
using System.Collections.Generic;

public enum GameState { FreeRoam, Battle, Dialog }

public class GameController : MonoBehaviour
{
    [SerializeField] MovementV2 movementV2;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;


    private void Start()
    {

       movementV2.OnEncountered += StartBattle;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
      //battleSystem.OnBattleOver += EndBattle;
=======
=======
>>>>>>> Stashed changes
       battleSystem.OnBattleOver += EndBattle;

        DialogManager.Instance.OnShowDialog += () => {
            state = GameState.Dialog;
        };  
        DialogManager.Instance.OnCloseDialog += () => {
            if(state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

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
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
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

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

}
