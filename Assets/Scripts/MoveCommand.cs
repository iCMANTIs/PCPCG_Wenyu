using UnityEngine;

public class MoveCommand : ICommand
{
    private GameObject objectToMove;  
    private Vector2 newPosition;
    private Vector2 oldPosition;

    public MoveCommand(GameObject moveObject, Vector2 newPos)
    {
        objectToMove = moveObject;
        newPosition = newPos;
        oldPosition = moveObject.transform.position;
    }

    public void Execute()
    {
        
        PlayerController playerController = objectToMove.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.StartCoroutine(playerController.SmoothMove(newPosition));
        }

        
        Box box = objectToMove.GetComponent<Box>();
        if (box != null)
        {
            box.StartCoroutine(box.SmoothMove(newPosition));
        }
    }

    public void Undo()
    {
        
        PlayerController playerController = objectToMove.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.StartCoroutine(playerController.SmoothMove(oldPosition));
        }

        
        Box box = objectToMove.GetComponent<Box>();
        if (box != null)
        {
            box.StartCoroutine(box.SmoothMove(oldPosition));
        }
    }

}
