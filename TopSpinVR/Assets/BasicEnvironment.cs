using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[Serializable]
public class EnvironmentPrefab
{
    public GameObject prefab;
    public bool disabled;
    public bool pcOnly;
    public bool oculusOnly;
}

public class BasicEnvironment : MonoBehaviour
{
    public EnvironmentPrefab[] prefabs;

    public static BasicEnvironment Instance
    {
        get; private set;
    }
    
    void Awake()
    {
        Instance = this;

        foreach (var prefab in prefabs)
        {
            if (VRDevice.isPresent)
            {
                // Turn off prefabs that is for PC device only
                if (prefab.pcOnly)
                {
                    prefab.disabled = true;
                }
            }
            else
            {
                // Turn off prefabs that is for Oculus device only
                if (prefab.oculusOnly)
                {
                    prefab.disabled = true;
                }
            }

            if (!prefab.disabled)
            {
                if (prefab.prefab != null)
                {
                    var gameObject = Instantiate(prefab.prefab, transform, false);
                }
            }
        }
    }
}
