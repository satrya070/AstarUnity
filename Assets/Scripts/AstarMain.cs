using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class AstarMain : MonoBehaviour
{
    Tilemap tilemap;
    (int, int) start_node;
    (int, int) end_node;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        tilemap.SetTileFlags(new Vector3Int(-9, 4, 0), TileFlags.None);
        Debug.Log(tilemap.GetTileFlags(new Vector3Int(-9, 4, 0)));
        tilemap.SetColor(new Vector3Int(-9, 4, 0), new Color(253f, 0.4f, 0.6f));

        StartCoroutine(pathRenderer());
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
            

            //Debug.Log(tile.);

            // try set color and couroutine etst
            //var tilo = tilemap.GetTile(tile)
            //Debug.Log();
            tilemap.SetTileFlags(tile_coords, TileFlags.None);
            tilemap.SetColor(tile_coords, new Color(253f, 0.4f, 0.6f));

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

    IEnumerator pathRenderer()
    {
        var nodes = new List<(int, int)>();
        nodes.Add((0, 1));
        nodes.Add((1, 2));
        nodes.Add((2, 3));

        foreach (var node in nodes)
        {
            Debug.Log(node);
            yield return new WaitForSeconds(5f);
        }
    }
}
