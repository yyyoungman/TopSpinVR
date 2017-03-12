using UnityEngine;
using System.Collections;

public class Human_Player : MonoBehaviour
{

    private Vector3 currentPosition;
    private Vector3 lastPosition;

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
