using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public partial class AstarMain : MonoBehaviour
{
    static Tilemap tilemap;
    static Node start_node;
    static Node end_node;

    static List<Node> OpenPriorityQueue = new List<Node>();
    static Dictionary<(int, int)?, Node> closed = new Dictionary<(int, int)?, Node>();
    [SerializeField] Tile wallTile;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

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
        Vector3 world_position = Camera.main.ScreenToWorldPoint(click_position);
        Vector3Int tile_coords = Vector3Int.RoundToInt(tilemap.WorldToCell(world_position));

        // use the left click to mark the starting point
        if (Input.GetMouseButtonDown(0))
        {

            start_node = new Node((tile_coords.x, tile_coords.y), null, 0);

            Debug.Log($"start_node is: {start_node.position}");
            
            tilemap.SetTileFlags(tile_coords, TileFlags.None);
            tilemap.SetColor(tile_coords, new Color(0.9f, 0.71f, 0.2f));

        }

        // use the right click to mark the end point
        if (Input.GetMouseButtonDown(2))
        {
            end_node = new Node((tile_coords.x, tile_coords.y));
            Debug.Log(end_node == null);

            Debug.Log($"end_node is: {end_node.position}");

            tilemap.SetTileFlags(tile_coords, TileFlags.None);
            tilemap.SetColor(tile_coords, new Color(0.9f, 0.71f, 0.2f));
        }

        // mark walls
        if (Input.GetMouseButtonDown(1))
        {
            var tile = tilemap.GetTile(tile_coords);
            tilemap.SetTile(tile_coords, wallTile);
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
                var sorted_list = OpenPriorityQueue.OrderBy(node => node.f_value).ToList();
                Node popped_node = sorted_list.First();

                // remove from open list prioqueue
                OpenPriorityQueue.Remove(popped_node);

                Debug.Log($"Expanding node: {popped_node.position} {popped_node.f_value}");
                found_goal = expand_node(popped_node);

                // push to closed list
                closed[popped_node.position] = popped_node;

                if (found_goal is true)
                {
                    // register end node in closed_list to perform backtrack
                    closed[end_node.position] = new Node(end_node.position, popped_node.position, popped_node.g_value + 1);
                    closed[end_node.position].f_value = popped_node.g_value + 1;
                    break;
                }
            }

            if (found_goal)
            {
                drawShortestPath();
            } else
            {
                Debug.Log("No path possible path exists!");
            }
        }
    }

    void drawShortestPath()
    {
        // recolor start and end node
        tilemap.SetTileFlags(new Vector3Int(end_node.position.Item1, end_node.position.Item2, 0), TileFlags.None);
        tilemap.SetColor(new Vector3Int(end_node.position.Item1, end_node.position.Item2, 0), new Color(0.9f, 0.71f, 0.2f));

        tilemap.SetTileFlags(new Vector3Int(start_node.position.Item1, start_node.position.Item2, 0), TileFlags.None);
        tilemap.SetColor(new Vector3Int(start_node.position.Item1, start_node.position.Item2, 0), new Color(0.9f, 0.71f, 0.2f));

        var backtrack_list = new List<Node>();

        Node backtrack = closed[end_node.position];
        while (backtrack.previous_node is not null)
        {
            backtrack = closed[backtrack.previous_node];
            backtrack_list.Add(backtrack);
        }

        // draws the backtrack path
        backtrack_list.Reverse();
        StartCoroutine(DrawTracks(backtrack_list));

        Debug.Log($"DONE");
    }

    IEnumerator DrawTracks(List<Node> nodelist)
    {
        foreach (Node node in nodelist.Skip(1))
        {
            tilemap.SetColor(new Vector3Int(node.position.Item1, node.position.Item2, 0), new Color(0.85f, 0.15f, 0.15f));
            Debug.Log(node.position);
            yield return new WaitForSeconds(0.25f);
        }
    }

    static bool expand_node(Node expanding_node)
    {
        var neighbors = get_neighbors(expanding_node.position);

        var expanding_tilemap_position = new Vector3Int(expanding_node.position.Item1, expanding_node.position.Item2, 0);
        tilemap.SetTileFlags(expanding_tilemap_position, TileFlags.None);
        tilemap.SetColor(expanding_tilemap_position, new Color(0.88f, 0.75f, 0.90f));

        Console.WriteLine($"gathering all neighbors for node: {expanding_node.position}");

        foreach ((int, int) neighbor_position in neighbors)
        {
            if (neighbor_position == end_node.position)
            {
                Debug.Log($"Goal found at node: {expanding_node.position}, with distance: {expanding_node.f_value}!");
                return true;
            }

            // check if neighbor position outside tilemap bounds
            if (!tilemap.HasTile(new Vector3Int(neighbor_position.Item1, neighbor_position.Item2, 0))) { continue; }

            // if node position is a wall block skip
            TileBase tile = tilemap.GetTile(new Vector3Int(neighbor_position.Item1, neighbor_position.Item2, 0));
            if (tile is not null && tile.name == "wall") { continue; }

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
}
