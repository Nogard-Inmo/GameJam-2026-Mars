using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public enum BattleState { Start, PlayerAction, PlayerAbility, EnemyAbility, Busy, PartyScreen }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentAbility;
    int currentPartyMember;

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

        partyScreen.Init();

        dialogBox.SetAbilityNames(playerUnit.Monster.Abilities);

        yield return dialogBox.TypeDialog($"An endangered {enemyUnit.Monster.Base.Name} has spawned.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Monsters);
        partyScreen.gameObject.SetActive(true);
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

        yield return new WaitForSeconds(1f);

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

        yield return new WaitForSeconds(1f);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.Name} got disintergrated and is unable to fight");
            yield return new WaitForSeconds(2f);

            var nextMonster = playerParty.GetHealthyMonster();
            if (nextMonster != null)
            {
                OpenPartyScreen();
            }
            else
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
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
                ++currentAction;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
                --currentAction;
        }

        currentAction = Mathf.Clamp(currentAction, 0,1);

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
                // Open the PartyMenu
                OpenPartyScreen();

            }
        }
    }

    void HandleAbilitySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
                ++currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAbility;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAbility += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAbility -= 2;
        }

        currentAbility = Mathf.Clamp(currentAbility, 0,playerUnit.Monster.Abilities.Count -1);

        dialogBox.UpdateAbilitySelection(currentAbility, playerUnit.Monster.Abilities[currentAbility]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableAbilitySelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAbility());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableAbilitySelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentPartyMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentPartyMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentPartyMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentPartyMember -= 2;
        }

        currentPartyMember = Mathf.Clamp(currentPartyMember, 0, playerParty.Monsters.Count - 1);

        partyScreen.UpdateMemberSelection(currentPartyMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Monsters[currentPartyMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't ducking send out a disintergrated monster!!");
                return;
            }
            if (selectedMember == playerUnit.Monster)
            {
                partyScreen.SetMessageText("You can't send out a monster that is already on the battlefield!");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchMonster(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }
    }

    IEnumerator SwitchMonster(Monster newMonster)
    {
        if (playerUnit.Monster.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Get back here {playerUnit.Monster.Base.Name}");
            yield return new WaitForSeconds(0.75f);
        }
        playerUnit.Setup(newMonster);
        playerHud.SetData(newMonster);
        dialogBox.SetAbilityNames(newMonster.Abilities);
        yield return dialogBox.TypeDialog($"Get out on the battlefield {newMonster.Base.Name}!");

        StartCoroutine(EnemyAbility());
    }
  
}
