using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpDrag : MonoBehaviour
{
    [Header("Drag")]
    public float linearDrag;
    public float freeWheelDrag;
    public float brakingDrag;
    public float angularDrag;

    public bool linearDragCheck;
    public bool brakingDragCheck;
    public bool freeWheelDragCheck;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();

        _cpMain.rb.angularDrag = angularDrag;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDrag(
            _cpMain.rb,
            _cpMain.wheelData.grounded,
            _cpMain.input,
            _cpMain.speedData
        );

    }

    private void UpdateDrag(Rigidbody rb, bool grounded, PlayerInputs input, VehicleSpeed speedData)
    {
        linearDragCheck = Mathf.Abs(input.accelInput) < 0.05 || grounded;
        float linearDragToAdd = linearDragCheck ? linearDrag : 0;

        brakingDragCheck = input.accelInput < 0 && speedData.forwardSpeed > 0;
        float brakingDragToAdd = brakingDragCheck ? brakingDrag : 0;
        
        freeWheelDragCheck = Math.Abs(input.accelInput) < 0.02f && grounded;
        float freeWheelDragToAdd = freeWheelDragCheck ? freeWheelDrag : 0;

        rb.drag = linearDragToAdd + brakingDragToAdd + freeWheelDragToAdd;
    }
}
