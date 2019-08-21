using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CvWheelsBase : MonoBehaviour
{

    [SerializeField] private GameObject wheelPrefab;
    [SerializeField] private Transform[] wheelAttachPoints;    //Different to the placement of physics wheels
    [SerializeField] private List<CvWheels> wheelVisuals = new List<CvWheels>();
    
    private CpMain _cpMain;

    private void Awake()
    {
        _cpMain = transform.parent.GetComponentInChildren<CpMain>();
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitWheels();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (CvWheels wheelVisual in wheelVisuals)
        {
            wheelVisual.ProcessWheelVisuals(_cpMain.input, _cpMain.speedData);
        }
    }

    private void InitWheels()
    {
        foreach (Transform wheelAttachPoint in wheelAttachPoints)
        {
            CvWheels wheelVisual = Instantiate(wheelPrefab, wheelAttachPoint.position, wheelAttachPoint.rotation, wheelAttachPoint).GetComponent<CvWheels>();
            wheelVisual.SetUpWheel(_cpMain.rb);
            wheelVisuals.Add(wheelVisual);
        }
    }
}
