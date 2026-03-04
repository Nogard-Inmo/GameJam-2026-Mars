using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BattleState { Start, PlayerAction, PlayerAbility, EnemyAbility  , Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.monster);
        enemyHud.SetData(enemyUnit.monster);

        dialogBox.SetAbilityNames(playerUnit.monster.Abilities);

        yield return dialogBox.TypeDialog($"An endangered {enemyUnit.monster.Base.Name} has spawned.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerAbility() 
    { 
        state = BattleState.PlayerAbility;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false); 
        dialogBox.EnableAbilitySelector(true);
    }
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelector();
        }
    }   

    void HandleActionSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                ++currentAction;
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                // Fight
                PlayerAbility();
            }
            else if (currentAction == 1)
            {
                // Run
                dialogBox.EnableActionSelector(false);
                
            }
        }
    }
}
