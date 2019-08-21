using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CaAirControl : CaAbility
{
    
    //Tweakable Variables
    public float airControlFactor;
    public Vector3 turnAxis;
    public float torqueAmount;
    
    //Internal Variables
    public float currentInput = 0;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = GetComponent<CpMain>();
    }
    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        DoAbility();
    }

    public override void CheckInput()
    {
        currentInput = Input.GetAxis(axisKey);
    }

    public override void DoAbility()
    {
        if (Math.Abs(currentInput) < 0.01f)
            return;

        if (_cpMain.wheelData.grounded || _cpMain.averageColliderSurfaceNormal!=Vector3.zero)
            return;

        float rotationTorque = currentInput * torqueAmount * Time.fixedDeltaTime * airControlFactor;
        _cpMain.rb.AddRelativeTorque(turnAxis * rotationTorque, ForceMode.VelocityChange);
    }
} 