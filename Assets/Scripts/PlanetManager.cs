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
			// Simula el moviment cridant SimulateStep(), que fa servir el m�tode de Runge-Kutta per moure el sistema
			planet.SimulateStep();
		}
	}

	// Calcula l'acceleraci� gravitat�ria sobre cada planeta deguda al Sol i als altres planetes
	void CalculateGravitationalAccelerations()
	{
		// Recorrer� cada planeta de la llista per calcular la seva acceleraci� total
		foreach (Planetari planet in planets)
		{
			Vector3 totalAcceleration = Vector3.zero;

			// Afegeix la gravetat del Sol (es considera fix a l'origen amb massa = 1)
			Vector3 directionToSun = -planet.planetPosition;

			// Es calcula la dist�ncia quadrada(per evitar fer arrels, que s�n m�s costoses)
			float distanceToSunSqr = directionToSun.sqrMagnitude;

			// Despr�s es normalitza el vector per convertir-lo en una direcci� unitaria
			directionToSun.Normalize();

			// S'aplica la f�rmula (GM / r�) * direccio
			Vector3 sunAcceleration = (GM * 1.0f / distanceToSunSqr) * directionToSun;

			// Es suma a la total
			totalAcceleration += sunAcceleration;

			// Calcula les interaccions gravitat�ries amb la resta de planetes
			foreach (Planetari other in planets)
			{
				// Salta si s�n el mateix, perqu� un planeta no s'ha d'atreure a si mateix
				if (planet == other) continue;

				// Es calcula el vector entre els dos planetes i la seva dist�ncia quadrada
				Vector3 direction = other.planetPosition - planet.planetPosition;
				float distanceSquared = direction.sqrMagnitude;

				// Si estan molt a prop (menys de 1e-6f), es salta el c�lcul per evitar errors de divisi�
				if (distanceSquared < 1e-6f) continue;

				// Es normalitza el vector, per tenir nomes direccio
				direction.Normalize();

				// S�aplica la for�a gravitat�ria entre dues masses
				// F = G * m1 * m2 / r�
				Vector3 acc = (GM * other.Mass / distanceSquared) * direction;

				// S�afegeix a la total
				totalAcceleration += acc;
			}

			//Despr�s d'haver sumat totes les acceleracions, s�envia aquesta acceleraci� al planeta perqu� la pugui usar a SimulateStep().
			planet.SetAcceleration(totalAcceleration);
		}
	}
}
