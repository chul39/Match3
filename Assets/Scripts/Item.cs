using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] private Vector2Int indexPosition;
    private Board board;

    private Vector2 initialPosition;
    private Vector2 finalPosition;
    private bool isPressed;
    private float swipeAngle;
    private Item toBeSwappedItem;
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        // Lerp item to target position
        if (Vector2.Distance(transform.position, indexPosition) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, indexPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(indexPosition.x, indexPosition.y, 0f);
            board.SetItemAtPosition(indexPosition, this);
        }
        
        // Move when mouse is up
        if (isPressed && Input.GetMouseButtonUp(0))
        {
            finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPressed = false;
            CalculateMovementAngle();
        }
    }

    public void SetupItem(Vector2Int indexPosition, Board board) 
    {
        this.indexPosition = indexPosition;
        this.board = board;
    }

    private void OnMouseDown() {
        initialPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isPressed = true;
    }

    private void CalculateMovementAngle()
    {
        swipeAngle = Mathf.Atan2(
            finalPosition.y - initialPosition.y,
            finalPosition.x - initialPosition.x
        ) * 180 / Mathf.PI;
        if (Vector3.Distance(initialPosition, finalPosition) > .5f) MoveItem();
    }

    private void MoveItem()
    {
        int targetX = indexPosition.x;
        int targetY = indexPosition.y;
        if (swipeAngle < 45 && swipeAngle > -45) targetX += 1;
        if (swipeAngle > 135 || swipeAngle < -135) targetX -= 1;
        if (swipeAngle >= 45 && swipeAngle <= 135) targetY += 1;
        if (swipeAngle <= -45 && swipeAngle >= -135) targetY -= 1;

        Vector2Int newPosition = new Vector2Int(targetX, targetY);
        toBeSwappedItem = board.GetItemAtPosition(newPosition);
        if (toBeSwappedItem == null) return;

        toBeSwappedItem.SetIndexPosition(this.indexPosition);
        board.SetItemAtPosition(this.indexPosition, toBeSwappedItem);

        this.SetIndexPosition(newPosition);
        board.SetItemAtPosition(newPosition, this);
    }

    public void SetIndexPosition(Vector2Int newPosition)
    {
        this.indexPosition = newPosition;
    }

    public Vector2Int GetIndexPosition()
    {
        return indexPosition;
    }

}
