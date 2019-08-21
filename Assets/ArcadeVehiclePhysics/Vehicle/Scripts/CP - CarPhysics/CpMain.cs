using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpMain : MonoBehaviour
{
    public PlayerInputs input;
    public VehicleSpeed speedData;
    public CpWheelData wheelData;

    [HideInInspector] public Rigidbody rb;
    public Vector3 averageColliderSurfaceNormal;

    private bool _prevGroundedState;
    public static event Action<CpMain> OnLeavingGround = cpMain => { };
    public static event Action<CpMain> OnLanding = cpMain => { };
    
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (_prevGroundedState == false && wheelData.grounded)
        {
            OnLanding(this);
        }
        else if (_prevGroundedState == true && !wheelData.grounded)
        {
            OnLeavingGround(this);
        }

        _prevGroundedState = wheelData.grounded;
    }
}
