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

        yield return dialogBox.TypeDialog($"An endangered {playerUnit.monster.Base.Name} has spawned.");
        
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

}