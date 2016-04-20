using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISelectionIndicator : MonoBehaviour
{
    MouseController MC;
    private Camera MainCamera;
    public float RectBuffer = 10f;

	void Start ()
	{
	    MC = FindObjectOfType<MouseController>();
	    MainCamera = Camera.main;

	}

    void Update()
    {
        if (MC.ClickedNanite != null)
        {
            GetComponent<Image>().enabled = true;

            Bounds bigBounds = MC.ClickedNanite.GetComponentInChildren<Renderer>().bounds;

            Vector3[] screenSpaceCorners = new Vector3[8];

            screenSpaceCorners[0] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[1] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
            screenSpaceCorners[2] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[3] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));

            screenSpaceCorners[4] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[5] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
            screenSpaceCorners[6] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[7] = MainCamera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));

            float min_x = screenSpaceCorners[0].x;
            float min_y = screenSpaceCorners[0].y;
            float max_x = screenSpaceCorners[0].x;
            float max_y = screenSpaceCorners[0].y;

            for (int i = 0; i < 8; i++)
            {
                if (screenSpaceCorners[i].x < min_x)
                {
                    min_x = screenSpaceCorners[i].x;
                }
                if (screenSpaceCorners[i].y < min_y)
                {
                    min_y = screenSpaceCorners[i].y;
                }
                if (screenSpaceCorners[i].x > max_x)
                {
                    max_x = screenSpaceCorners[i].x;
                }
                if (screenSpaceCorners[i].y > max_y)
                {
                    max_y = screenSpaceCorners[i].y;
                }
            }
            RectTransform rt = GetComponent<RectTransform>();
            rt.position = new Vector2(min_x - RectBuffer ,min_y - RectBuffer);
            rt.sizeDelta = new Vector2((max_x - (min_x - (RectBuffer * 2))) , (max_y - (min_y - (RectBuffer * 2))));

        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
    }
}
