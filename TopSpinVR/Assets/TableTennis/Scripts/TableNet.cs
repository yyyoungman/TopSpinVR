using UnityEngine;
using System.Collections;

public class TableNet : MonoBehaviour {

	private bool getCollision = true;



	void OnTriggerEnter(Collider other)
	{
        return;

        if (other.GetComponent<Collider>().tag == "ball")
        {
            if (other.GetComponent<Rigidbody>().GetComponent<PingPongBall>().batStatus == "abat" && getCollision)
			{
					getCollision = false;
					other.GetComponent<Rigidbody>().velocity = Vector3.zero;
					other.GetComponent<Rigidbody>().AddForce(transform.forward*5,ForceMode.Impulse);
					StartCoroutine(ResetThings(other.transform));
					HumanPlayer.points++;
					CameraFollow.userPoints = HumanPlayer.points.ToString();
			}
			if(other.GetComponent<Rigidbody>().GetComponent<PingPongBall>().batStatus == "ubat" && getCollision)
			{
					getCollision = false;
					other.GetComponent<Rigidbody>().velocity = Vector3.zero;
					other.GetComponent<Rigidbody>().AddForce(-transform.forward*5,ForceMode.Impulse);
					StartCoroutine(ResetThings(other.transform));
					AiPlayer.points++;
					CameraFollow.aiPoints = AiPlayer.points.ToString();
			}
		}
	}

	IEnumerator ResetThings(Transform other)
	{
		yield return new WaitForSeconds(1);
		other.GetComponent<Rigidbody>().GetComponent<PingPongBall>().Reset();
		getCollision = true;
	}
}
