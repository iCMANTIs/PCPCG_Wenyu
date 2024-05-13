using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 targetPosition;
    public float moveSpeed = 3f;
    public LayerMask detectLayer;
    bool isMoving = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = false; 

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 moveDir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir = Vector2.right;
            if (!facingRight) Flip();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir = Vector2.left;
            if (facingRight) Flip();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector2.down;

        if (moveDir != Vector2.zero && !isMoving)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y) + moveDir;
            if (CanMoveToDir(newPos))
            {
                var moveCommand = new MoveCommand(gameObject, newPos);
                GameManager.instance.ExecuteCommand(moveCommand);
                StartCoroutine(SmoothMove(newPos));
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        animator.SetBool("IsRunning", isMoving);
    }

    public IEnumerator SmoothMove(Vector2 newPos)
    {
        isMoving = true;
        targetPosition = newPos;
        while ((Vector2)transform.position != newPos)
        {
            yield return null;
        }
        isMoving = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    bool CanMoveToDir(Vector2 newPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, newPos - (Vector2)transform.position, 1f, detectLayer);
        if (!hit)
            return true;
        else
        {
            if (hit.collider.GetComponent<Box>() != null)
                return hit.collider.GetComponent<Box>().CanMoveToDir(newPos - (Vector2)transform.position);
        }
        return false;
    }
}
