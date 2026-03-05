using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterParty : MonoBehaviour
{
    [SerializeField] List<Monster> monsters;
    public List<Monster> Monsters { get { return monsters; } }
    public void AddMonster(Monster monster) 
    {
        if (monsters.Count >= 6)
        {
            Debug.Log("Party is full");
            return;
        }
        monsters.Add(monster);
    }
}