using UnityEngine;
using System.Collections;

public class HumanPlayer : MonoBehaviour
{

    private Vector3 currentPosition;
    private Vector3 lastPosition;

    private const float mouseSpeed = 0.05f;
    private const float paddleXMin = -1.0f;
    private const float paddleXMax = 1.0f;
    private const float paddleYMin = 0.5f;
    private const float paddleYMax = 1.5f;

    private const float ballMassScale = 0.1f;

    public float speed;

    public float lastVelocity;

    public AiPlayer aiBat;

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
            other.rigidbody.GetComponent<PingPongBall>().batStatus = "ubat";

            other.rigidbody.velocity = Vector3.zero;
            other.rigidbody.isKinematic = true;

            other.transform.position = other.contacts[0].point;
            other.transform.position += new Vector3(0, 0, 0.1f);
            other.rigidbody.isKinematic = false;
            other.rigidbody.AddForce(transform.forward * 15 * ballMassScale, ForceMode.Impulse);

            other.rigidbody.AddForce(transform.right * diff * 2 * ballMassScale, ForceMode.Impulse);
            if (other.transform.position.y < paddleYMin)
            {
                other.rigidbody.AddForce(Vector3.up * 1.6f * ballMassScale, ForceMode.Impulse);
            }
            else
            {
                other.rigidbody.AddForce(Vector3.up * 1.2f * ballMassScale, ForceMode.Impulse);
            }

            //ballParticalEffect.GetComponent<ParticleRenderer>().materials[0].mainTexture = particleTexture;

            AiPlayer.move = true;
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
