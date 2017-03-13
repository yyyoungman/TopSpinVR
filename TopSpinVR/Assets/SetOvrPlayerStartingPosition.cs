using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOvrPlayerStartingPosition : MonoBehaviour
{
    public Vector3 startingPositionOffset;
	// Use this for initialization
	void Start()
    {
        Transform centerEyeStartingTransform = transform.FindChild("TrackingSpace/CenterEyeAnchor");
        transform.position = new Vector3(-centerEyeStartingTransform.position.x + startingPositionOffset.x, 
                                         startingPositionOffset.y, 
                                         -centerEyeStartingTransform.position.z + startingPositionOffset.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
