using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementV2 : MonoBehaviour
{
    public float speed;


    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private bool isMoving;
    private Vector2 input;


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
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                {

                    StartCoroutine(Move(targetPos));

                }

                CheackForEncounters();
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
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheackForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, grassLayer) != null)
        {
            if (Random.Range(1, 101) <= 10)
            {
                Debug.Log("Encountered a wild monster!");
            }
        }
    }

}
