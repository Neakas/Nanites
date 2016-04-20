using UnityEngine;
using System.Collections;

public class World
{
    Tile[,] tiles;
    public PathTileGraph TileGraph;
    public int Width { get; set; }
    public int Height { get; set; }

    public World(int width, int height)
    {
        SetupWorld(width,height);
    }

    public World()
    {
    }

    void SetupWorld(int width, int height)
    {
        Width = width;
        Height = height;
        tiles = new Tile[Width,Height];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                tiles[x,z] = new Tile(this,x,z);
            }
        }
        TileGraph = new PathTileGraph(this);
    }

    public Tile GetTileAt(int x, int z)
    {
        if (x >= Width || x < 0 || z >= Height || z < 0)
        {
            return null;
        }
        return tiles[x, z];
    }

    public void InvalidateTileGraph()
    {
        TileGraph = null;
    }

}
