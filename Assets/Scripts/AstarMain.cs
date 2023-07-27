using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class AstarMain : MonoBehaviour
{
    Tilemap tilemap;
    static Node start_node;
    static Node end_node;

    static List<Node> OpenPriorityQueue = new List<Node>();
    static Dictionary<(int, int), Node> closed = new Dictionary<(int, int), Node>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        tilemap.SetTileFlags(new Vector3Int(-9, 4, 0), TileFlags.None);
        tilemap.SetColor(new Vector3Int(-9, 4, 0), new Color(253f, 0.4f, 0.6f));

        // StartCoroutine(pathRenderer());

        Debug.Log(start_node);
       
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

        // use the left click to mark the starting point
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
            Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

            start_node = new Node((tile_coords.x, tile_coords.y), null, 0);

            //Debug.Log(tile_coords);

            var tile = tilemap.GetTile(tile_coords);

            Debug.Log($"start_node is: {start_node.position}");
            

            //Debug.Log(tile.);

            // try set color and couroutine etst
            //var tilo = tilemap.GetTile(tile)
            //Debug.Log();
            tilemap.SetTileFlags(tile_coords, TileFlags.None);
            tilemap.SetColor(tile_coords, new Color(253f, 0.4f, 0.6f));

        }

        // use the right click to mark the end point
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
            Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

            end_node = new Node((tile_coords.x, tile_coords.y));
            Debug.Log(end_node == null);

            Debug.Log($"end_node is: {end_node.position}");
        }
    }

    void startAstar()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (end_node is null || start_node is null || start_node.position == end_node.position)
            {
                Debug.Log("Properly set (distinct) start and end node!");
                return;
            }

            Debug.Log($"starting Astar from node{start_node.position} to {end_node.position}!");
            // first node in open queue is start node
            OpenPriorityQueue.Add(start_node);
            bool found_goal = false;

            //for (int i = 0; i < 15; i++)
            while (found_goal is false && OpenPriorityQueue.Count > 0)
            {
                // imitate priority queue behavior (pop node with lowest dist)
                var sorted_list = OpenPriorityQueue.OrderBy(node => node.g_value).ToList();
                Node popped_node = sorted_list.First();

                // remove from open list prioqueue
                OpenPriorityQueue.Remove(popped_node);

                Debug.Log($"Expanding node: {popped_node.position}");
                found_goal = expand_node(popped_node);
            }
        }
        
    }

    static bool expand_node(Node expanding_node)
    {
        var neighbors = get_neighbors(expanding_node.position);

        Console.WriteLine($"gathering all neighbors for node: {expanding_node.position}");

        foreach ((int, int) neighbor_position in neighbors)
        {
            if (neighbor_position == end_node.position)
            {
                Debug.Log($"Goal found at node: {expanding_node.position}, with distance: {expanding_node.f_value}!");
                return true;
            }

            // if node in closed, already expanded so skip
            if (closed.ContainsKey(neighbor_position)) { continue; }

            double neighbor_g_value = expanding_node.g_value + 1;
            double neighbor_f_value = neighbor_g_value + heuristic(neighbor_position, end_node.position);
            var neighbor = new Node(neighbor_position, expanding_node.position, neighbor_g_value);
            neighbor.f_value = neighbor_f_value;

            // if node in open and smaller: update, else continue this iteration
            Node? neighbor_in_open = OpenPriorityQueue.Find(node => node.position == neighbor_position);
            if (neighbor_in_open != null)
            {
                if (neighbor_in_open.f_value > neighbor.f_value)
                {
                    // update node in open, with smaller one and continue
                    // Console.WriteLine("smaller open found!");
                    neighbor_in_open.g_value = neighbor.g_value;
                    neighbor_in_open.f_value = neighbor.f_value;
                    neighbor_in_open.previous_node = expanding_node.position;

                    continue;
                }
                else
                {
                    // there's a node in open list that already with a smaller distance
                    continue;
                }
            }

            OpenPriorityQueue.Add(neighbor);
        }

        return false;
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
