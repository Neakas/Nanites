using UnityEngine;
using System.Collections;

public class Methods
{
    private Vector3 _moveDirection = Vector3.forward;
    private Vector3 _pivot;
    private Vector3 _axis = Vector3.right;
    private const float Degrees = 90;
    private readonly Nanite _linkedNanite;

    public int MovementRight = 10;
    public int MovementUp = 10;

    public enum Methodtype
    {
        Wander = 0,
        Move = 1,
        None = 2,
        CheckTerritory = 4
    }

    public Methods(Nanite sourceNanite)
    {
        _linkedNanite = sourceNanite;
    }

    private void Start ()
	{
        
    }

    private void Update ()
	{
	}

    public IEnumerator MoveUp(int move)
    {
        _linkedNanite.JobRunning = true;
        _moveDirection = Vector3.forward;
        if (WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position + _moveDirection) != null)
        {
            //WorldController.SetTile(_linkedNanite.transform.position + _moveDirection, true);
            _pivot = _linkedNanite.transform.position;
            _pivot += _moveDirection * 0.5f; // half a cube width toward destination
            _pivot -= Vector3.up * 0.5f; // subtract half cube width from y for the floor
            _axis = Vector3.right;

            for (var z = 0; z < move; z++)
            {
                var frames = 25; // how many FixedUpdate frames should this take?
                // note you could handle this another way,
                // this is just the easiest way to get it working fast
                var degreesPerFrame = Degrees / frames;
                for (var i = 1; i <= frames; i++)
                {
                    _linkedNanite.transform.RotateAround(_pivot, _axis, degreesPerFrame);
                    yield return new WaitForFixedUpdate();
                }
                _pivot = _linkedNanite.transform.position;
                _pivot += _moveDirection * 0.5f; // half a cube width toward destination
                _pivot -= Vector3.up * 0.5f; // subtract half cube width from y for the floor
            }
            //WorldController.SetTile(_linkedNanite.transform.position - _moveDirection, false);
        }
        yield return null;
        _linkedNanite.JobRunning = false;
        _linkedNanite.IsMoving = false;
        _linkedNanite.CurrentTile = WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position);
        _linkedNanite.ReloadTerritory();
        _linkedNanite.DoStoredJobs();
        
    }


    public IEnumerator MoveRight(int move)
    {
        _linkedNanite.JobRunning = true;
        _moveDirection = Vector3.right;
        if (WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position + _moveDirection) != null)
        {
            _pivot = _linkedNanite.transform.position;
            _pivot += _moveDirection * 0.5f; // half a cube width toward destination
            _pivot -= Vector3.up * 0.5f;
            _axis = Vector3.back;

            for (var z = 0; z < move; z++)
            {
                var frames = 25; // how many FixedUpdate frames should this take?
                // note you could handle this another way,
                // this is just the easiest way to get it working fast
                var degreesPerFrame = Degrees / frames;
                for (var i = 1; i <= frames; i++)
                {
                    _linkedNanite.transform.RotateAround(_pivot, _axis, degreesPerFrame);
                    yield return new WaitForFixedUpdate();
                }
                _pivot = _linkedNanite.transform.position;
                _pivot += _moveDirection * 0.5f; // half a cube width toward destination
                _pivot -= Vector3.up * 0.5f; // subtract half cube width from y for the floor
            }
            _linkedNanite.IsMoving = false;
        }
        yield return null;
        _linkedNanite.JobRunning = false;
        _linkedNanite.IsMoving = false;
        _linkedNanite.CurrentTile = WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position);
        _linkedNanite.ReloadTerritory();
        _linkedNanite.DoStoredJobs();
        
    }

    public IEnumerator MoveLeft(int move)
    {
        _linkedNanite.JobRunning = true;
        _moveDirection = Vector3.left;
        if (WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position + _moveDirection) != null)
        {
            _pivot = _linkedNanite.transform.position;
            _pivot += _moveDirection * 0.5f; // half a cube width toward destination
            _pivot -= Vector3.up * 0.5f;
            _axis = Vector3.forward;

            for (var z = 0; z < move; z++)
            {
                var frames = 25; // how many FixedUpdate frames should this take?
                // note you could handle this another way,
                // this is just the easiest way to get it working fast
                var degreesPerFrame = Degrees / frames;
                for (var i = 1; i <= frames; i++)
                {
                    _linkedNanite.transform.RotateAround(_pivot, _axis, degreesPerFrame);
                    yield return new WaitForFixedUpdate();
                }
                _pivot = _linkedNanite.transform.position;
                _pivot += _moveDirection * 0.5f; // half a cube width toward destination
                _pivot -= Vector3.up * 0.5f; // subtract half cube width from y for the floor
            }
            _linkedNanite.IsMoving = false;
        }
        yield return null;
        _linkedNanite.JobRunning = false;
        _linkedNanite.IsMoving = false;
        _linkedNanite.CurrentTile = WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position);
        _linkedNanite.ReloadTerritory();
        _linkedNanite.DoStoredJobs();
        
    }

    public IEnumerator MoveDown(int move)
    {
        _linkedNanite.JobRunning = true;
        _moveDirection = Vector3.back;
        if (WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position + _moveDirection) != null)
        {
            _pivot = _linkedNanite.transform.position;
            _pivot += _moveDirection * 0.5f; // half a cube width toward destination
            _pivot -= Vector3.up * 0.5f;
            _axis = Vector3.left;

            for (var z = 0; z < move; z++)
            {
                var frames = 25; // how many FixedUpdate frames should this take?
                // note you could handle this another way,
                // this is just the easiest way to get it working fast
                var degreesPerFrame = Degrees / frames;
                for (var i = 1; i <= frames; i++)
                {
                    _linkedNanite.transform.RotateAround(_pivot, _axis, degreesPerFrame);
                    yield return new WaitForFixedUpdate();
                }
                _pivot = _linkedNanite.transform.position;
                _pivot += _moveDirection * 0.5f; // half a cube width toward destination
                _pivot -= Vector3.up * 0.5f; // subtract half cube width from y for the floor
            }
            _linkedNanite.IsMoving = false;
        }
        yield return null;
        _linkedNanite.JobRunning = false;
        _linkedNanite.IsMoving = false;
        _linkedNanite.CurrentTile = WorldController.Instance.GetTileAtWorldCoord(_linkedNanite.transform.position);
        _linkedNanite.ReloadTerritory();
        _linkedNanite.DoStoredJobs();
        
    }
}
