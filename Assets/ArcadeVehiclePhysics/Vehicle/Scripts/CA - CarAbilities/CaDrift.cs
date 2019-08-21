using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaDrift : CaAbility
{
    //Tweakable values
    public AnimationCurve driftCurve;
    float _driftInDuration = 0.5f;
    float _driftOutDuration = 1f;

    //Internal variables
    private float _currentDriftTime;
    public float _currentDriftFactor;
    public bool isDrifting;
    
    
    private CpMain _cpMain;
    private CpLateralFriction _cpLateralFriction;

    private void Awake()
    {
        _cpMain = GetComponent<CpMain>();
        _cpLateralFriction = GetComponentInChildren<CpLateralFriction>();
    }

    private void Update()
    {
        CheckInput();
        UpdateAbility();
    }

    private void FixedUpdate()
    {
        //DoAbility();
    }

    public override void CheckInput()
    {
        if (_cpMain.wheelData.grounded)
        {
            isDrifting = Input.GetKey(abilityButton);
        }
    }

    private void UpdateAbility()
    {
        if (isDrifting)
        {
            _currentDriftTime += Time.deltaTime * 1 / _driftInDuration;
        }
        else if (_currentDriftTime > 0)
        {
            _currentDriftTime -= Time.deltaTime * 1 / _driftOutDuration;
        }

        _currentDriftTime = Mathf.Clamp01(_currentDriftTime);
        _currentDriftFactor = driftCurve.Evaluate(_currentDriftTime);
        
        _cpLateralFriction.currentTireStickiness =_cpLateralFriction.baseTireStickiness * _currentDriftFactor;
    }

    public override void DoAbility()
    {
        bool belowBaseTireStickiness =  _cpLateralFriction.currentTireStickiness < _cpLateralFriction.baseTireStickiness;
    
        if (belowBaseTireStickiness && 
            !isDrifting && 
            _cpMain.wheelData.grounded
            )
        {
            //This is to try recover some lost speed while drifting
            _cpMain.rb.AddForce(Mathf.Abs(_cpLateralFriction.slidingFrictionForceAmount) * _cpMain.rb.transform.forward, ForceMode.Acceleration);
        }
    }
}
