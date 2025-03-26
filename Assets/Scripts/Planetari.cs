using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planetari : MonoBehaviour
{
    private Vector3 planetPosition;
    private Vector3 planetVelocity;
    public float Mass = 1.66f + Mathf.Pow(10, -7);

    public float stepTime = 0.0001f;
    private float GM = 39.478f;
    public Vector3 initialVelocity = new Vector3(0, 0, 10.07f); 
    public Vector3 initialPosition = new Vector3(0.39f, 0, 0); 
    private float time = 0;

    void Start()
    {
        transform.position = initialPosition;
        planetPosition = initialPosition;
        planetVelocity = initialVelocity;
        time = 0;
    }

    void Update()
    {
        
        (planetPosition, planetVelocity, time) = RungeKutta4(planetPosition, planetVelocity, time);
        transform.position = (planetPosition * 10f);
    }

    Vector3 GravitationalAcceleration(Vector3 position)
    {
        float distanceSquared = position.sqrMagnitude;
        Vector3 directionVector = -position.normalized;
        Vector3 acceleration = (GM / distanceSquared) * directionVector; ;
        return acceleration;
    }

    (Vector3, Vector3, float) RungeKutta4(Vector3 position, Vector3 velocity, float time)
    {
        Vector3 K1position, K1velocity, K2position, K2velocity, K3position, K3velocity, K4position, K4velocity;

        (K1position, K1velocity) = DifferentialFunction(position, velocity);
        (K2position, K2velocity) = DifferentialFunction(position + 0.5f * stepTime * K1position, velocity + 0.5f * stepTime * K1velocity);
        (K3position, K3velocity) = DifferentialFunction(position + 0.5f * stepTime * K2position, velocity + 0.5f * stepTime * K2velocity);
        (K4position, K4velocity) = DifferentialFunction(position + stepTime * K3position, velocity + stepTime * K3velocity);

        Vector3 newPosition = position + (stepTime / 6f) * (K1position + 2f * K2position + 2f * K3position + K4position);
        Vector3 newVelocity = velocity + (stepTime / 6f) * (K1velocity + 2f * K2velocity + 2f * K3velocity + K4velocity);
        float newTime = time + stepTime;

        return (newPosition, newVelocity, newTime);
    }

    private (Vector3 position, Vector3 velocity) DifferentialFunction(Vector3 position, Vector3 velocity)
    {
        return (velocity, GravitationalAcceleration(position));
    }
}
