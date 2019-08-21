using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpStabilityForces : MonoBehaviour
{
    [Header("Stability Forces")]
    public float linearStabilityForce = 500;
    public float angularStabilityForce = 1000;

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
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyLinearStabilityForces(
            _cpMain.rb,
            _cpMain.wheelData.physicsWheelPoints,
            _cpMain.wheelData.grounded,
            _cpMain.wheelData.numberOfGroundedWheels
            );

        ApplyAngularStabilityForces(
            _cpMain.rb,
            _cpMain.averageColliderSurfaceNormal,
            _cpMain.wheelData.grounded
            );
    }

    private void ApplyLinearStabilityForces(Rigidbody rigidbody, Transform[] physicsWheelPoints, bool grounded, int numberOfGroundedWheels)
    {
        if (linearStabilityForce > 0 && grounded && numberOfGroundedWheels < 3)
        {
            Vector3 downwardForce = linearStabilityForce * Vector3.down * Time.fixedDeltaTime;
            foreach (var wheel in physicsWheelPoints)
            {
                rigidbody.AddForceAtPosition(downwardForce, wheel.position, ForceMode.Acceleration);
            }
        }
    }

    private void ApplyAngularStabilityForces(Rigidbody rigidbody, Vector3 averageColliderSurfaceNormal, bool grounded)
    {
        if (averageColliderSurfaceNormal != Vector3.zero && !grounded)
        {
            //Gets the angle in order to determine the direction the vehicle needs to roll
            float angle = Vector3.SignedAngle(rigidbody.transform.up, averageColliderSurfaceNormal, rigidbody.transform.forward);

            //Angular stability only uses roll - Using multiple axis becomes unpredictable 
            Vector3 torqueAmount = Mathf.Sign(angle) * rigidbody.transform.forward * angularStabilityForce * Time.fixedDeltaTime;

            rigidbody.AddTorque(torqueAmount, ForceMode.Acceleration);
        }
    }
}
