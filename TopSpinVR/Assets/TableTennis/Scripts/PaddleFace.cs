using System.Collections;
using UnityEngine;

//namespace TopSpinVR
//{
    public class PaddleFace : MonoBehaviour
    {
        private const float paddleXMin = -1.0f;
        private const float paddleXMax = 1.0f;
        private const float paddleYMin = 0.5f;
        private const float paddleYMax = 1.5f;

        private const float contactTimeCoeff = 1;
        private const float bounciness = 1;
        private const float friction = 1;

        private Vector3 lastPosition;

        public bool isServe;

        public enum paddleMode { paddle3D = 0, paddle2D = 1};
        public paddleMode currentMode = paddleMode.paddle2D;
 
        // Use this for initialization
        void Start()
        {

        }

    public void Reset()
    {
        isServe = true;
        currentMode = paddleMode.paddle2D;
    }

        // Update is called once per frame
        void Update()
        {
            lastPosition = transform.position;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.collider.tag == "ball")
            {
                if (currentMode == paddleMode.paddle3D)
                {
                    // decompose paddle speed to tangent and normal directions.
                    // Normal force applies stronger bouncy force. Tangent force applies weak friction.
                    //Vector3 paddleSpeed = transform.position - lastPosition;
                    Vector3 relativeSpeed = GetComponent<Rigidbody>().velocity - other.rigidbody.velocity;
                    Debug.Log("paddle collision model!!!!!" + other.rigidbody.velocity);
                    Vector3 speedNormal = Vector3.Dot(relativeSpeed, transform.forward) * relativeSpeed;
                    Vector3 speedTangent = relativeSpeed - speedNormal;

                    //float contactTime = 1 / Vector3.Magnitude(speedNormal);
                    /*float diff = (currentPosition.x - lastPosition.x);
                    if (diff < -1.1f)
                    {
                        diff = -1.1f;
                    }
                    if (diff > 1.1f)
                    {
                        diff = 1.1f;
                    }*/

                    other.rigidbody.AddForce(speedNormal * contactTimeCoeff, ForceMode.VelocityChange);


                    // enable spin

                }
                else if (currentMode == paddleMode.paddle2D)
                {
                    if (isServe == true)
                    {
                        float forwardSpeed = 5.0f;
                        float downSpeed = 1.0f;
                        float sideSpeed = 0.8f;

                        other.rigidbody.useGravity = true;
                        other.rigidbody.AddForce(Vector3.down * downSpeed, ForceMode.VelocityChange);
                        other.rigidbody.AddForce(transform.forward * forwardSpeed, ForceMode.VelocityChange);

                        // Serve to two side directions randomly
                        if (Random.Range(1, 3) == 1)
                        {
                            other.rigidbody.AddForce(Vector3.right * sideSpeed, ForceMode.VelocityChange);
                        }
                        else
                        {
                            other.rigidbody.AddForce(Vector3.right * -sideSpeed, ForceMode.VelocityChange);
                        }
                        isServe = false;
                        currentMode = paddleMode.paddle3D;
                    }
                    else
                    {
                        Vector3 ballSpeed = other.rigidbody.velocity;

                        float hitDirection = 1;
                        float ballMassScale = 0.1f;
                        if (transform.position.x < -1.7f || transform.position.x > 1.7f)
                        {
                            hitDirection *= -1;
                            hitDirection = hitDirection / 2;
                        }
                        else
                        {

                            if (Random.Range(1, 3) == 1)
                                hitDirection = -hitDirection;
                        }

                        other.rigidbody.GetComponent<PingPongBall>().batStatus = "abat";
                        float speed = 15 * 1.0f * ballMassScale;
                        other.rigidbody.velocity = Vector3.zero;
                        other.rigidbody.isKinematic = true;

                        other.transform.position = other.contacts[0].point;
                        other.transform.position -= new Vector3(0, 0, 0.1f);
                        other.rigidbody.isKinematic = false;

                        if (transform.position.y < paddleYMin - 0.15f)
                        {
                            other.rigidbody.AddForce(Vector3.up * 4, ForceMode.Impulse);
                            other.rigidbody.AddForce(transform.forward * speed / 1.5f, ForceMode.Impulse);

                            other.rigidbody.AddForce(transform.right * hitDirection * 1.5f, ForceMode.Impulse);
                        }
                        else if (transform.position.y < paddleYMin)
                        {
                            other.rigidbody.AddForce(Vector3.up * 2, ForceMode.Impulse);
                            other.rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);

                            other.rigidbody.AddForce(transform.right * hitDirection * 2, ForceMode.Impulse);
                        }
                        else
                        {
                            other.rigidbody.AddForce(Vector3.up * 1.5f * 8.0f * ballMassScale, ForceMode.Impulse);
                            other.rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);

                            other.rigidbody.AddForce(transform.right * hitDirection * 2 * ballMassScale, ForceMode.Impulse);
                        }

                        /*public Texture2D particleTexture;
                        public Transform p;
                        p.GetComponent<ParticleRenderer>().materials[0].mainTexture = particleTexture;
                        move = false;*/
                    } // end isServe
                } // end currentMode
            } // end if other == "ball"
        } // end onCollision()
    }
//}