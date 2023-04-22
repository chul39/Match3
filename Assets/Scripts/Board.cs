using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private Item[] items;
    private Item[,] inGameItems;

    private void Start()
    {
        inGameItems = new Item[width, height];
        Setup();
    }   
    
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(
                    tilePrefab, 
                    new Vector3(x, y, 0f), 
                    Quaternion.identity
                );
                tile.transform.parent = transform;
                tile.name = $"Tile ({x},{y})";
                SpawnItem(new Vector2Int(x, y));
            }
        }
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, (float)(height - 1) / 2f, -10f);
    }

    private void SpawnItem(Vector2Int position)
    {
        int targetIndex = Random.Range(0, items.Length);
        Item item = Instantiate(
            items[targetIndex], 
            new Vector3(position.x, position.y, 0f), 
            Quaternion.identity
        );
        item.transform.parent = this.transform;
        item.name = $"Item ({position.x},{position.y})";
        item.SetupItem(position, this);
        inGameItems[position.x, position.y] = item;
    }

    public void SetItemAtPosition(Vector2Int targetPosition, Item item)
    {
        inGameItems[targetPosition.x, targetPosition.y] = item;
    }

    public Item GetItemAtPosition(Vector2Int targetPosition)
    {
        if (targetPosition.x < 0 || targetPosition.x >= width) return null;
        if (targetPosition.y < 0 || targetPosition.y >= height) return null;
        return inGameItems[targetPosition.x , targetPosition.y];
    }

}