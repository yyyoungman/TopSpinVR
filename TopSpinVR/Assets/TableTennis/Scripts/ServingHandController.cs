using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingHandController : MonoBehaviour
{

    public GameObject pingPongBallPrefab;
    public Transform pingPongBallSpawn;

    private GameObject currentBall;
    private bool canCreateNewBall = true;
    
    private List<Vector3> positionList;
    private List<float> deltaTimeList;

    // Use this for initialization
    void Start()
    {
        positionList = new List<Vector3>();
        deltaTimeList = new List<float>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OVRInput.Controller conectedControllers = OVRInput.GetConnectedControllers();

        if ((conectedControllers & OVRInput.Controller.LTouch) != 0)
        {
            if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch))
            {
                transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            }
            else
            {
                Debug.Log("Lost controller position");
            }

            if (OVRInput.GetControllerOrientationTracked(OVRInput.Controller.LTouch))
            {
                transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            }
            else
            {
                Debug.Log("Lost controller orientation");
            }

            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
                CreateNewPingPongBall();
            }
            else if (OVRInput.Get(OVRInput.RawButton.X))
            {
                currentBall.transform.position = pingPongBallSpawn.transform.position;
                currentBall.transform.rotation = pingPongBallSpawn.transform.rotation;

                deltaTimeList.Add(Time.deltaTime);
                positionList.Add(pingPongBallSpawn.transform.position);
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.X))
            {
                var ballRigidBoday = currentBall.GetComponent<Rigidbody>();
                ballRigidBoday.velocity = CalculateBallSpeed();
                positionList.Clear();
                deltaTimeList.Clear();
                currentBall = null;
                //canCreateNewBall = false;
            }
        }
        else
        {
        }
    }

    void CreateNewPingPongBall()
    {
        if (currentBall == null && canCreateNewBall)
        {
            // Create the Bullet from the Bullet Prefab
            currentBall = (GameObject)Instantiate(
                pingPongBallPrefab,
                pingPongBallSpawn.position,
                pingPongBallSpawn.rotation);
        }
    }

    // Calculate Pingpong ball speed when it left the controller
    //   TODO: here we only use the velocity calculated from the last 10 updates
    //         we can use more complex interpolation method later
    Vector3 CalculateBallSpeed()
    {
        int count = positionList.Count;
        const int velocityAverageCount = 10;
        int i = 0;
        Vector3 speedSum = Vector3.zero;
        for (; i < velocityAverageCount && i < count - 1; i++)
        {
            Vector3 currentBallSpeed = (positionList[count - 1 - i] - positionList[count - 2 - i]) / deltaTimeList[count - 1 - i];
            speedSum += currentBallSpeed;
        }
        Vector3 ballSpeed = speedSum / i;
        return ballSpeed;
    }
}
