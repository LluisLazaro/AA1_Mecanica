using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
	public List<Planetari> planets;
	private float GM = 39.478f;

	void Update()
	{
		// Calcula totes les acceleracions causades per la gravetat
		CalculateGravitationalAccelerations();

		foreach (Planetari planet in planets)
		{
			// Simula el moviment cridant SimulateStep(), que fa servir el mètode de Runge-Kutta per moure el sistema
			planet.SimulateStep();
		}
	}

	// Calcula l'acceleració gravitatòria sobre cada planeta deguda al Sol i als altres planetes
	void CalculateGravitationalAccelerations()
	{
		// Recorrerà cada planeta de la llista per calcular la seva acceleració total
		foreach (Planetari planet in planets)
		{
			Vector3 totalAcceleration = Vector3.zero;

			// Afegeix la gravetat del Sol (es considera fix a l'origen amb massa = 1)
			Vector3 directionToSun = -planet.planetPosition;

			// Es calcula la distància quadrada(per evitar fer arrels, que són més costoses)
			float distanceToSunSqr = directionToSun.sqrMagnitude;

			// Després es normalitza el vector per convertir-lo en una direcció unitaria
			directionToSun.Normalize();

			// S'aplica la fórmula (GM / r²) * direccio
			Vector3 sunAcceleration = (GM * 1.0f / distanceToSunSqr) * directionToSun;

			// Es suma a la total
			totalAcceleration += sunAcceleration;

			// Calcula les interaccions gravitatòries amb la resta de planetes
			foreach (Planetari other in planets)
			{
				// Salta si són el mateix, perquè un planeta no s'ha d'atreure a si mateix
				if (planet == other) continue;

				// Es calcula el vector entre els dos planetes i la seva distància quadrada
				Vector3 direction = other.planetPosition - planet.planetPosition;
				float distanceSquared = direction.sqrMagnitude;

				// Si estan molt a prop (menys de 1e-6f), es salta el càlcul per evitar errors de divisió
				if (distanceSquared < 1e-6f) continue;

				// Es normalitza el vector, per tenir nomes direccio
				direction.Normalize();

				// S’aplica la força gravitatòria entre dues masses
				// F = G * m1 * m2 / r²
				Vector3 acc = (GM * other.Mass / distanceSquared) * direction;

				// S’afegeix a la total
				totalAcceleration += acc;
			}

			//Després d'haver sumat totes les acceleracions, s’envia aquesta acceleració al planeta perquè la pugui usar a SimulateStep().
			planet.SetAcceleration(totalAcceleration);
		}
	}
}
