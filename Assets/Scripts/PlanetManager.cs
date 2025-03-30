using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
	public List<Planetari> planets;
	private float GM = 39.478f;

	void Update()
	{
		CalculateGravitationalAccelerations();

		foreach (Planetari planet in planets)
		{
			planet.SimulateStep();
		}
	}

	void CalculateGravitationalAccelerations()
	{
		foreach (Planetari planet in planets)
		{
			Vector3 totalAcceleration = Vector3.zero;

			// Afegim la gravetat del Sol fix a (0,0,0), amb massa 1 M
			Vector3 directionToSun = -planet.planetPosition;
			float distanceToSunSqr = directionToSun.sqrMagnitude;
			directionToSun.Normalize();
			Vector3 sunAcceleration = (GM * 1.0f / distanceToSunSqr) * directionToSun;
			totalAcceleration += sunAcceleration;

			// Interaccions entre planetes
			foreach (Planetari other in planets)
			{
				if (planet == other) continue;

				Vector3 direction = other.planetPosition - planet.planetPosition;
				float distanceSquared = direction.sqrMagnitude;
				if (distanceSquared < 1e-6f) continue; // Evita dividir per 0

				direction.Normalize();
				Vector3 acc = (GM * other.Mass / distanceSquared) * direction;
				totalAcceleration += acc;
			}

			planet.SetAcceleration(totalAcceleration);
		}
	}
}
