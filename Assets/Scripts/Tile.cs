using UnityEngine;

[System.Serializable]
public class Tile
{
    public int X { get; protected set; }
    public int Z { get; protected set; }
    public bool IsBlocked = false;
    public int Movementcost = 1;
    public World world { get; protected set; }
    const float baseTileMovementCost = 1;

    public Tile(World world, int x, int z)
    {
        this.world = world;
        this.X = x;
        this.Z = z;
    }

    public bool IsNeighbour(Tile tile, bool diagOkay = false)
    {
        return Mathf.Abs(this.X - tile.X) + Mathf.Abs(this.Z - tile.Z) == 1 ||
               (diagOkay && (Mathf.Abs(this.X - tile.X) == 1 && Mathf.Abs(this.Z - tile.Z) == 1));
    }

    public Tile[] GetNeighbours(bool diagOkay = false)
    {
        Tile[] ns;

        if (diagOkay == false)
        {
            ns = new Tile[4];   // Tile order: N E S W
        }
        else
        {
            ns = new Tile[8];   // Tile order : N E S W NE SE SW NW
        }

        Tile n;

        n = world.GetTileAt(X, Z + 1);
        ns[0] = n;  // Could be null, but that's okay.
        n = world.GetTileAt(X + 1, Z);
        ns[1] = n;  // Could be null, but that's okay.
        n = world.GetTileAt(X, Z - 1);
        ns[2] = n;  // Could be null, but that's okay.
        n = world.GetTileAt(X - 1, Z);
        ns[3] = n;  // Could be null, but that's okay.

        if (diagOkay == true)
        {
            n = world.GetTileAt(X + 1, Z + 1);
            ns[4] = n;  // Could be null, but that's okay.
            n = world.GetTileAt(X + 1, Z - 1);
            ns[5] = n;  // Could be null, but that's okay.
            n = world.GetTileAt(X - 1, Z - 1);
            ns[6] = n;  // Could be null, but that's okay.
            n = world.GetTileAt(X - 1, Z + 1);
            ns[7] = n;  // Could be null, but that's okay.
        }

        return ns;
    }

    public Tile North()
    {
        return world.GetTileAt(X, Z + 1);
    }
    public Tile South()
    {
        return world.GetTileAt(X, Z - 1);
    }
    public Tile East()
    {
        return world.GetTileAt(X + 1, Z);
    }
    public Tile West()
    {
        return world.GetTileAt(X - 1, Z);
    }
}
