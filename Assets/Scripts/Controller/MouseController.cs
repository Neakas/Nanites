using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Transform _naniteTransform;
    private Ray _ray;
    private RaycastHit _rayhit;
    private const float Raylength = 100f;
    private int _setTerritory;
    public GameObject ClickedNanite;
    public bool MouseMoveMode;

    public RaycastHit MainRaycastHit
    {
        get
        {
            Physics.Raycast(MainRay, out _rayhit, Raylength);
            return _rayhit;
        }
        set
        {
            _rayhit = value;
        }
    }

    public Ray MainRay
    {
        get
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return _ray;
        }
    }

    private void Start()
    {
    }

    public void SetMouseMoveMode()
    {
        if (MouseMoveMode)
        {
            MouseMoveMode = false;
        }
        MouseMoveMode = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && MainRaycastHit.transform?.GetComponent("Nanite") != null)
        {
            ClickedNanite = MainRaycastHit.transform.gameObject;
        }
        if (Input.GetMouseButtonDown(0) && MouseMoveMode && MainRaycastHit.transform?.GetComponent("Nanite") == null && ClickedNanite != null)
        {
            var t = WorldController.Instance.GetTileAtWorldCoord(MainRaycastHit.transform.position);
            var nanite = ClickedNanite?.GetComponent("Nanite") as Nanite;
            if (nanite != null)
            {
                var targeTile = nanite.TargetTile;
                if (targeTile != nanite.CurrentTile)
                {
                    WorldController.Instance.ChangeTileColor(t,Color.yellow);
                    nanite.TargetTile = t;
                    Debug.Log("Move to: " + nanite.TargetTile.X + "-" + nanite.TargetTile.Z);
                    nanite.AddMovetoTarget();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClickedNanite = null;
        }


        //Only Show if Over Nanite
        if (MainRaycastHit.transform?.GetComponent("Nanite") != null)
        {
            _naniteTransform = MainRaycastHit.transform;
            var nanite = _naniteTransform.gameObject;
            var nanitescript = (Nanite) nanite.GetComponent("Nanite");
            // Only Show if Not Moving
            if (nanitescript.IsMoving)
            {
                return;
            }
            _setTerritory = nanitescript.Territory;
            for (var x = 1; x <= nanitescript.Territory; x++)
            {
                for (var z = 1; z <= nanitescript.Territory; z++)
                {
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z + z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z - z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z + z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z - z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z - z)), Color.green);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z + z)), Color.green);
                    }
                }
            }
        }
        else if (MainRaycastHit.transform != null && MainRaycastHit.transform.GetComponent("Nanite") == null || MainRaycastHit.transform == null && _setTerritory != 0)
        {
            for (var x = 1; x <= _setTerritory; x++)
            {
                for (var z = 1; z <= _setTerritory; z++)
                {
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z + z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x, 0, _naniteTransform.position.z - z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z + z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z - z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z - z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x + x, 0, _naniteTransform.position.z - z)), Color.white);
                    }
                    if (WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z + z)) != null)
                    {
                        WorldController.Instance.ChangeTileColor(WorldController.Instance.GetTileAtWorldCoord(new Vector3(_naniteTransform.position.x - x, 0, _naniteTransform.position.z + z)), Color.white);
                    }
                }
            }
            _naniteTransform = null;
            _setTerritory = 0;
        }
    }
}
