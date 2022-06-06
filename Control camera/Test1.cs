/*
 
Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.
 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test1 : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    public Plane Plane;

    public float scrollSpeed;
    public float rotateSpeed;
    public float zoomSpeed;

    public float zoomMinY;
    public float zoomMaxY;

    public float camsummer;

    float zoomTime;

    Vector3 pos;
    public Vector3 velocity = Vector3.zero;

    private static readonly float[] ZoomBounds = new float[] { 10f, 85f };



    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;

        Rotate = true;

        scrollSpeed = 50f;
        rotateSpeed = 50f;
        zoomSpeed = 1f;

        zoomMinY = 50f;
        zoomMaxY = 200f;

        camsummer = 4f;

        pos = new Vector3(0f, 0f, 0f);



    }

    private void Update()
    {

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount == 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                Camera.transform.Translate(Delta1 * scrollSpeed * Time.deltaTime, Space.World);
        }

        //Pinch
        if (Input.touchCount == 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom to be done relative to the distance moved between the fingers
            var zoom = (Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b));

            //edge case (Bad calculation)
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray. This is where the zoom is applied
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, (1 / zoom));


            //This is where the rotation is applied
            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal,
                  Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal) * rotateSpeed * Time.deltaTime);



            // Vector3 pos = Camera.transform.position;

            // pos = Vector3.LerpUnclamped(pos1, Camera.transform.position, (1 / zoom));

            // pos.y = Mathf.Clamp(pos.y, zoomMinY, zoomMaxY);

            // Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView - ((1/zoom) * zoomSpeed), ZoomBounds[0], ZoomBounds[1]);


            // Vector3 pos = Camera.transform.position;
            // pos.y = Mathf.Clamp(pos.y, zoomMinY, zoomMaxY);
            // Camera.transform.position = Vector3.Lerp(pos1, Camera.transform.position, (1 / zoom));
            // Camera.transform.position = pos;



            // if (Camera.transform.position.y < zoomMinY | Camera.transform.position.y > zoomMaxY)
            // {
            //   // Camera.transform.position = pos;
            //   Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, pos, ref velocity, camsummer);
            //   // Camera.transform.position = Vector3.LerpUnclamped(Camera.transform.position, pos, camsummer);

            // }
            // else if (Camera.transform.position.y >= zoomMinY && Camera.transform.position.y <= zoomMaxY)
            // {
            //   pos = Camera.transform.position;
            //   Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, (1 / zoom));
            //   //This is where the rotation is applied
            //   if (Rotate && pos2b != pos2)
            //     Camera.transform.RotateAround(pos1, Plane.normal,
            //       Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal) * rotateSpeed * Time.deltaTime);
            // }



        }

    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta: How far have we moved from A to B by sliding the finger
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }

    public void from2dTo3d()
    {
        Quaternion pos;
        pos = Camera.transform.rotation;
        pos.x = 90f;
        pos.y = 0f;
        pos.z = 0f;
        Camera.transform.rotation = pos;

    }

    public void from3dTo2d()
    {
        Quaternion pos;
        pos = Camera.transform.rotation;
        pos.x = 45f;
        pos.y = 45f;
        pos.z = 0f;
        Camera.transform.rotation = pos;
    }

#endif
}
