using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpColliderData : MonoBehaviour
{
    private CpMain _cpMain;
    
    public static event Action<Collision> OnCollision = collisionPosition => { };

    private void Awake()
    {
        _cpMain = transform.parent.GetComponent<CpMain>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        OnCollision(other);
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 surfaceNormalSum = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
        {
            surfaceNormalSum += collision.contacts[i].normal;
        }
        _cpMain.averageColliderSurfaceNormal = surfaceNormalSum.normalized;
    }

    private void OnCollisionExit(Collision collision)
    {
        _cpMain.averageColliderSurfaceNormal = Vector3.zero;
    }
}
