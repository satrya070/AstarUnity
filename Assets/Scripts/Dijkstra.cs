using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Dijkstra : MonoBehaviour
{
    //PriorityQueue<Tuple<int, int>, int> queue;
    Tilemap tilemap;
    (int, int) start_node = (-9, -5);
    (int, int) end_node = (8, 4);
    List<((int, int), int)> open = new List<((int, int), int)>();
    List<((int, int), int)> closed = new List<((int, int), int)>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        runAlgo();
    }

    // Update is called once per frame
    void Update()
    {
        mouseClick();
    }

    void mouseClick()
    {
        Vector3 click_position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
            Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

            //Debug.Log(tilemap.WorldToCell(world_position));
            Debug.Log(tile_coords);

            var tile = tilemap.GetTile(tile_coords);

            Debug.Log(tile.name);
        }
    }

    void runAlgo()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked here!");
    }

    int getDistance((int, int) base_node, (int, int) neighbor_node)
    {
        // as the heuristic function use the euclidian distance
        return 0;
    }

}
