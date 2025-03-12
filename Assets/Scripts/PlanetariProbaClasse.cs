using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetariProbaClasse : MonoBehaviour
{
    Vector2 EarthPosition;
    Vector2 EarthVelocity;
    Vector2 EarthAcceleracio;
    Vector2 initialPosition = new Vector2(1, 0);
    Vector2 initialVelocity = new Vector2(0, 2 * Mathf.PI);

    public float totalTime = 10;
    private float timeMotion;
    public float stepTime = 0.0001f;

    private float GM = 4 * Mathf.PI * Mathf.PI;

    void Start()
    {
        transform.position = initialPosition;
        EarthVelocity = initialVelocity;
        EarthPosition = initialPosition;

        timeMotion = 0;
    }

    void Update()
    {
        if (timeMotion < totalTime)
        {
            EarthAcceleracio = GravitationalAcceleration();
            (EarthPosition, EarthVelocity, timeMotion) = EulerMethod(EarthPosition, EarthVelocity, EarthAcceleracio, timeMotion);

            transform.position = EarthPosition;
        }
    }

    Vector2 GravitationalAcceleration()
    {
        float distanceSquaredES = EarthPosition.magnitude * EarthPosition.magnitude;
        Vector2 unityVector = -EarthPosition.normalized;

        Vector2 acceleration = (GM / distanceSquaredES) * unityVector;

        return acceleration;
    }

    (Vector2, Vector2, float) EulerMethod(Vector2 position, Vector2 velocity, Vector2 acceleration, float time)
    {
        Vector2 newPosition = position + velocity * stepTime;
        Vector2 newVelocity = velocity + stepTime * acceleration;
        time = time + stepTime;

        return (newPosition, newVelocity, time);
    }


}
