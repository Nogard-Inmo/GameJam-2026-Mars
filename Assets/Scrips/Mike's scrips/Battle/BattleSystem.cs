using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public enum BattleState { Start, ActionSelection, AbillitySelection, PerformAbility, Busy, PartyScreen }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    //[SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentAbility;
    int currentMember;

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
        //playerUnit.Setup(playerParty.GetHealthyMonster());
        enemyUnit.Setup(wildMonster);
        playerHud.SetData(playerUnit.monster);
        enemyHud.SetData(enemyUnit.monster);

        //partyScreen.Init();

        dialogBox.SetAbilityNames(playerUnit.monster.Abilities);

        yield return dialogBox.TypeDialog($"An endangered {enemyUnit.monster.Base.Name} has spawned.");
        yield return new WaitForSeconds(1f);

        ActionSelection();
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        /*
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Monsters);
        partyScreen.gameObject.SetActive(true);
        */
    }

    void AbillitySelection()
    {
        state = BattleState.AbillitySelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableAbilitySelector(true);
    }

    IEnumerator PlayerAbility()
    {
        state = BattleState.PerformAbility;  
        
        var ability = playerUnit.monster.Abilities[currentAbility];
        ability.up--;
        yield return dialogBox.TypeDialog($"{playerUnit.monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = enemyUnit.monster.TakeDamage(ability, playerUnit.monster);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.monster.Base.Name} is unable to fight");
        }
        else
        {
            StartCoroutine(EnemyAbility());
        }

        
    }

    IEnumerator EnemyAbility()
    {
        state = BattleState.PerformAbility;

        var ability = enemyUnit.monster.GetRandomAbility();
        ability.up--;
        yield return dialogBox.TypeDialog($"{enemyUnit.monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = playerUnit.monster.TakeDamage(ability, playerUnit.monster);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.monster.Base.Name} is unable to fight");
        }
        else
        {
            ActionSelection();
        }
    }

    IEnumerator RunAbility(BattleUnit sourceUnit, BattleUnit targetUnit, Ability ability)
    {
        ability.up--;
        yield return dialogBox.TypeDialog($"{sourceUnit.monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = targetUnit.monster.TakeDamage(ability, sourceUnit.monster);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.monster.Base.Name} is unable to fight");


        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        /*
        if(faintedUnit.IsPlayerUnit)
        {
            // Open party screen
            OpenPartyScreen();
        }
        else
        {
            // Win battle
            OnBattleOver?.Invoke(true);
        }
        */
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
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelector();
        }
        else if (state == BattleState.AbillitySelection)
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
                AbillitySelection();
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
            StartCoroutine(PlayerAbility());
        }
    }
}
