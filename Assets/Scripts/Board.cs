using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject tilePrefab;

    private void Start()
    {
        this.Init();
    }   
    
    private void Init()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x,y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.transform.parent = transform;
                tile.name = $"Tile ({x},{y})";
            }
        }
        transform.position = new Vector2(-(float)(width - 1) / 2f, -(float)(height - 1) / 2f);
    }

}