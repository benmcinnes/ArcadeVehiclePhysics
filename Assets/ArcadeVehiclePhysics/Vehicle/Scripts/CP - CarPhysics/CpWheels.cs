using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpWheels : MonoBehaviour
{
    private CpMain _cpMain;
    private Dictionary<Transform, WheelHitData> _mapWheelToLastHitCache = new Dictionary<Transform, WheelHitData>();

    private class WheelHitData
    {
        public bool IsGrounded;
        public RaycastHit GroundData;
    }

    //Wheel Variables 
    public float wheelHeight;
    public LayerMask groundCheckLayer;
    [Space]

    public CpWheelData cpWheelData;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }

    private void Start()
    {
        foreach (var wheel in cpWheelData.physicsWheelPoints)
        {
            _mapWheelToLastHitCache[wheel] = new WheelHitData();
        }
    }

    private void Update()
    {
        UpdateWheelStates();
        _cpMain.wheelData = cpWheelData;
    }

    private void UpdateWheelStates()
    {
        Vector3 surfaceNormal = Vector3.zero;

        cpWheelData.numberOfGroundedWheels = 0;
        cpWheelData.grounded = false;
        RaycastHit hit;
        foreach (var wheel in cpWheelData.physicsWheelPoints)
        {
            bool didhit = Physics.Raycast(wheel.position, -wheel.transform.up, out hit, wheelHeight, groundCheckLayer);

            WheelHitData wheelHitData = _mapWheelToLastHitCache[wheel];

            wheelHitData.IsGrounded = didhit;
            wheelHitData.GroundData = hit;

            if (!didhit)
                continue;

            cpWheelData.grounded = true;
            cpWheelData.numberOfGroundedWheels += 1;

            surfaceNormal += hit.normal;
        }

        cpWheelData.averageWheelSurfaceNormal = surfaceNormal.normalized;
    }
}

[System.Serializable]
public class CpWheelData
{
    public Transform[] physicsWheelPoints;
    public bool grounded;
    public int numberOfGroundedWheels;
    public Vector3 averageWheelSurfaceNormal;
}