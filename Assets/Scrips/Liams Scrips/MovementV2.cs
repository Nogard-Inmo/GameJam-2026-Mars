using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MovementV2 : MonoBehaviour
{
    public event Action OnEncountered;

    private Vector2 input;

    private Character character;

    public LayerMask grassLayer;

    private void Awake()
    {
        character = GetComponent<Character>();
    }


    // Update is called once per frame
    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // prevent diagonal movement
            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }

            character.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Interact();
            }
        }
    }
    private void OnMoveOver()
    {

        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, GameLayers.i.TriggerableLayers);
        foreach (var collider in colliders) 
        {
           var triggerable = collider.GetComponent<EncounterThingy>();
            if (triggerable != null)
            {
                OnEncountered();
                break;
            }
        }


    }

    void Interact()
    {
        var faceingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + faceingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interacteblels>()?.Interact();
        }
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                character.Animator.IsMoving = false;
                OnEncountered();
            }
        }
    }

    private void CheckIfInTrainersView()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer ) != null)
        {

        }

    }
}
