using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Monster> wildMonsters;

    public Monster GetRandomWildMonster()
    {
        var wildMonster = wildMonsters[Random.Range(0, wildMonsters.Count)];
        wildMonster.Init();
        return wildMonster;
    }
}
