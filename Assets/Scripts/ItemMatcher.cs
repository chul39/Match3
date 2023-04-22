using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemMatcher : MonoBehaviour
{
    private Board board;

    public List<Item> currentMatches = new List<Item>();

    private void Awake() {
        board = FindObjectOfType<Board>();
    }

    public void CheckMatches()
    {
        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                Item currentItem = board.GetItemAtPosition(x, y);
                if (currentItem == null) continue;

                // Check horizontal
                if (x > 0 && x < board.GetWidth() - 1) CheckMatchesHorizontal(currentItem);

                // Check vertical
                if (y > 0 && y < board.GetHeight() - 1) CheckMatchesVertical(currentItem);
            }
        }
        if (currentMatches.Count > 0) currentMatches = currentMatches.Distinct().ToList();
    }

    private void CheckMatchesHorizontal(Item item)
    {
        Vector2Int itemPosition = item.GetIndexPosition();
        Item leftItem = board.GetItemAtPosition(itemPosition.x - 1, itemPosition.y);
        Item rightItem = board.GetItemAtPosition(itemPosition.x + 1, itemPosition.y);
        if (leftItem == null && rightItem == null) return;
        if (CheckIsSameType(item, leftItem) && CheckIsSameType(item, rightItem))
        {
            AddToCurrentMatches(item);
            AddToCurrentMatches(leftItem);
            AddToCurrentMatches(rightItem);
        }
    }

    private void CheckMatchesVertical(Item item)
    {
        Vector2Int itemPosition = item.GetIndexPosition();
        Item aboveItem = board.GetItemAtPosition(itemPosition.x, itemPosition.y - 1);
        Item belowItem = board.GetItemAtPosition(itemPosition.x, itemPosition.y + 1);
        if (aboveItem == null && belowItem == null) return;
        if (CheckIsSameType(item, aboveItem) && CheckIsSameType(item, belowItem))
        {
            AddToCurrentMatches(item);
            AddToCurrentMatches(aboveItem);
            AddToCurrentMatches(belowItem);
        }
    }

    private bool CheckIsSameType(Item a, Item b)
    {
        return a.GetItemType() == b.GetItemType();
    }

    private void AddToCurrentMatches(Item item)
    {
        item.SetIsMatched(true);
        currentMatches.Add(item);
    }

}
