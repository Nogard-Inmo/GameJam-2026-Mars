using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

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
    int currentAbility;

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

    IEnumerator PerformPlayerAbility()
    {
        var ability = playerUnit.monster.Abilities[currentAbility];
        yield return dialogBox.TypeDialog($"{playerUnit.monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.monster.TakeDamage(ability.playerUnit.monster);
    }
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelector();
        }
        else if (state == BattleState.PlayerAbility)
        {
            HandleAbilitySelection();
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
    void HandleAbilitySelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < playerUnit.monster.Abilities.Count - 1)
                ++currentAction;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }
    }

    void HandleAbilitySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAbility < playerUnit.monster.Abilities.Count - 1)
                ++currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAbility > 0)
                --currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAbility < playerUnit.monster.Abilities.Count - 2)
                currentAbility += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAbility > 1)
                currentAbility -= 2;
        }

        dialogBox.UpdateAbilitySelection(currentAbility, playerUnit.monster.Abilities[currentAbility]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableAbilitySelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAbility());
        }
    }
}
