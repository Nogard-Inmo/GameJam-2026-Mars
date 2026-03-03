using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;

    private void Start()
    {
        SetupBattle();
    }
    void SetupBattle()
    {
        playerUnit.Setup();
        playerHud.SetData(playerUnit.monster);
    }

}