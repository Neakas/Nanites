using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Nanite : MonoBehaviour
{
    public List<int> BaseCode;
    public World World;
    public bool IsMoving;
    public int Territory = 1;
    public bool JobRunning;
    public List<Methods.Methodtype> AvailableMethods;
    private Methods _methodHandler;
    public List<Tile> TerritoryTiles;
    public Tile TargetTile;
    public Tile CurrentTile;
    public Tile NextTile;
    private PathAStar _pathAStar;


    public Dictionary<string, object> JobList = new Dictionary<string, object>();

    //public Nanite( int territory, WorldController world )
    //{
    //    Territory = territory;
    //    _world = world;
    //}

    private void Start()
    {
        _methodHandler = new Methods(this);
        AvailableMethods = new List<Methods.Methodtype> { Methods.Methodtype.CheckTerritory };
        TerritoryTiles = new List<Tile>();
        DoStoredJobs();
        ReloadTerritory();
        CurrentTile = WorldController.Instance.GetTileAtWorldCoord(transform.position);
        NextTile = null;
        _pathAStar = null;
        World = WorldController.Instance.world;
    }

    private void BuildFunction()
    {
        GetComponent(typeof(Methods));
    }

    private void Update()
    {
        if (JobList.Count > 0 && !JobRunning)
        {
            DoStoredJobs();
        }
    }

    public void DoStoredJobs()
    {
        if (JobList.Count <= 0)
        {
            return;
        }

        var firstJob = JobList.FirstOrDefault();
        var index = firstJob.Key.IndexOf("#", StringComparison.Ordinal);
        var startableJob = index > 0 ? firstJob.Key.Substring(0, index) : firstJob.Key;

        if (firstJob.Key == null && firstJob.Key == "") return;
        var parameter = (int)firstJob.Value;

        if ((int)firstJob.Value != 0 && firstJob.Value != null)
        {
            StartCoroutine(startableJob, parameter);
        }
        else
        {
            StartCoroutine(firstJob.Key);
        }
        JobList.Remove(firstJob.Key);
    }

    public void ReloadTerritory()
    {
        TerritoryTiles.Clear();
        for (var x = 1; x <= Territory; x++)
        {
            for (var z = 1; z <= Territory; z++)
            {
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x, 0, transform.position.z + z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x, 0, transform.position.z + z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x, 0, transform.position.z - z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x, 0, transform.position.z - z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z + z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z + z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z - z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z - z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z - z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x + x, 0, transform.position.z - z)));
                }
                if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z + z)) != null)
                {
                    TerritoryTiles.Add(WorldController.Instance.GetTileAtWorldCoord(new Vector3(transform.position.x - x, 0, transform.position.z + z)));
                }
            }
        }
    }


    public void CheckTerritory()
    {

    }

    public void AddMovetoTarget()
    {
        IsMoving = true;
        JobList.Add("MoveToTarget" + "#" + UnityEngine.Random.Range(0, 10000), 1);
        //ReloadTerritory();
    }

    public void AddOneMovementForward()
    {
        IsMoving = true;
        JobList.Add("MoveUp" + "#" + UnityEngine.Random.Range(0, 10000), 1);
        //ReloadTerritory();
    }

    public void AddOneMovementRight()
    {
        IsMoving = true;
        JobList.Add("MoveRight" + "#" + UnityEngine.Random.Range(0, 10000), 1);
        //ReloadTerritory();
    }

    public void AddOneMovementLeft()
    {
        IsMoving = true;
        JobList.Add("MoveLeft" + "#" + UnityEngine.Random.Range(0, 10000), 1);
        //ReloadTerritory();
    }

    public void AddOneMovementDown()
    {
        IsMoving = true;
        JobList.Add("MoveDown" + "#" + UnityEngine.Random.Range(0, 10000), 1);
        //ReloadTerritory();
    }

    public IEnumerator MoveToTarget( int move )
    {
        if (CurrentTile.X == TargetTile.X && CurrentTile.Z == TargetTile.Z)
        {
            _pathAStar = null;
               // We're already were we want to be.
        }
        else
        {
            if (NextTile == null || CurrentTile.X == TargetTile.X && CurrentTile.Z == TargetTile.Z)
            {
                // Get the next tile from the pathfinder.
                if (_pathAStar == null || _pathAStar.Length() == 0)
                {
                    // Generate a path to our destination
                    _pathAStar = new PathAStar(CurrentTile.world, CurrentTile, TargetTile); // This will calculate a path from curr to dest.
                    if (_pathAStar.Length() == 0)
                    {
                        Debug.LogError("Path_AStar returned no path to destination!");
                    }

                    // Let's ignore the first tile, because that's the tile we're currently in.
                    NextTile = _pathAStar.Dequeue();

                }
                // Grab the next waypoint from the pathing system!
                NextTile = _pathAStar.Dequeue();

                if (NextTile == CurrentTile)
                {
                    Debug.LogError("Update_DoMovement - nextTile is currTile?");
                }
            }
        }
        //if (CurrentTile != TargetTile)
        //{
        //    //Debug.Log(NextTile.X + "-" + NextTile.Z);
        //    NextTile = null;
        //    if (NextTile == null || NextTile == CurrentTile)
        //    {
        //        //get the next tile from the PathFinder
        //        if (_pathAStar == null)
        //        {
        //            //Generate a path to our Destination
        //            _pathAStar = new PathAStar(World, CurrentTile, TargetTile);
        //            if (_pathAStar.Length() == 0)
        //            {
        //                Debug.LogError("The PathAStar returned no Path to Destination");
        //                _pathAStar = null;
        //            }
        //        }
        //        // grab the next waypoint from the Pathing system
        //        if (_pathAStar != null)
        //        {
        //            NextTile = _pathAStar.Dequeue();
        //            Debug.Log("Next Tile : " + NextTile.X + "-" + NextTile.Z);
        //        }
        //        if (NextTile == CurrentTile)
        //        {
        //            Debug.LogError("MoveTaget - NextTile is CurrentTile?");
        //        }
        //    }
        //}
        if (NextTile.X > CurrentTile.X)
        {
            IsMoving = true;
            StartCoroutine(_methodHandler.MoveRight(1));
            //yield return new WaitForFixedUpdate();
        }
        else if (NextTile.X < CurrentTile.X)
        {
            IsMoving = true;
            StartCoroutine(_methodHandler.MoveLeft(1));
            //yield return new WaitForFixedUpdate();
        }
        else if (NextTile.Z > CurrentTile.Z)
        {
            IsMoving = true;
            StartCoroutine(_methodHandler.MoveUp(1));
            //yield return new WaitForFixedUpdate();
        }
        else if (NextTile.Z < CurrentTile.Z)
        {
            IsMoving = true;
            StartCoroutine(_methodHandler.MoveDown(1));
            //yield return new WaitForFixedUpdate();
        }
        if (CurrentTile.X != TargetTile.X && CurrentTile.Z != TargetTile.Z)
        {
            AddMovetoTarget();
        }
        else
        {
            _pathAStar = null;
            WorldController.Instance.ChangeTileColor(TargetTile,Color.white);
            //Debug.Log("Reached Destination: " + TargetTile.X + "-" + TargetTile.Z);
            //Debug.Log("Current Location: " + CurrentTile.X + "-" + CurrentTile.Z);
            TargetTile = null;
            IsMoving = false;

        }
        //Debug.Log("Current Tile: " + CurrentTile.X + "-" + CurrentTile.Z);
        yield return new WaitForSeconds(1);
    }

    public IEnumerator MoveUp(int move)
    {
        StartCoroutine(_methodHandler.MoveUp(move));
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator MoveRight(int move)
    {
        StartCoroutine(_methodHandler.MoveRight(move));
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator MoveLeft(int move)
    {
        StartCoroutine(_methodHandler.MoveLeft(move));
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator MoveDown(int move)
    {
        StartCoroutine(_methodHandler.MoveDown(move));
        yield return new WaitForFixedUpdate();
    }





    //private IEnumerator GrindBaseCode()
    //{
    //    var foundHigh = 0;
    //    while (BaseCode.Count > 0)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        if (BaseCode[(BaseCode.Count - 1)] == 1)
    //        {
    //            foundHigh ++;
    //        }
    //        BaseCode.RemoveAt(BaseCode.Count-1);
    //    }
    //    if (foundHigh >= 4)
    //    {
    //        Debug.Log("Found more than 4!");
    //    }
    //}

}
