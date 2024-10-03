using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    private RaceCarController _raceCarController;

    private TrailRenderer Trail;

    private ParticleSystem smoke;

    private ParticleSystem.EmissionModule smokeEmission;
    // Start is called before the first frame update
    void Awake()
    {
        Trail = GetComponent<TrailRenderer>();
        _raceCarController = GetComponentInParent<RaceCarController>();
        smoke = GetComponentInChildren<ParticleSystem>();
        smokeEmission = smoke.emission;
        Trail.emitting = false;
        smokeEmission.rateOverTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_raceCarController.isScreeching(out float latVel, out bool braking))
        {
            Trail.emitting = true;
            smokeEmission.rateOverTime = 50f;

        }
        else
        {
            Trail.emitting = false;
            smokeEmission.rateOverTime = 0f;
        }
    }
}
