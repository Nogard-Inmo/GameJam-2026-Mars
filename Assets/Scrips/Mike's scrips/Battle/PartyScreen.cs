using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUi[] memberSlots;
    List<Monster> monsters;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUi>();
    }

    public void SetPartyData(List<Monster> monsters)
    {
        this.monsters = monsters;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < monsters.Count)
                memberSlots[i].SetData(monsters[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Pick your monster to send out on the battlefield!";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0;i < monsters.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
