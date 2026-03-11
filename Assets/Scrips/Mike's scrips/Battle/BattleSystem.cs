using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public enum BattleState { Start, PlayerAction, PlayerAbility, EnemyAbility, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentAbility;

    MonsterParty playerParty;
    Monster wildMonster;

    public void StartBattle(MonsterParty playerParty, Monster wildMonster)
    {
        this.playerParty = playerParty;
        this.wildMonster = wildMonster;

        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {

        playerUnit.Setup(playerParty.GetHealthyMonster());
        enemyUnit.Setup(wildMonster);
        playerHud.SetData(playerUnit.Monster);
        enemyHud.SetData(enemyUnit.Monster);

        dialogBox.SetAbilityNames(playerUnit.Monster.Abilities);

        yield return dialogBox.TypeDialog($"An endangered {enemyUnit.Monster.Base.Name} has spawned.");
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
        state = BattleState.Busy;       

        var ability = playerUnit.Monster.Abilities[currentAbility];
        ability.up--;
        yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = enemyUnit.Monster.TakeDamage(ability, playerUnit.Monster);
        yield return enemyHud.UpdateHP();
        Debug.Log(damageDetails.Fainted);
        
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Monster.Base.Name} disintergrated and is unable to fight");
            
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyAbility());
        }

        
    }

    IEnumerator EnemyAbility()
    {
        state = BattleState.EnemyAbility;
        var ability = enemyUnit.Monster.GetRandomAbility();
        ability.up--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = playerUnit.Monster.TakeDamage(ability, playerUnit.Monster);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.Name} is unable to fight");
            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A Brutal hit!");
        if (damageDetails.Type > 1f)
            yield return dialogBox.TypeDialog("It's super effective!");
        else if (damageDetails.Type < 1f)
            yield return dialogBox.TypeDialog("It's not very effective...");
    }   

    public void HandleUpdate()
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
            if (currentAction < playerUnit.Monster.Abilities.Count - 1)
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
            if (currentAbility < playerUnit.Monster.Abilities.Count - 1)
                ++currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAbility > 0)
                --currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAbility < playerUnit.Monster.Abilities.Count - 2)
                currentAbility += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAbility > 1)
                currentAbility -= 2;
        }

        dialogBox.UpdateAbilitySelection(currentAbility, playerUnit.Monster.Abilities[currentAbility]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableAbilitySelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAbility());
        }
    }
}
