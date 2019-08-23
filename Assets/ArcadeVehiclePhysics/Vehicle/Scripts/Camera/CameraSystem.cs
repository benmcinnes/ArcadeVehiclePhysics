using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineVirtualCamera carCamGround;
    private CinemachineTransposer _carCamTransposerGround;
    private CinemachineBasicMultiChannelPerlin _camNoiseGround;

    public CinemachineVirtualCamera carCamAir;
    private CinemachineTransposer _carCamTransposerAir;
    private CinemachineBasicMultiChannelPerlin _camNoiseAir;
    
    public AnimationCurve carCamSpeedCurve;

    public CinemachineVirtualCamera currentVCam;
    public CinemachineTransposer currentTransposer;
    public CinemachineBasicMultiChannelPerlin currentNoise;

    public FloatRange fov = new FloatRange(30, 60);
    public FloatRange zPos = new FloatRange(-22, -16f);
    public FloatRange zDamp = new FloatRange(0.6f, 0.1f);
    public FloatRange yDamp = new FloatRange(0.8f, 0.4f);
    
    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = GetComponent<CpMain>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _carCamTransposerGround = carCamGround.GetCinemachineComponent<CinemachineTransposer>();
        _carCamTransposerAir = carCamAir.GetCinemachineComponent<CinemachineTransposer>();
        _camNoiseGround = carCamGround.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _camNoiseAir = carCamAir.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CpMain.OnLanding += SwitchToGroundCamera;
        CpMain.OnLeavingGround += SwitchToAirCamera;

        SwitchToGroundCamera(_cpMain);
    }

    // Update is called once per frame
    void Update()
    {
        SpeedAdjustment();
    }
    
    public void SwitchToGroundCamera(CpMain cpMain)
    {
        if (cpMain.rb == _cpMain.rb)
        {
            carCamGround.Priority = 11;
            currentTransposer = _carCamTransposerGround;
            currentVCam = carCamGround;
            currentNoise = _camNoiseGround;
        }
    }

    public void SwitchToAirCamera(CpMain cpMain)
    {
        if (cpMain.rb == _cpMain.rb)
        {
            carCamGround.Priority = 9;
            currentTransposer = _carCamTransposerAir;
            currentVCam = carCamAir;
            currentNoise = _camNoiseAir;
        }
    }

    private void SpeedAdjustment()
    {
        //Car cam speed settings
        float currentSpeedPercentage = carCamSpeedCurve.Evaluate(_cpMain.speedData.ForwardSpeedPercent);

        currentVCam.m_Lens.FieldOfView = fov.GetCurrentWithPercent(currentSpeedPercentage);
        
        zPos.GetCurrentWithPercent(currentSpeedPercentage);
        currentTransposer.m_FollowOffset = new Vector3(0, 5, zPos.current);
        currentTransposer.m_YDamping = yDamp.GetCurrentWithPercent(currentSpeedPercentage);
        currentTransposer.m_ZDamping = zDamp.GetCurrentWithPercent(currentSpeedPercentage);
    }
}


[System.Serializable]
public class FloatRange
{
    public float min;
    public float max;
    public float current;

    public float Range => (max - min);


    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GetCurrentWithPercent(float percentage)
    {
        current = min + percentage * Range;
        return current;
    }
}
