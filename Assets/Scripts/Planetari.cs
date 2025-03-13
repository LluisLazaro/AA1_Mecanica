using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planetari : MonoBehaviour
{
    public Transform sun; 
    public GameObject[] planets; 

    private List<Vector3> velocities = new List<Vector3>();
    private List<Vector3> accelerations = new List<Vector3>();
    private List<Transform> planetTransforms = new List<Transform>();

    public float totalTime = 10f;
    private float timeMotion;
    public float stepTime = 0.0001f;
    private float GM = 4 * Mathf.PI * Mathf.PI; 

    void Start()
    {
        timeMotion = 0f;
        foreach (GameObject planet in planets)
        {
           //aqui s'hauria de col·locar cada velocitat, acceleració i transforms de cada planeta
        }
    }

    void Update()
    {
     
    }

    //Aixo ns com fer-ho he possat en vector3 i ja
    //Vector3 GravitationalAcceleration()
    //{
    //    float distanceSquaredES = EarthPosition.magnitude * EarthPosition.magnitude;
    //    Vector2 unityVector = -EarthPosition.normalized;
    //    Vector2 acceleration = (GM / distanceSquaredES) * unityVector;

    //    return acceleration;
    //}

    (Vector3, Vector3, float) EulerMethod(Vector3 position, Vector3 velocity, Vector3 acceleration, float time)
    {
        Vector3 newPosition = position + velocity * stepTime;
        Vector3 newVelocity = velocity + acceleration * stepTime;
        time += stepTime;

        return (newPosition, newVelocity, time);
    }
}
