using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpTurning : MonoBehaviour
{
    public float baseTurningForce;
    public float speedFactorOffset = 0.25f;
    [Space]
    public float currentTurningForce;
    public Vector3 currentAngularVelocity;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTurningForce = baseTurningForce;
    }

    private void Update()
    {
        currentAngularVelocity = _cpMain.rb.angularVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyTurningForce(
            _cpMain.input,
            _cpMain.speedData,
            _cpMain.rb,
            _cpMain.wheelData.grounded
            );
    }

    //TODO Make a version of this that works with the Vel-Time Curve
    private void ApplyTurningForce(PlayerInputs input, VehicleSpeed speedData, Rigidbody rigidbody, bool grounded)
    {
        if (input.steeringInput == 0)
            return;

        if (!grounded)
            return;

        //Adjusts turning with speed
        float speedFactor = Mathf.Clamp01(speedData.SpeedPercent + speedFactorOffset);
        float rotationTorque = input.steeringInput * baseTurningForce * speedFactor * Time.fixedDeltaTime;

        //Apply the torque to the ship's Y axis
        rigidbody.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);
    }
}
