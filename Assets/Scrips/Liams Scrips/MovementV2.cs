using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;


public class MovementV2 : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private bool isMoving;
    private Vector2 input;

    private CharacterAnimator animator;

    public event Action OnEncountered;

    [SerializeField] Transform player;

    
    private void Start()
    {
        animator = GetComponent<CharacterAnimator>();
        player.position = new Vector2(2,2);
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;      

             

            if (input != Vector2.zero)
            {
                animator.MoveX = input.x;
                animator.MoveY = input.y;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWalkable(targetPos)) 
                    StartCoroutine(Move(targetPos));


            }
        }

        animator.IsMoving = isMoving;

        //if (Input.GetKeyDown(KeyCode.T))
            

        /*if (Input.GetKeyDown(KeyCode.Z))
            Interact();*/
    }

    /*void Interact()
    {
        var facingDir = new Vector3(animator.MoveX, animator MoveY");
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }*/
    IEnumerator Move(Vector3 targetPos) 
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.IsMoving = false;
                OnEncountered();
                //SceneManager.LoadScene(2);
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
       if( Physics2D.OverlapCircle(targetPos, 0.3f, GameLayers.i.SolidLayer) != null)
        {
            return false;
        }
       return true;
    }
}




   