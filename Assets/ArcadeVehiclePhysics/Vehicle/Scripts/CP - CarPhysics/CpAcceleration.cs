using System;
using UnityEngine;

public class CpAcceleration : MonoBehaviour
{
    [Header("Acceleration")] 
    public AnimationCurve velocityTimeCurve;

    public float accelerationToApply;
    public float currentTimeValue;
    public float nextTimeValue;
    public float nextVelocityMagnitude;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _cpMain.speedData = new VehicleSpeed(velocityTimeCurve.keys[velocityTimeCurve.length - 1].value);
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateSpeedData(_cpMain.rb, _cpMain.speedData);
        accelerationToApply = GetAccelerationFromVelocityTimeCurve(velocityTimeCurve, _cpMain.input, _cpMain.speedData);
    }

    private void FixedUpdate()
    {
        float inputScaledAccel = Mathf.Abs(_cpMain.input.accelInput) * accelerationToApply;
        ApplyAcceleration(inputScaledAccel, _cpMain.rb, _cpMain.wheelData.grounded);
    }

    private void CalculateSpeedData(Rigidbody rb, VehicleSpeed speedData)
    {
        speedData.sideSpeed = Vector3.Dot(rb.transform.right, rb.velocity);
        speedData.forwardSpeed = Vector3.Dot(rb.transform.forward, rb.velocity);
        speedData.speed = rb.velocity.magnitude;
    }

    //An Alternative to the Vel-Time Curve approach
    //This works by adjusting the force applied according to how fast the car is moving
    //Top speed is defined on the curve by the value of the first key
    private float GetForceFromVelocityForceCurve(AnimationCurve velocityForceCurve, PlayerInputs input,
        VehicleSpeed speedData)
    {
        if (Math.Abs(input.accelInput) < 0.05f)
            return 0;

        float curveTopSpeed = velocityForceCurve.keys[0].value;
        float velocityForceCurveEvaluation = velocityForceCurve.Evaluate(speedData.forwardSpeed / curveTopSpeed);

        return velocityForceCurveEvaluation * curveTopSpeed;
    }

    //This process works using reverse evaluation of a Velocity-Time curve
    //Binary search using current forward speed to find the time value on the graph
    //Add one time step onto that time value and evaluate the graph to get the new velocity
    //Calculate a = (Vf - Vi)/deltaTime
    //Return the new force to apply (Must use ForceMode.Acceleration to ignore mass)
    //timeScaler is the scale of the time buckets used in the binary search as it uses Ints
    private float GetAccelerationFromVelocityTimeCurve(AnimationCurve velocityTime, PlayerInputs input,
        VehicleSpeed speedData)
    {
        if (speedData.forwardSpeed > velocityTime.keys[velocityTime.length - 1].value)
            return 0;

        float speedClamped = Mathf.Clamp(
            speedData.forwardSpeed,
            velocityTime.keys[0].value,
            velocityTime.keys[velocityTime.length - 1].value);

        currentTimeValue = BinarySearchDisplay(velocityTime, speedClamped);

        if (currentTimeValue != -1)
        {
            float inputDir = input.accelInput > 0 ? 1 : -1;
            nextTimeValue = currentTimeValue + inputDir * Time.fixedDeltaTime;
            nextTimeValue = Mathf.Clamp(nextTimeValue, velocityTime.keys[0].time,
                velocityTime.keys[velocityTime.length - 1].time);

            nextVelocityMagnitude = velocityTime.Evaluate(nextTimeValue);
            float accelMagnitude = (nextVelocityMagnitude - speedData.forwardSpeed) / (Time.fixedDeltaTime);

            return accelMagnitude;
        }
        
        return 0;
    }

    private float BinarySearchDisplay(AnimationCurve velTimeCurve, float currentVel)
    {
        const int timeScale = 10000;
        
        int minTime = (int)(velTimeCurve.keys[0].time * timeScale);
        int maxTime = (int)(velTimeCurve.keys[velTimeCurve.length - 1].time * timeScale);
        int numSteps = 0;

        while (minTime <= maxTime)
        {
            int mid = (minTime + maxTime) / 2;

            float scaledMid = (float) mid / timeScale;
            if (Mathf.Abs(velTimeCurve.Evaluate(scaledMid) - currentVel) <= 0.01f)
            {
                //Debug.Log(string.Format("Final mid: {0}", mid));
                return (float)mid/timeScale;
            }
            
            if (currentVel < velTimeCurve.Evaluate(scaledMid))
            {
                maxTime = mid - 1;
            }
            else
            {
                minTime = mid + 1;
            }

            //Debug.Log(string.Format("minTime: {0}   maxTime:{1}   mid: {2}   numSteps: {3}", minTime, maxTime, mid, numSteps));
            numSteps += 1;
        }

        //Debug.Log("[BinarySearchDisplay] Something went wrong with the BinarySearch - Returning -1");
        return -1;
    }

    private void ApplyAcceleration(float accelToApply, Rigidbody rb, bool grounded)
    {
        if (!grounded)
            return;

        //Note sign has been accounted for when calculating acceleration
        Vector3 force = rb.transform.forward * accelToApply; 
        rb.AddForce(force, ForceMode.Acceleration);
    }
}

[Serializable]
public class VehicleSpeed
{
    public float speed;
    public float forwardSpeed;
    public float sideSpeed;
    public float topSpeed;

    //Percent of top speed
    public float ForwardSpeedPercent => Mathf.Abs(forwardSpeed / topSpeed);
    public float SideSpeedPercent => Mathf.Abs(sideSpeed / topSpeed);
    public float SpeedPercent => Mathf.Abs(speed / topSpeed);

    public VehicleSpeed(float topSpeed)
    {
        this.topSpeed = topSpeed;
    }
}