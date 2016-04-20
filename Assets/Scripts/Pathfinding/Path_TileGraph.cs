using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PathTileGraph
{
    public Dictionary<Tile, PathNode<Tile>> Nodes;

    public PathTileGraph(World world)
    {
        Nodes = new Dictionary<Tile, PathNode<Tile>>();
        for (int x = 0; x < world.Width; x++)
        {
            for (int z = 0; z < world.Height; z++)
            {

                Tile t = world.GetTileAt(x, z);

                //if(t.movementCost > 0) {	// Tiles with a move cost of 0 are unwalkable
                PathNode<Tile> n = new PathNode<Tile>();
                n.Data = t;
                Nodes.Add(t, n);
                //}

            }
        }

        Debug.Log("Path_TileGraph: Created " + Nodes.Count + " nodes.");

        var edgeCount = 0;

        foreach (var t in Nodes.Keys)
        {
            var n = Nodes[t];

            var edges = new List<PathEdge<Tile>>();

            var neighbours = t.GetNeighbours();

            foreach (var e in from t1 in neighbours where t1 != null && t1.Movementcost > 0 select new PathEdge<Tile>
            {
                Cost = t1.Movementcost,
                Node = Nodes[t1]
            })
            {
                edges.Add(e);
                edgeCount ++;
            }
            n.Edges = edges.ToArray();
        }
        Debug.Log("Path_TileGraph: Created " + edgeCount + " edges.");
    }
}
