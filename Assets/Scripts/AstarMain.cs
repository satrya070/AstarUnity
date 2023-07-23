using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarMain : MonoBehaviour
{
    Tilemap tilemap;
    (int, int) start_node;
    (int, int) end_node;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseClick();
        startAstar();
    }

    void mouseClick()
    {
        Vector3 click_position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
            Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

            start_node = (tile_coords.x, tile_coords.y);

            //Debug.Log(tile_coords);

            var tile = tilemap.GetTile(tile_coords);
            Debug.Log($"start_node is: {start_node}");

            Debug.Log(tile.name);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // use the right click to mark the end point
            Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
            Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

            end_node = (tile_coords.x, tile_coords.y);

            Debug.Log($"end_node is: {end_node}");
        }
    }

    void startAstar()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Starting Astar!");
        }
    }
}
