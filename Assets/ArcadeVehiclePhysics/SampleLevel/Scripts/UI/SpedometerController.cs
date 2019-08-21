using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpedometerController : MonoBehaviour
{
    public TextMeshProUGUI forwardSpeedText;
    public Image forwardSpeedBar;
    public TextMeshProUGUI sideSpeedText;
    public Image sideSpeedBar;
    public TextMeshProUGUI velMagText;
    public Image velMagBar;
    //public TextMeshProUGUI vertText;
    //public Image vertBar;

    public CpMain _CPMain;
    private VehicleSpeed _speedData;

    // Start is called before the first frame update
    void Start()
    {
        if (_CPMain == null)
        {
            _CPMain = FindObjectOfType<CpMain>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _speedData = _CPMain.speedData;

        float speedPercentage = (_speedData.ForwardSpeedPercent);
        UpdateSpeedBar(speedPercentage, forwardSpeedBar, forwardSpeedText);

        float sideSpeedPercentage = Mathf.Abs(_speedData.SideSpeedPercent);
        UpdateSpeedBar(sideSpeedPercentage, sideSpeedBar, sideSpeedText);

        float velMagPercentage = new Vector2(_speedData.forwardSpeed, _speedData.sideSpeed).magnitude / _speedData.topSpeed;
        UpdateSpeedBar(velMagPercentage, velMagBar, velMagText);

        //float vertPercentage = carPhysics.currentGlobalVelocity.y;
        //UpdateVerticalSpeedBar(vertPercentage, vertBar, vertText);
    }

    private void UpdateSpeedBar(float valuePercentage, Image bar, TextMeshProUGUI text)
    {
        text.text = ((int)(valuePercentage * _speedData.topSpeed)).ToString();

        valuePercentage = Mathf.Clamp01(valuePercentage);
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, -500 + valuePercentage * 1000, text.rectTransform.localPosition.z);
        bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, 1000 * valuePercentage);
    }

    //private void UpdateVerticalSpeedBar(float value, Image bar, TextMeshProUGUI text)
    //{
    //    text.text = ((int)carPhysics.ForwardSpeed).ToString();
    //    text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, value * 1000, text.rectTransform.localPosition.z);
    //    bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, 1000 * value);
    //}
}
