using UnityEngine;
using System.Collections;

public class PingPongBall : MonoBehaviour
{

    public int factor;

    public bool reset = false;

    public HumanPlayer userBatScript;
    public AiPlayer aiBatScript;

    public string batStatus;

    private float forwardSpeed = 5.0f;
    private float downSpeed = 1.0f;
    private float sideSpeed = 0.8f;
    private Vector3 firstpostion;
    private Quaternion firstRotation;
    private bool firstServe;
    private Transform batTransform;
    private int groundCount;
    private string tableSideName;

    void Start()
    {
        firstpostion = transform.position;
        firstRotation = transform.rotation;
    }

    public void Reset()
    {
        reset = false;

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = firstpostion;
        transform.rotation = firstRotation;
        aiBatScript.Reset();
        userBatScript.Reset();

    }


    public void Serve(Transform t)
    {
        // TODO: don't know what variable "factor" is for
        if (Random.Range(1, 3) == 1)
        {
            factor = -1;
        }
        else
        {
            factor = 1;
        }
        reset = true;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(Vector3.down * downSpeed, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddForce(transform.forward * forwardSpeed, ForceMode.VelocityChange);

        if (Random.Range(1, 3) == 1)
        {
            GetComponent<Rigidbody>().AddForce(transform.right * sideSpeed, ForceMode.VelocityChange);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(transform.right * -sideSpeed, ForceMode.VelocityChange);
        }

        firstServe = true;
        batTransform = t;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.name == "UserSideTable")
        {
            userBatScript.RegisterSpin();
            tableSideName = "UserSideTable";
        }

        if (collisionInfo.collider.name == "AiSideTable")
        {
            if (firstServe)
            {
                batTransform.GetComponent<Collider>().isTrigger = false;
                //batTransform.GetComponent<Rigidbody>().isKinematic = true;
                batTransform.GetComponent<HumanPlayer>().firstServe = false;
                firstServe = false;
            }

            tableSideName = "AiSideTable";
        }

        if (collisionInfo.collider.name == "Wall")
        {
            groundCount++;
            if (groundCount == 2)
            {
                Reset();
                groundCount = 0;

                // TODO: doesn't matter if it's aibat or userbat. Logics are the same.
                if (batStatus == "abat")
                {
                    if (tableSideName == "UserSideTable")
                    {
                        AiPlayer.points++;
                        //CameraFollow.aiPoints = AiBat.points.ToString();
                    }
                    if (tableSideName == "AiSideTable")
                    {
                        HumanPlayer.points++;
                        //CameraFollow.userPoints = UserBat.points.ToString();
                    }
                }

                if (batStatus == "ubat")
                {
                    if (tableSideName == "AiSideTable")
                    {
                        HumanPlayer.points++;
                        //CameraFollow.userPoints = UserBat.points.ToString();
                    }
                    if (tableSideName == "UserSideTable")
                    {
                        HumanPlayer.points++;
                        //CameraFollow.aiPoints = AiBat.points.ToString();
                    }
                }
            }
        }
    }
}
