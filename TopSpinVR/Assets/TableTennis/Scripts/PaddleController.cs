using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public enum GribMode { PenHold, ShakeHands };
    
    public GribMode gribMode;

    public Vector3 gribBasicPosition;
    public Vector3 gribBasicRotation;

    // Use this for initialization
    void Start()
    {
        if (gribMode == GribMode.PenHold)
        {
            gribBasicPosition = Vector3.zero;
            gribBasicRotation = Vector3.zero;
        }
        else if (gribMode == GribMode.ShakeHands)
        {
            gribBasicPosition = Vector3.zero;
            gribBasicRotation = Vector3.zero;
        }
        else
        {
            gribBasicPosition = Vector3.zero;
            gribBasicRotation = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Controller conectedControllers = OVRInput.GetConnectedControllers();

        if ((conectedControllers & OVRInput.Controller.RTouch) != 0)
        {
            if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch))
            {
                transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + gribBasicPosition;
            }
            else
            {
                Debug.Log("Lost controller position");
            }

            if (OVRInput.GetControllerOrientationTracked(OVRInput.Controller.RTouch))
            {
                transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
                transform.Rotate(gribBasicRotation, Space.World);
            }
            else
            {
                Debug.Log("Lost controller orientation");
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.position.x * -10); //= new  transform.position.x*-10;


            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                foreach (Touch touch in Input.touches)
                {
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + touch.deltaPosition.x * Time.deltaTime * 2, 0.2f), Mathf.Lerp(transform.position.y, transform.position.y + touch.deltaPosition.y * Time.deltaTime * 2, 0.2f), transform.position.z);
                }
            }
            else
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + Input.GetAxis("Mouse X"), 0.2f), Mathf.Lerp(transform.position.y, transform.position.y + Input.GetAxis("Mouse Y"), 0.2f), transform.position.z);
            }

            if (transform.position.x > 4)
                transform.position = new Vector3(4, transform.position.y, transform.position.z);
            if (transform.position.x < -4)
                transform.position = new Vector3(-4, transform.position.y, transform.position.z);

            if (transform.position.y < 1.5f)
                transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            if (transform.position.y > 2.5)
                transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
        }
    }

}
