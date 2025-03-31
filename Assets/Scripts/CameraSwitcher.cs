using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera topDownCamera; // Camera de sus
    public Camera behindCamera;  // Camera din spatele juc?torului

    private Camera activeCamera;

    void Start()
    {
        // Set?m camera ini?ial? (cea de sus)
        activeCamera = topDownCamera;
        topDownCamera.enabled = true;
        behindCamera.enabled = false;
    }

    void Update()
    {
        // Comut? între camere la ap?sarea tastei "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        // Dezactiveaz? camera curent? ?i activeaz? cealalt? camer?
        if (activeCamera == topDownCamera)
        {
            topDownCamera.enabled = false;
            behindCamera.enabled = true;
            activeCamera = behindCamera;
        }
        else
        {
            behindCamera.enabled = false;
            topDownCamera.enabled = true;
            activeCamera = topDownCamera;
        }
    }
}
