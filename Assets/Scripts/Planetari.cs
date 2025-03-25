using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planetari : MonoBehaviour
{
    public Transform sun; 
    public GameObject[] planets; 

    private List<Vector3> velocities = new List<Vector3>();
    private List<Transform> planetTransforms = new List<Transform>();

    public float stepTime = 0.00001f; 
    private float GM = 4 * Mathf.PI * Mathf.PI; 
    private float scaleFactor = 10f;
    private float[] initialDistances = { 0.39f, 0.72f, 1.0f, 1.52f, 5.2f, 9.58f, 19.22f, 30.05f };
    private float[] initialVelocities = { 10.07f, 7.38f, 6.28f, 5.06f, 2.75f, 2.04f, 1.43f, 1.14f };

    void Start()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            Vector3 initialPosition = new Vector3(initialDistances[i] * scaleFactor, 0, 0); 
            Vector3 initialVelocity = new Vector3(0, initialVelocities[i], 0); 

            planetTransforms.Add(planets[i].transform);
            velocities.Add(initialVelocity);

            planets[i].transform.position = initialPosition;
        }
    }

    void Update()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            Vector3 acceleration = GravitationalAcceleration(planetTransforms[i].position);
            (Vector3 newPosition, Vector3 newVelocity) = RungeKutta4(planetTransforms[i].position, velocities[i], acceleration);

            planetTransforms[i].position = newPosition;
            velocities[i] = newVelocity;
        }
    }

    Vector3 GravitationalAcceleration(Vector3 position)
    {
        float distanceSquared = position.sqrMagnitude;
        Vector3 unitVector = -position.normalized;
        return (GM / distanceSquared) * unitVector;
    }

    (Vector3, Vector3) RungeKutta4(Vector3 position, Vector3 velocity, Vector3 acceleration)
    {
        Vector3 K1position, K1velocity, K2position, K2velocity, K3position, K3velocity, K4position, K4velocity;

        K1position = velocity * stepTime;
        K1velocity = acceleration * stepTime;

        K2position = (velocity + 0.5f * K1velocity) * stepTime;
        K2velocity = GravitationalAcceleration(position + 0.5f * K1position) * stepTime;

        K3position = (velocity + 0.5f * K2velocity) * stepTime;
        K3velocity = GravitationalAcceleration(position + 0.5f * K2position) * stepTime;

        K4position = (velocity + K3velocity) * stepTime;
        K4velocity = GravitationalAcceleration(position + K3position) * stepTime;

        position += (K1position + 2 * K2position + 2 * K3position + K4position) / 6;
        velocity += (K1velocity + 2 * K2velocity + 2 * K3velocity + K4velocity) / 6;

        return (position, velocity);
    }
}