using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CvCollisionParticle : MonoBehaviour
{
    public ParticleSystem collisionDustParticles;
    public float currentDeadTime=1f;

/*    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.EmissionModule _emissionModule;*/

    private void Awake()
    {
/*        _mainModule = collisionDustParticles.main;
        _emissionModule = collisionDustParticles.emission;*/
    }

    // Start is called before the first frame update
    void Start()
    {
        CpColliderData.OnCollision+=PlayCollisionParticles;
        
    }

    void Update()
    {
        if (currentDeadTime > 0)
        {
            currentDeadTime -= Time.deltaTime;
        }
    }


    public void PlayCollisionParticles(Collision collision)
    {
        if (currentDeadTime <= 0)
        {
            float impulseMag = collision.impulse.magnitude;
            var mainModule = collisionDustParticles.main;
            mainModule.startSpeed = 5 + impulseMag / 5;
            
            mainModule.startSize =  Mathf.Clamp01( impulseMag / 20);
            
            var emissionModule = collisionDustParticles.emission;
            var burst = emissionModule.GetBurst(0);
            burst.count = 10 + (int)impulseMag / 5;
            
            collisionDustParticles.transform.position = collision.contacts[0].point;
            collisionDustParticles.transform.rotation = Quaternion.Euler(collision.contacts[0].normal);
            collisionDustParticles.Play();
            currentDeadTime += 0.05f;
        }
    }
}
