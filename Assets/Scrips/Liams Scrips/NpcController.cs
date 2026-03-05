using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NpcController : MonoBehaviour, Interacteblels
{

    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;

    NpcState state;
    float idleTimer = 0f;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

    private void Update()
    {
        if (state == NpcState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > 2f)
            {
                idleTimer = 0f;
                StartCoroutine(character.Move(new Vector2(2, 0)));
            }

            character.HandleUpdate();
        }
    }
}
public enum NpcState { Idle, Walking }
