using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevolutionCounterController : MonoBehaviour
{
    public TextMeshProUGUI RevText;

    public float amountOfRotation;
    public float rotationInDegrees;
    public int NumberOfRevolutions;

    private CpMain _CPMain;

    private void Awake()
    {
        _CPMain = FindObjectOfType<CpMain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CpMain.OnLeavingGround += StartCountingRevs;
        CpMain.OnLanding += StopCountingRevolutions;
    }

    // Update is called once per frame
    void Update()
    {
        bool isCountingRevolutions =  _CPMain !=null ? true : false;
        RevText.gameObject.SetActive(isCountingRevolutions);

        if (isCountingRevolutions && !_CPMain.wheelData.grounded)
        {
            amountOfRotation += (_CPMain.rb.angularVelocity.y * Time.deltaTime);
            rotationInDegrees = amountOfRotation * 180 / Mathf.PI;
            NumberOfRevolutions = (int)(rotationInDegrees / 180) * 180; 
            RevText.text = (NumberOfRevolutions).ToString();
        }
    }

    public void StartCountingRevs(CpMain _CPMain)
    {
        this._CPMain = _CPMain;
        amountOfRotation = 0;
    }

    public void StopCountingRevolutions(CpMain _CPMain)
    {
        Debug.Log("Stopping counting Revolutions");
    }
}
