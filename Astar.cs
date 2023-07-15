using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		var open = new Dictionary<(int, int), int>();
		var closed = new Dictionary<(int, int), int>();
		var pQueue = new Dictionary<(int, int), int>();
		
		pQueue.Add((0, 0), 3);
		pQueue.Add((1, 1), 1);
		pQueue.Add((2, 2), 5);
		
		var sorted_dict = pQueue.OrderBy(item => item.Value).ToDictionary(item => item.Key, item => item.Value);
		/*foreach(var item in sorted_dict)
		{
			Console.WriteLine(item.Key + "-" + item.Value);
		}*/
		KeyValuePair<(int, int), int> pop = sorted_dict.First();
		
		Console.WriteLine(pop.Value);
		Console.WriteLine(sorted_dict.First().Key);
		
		Console.WriteLine(pQueue[(0, 0)]);
		
		heuristic((0, 0), (3, 7));
	}
	
	int expand_node(KeyValuePair<(int, int), int> current_node, KeyValuePair<(int, int), int> end_node, Dictionary<(int, int), int> pQueue)
	{
		//throw new NotImplementedException();
		
		// runs for each neibors
		// gets the eucleudian distance between neighbor_node and end_node
		// checks if 
		//(int x_1, int y_1) = pQueue[current_node];
		
		return 8;
	}
	
	static double heuristic((int, int) start_node, (int, int) end_node)
	{
		(int x_1, int y_1) = start_node;
		(int x_2, int y_2) = end_node;
		
		var eucDist = Math.Sqrt(Math.Pow(x_1 - x_2, 2) + Math.Pow(x_1 - x_2, 2));
		
		return eucDist;
	}
	
	public class Node
	{
		(int, int) position;
		int g_value = 9999;
		int f_value = 9999;
		(int, int) previous_node;
	}
}