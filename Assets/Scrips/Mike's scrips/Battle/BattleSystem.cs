using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public enum BattleState { Start, ActionSelection, AbillitySelection, RunningTurn, Busy, PartyScreen, BattleOver }

public enum BattleAction { Ability, SwitchMonster, Run }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

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
        playerUnit.Setup(playerParty.GetHealthyMonster());
        enemyUnit.Setup(wildMonster);

        //partyScreen.Init();

        dialogBox.SetAbilityNames(playerUnit.monster.Abilities);

        yield return dialogBox.TypeDialog($"An endangered {enemyUnit.monster.Base.Name} has spawned.");
        yield return new WaitForSeconds(1f);

        ActionSelection();
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        OnBattleOver(won);
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        
        state = BattleState.PartyScreen;
        //partyScreen.gameObject.SetActive(true);
        //partyScreen.SetPartyData(playerParty.Monsters);
        
    }

    void AbillitySelection()
    {
        state = BattleState.AbillitySelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableAbilitySelector(true);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;
        if (playerAction == BattleAction.Ability)
        {
            playerUnit.monster.CurrentAbility = playerUnit.monster.Abilities[currentAbility];
            enemyUnit.monster.CurrentAbility = enemyUnit.monster.GetRandomAbility();

            //check who goes first
            bool playerGoesFirst = playerUnit.monster.Speed >= enemyUnit.monster.Speed;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            //First turn
            yield return RunAbility(firstUnit, secondUnit, firstUnit.monster.CurrentAbility);
            if (state == BattleState.BattleOver) yield break;

            //Second turn
            yield return RunAbility(secondUnit, firstUnit, secondUnit.monster.CurrentAbility);
            if (state == BattleState.BattleOver) yield break;
        }
    }

    IEnumerator PlayerAbility()
    {
        state = BattleState.RunningTurn;  
        
        var ability = playerUnit.monster.Abilities[currentAbility];

        yield return RunAbility(playerUnit, enemyUnit, ability);
        //If battle state was not changed by RunAbility, then go to the next step
        if (state == BattleState.RunningTurn)
            StartCoroutine(EnemyAbility());

    }

    IEnumerator EnemyAbility()
    {
        state = BattleState.RunningTurn;

        var ability = enemyUnit.monster.GetRandomAbility();
       
        yield return RunAbility(enemyUnit, playerUnit, ability);
        //If battle state was not changed by RunAbility, then go to the next step
        if (state == BattleState.RunningTurn)
            ActionSelection();
        
    }

    IEnumerator RunAbility(BattleUnit sourceUnit, BattleUnit targetUnit, Ability ability)
    {
        ability.up--;
        yield return dialogBox.TypeDialog($"{sourceUnit.monster.Base.Name} used {ability.Base.Name}!");

        yield return new WaitForSeconds(1f);

        var damageDetails = targetUnit.monster.TakeDamage(ability, sourceUnit.monster);
        yield return targetUnit.Hud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.monster.Base.Name} got disintergrated!");


            CheckForBattleOver(targetUnit);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        
        if(faintedUnit.IsPlayerUnit)
        {
            var nextMonster = playerParty.GetHealthyMonster();
            if (nextMonster != null)
            {
                OpenPartyScreen();
            }
            else
            {
                // Lose
                BattleOver(false);
            }
        }

        else
        {
            BattleOver(true);
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

    void HandlePartySelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if(Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if(Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Monsters.Count - 1);

        //partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMonster = playerParty.Monsters[currentMember];
            if (selectedMonster.Hp <= 0)
            {
               // partyScreen.SetMessageText("You can't send out a fainted monster");
                return;
            }
            if (selectedMonster == playerUnit.monster)
            {
                // partyScreen.SetMessageText("You can't switch with the same monster");
                return;
            }

            // partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchMonster(selectedMonster));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            // partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    IEnumerator SwitchMonster(Monster newMonster)
    {
        state = BattleState.Busy;
        if (playerUnit.monster.Hp > 0)
        {
            yield return dialogBox.TypeDialog($"Get over here {playerUnit.monster.Base.Name}!");
            yield return new WaitForSeconds(2f);
        }
        playerUnit.Setup(newMonster);
        dialogBox.SetAbilityNames(playerUnit.monster.Abilities);
        yield return dialogBox.TypeDialog($"Get moving {playerUnit.monster.Base.Name}!");
        
        StartCoroutine(EnemyAbility());
    }
}
