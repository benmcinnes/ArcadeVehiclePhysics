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

    public CpMain cpMain;
    private VehicleSpeed _speedData;

    private CaBoost CaBoost;
    public Image BoostBar;
    
    private CaJump CaJump;
    public Image JumpCharge;

    // Start is called before the first frame update
    void Start()
    {
        if (cpMain == null)
        {
            cpMain = FindObjectOfType<CpMain>();
        }

        CaBoost = cpMain.GetComponent<CaBoost>();
        CaJump = cpMain.GetComponent<CaJump>();
    }

    // Update is called once per frame
    void Update()
    {
        _speedData = cpMain.speedData;

        float speedPercentage = (_speedData.ForwardSpeedPercent);
        UpdateSpeedBar(speedPercentage, forwardSpeedBar, forwardSpeedText,900);
        
        float velMagPercentage = new Vector2(_speedData.forwardSpeed, _speedData.sideSpeed).magnitude / _speedData.topSpeed;
        UpdateSpeedBar(velMagPercentage, velMagBar, velMagText,910);
        velMagText.rectTransform.localPosition += Vector3.up * 50;
        
        float sideSpeedPercentage = _speedData.SideSpeedPercent;
        UpdateSpeedBar(sideSpeedPercentage, sideSpeedBar, sideSpeedText, 900);

        
        if (CaBoost!=null)
        {
            BoostBar.rectTransform.sizeDelta = new Vector2(BoostBar.rectTransform.sizeDelta.x, 900 * (CaBoost.currentBoostTimeLeft/CaBoost.boostTimeMax));
        }
        
        if (CaJump!=null)
        {
            JumpCharge.rectTransform.sizeDelta = new Vector2(JumpCharge.rectTransform.sizeDelta.x, 900 * (CaJump.currentCharge/CaJump.forceMax));
        }
    }

    private void UpdateSpeedBar(float valuePercentage, Image bar, TextMeshProUGUI text, int imageSize)
    {
        text.text = ((int)(valuePercentage * _speedData.topSpeed)).ToString();

        valuePercentage = Mathf.Clamp01(valuePercentage);
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, -imageSize/2 + valuePercentage * imageSize, text.rectTransform.localPosition.z);
        bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, imageSize * valuePercentage);
    }
    
}
