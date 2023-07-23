using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
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
