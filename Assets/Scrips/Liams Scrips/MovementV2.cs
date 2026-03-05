using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementV2 : MonoBehaviour
{
    public float speed;

    private bool isMoving;
    private Vector2 input;

    private CharacterAnimator animator;
    private Character character;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
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
                animator.MoveX = input.x;
                animator.MoveY = input.y;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                {

                    StartCoroutine(Move(targetPos));

                }

                OnMoveOver();
            }

            animator.IsMoving = isMoving;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Interact();
            }
        }
    }

    void Interact()
    {
        var faceingDir = new Vector3(animator.MoveX, animator.MoveY);
        var interactPos = transform.position + faceingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<NpcController>();
        }
    }

    private void OnMoveOver() 
    {

        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, GameLayers.i.TriggerableLayers);

        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                
                triggerable.OnPLayerTriggered(this);
                break;
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos -transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

}
