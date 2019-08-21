using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpDrag : MonoBehaviour
{
    [Header("Drag")]
    public float linearDrag;
    public float brakingDrag;
    public float angularDrag;

    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyDrag(
            _cpMain.rb,
            _cpMain.wheelData.grounded,
            _cpMain.input,
            _cpMain.speedData
            );
    }

    private void ApplyDrag(Rigidbody rigidbody, bool grounded, PlayerInputs input, VehicleSpeed speedData)
    {
        bool linearDragCheck = Mathf.Abs(input.accelInput) < 0.05 || grounded;
        float linearDrag = linearDragCheck ? this.linearDrag : 0;

        bool brakingDragCheck = input.accelInput < 0 && speedData.forwardSpeed > 0;
        float brakingDrag = brakingDragCheck ? this.brakingDrag : 0;

        rigidbody.drag = linearDrag + brakingDrag;
    }
}
