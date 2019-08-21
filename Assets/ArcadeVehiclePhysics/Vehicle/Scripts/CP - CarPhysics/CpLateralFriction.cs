using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpLateralFriction : MonoBehaviour
{
    //TweakableVariables
    public AnimationCurve slideFrictionCurve;
    [Range(0, 1)]
    public float baseTireStickiness;
    [Space]
    public float currentTireStickiness;
    [Space]
    public float slidingFrictionRatio;
    public float slidingFrictionForceAmount;
    public float slidingFrictionToForwardSpeedAmount;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        CalculateLateralFriction(_cpMain.speedData);
    }

    private void FixedUpdate()
    {
        ApplyLateralFriction(_cpMain.wheelData.grounded, _cpMain.rb);
    }

    private void CalculateLateralFriction(VehicleSpeed speedData)
    {
        float slideFrictionRatio = 0;

        if (Math.Abs(speedData.sideSpeed + speedData.forwardSpeed) > 0.01f)
            slideFrictionRatio = Mathf.Clamp01(Mathf.Abs(speedData.sideSpeed) / (Mathf.Abs(speedData.sideSpeed) + speedData.forwardSpeed));

        slidingFrictionRatio = slideFrictionCurve.Evaluate(slideFrictionRatio);

        //TODO: Factor in surface normal - will make car more slippery non-horizontal surfaces

        slidingFrictionForceAmount = slidingFrictionRatio * -speedData.sideSpeed * currentTireStickiness;
    }

    private void ApplyLateralFriction(bool grounded, Rigidbody rb)
    {
        if (!grounded)
            return;

        //Stops sideways sliding 
        rb.AddForce(slidingFrictionForceAmount * rb.transform.right, ForceMode.Impulse);
        currentTireStickiness = baseTireStickiness;
    }
}
