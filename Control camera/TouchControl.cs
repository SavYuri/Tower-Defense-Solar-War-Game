using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl: MonoBehaviour
{

    // variables for camera pan

    public float speedPan;



    // variables for camera zoom in and out

    public float perspectiveZoomSpeed;

    public float orthoZoomSpeed;

    public Camera mainCamera;



    //variables for camera orbit

    public Vector3 FirstPoint;

    public Vector3 SecondPoint;

    public float xAngle; //angle for axes x for rotation

    public float yAngle;

    public float xAngleTemp; //temp variable for angle

    public float yAngleTemp;



    // Use this for initialization

    void Start()

    {



    }



    // Update is called once per frame

    void Update()

    {



        // This part is for camera pan only & for 2 fingers stationary gesture

        if (Input.touchCount > 0 && Input.GetTouch(1).phase == TouchPhase.Moved)

        {



            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            transform.Translate(-touchDeltaPosition.x * speedPan, -touchDeltaPosition.y * speedPan, 0);

        }





        //this part is for zoom in and out 

        if (Input.touchCount == 2)

        {

            Touch touchZero = Input.GetTouch(0);

            Touch touchOne = Input.GetTouch(1);



            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;

            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;



            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;

            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;



            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;





            if (mainCamera.orthographic)

            {

                mainCamera.orthographicSize += deltaMagDiff * orthoZoomSpeed;

                mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, .1f);

            }
            else

            {

                mainCamera.fieldOfView += deltaMagDiff * perspectiveZoomSpeed;

                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, .1f, 179.9f);

            }



        }



    }

}

