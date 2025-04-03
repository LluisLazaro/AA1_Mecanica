using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planetari : MonoBehaviour
{
    public Vector3 planetPosition;
    public Vector3 planetVelocity;

    public float Mass; // Massa realista (en unitats relatives al Sol)
    private TrailRenderer trailRenderer;
    public float stepTime = 0.0001f;
    private float time = 0;

    private Vector3 netAcceleration;
    private bool hasActivatedTrail = false;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        Initialize(planetPosition, planetVelocity);
    }

    public void Initialize(Vector3 initialPosition, Vector3 initialVelocity)
    {
        planetPosition = initialPosition;
        planetVelocity = initialVelocity;
        planetPosition = planetPosition * 2f; // escala per visualització
    }

    public void SetAcceleration(Vector3 acc)
    {
        netAcceleration = acc;
    }

    public void SimulateStep()
    {
        (planetPosition, planetVelocity, time) = RungeKutta4(planetPosition, planetVelocity, time);
        transform.position = planetPosition * 10f;
    }

    (Vector3, Vector3, float) RungeKutta4(Vector3 position, Vector3 velocity, float time)
    {
        Vector3 K1position, K1velocity, K2position, K2velocity, K3position, K3velocity, K4position, K4velocity;

        (K1position, K1velocity) = DifferentialFunction(position, velocity, netAcceleration);
        (K2position, K2velocity) = DifferentialFunction(position + 0.5f * stepTime * K1position, velocity + 0.5f * stepTime * K1velocity, netAcceleration);
        (K3position, K3velocity) = DifferentialFunction(position + 0.5f * stepTime * K2position, velocity + 0.5f * stepTime * K2velocity, netAcceleration);
        (K4position, K4velocity) = DifferentialFunction(position + stepTime * K3position, velocity + stepTime * K3velocity, netAcceleration);

        Vector3 newPosition = position + (stepTime / 6f) * (K1position + 2f * K2position + 2f * K3position + K4position);
        Vector3 newVelocity = velocity + (stepTime / 6f) * (K1velocity + 2f * K2velocity + 2f * K3velocity + K4velocity);
        float newTime = time + stepTime;

        if (!hasActivatedTrail)
        {
            trailRenderer.enabled = true;
            hasActivatedTrail = true;
        }

        return (newPosition, newVelocity, newTime);
    }

    (Vector3, Vector3) DifferentialFunction(Vector3 position, Vector3 velocity, Vector3 acceleration)
    {
        return (velocity, acceleration);
    }

    public void SetStepTime(float newStepTime)
    {
        stepTime = newStepTime;
    }

}
