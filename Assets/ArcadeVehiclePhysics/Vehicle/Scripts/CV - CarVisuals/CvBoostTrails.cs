using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CvBoostTrails : MonoBehaviour
{
    public TrailRenderer leftLightTrail;
    public TrailRenderer rightLightTrail;
    

    private CaBoost _caBoost;
    private CpMain _cpMain;
    // Start is called before the first frame update

    private void Awake()
    {
        _caBoost = transform.parent.GetComponent<CaBoost>();
        _cpMain = transform.parent.GetComponent<CpMain>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_caBoost!=null)
        {
            leftLightTrail.emitting = _caBoost.isBoosting || _cpMain.speedData.ForwardSpeedPercent>1;
            rightLightTrail.emitting = _caBoost.isBoosting|| _cpMain.speedData.ForwardSpeedPercent>1;
        }
    }
}
