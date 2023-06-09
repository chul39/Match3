using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemMatcher : MonoBehaviour
{
    private Board board;

    private List<Item> currentMatches = new List<Item>();

    private void Awake() {
        board = FindObjectOfType<Board>();
    }

    public void CheckMatches()
    {
        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                Item currentItem = board.GetItem(x, y);
                if (currentItem == null) continue;

                // Check horizontal
                if (x > 0 && x < board.GetWidth() - 1) CheckMatchesHorizontal(currentItem);

                // Check vertical
                if (y > 0 && y < board.GetHeight() - 1) CheckMatchesVertical(currentItem);
            }
        }
        if (currentMatches.Count > 0) {
            currentMatches.RemoveAll(item => item == null);
            currentMatches = currentMatches.Distinct().ToList();
        }
    }

    private void CheckMatchesHorizontal(Item item)
    {
        Vector2Int itemPosition = item.GetIndexPosition();
        Item leftItem = board.GetItem(itemPosition.x - 1, itemPosition.y);
        Item rightItem = board.GetItem(itemPosition.x + 1, itemPosition.y);
        if (leftItem == null || rightItem == null) return;
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
        Item belowItem = board.GetItem(itemPosition.x, itemPosition.y - 1);
        Item aboveItem = board.GetItem(itemPosition.x, itemPosition.y + 1);
        if (aboveItem == null || belowItem == null) return;
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

    public void UpdateCurrentMatchesPostRemoval()
    {
        currentMatches.RemoveAll(item => item == null);
    }

    public List<Item> GetCurrentMatches()
    {
        return currentMatches;
    }

}
