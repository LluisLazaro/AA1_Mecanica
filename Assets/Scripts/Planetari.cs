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

	public PlanetHUD planetHUD;
    int speedIndex;

	// S'executa al començar: desactiva el TrailRenderer fins que comenci la simulació
	private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }

	// Actualitza la velocitat de simulació segons la configuració de l'HUD
	private void Update()
	{
		speedIndex = planetHUD.speedIndex;
		switch (speedIndex)
		{
			case 1:
				stepTime = 0.0001f;
				break;
			case 2:
				stepTime = 0.0005f;
				break;
			case 3:
				stepTime = 0.001f;
				break;
		}
	}

	public void Initialize(Vector3 initialPosition, Vector3 initialVelocity)
    {
		// Inicialitza la posició i velocitat inicial del planeta
		planetPosition = initialPosition;
        planetVelocity = initialVelocity;

		// Col·loca el planeta visualment en una posició escalar per a millor visibilitat
		transform.position = initialPosition * 20f;
    }

	// Assigna l'acceleració total (calculada pel PlanetManager) que actuarà sobre aquest planeta
	public void SetAcceleration(Vector3 acc)
    {
        netAcceleration = acc;
    }

	// Simula un pas temporal del moviment del planeta
	public void SimulateStep()
    {
		// Calcula la nova posició, velocitat i temps amb el mètode de Runge-Kutta
		(planetPosition, planetVelocity, time) = RungeKutta4(planetPosition, planetVelocity, time);

		// Actualitza la posició visual del planeta amb un factor d’escala per fer-lo visible a Unity
		transform.position = planetPosition * 20f;
    }

	// Implementa el mètode de Runge-Kutta d'ordre 4 per calcular el següent estat físic del planeta
	(Vector3, Vector3, float) RungeKutta4(Vector3 position, Vector3 velocity, float time)
    {
        Vector3 K1position, K1velocity, K2position, K2velocity, K3position, K3velocity, K4position, K4velocity;

		// Calcul per estimar els canvis de posició i velocitat en diferents punts dins del pas de temps.
		// Cada calcul es basa en la informació de l’anterior, millorant així la precisió del càlcul final.
		(K1position, K1velocity) = DifferentialFunction(position, velocity, netAcceleration);
        (K2position, K2velocity) = DifferentialFunction(position + 0.5f * stepTime * K1position, velocity + 0.5f * stepTime * K1velocity, netAcceleration);
        (K3position, K3velocity) = DifferentialFunction(position + 0.5f * stepTime * K2position, velocity + 0.5f * stepTime * K2velocity, netAcceleration);
        (K4position, K4velocity) = DifferentialFunction(position + stepTime * K3position, velocity + stepTime * K3velocity, netAcceleration);

		// Càlcul de la nova posició amb una mitjana ponderada dels 4 valors
		Vector3 newPosition = position + (stepTime / 6f) * (K1position + 2f * K2position + 2f * K3position + K4position);

		// Càlcul de la nova velocitat amb el mateix principi
		Vector3 newVelocity = velocity + (stepTime / 6f) * (K1velocity + 2f * K2velocity + 2f * K3velocity + K4velocity);

		// Avança el temps simulat
		float newTime = time + stepTime;

		// Activa el Trail només un cop comenci el moviment
		if (!hasActivatedTrail)
        {
            trailRenderer.enabled = true;
            hasActivatedTrail = true;
        }

		// Retorna la nova posició, velocitat i temps perquè siguin aplicats al planeta
		return (newPosition, newVelocity, newTime);
    }

	// Retorna la derivada de la posició (velocitat) i la derivada de la velocitat (acceleració), necessàries per RK4
	(Vector3, Vector3) DifferentialFunction(Vector3 position, Vector3 velocity, Vector3 acceleration)
    {
        return (velocity, acceleration);
    }

	// Incrementa l’índex de velocitat fins a un màxim
	public void SpeedTimeUp()
	{
		if (speedIndex < 5)
		{
			speedIndex++;
		}
	}

	// Redueix l’índex de velocitat fins a un mínim
	public void SlowTimeDown()
	{
		if (speedIndex > 0)
		{
			speedIndex--;
		}
	}
}
