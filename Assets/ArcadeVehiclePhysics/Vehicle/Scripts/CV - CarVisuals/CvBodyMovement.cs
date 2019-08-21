using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CvBodyMovement : MonoBehaviour
{
    public Transform modelBase;
    public Vector3 modelBaseOffset;

    public BodyAxisMovement roll;
    public BodyAxisMovement pitch;
    
    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponentInChildren<CpMain>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCarBodyTransform();
    }
    
    private void UpdateCarBodyTransform()
    {
        transform.position = _cpMain.rb.position;
        transform.rotation = _cpMain.rb.rotation;

        //CarBody Roll - Steering and Side Speed
        roll.currentAngle = Mathf.Lerp(roll.currentAngle, roll.inputMaxAngle * _cpMain.input.steeringInput, Time.deltaTime*10);
        float currentBodyRoll = roll.currentAngle - _cpMain.speedData.SideSpeedPercent * roll.speedMaxAngle;
        
        //CarBody Pitch - Accel and Forward Speed
        pitch.currentAngle = Mathf.Lerp(pitch.currentAngle, _cpMain.input.accelInput * pitch.inputMaxAngle, Time.deltaTime*10);
        float currentBodyPitch = pitch.currentAngle + Mathf.Clamp01(_cpMain.speedData.ForwardSpeedPercent) * pitch.speedMaxAngle;

        modelBase.rotation = _cpMain.rb.rotation * Quaternion.Euler(currentBodyPitch, 0, currentBodyRoll);
    }
}

[System.Serializable]
public class BodyAxisMovement
{
    public float currentAngle;
    public float inputMaxAngle;
    public float speedMaxAngle;
}
