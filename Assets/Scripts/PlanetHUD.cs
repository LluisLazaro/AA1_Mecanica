using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHUD : MonoBehaviour
{
	[Header("Planetes i Visuals")]
	public List<Planetari> allPlanets;
	public Text hudText;

	[Header("Càmera")]
	public Camera mainCamera;
	private Vector3 solarViewOffset = new Vector3(0, 250, -250);
	public float zoomSpeed = 20f;
	public float minZoom = 10f;
	public float maxZoom = 1000f;

	[Header("Vista del sistema solar")]
	public Vector3 solarViewLookAt = Vector3.zero;

	private Vector3 cameraOffset; // offset actual
	private Planetari target;
	private int currentIndex;
	private float GM = 39.478f;

	void Start()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;

		currentIndex = allPlanets.Count; // comença amb vista solar
		target = null;
		cameraOffset = solarViewOffset;

		float initialDistance = solarViewOffset.magnitude;
		if (maxZoom < initialDistance)
			maxZoom = initialDistance + 200f;
	}

	void Update()
	{
		if (hudText == null) return;

		// Zoom amb la rodeta del ratolí
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (Mathf.Abs(scroll) > 0.001f)
		{
			float distance = cameraOffset.magnitude;
			distance -= scroll * zoomSpeed;
			distance = Mathf.Clamp(distance, minZoom, maxZoom);
			cameraOffset = cameraOffset.normalized * distance;
		}

		// Mostrar informació
		if (target != null)
		{
			float distance = target.planetPosition.magnitude;
			float speed = target.planetVelocity.magnitude;
			float kinetic = 0.5f * speed * speed;
			float potential = -(GM * 1.0f) / distance;
			float energy = kinetic + potential;

			hudText.text =
				$"{target.name}";
		}
		else
		{
			hudText.text =
				$"Sistema Solar";
		}

		if (mainCamera != null)
		{
			if (target != null)
			{
				Vector3 lookAt = target.transform.position;
				mainCamera.transform.position = lookAt + cameraOffset;
				mainCamera.transform.LookAt(lookAt);
			}
			else
			{
				mainCamera.transform.position = solarViewLookAt + cameraOffset;
				mainCamera.transform.LookAt(solarViewLookAt);
			}
		}
	}

	public void NextPlanet()
	{
		currentIndex = (currentIndex + 1) % (allPlanets.Count + 1);
		target = currentIndex < allPlanets.Count ? allPlanets[currentIndex] : null;

		cameraOffset = target != null ? CalculateOffsetByMass(target.Mass) : solarViewOffset;
	}

	public void PreviousPlanet()
	{
		currentIndex--;
		if (currentIndex < 0) currentIndex = allPlanets.Count;
		target = currentIndex < allPlanets.Count ? allPlanets[currentIndex] : null;

		cameraOffset = target != null ? CalculateOffsetByMass(target.Mass) : solarViewOffset;
	}

	// Calcula la distància de càmera inicial en funció de la massa del planeta
	Vector3 CalculateOffsetByMass(float mass)
	{
		float baseDistance = Mathf.Clamp(Mathf.Log10(mass + 1e-8f) + 8f, 3f, 30f);
		baseDistance *= 5;
		return new Vector3(0, baseDistance * 0.5f, -baseDistance);
	}
}
