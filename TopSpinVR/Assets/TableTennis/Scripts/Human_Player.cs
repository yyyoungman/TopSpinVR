using UnityEngine;
using System.Collections;

public class Human_Player : MonoBehaviour
{

    private Vector3 currentPosition;
    private Vector3 lastPosition;

    private const float mouseSpeed = 0.05f;
    private const float paddleXMin = -1.0f;
    private const float paddleXMax = 1.0f;
    private const float paddleYMin = 0.5f;
    private const float paddleYMax = 1.5f;

    public float speed;

    public float lastVelocity;

    public AI_Player aiBat;

    public Texture2D particleTexture;

    public Transform ballParticalEffect;

    public bool firstServe = true;

    public Vector3 firstPostion;

    public static int points;

    // Use this for initialization
    void Start()
    {
        points = 0;
        firstPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Controller conectedControllers = OVRInput.GetConnectedControllers();

        if ((conectedControllers & OVRInput.Controller.RTouch) != 0)
        {
            if (OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch))
            {
                transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            }
            else
            {
                Debug.Log("Lost controller position");
            }

            if (OVRInput.GetControllerOrientationTracked(OVRInput.Controller.RTouch))
            {
                transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            }
            else
            {
                Debug.Log("Lost controller orientation");
            }
        }
        else
        {
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x,transform.eulerAngles.y,transform.position.x*-10); //= new  transform.position.x*-10;
			
			
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				foreach (Touch touch in Input.touches) 
				{
					transform.position = new Vector3(Mathf.Lerp(transform.position.x,transform.position.x+touch.deltaPosition.x * Time.deltaTime*2,0.2f),Mathf.Lerp(transform.position.y,transform.position.y+touch.deltaPosition.y * Time.deltaTime*2,0.2f),transform.position.z);
				}
			}
			else
			{
				transform.position = new Vector3(Mathf.Lerp(transform.position.x,transform.position.x+Input.GetAxis("Mouse X"), mouseSpeed),Mathf.Lerp(transform.position.y,transform.position.y+Input.GetAxis("Mouse Y"), mouseSpeed),transform.position.z);
			}
			
			
			
			if(transform.position.x > paddleXMax)
				transform.position = new Vector3(paddleXMax,transform.position.y,transform.position.z);
			if(transform.position.x < paddleXMin)
				transform.position = new Vector3(paddleXMin,transform.position.y,transform.position.z);
				
			if(transform.position.y < paddleYMin)
				transform.position = new Vector3(transform.position.x,paddleYMin,transform.position.z);
			if(transform.position.y > paddleYMax)
				transform.position = new Vector3(transform.position.x, paddleYMax,transform.position.z);
		}
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "ball" && !firstServe)
        {
            currentPosition = transform.position;
            float diff = (currentPosition.x - lastPosition.x);
            if (diff < -1.1f)
            {
                diff = -1.1f;
            }
            if (diff > 1.1f)
            {
                diff = 1.1f;
            }
            other.rigidbody.GetComponent<PingPong_Ball>().batStatus = "ubat";

            other.rigidbody.velocity = Vector3.zero;
            other.rigidbody.isKinematic = true;

            other.transform.position = other.contacts[0].point;
            other.transform.position += new Vector3(0, 0, 0.1f);
            other.rigidbody.isKinematic = false;
            other.rigidbody.AddForce(transform.forward * 15, ForceMode.Impulse);

            other.rigidbody.AddForce(transform.right * diff * 2, ForceMode.Impulse);
            if (other.transform.position.y < 1.68f)
            {
                other.rigidbody.AddForce(Vector3.up * 1.6f, ForceMode.Impulse);
            }
            else
            {
                other.rigidbody.AddForce(Vector3.up * 1.2f, ForceMode.Impulse);
            }

            ballParticalEffect.GetComponent<ParticleRenderer>().materials[0].mainTexture = particleTexture;

            AI_Player.move = true;
            aiBat.HitDirection(diff);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "ball")
        {
            other.transform.SendMessage("Serve", transform);
        }
    }

    public void RegisterSpin()
    {
        lastPosition = transform.position;
    }

    public void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = false;
        firstServe = true;
    }
}
