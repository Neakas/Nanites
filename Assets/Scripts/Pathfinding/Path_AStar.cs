using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

public class PathAStar
{
    private Queue<Tile> _path;

    public PathAStar( World world, Tile tileStart, Tile tileEnd )
    {
        if (world.TileGraph == null)
        {
            world.TileGraph = new PathTileGraph(world);
        }

        var nodes = world.TileGraph.Nodes;
        var start = nodes[tileStart];
        var goal = nodes[tileEnd];
        var closedSet = new List<PathNode<Tile>>();
        var openSet = new SimplePriorityQueue<PathNode<Tile>>();

        if (nodes.ContainsKey(tileStart) == false)
        {
            Debug.LogError("Path_Astar: The Starting Tile isnt in the List of Nodes");
            return;
        }
        if (nodes.ContainsKey(tileEnd) == false)
        {
            Debug.LogError("Path_Astar: The Ending Tile isnt in the List of Nodes");
            return;
        }

        openSet.Enqueue(start,0);

        var cameFrom = new Dictionary<PathNode<Tile>, PathNode<Tile>>();
        var gScore = new Dictionary<PathNode<Tile>, float>();

        foreach (var n in nodes.Values)
        {
            gScore[n] = Mathf.Infinity;
        }

        gScore[start] = 0;
        var fScore = new Dictionary<PathNode<Tile>, float>();

        foreach (var n in nodes.Values)
        {
            fScore[n] = Mathf.Infinity;
        }
        fScore[start] = heuristic_cost_estimate(start,goal);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == goal)
            {
                reconstruct_path(cameFrom,current);
                return;
            }

            closedSet.Add(current);

            foreach (var edgeNeighbour in current.Edges)
            {
                var neighbour = edgeNeighbour.Node;
                if (closedSet.Contains(neighbour)) continue;
                var tentativeGScore = gScore[current] + dist_between(current, neighbour);

                if (openSet.Contains(neighbour) && tentativeGScore >= gScore[neighbour]) continue;

                cameFrom[neighbour] = current;
                gScore[neighbour] = tentativeGScore;
                fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, goal);

                if (openSet.Contains(neighbour) == false)
                {
                    openSet.Enqueue(neighbour, fScore[neighbour]);
                }
            }
        }

        // if we reached here, it meas that we`ve burned through the entire OpenSet without ever reaching a point where current = goal
        // This happen when there is no _path from start to goal

    }

    private static float heuristic_cost_estimate(PathNode<Tile> a, PathNode<Tile> b)
    {
        //Satz des Pythagoras im Grunde
        return Mathf.Sqrt(Mathf.Pow(a.Data.X - b.Data.X, 2) + Mathf.Pow(a.Data.Z - b.Data.Z, 2));
    }

    private static float dist_between( PathNode<Tile> a, PathNode<Tile> b )
    {
        if (Mathf.Abs(a.Data.X - b.Data.X) + Mathf.Abs(a.Data.Z - b.Data.Z) == 1)
        {
            return 1f;
        }
        if (Mathf.Abs(a.Data.X - b.Data.X) == 1 && Mathf.Abs(a.Data.Z - b.Data.Z) == 1)
        {
            //Quadratwurzel aus 2
            return 1.41421356237f;
        }
        return Mathf.Sqrt(Mathf.Pow(a.Data.X - b.Data.X, 2) + Mathf.Pow(a.Data.Z - b.Data.Z, 2));
    }

    private void reconstruct_path(IDictionary<PathNode<Tile>, PathNode<Tile>> cameFrom,PathNode<Tile> current )
    {
        var totalPath = new Queue<Tile>();
        totalPath.Enqueue(current.Data);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Enqueue(current.Data);
        }
        _path = new Queue<Tile>(totalPath.Reverse());
    }


    public Tile Dequeue()
    {
        return _path.Dequeue();
    }

    public int Length()
    {
        return _path?.Count ?? 0;
    }
}
