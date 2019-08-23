using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchToLoopCamera : MonoBehaviour
{
    public CinemachineVirtualCamera loopCamera;

    public bool PlayerOnLoop =false;
    private float offLoopTimerAmount = 0.2f;
    private float currentTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerOnLoop && currentTimer<=0)
        {
            loopCamera.Priority = 0;
        }
        else if (!PlayerOnLoop && currentTimer>0)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            loopCamera.Priority = 20;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayerOnLoop = true;
    }

    private void OnCollisionExit(Collision other)
    {
        currentTimer = offLoopTimerAmount;
        PlayerOnLoop = false;
    }
}
