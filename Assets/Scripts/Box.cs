using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Color finishColor;
    Color originColor;
    Vector2 targetPosition;
    public float moveSpeed = 5f;  
    bool isMoving = false;

    private void Start()
    {
        originColor = GetComponent<SpriteRenderer>().color;
        FindObjectOfType<GameManager>().totalBoxs++;
        targetPosition = transform.position;
    }

    public bool CanMoveToDir(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f);

        if (!hit)
        {
            Vector2 newPos = (Vector2)transform.position + dir;
            var moveCommand = new MoveCommand(gameObject, newPos);
            GameManager.instance.ExecuteCommand(moveCommand);
            StartCoroutine(SmoothMove(newPos));
            return true;
        }

        return false;
    }

    public  IEnumerator SmoothMove(Vector2 newPos)
    {
        isMoving = true;
        targetPosition = newPos;
        while ((Vector2)transform.position != newPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            FindObjectOfType<GameManager>().finishedBoxs++;
            FindObjectOfType<GameManager>().CheckFinish();
            GetComponent<SpriteRenderer>().color = finishColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            FindObjectOfType<GameManager>().finishedBoxs--;
            GetComponent<SpriteRenderer>().color = originColor;
        }
    }
}
