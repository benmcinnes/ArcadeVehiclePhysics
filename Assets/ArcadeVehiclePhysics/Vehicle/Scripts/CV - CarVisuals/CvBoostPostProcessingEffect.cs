using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CvBoostPostProcessingEffect : MonoBehaviour
{
    public PostProcessProfile _profile;
    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;

    private CpMain _cpMain;
    private CaBoost _caBoost;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
        _caBoost = transform.parent.GetComponent<CaBoost>();
        
        _chromaticAberration = _profile.GetSetting<ChromaticAberration>();
        _lensDistortion = _profile.GetSetting<LensDistortion>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_caBoost.isBoosting)
        {

            _chromaticAberration.intensity.value =
                Mathf.Lerp(_chromaticAberration.intensity.value, 1, Time.deltaTime * 5);
            _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, -20f, Time.deltaTime * 5);
        }
        else
        {
            _chromaticAberration.intensity.value =
                Mathf.Lerp(_chromaticAberration.intensity.value, 0, Time.deltaTime *10);
            _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, 0f, Time.deltaTime * 10);
        }
    }
}
