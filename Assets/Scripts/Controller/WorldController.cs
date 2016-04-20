using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class WorldController : MonoBehaviour {

    public static WorldController Instance { get; protected set; }
    private float GridOffset = 0.5f;
    public GameObject TileGameObject;
    public int WorldWidth = 10;
    public int WorldHeigth = 10;

    public World world { get; protected set; }

    void OnEnable()
    {
        if (Instance != null)
        {
            Debug.LogError("Error - Found WorldController Instance");
        }
        Instance = this;
        CreateWorld();
    }

    void CreateWorld()
    {
        world = new World(WorldWidth, WorldHeigth);
        BuildGrid();
    }

    private void Start ()
	{
	    
	}

    private void Update () 
	{
	
	}

    private void BuildGrid()
    {
        for (var x = 0; x < world.Width; x++)
        {
            for (var z = 0; z < world.Height; z++)
            {
                var tile = (GameObject)Instantiate(TileGameObject, new Vector3(x + GridOffset, 0, z + GridOffset),TileGameObject.transform.rotation);
                tile.name = "tile_" + x + "_" + z;
                tile.transform.SetParent(GameObject.Find("WorldController").transform);
            }
        }
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x - 0.5f);
        int z = Mathf.FloorToInt(coord.z - 0.5f);

        return world.GetTileAt(x, z);
    }

    public void ChangeTileColor(Tile tile, Color color)
    {
        GameObject tilego = Tile2GameObject(tile);
        var rend = tilego.GetComponent<Renderer>();
        rend.material.color = color;
    }

    public GameObject Tile2GameObject( Tile tile )
    {
        return GameObject.Find("tile_" + tile.X + "_" + tile.Z);
    }
}
