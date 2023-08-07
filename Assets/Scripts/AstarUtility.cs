using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class AstarMain : MonoBehaviour
{
    public static double heuristic((int, int) start_node, (int, int) end_node)
    {
        (int x_1, int y_1) = start_node;
        (int x_2, int y_2) = end_node;

        var euclidian_distance = Math.Sqrt(Math.Pow(x_1 - x_2, 2) + Math.Pow(y_1 - y_2, 2));

        return euclidian_distance;
    }

    static List<(int, int)> get_neighbors((int, int) base_node_position)
    {
        (int pos_x, int pos_y) = base_node_position;

        IEnumerable<int> neighbors_x_coords = Enumerable.Range(pos_x - 1, 3);
        IEnumerable<int> neighbors_y_coords = Enumerable.Range(pos_y - 1, 3);

        var neighbors = new List<(int, int)>();
        foreach (var x in neighbors_x_coords)
        {
            foreach (var y in neighbors_y_coords)
            {
                neighbors.Add((x, y));
            }
        }

        neighbors.Remove((pos_x, pos_y));

        return neighbors;
    }
}
