using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartyScreen: MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;

    public void Init()
        {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Monster> monsters)
    {
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < monsters.Count)
                memberSlots[i].SetData(monsters[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }


    }
}
