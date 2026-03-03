using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox DialogBox;

    private void Start()
    {
        SetupBattle();
    }
    void SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.monster);
        enemyHud.SetData(enemyUnit.monster);

        StartCoroutine(DialogBox.TypeDialog($"A wild {playerUnit.monster.Base.Name} appeared."));
    }

}