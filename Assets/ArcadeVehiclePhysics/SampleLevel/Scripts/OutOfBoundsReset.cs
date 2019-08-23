using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsReset : MonoBehaviour
{
    private CpMain _cpMain;
    
    // Start is called before the first frame update
    void Start()
    {
        _cpMain = FindObjectOfType<CpMain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer(_cpMain.rb);
        }

    }

    private void ResetPlayer(Rigidbody rb )
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = Vector3.up;
        rb.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerExit(Collider other)
    {
        ResetPlayer(_cpMain.rb);
    }
}
