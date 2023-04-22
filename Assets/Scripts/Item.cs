using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private Vector2Int position;
    private Board board;

    public void SetupItem(Vector2Int position, Board board) 
    {
        this.position = position;
        this.board = board;
    }

}
