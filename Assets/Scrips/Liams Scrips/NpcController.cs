using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

public class NpcController : MonoBehaviour, Interacteblels
{

    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NpcState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact()
    {
        if (state == NpcState.Idle)
        {
            state = NpcState.Dialog;
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                idleTimer = 0f;
                state = NpcState.Idle;
            }));
        }

    }

    private void Update()
    {

        if (state == NpcState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0) 
                    StartCoroutine(Walk());
            }

            character.HandleUpdate();
        }
    }

    IEnumerator Walk()
    {
        state = NpcState.Walking;

        yield return character.Move(movementPattern[currentPattern]);
        print("hello I'm working here");
        currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NpcState.Idle;
    }
}
public enum NpcState { Idle, Walking, Dialog }
