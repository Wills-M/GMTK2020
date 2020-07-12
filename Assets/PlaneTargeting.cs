using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTargeting : MonoBehaviour
{
    public Transform planeOrigin;
    public Camera mainCam;

    public Transform mousePositionedTransform;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 planePos = GetPlaneIntersection();
        if (mousePositionedTransform != null) mousePositionedTransform.position = planePos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(planeOrigin.position, new Vector3(1f, .01f, 1f));
    }

    // private void OnGUI()
    // {
    //     Vector3 labelPositionW = currentTilePos;
    //     Vector3 labelPositionS = mainCam.WorldToScreenPoint(labelPositionW);
    //     labelPositionS.y = Screen.height - labelPositionS.y; // flip the y coordinate to convert from camera screen space to GUI space.
    //     Rect box = new Rect(labelPositionS.x-16, labelPositionS.y-16, 32, 32);
    //     GUI.DrawTexture(box, GUITargetImage, ScaleMode.StretchToFill, true, 1f, Color.yellow, 0f, 0f);

    //     Vector3 screenMousePoint = Input.mousePosition;
    //     screenMousePoint.y = Screen.height - screenMousePoint.y; // flip the y coordinate to convert from camera screen space to GUI space.
    //     Rect mouseTileLabelRect = new Rect(screenMousePoint.x-50, screenMousePoint.y-20, 50, 20);
    //     GUI.Box(mouseTileLabelRect, currentTile.ToString());
    // }

    private Vector3 GetPlaneIntersection()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        float delta = ray.origin.y - planeOrigin.position.y;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 intersectionPos = ray.origin - dirNorm * delta;
        return intersectionPos;
    }
}
