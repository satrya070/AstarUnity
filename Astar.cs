using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    static Node end_node = new Node((7, 2), null, 3);
    static List<Node> OpenPriorityQueue = new List<Node>();
    static Dictionary<(int, int), Node> closed = new Dictionary<(int, int), Node>();

    public static void Main()
    {
        Node popped_node;
        List<Node> sorted_list;

        // add node with starting position and values
        OpenPriorityQueue.Add(new Node((0, 0), null, 0));

        // loop the open list until goal is found or no nodes to explore
        for (int i = 0; i < 3; i++)
        {
            // pop node with lowest f-value(lowest total distance)
            sorted_list = OpenPriorityQueue.OrderBy(node => node.g_value).ToList();
            popped_node = sorted_list.First();
            OpenPriorityQueue.RemoveAt(0);

            Console.WriteLine($"Expanding node: {popped_node.position}");
            expand_node(popped_node);

            // push to closed list
            closed[popped_node.position] = popped_node;

            Console.WriteLine(String.Concat(sorted_list.Select(o => o.position.ToString() + ',')));
            Console.WriteLine(String.Concat(OpenPriorityQueue.Select(o => o.position.ToString() + ',')));
        }
        OpenPriorityQueue.ForEach(delegate (Node node) { Console.WriteLine($"{node.position} | {node.g_value} | {node.f_value}"); });

        static void expand_node(Node expanding_node)
        {
            // get all neighbors
            // filter invalid neighbors
            // runs for each neibors
            // if node is goal stop
            // compute f: g+h gets the eucleudian distance between neighbor_node and end_node
            // continue if in open and lower
            // 
            // checks if 
            //(int x_1, int y_1) = pQueue[current_node];
            // -----------------------------//

            var neighbors = get_neighbors(expanding_node.position);

            Console.WriteLine($"gathering all neighbors for node: {expanding_node.position}");
            //neighbors.ForEach(delegate((int, int) x) { Console.WriteLine(x); });

            foreach ((int, int) neighbor_position in neighbors)
            {
                // if node == end_node: stop

                // if node in closed: stop
                //if (closed.ContainsKey())

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
                        Console.WriteLine("update node in open, with smaller one and continue");
                        continue;
                    }
                    else
                    {
                        // there's a node in open list that already with a smaller distance
                        continue;
                    }
                }

                OpenPriorityQueue.Add(neighbor);

                // Console.WriteLine($"{expanding_node.position} {neighbor.position} - {neighbor.g_value} - {neighbor.f_value}");
            }
        }

        static double heuristic((int, int) start_node, (int, int) end_node)
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

    public class Node
    {
        public (int, int) position { get; set; }
        public double g_value = 9999;
        public double f_value = 9999;
        public (int, int)? previous_node { get; set; }

        public Node((int, int) Position, (int, int)? Prev, double G_value)
        {
            this.position = Position;
            this.previous_node = Prev;
            this.g_value = G_value;
        }
    }
}