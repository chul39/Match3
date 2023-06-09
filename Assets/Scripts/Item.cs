using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    // Position
    [SerializeField] private Vector2Int indexPosition;
    private Board board;

    // Movement
    private Vector2 initialPosition;
    private Vector2 finalPosition;
    private bool isPressed;
    private float swipeAngle;
    private Item toBeSwappedItem;
    [SerializeField] private float moveSpeed = 5f;
    
    // Other props
    private Vector2Int previousPosition;
    [SerializeField] private ItemType type;
    [SerializeField] private bool isMatched = false;


    private void Update()
    {
        if (Vector2.Distance(transform.position, indexPosition) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, indexPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(indexPosition.x, indexPosition.y, 0f);
            board.SetItem(indexPosition, this);
        }
        
        if (isPressed && Input.GetMouseButtonUp(0))
        {
            if (board.GetCurrentState() != BoardState.isStandBy) return;
            finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPressed = false;
            board.SetCurrentState(BoardState.isUpdating);
            CalculateMovementAngle();
        }
    }

    public void SetupItem(Vector2Int indexPosition, Board board) 
    {
        this.indexPosition = indexPosition;
        this.board = board;
    }

    private void OnMouseDown() {
        if (board.GetCurrentState() != BoardState.isStandBy) return;
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
        previousPosition = indexPosition;
        int targetX = indexPosition.x;
        int targetY = indexPosition.y;
        if (swipeAngle < 45 && swipeAngle > -45) targetX += 1;
        if (swipeAngle > 135 || swipeAngle < -135) targetX -= 1;
        if (swipeAngle >= 45 && swipeAngle <= 135) targetY += 1;
        if (swipeAngle <= -45 && swipeAngle >= -135) targetY -= 1;

        Vector2Int newPosition = new Vector2Int(targetX, targetY);
        toBeSwappedItem = board.GetItem(newPosition);
        if (toBeSwappedItem == null) return;

        toBeSwappedItem.SetIndexPosition(this.indexPosition);
        board.SetItem(this.indexPosition, toBeSwappedItem);

        SetIndexPosition(newPosition);
        board.SetItem(newPosition, this);

        StartCoroutine(CheckMoveCoroutine());
    }

    public void SetIndexPosition(Vector2Int newPosition)
    {
        this.indexPosition = newPosition;
    }

    public Vector2Int GetIndexPosition()
    {
        return indexPosition;
    }

    public ItemType GetItemType()
    {
        return type;
    }

    public void SetIsMatched(bool isMatched)
    {
        this.isMatched = isMatched;
    }

    public bool GetIsMatched()
    {
        return isMatched;
    }

    private IEnumerator CheckMoveCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        board.CheckMatches();
        if (toBeSwappedItem == null) yield break;
        if (!isMatched && !toBeSwappedItem.GetIsMatched())
        {
            toBeSwappedItem.SetIndexPosition(indexPosition);
            board.SetItem(indexPosition, toBeSwappedItem);
            indexPosition = previousPosition;
            board.SetItem(previousPosition, this);
            board.SetCurrentState(BoardState.isStandBy);
        }
        else
        {
            board.RemoveMatches();
        }
    }

}
