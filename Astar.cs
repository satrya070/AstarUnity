using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		// position by 
		//var open = new Dictionary<(int, int), Node>();
		var closed = new Dictionary<(int, int), Node>();
		
		//var pQueue = new Dictionary<(int, int), int>();
		var pQueue = new List<Node>();
		
		var node_a = new Node((0, 0), (1, 3), 0, 10);
		node_a.g_value = 9;
		Console.WriteLine(node_a.g_value);
		
		pQueue.Add(new Node((0, 0), (0, 0), 3, 7));
		pQueue.Add(new Node((1, 1), (0, 0), 1, 4));
		pQueue.Add(new Node((2, 2), (1, 1), 5, 9));
		
		//var sorted_dict = pQueue.OrderBy(item => item.Value).ToDictionary(item => item.Key, item => item.Value);
		//KeyValuePair<(int, int), int> pop = sorted_dict.First();
		
		var sorted_list = pQueue.OrderBy(node => node.g_value).ToList();
		Console.WriteLine(sorted_list.Count);
		
		Node smallest = sorted_list.First();
		
		Console.WriteLine(smallest.position);
		
		//Console.WriteLine(pQueue[(0, 0)]);
		
		Console.WriteLine(heuristic((0, 0), (3, 7)));
		
		// --------------------//
		node_a = new Node((0, 0), (0, 0), 3, 7);
		var node_b = new Node((7, 2), (0, 0), 3, 7);
		
		expand_node(node_a, node_b);
	}
	
	static int expand_node(Node current_node, Node end_node)
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
		
		var neighbors = get_neighbors((5, 7));
		
		Console.WriteLine("gathering all neighbors");
		//neighbors.ForEach(delegate((int, int) x) { Console.WriteLine(x); });
		
		foreach((int, int) neighbor_position in neighbors)
		{
			// if node == end_node: stop
			float neighbor_g_value =  current_node.g_value + 1;
			float neighbor_f_value = neighbor_g_value + heuristic(neighbor_position, current_node.position);
			neighbor = new Node(neighbor_position, current_node.position, current_node.g_value + 1, neighbor_f_value);
		}
		
		return 8;
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
			foreach(var y in neighbors_y_coords)
			{
				neighbors.Add((x, y));
			}
		}
		
		neighbors.Remove((pos_x, pos_y));
		
		return neighbors;
	}
	
	public class Node
	{
		public (int, int) position { get; set; }
		public double g_value = 9999;
		public double f_value = 9999;
		public (int, int) previous_node { get; set; }
		
		public Node((int, int) Position, (int, int) Prev, int G_value, int F_value)
		{
			this.position = Position;
			this.previous_node = Prev;
			this.g_value = G_value;
			this.f_value = F_value;
		}
	}
}