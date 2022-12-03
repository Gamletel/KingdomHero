using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBig : Enemy
{
    [SerializeField] private ParticleSystem _leftStepParticle;
    [SerializeField] private ParticleSystem _rightStepParticle;

    protected void LeftLegStep()
    {
        _leftStepParticle.Play();
    }

    protected void RightLefStep()
    {
        _rightStepParticle.Play();
    }
}
