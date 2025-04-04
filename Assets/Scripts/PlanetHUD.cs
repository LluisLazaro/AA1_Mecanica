using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHUD : MonoBehaviour
{
	[Header("Càmera")]
	public Camera mainCamera;
	public float zoomSpeed = 20f;
	public float minZoom = 10f;
	public float maxZoom = 2000f;
	public Vector3 initialOffset = new Vector3(0, 250, -250);

	[Header("Controls de càmera")]
	public float mouseSensitivity = 3f;
	public float moveSpeed = 50f;

	private Vector3 cameraOffset;
	private Vector3 lookAt = Vector3.zero;
	private float rotationX = 0f;
	private float rotationY = 0f;

	public int speedIndex = 1;
	public Text speedText;

	// Inicialitza la càmera i estableix distància màxima de zoom
	void Start()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;

		cameraOffset = initialOffset;

		float initialDistance = cameraOffset.magnitude;
		if (maxZoom < initialDistance)
			maxZoom = initialDistance + 500f;
	}

	// Actualitza l'HUD i gestiona els controls de càmera cada frame
	void Update()
	{
		if (speedText != null)
		{
			speedText.text = "X " + speedIndex.ToString();
		}

		HandleCameraControls();
	}

	// Augmenta la velocitat de simulació (fins a màxim 3)
	public void SpeedTimeUp()
	{
		if(speedIndex < 3) {
			speedIndex++;
		}
	}

	// Disminueix la velocitat de simulació (mínim 1)
	public void SlowTimeDown()
	{
		if (speedIndex > 1)
		{
			speedIndex--;
		}
	}

	// Controla el moviment, rotació i zoom de la càmera amb el ratolí i teclat
	void HandleCameraControls()
	{
		// Rotació amb clic esquerre del ratolí
		if (Input.GetMouseButton(0))
		{
			rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
			rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			rotationY = Mathf.Clamp(rotationY, -85f, 85f);
		}

		// Zoom amb la roda del ratolí
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (Mathf.Abs(scroll) > 0.001f)
		{
			float distance = cameraOffset.magnitude;
			distance -= scroll * zoomSpeed;
			distance = Mathf.Clamp(distance, minZoom, maxZoom);
			cameraOffset = cameraOffset.normalized * distance;
		}

		// Posicionament de la càmera segons la rotació i la distància
		Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);
		Vector3 offset = rotation * Vector3.forward * cameraOffset.magnitude;

		mainCamera.transform.position = lookAt - offset;
		mainCamera.transform.LookAt(lookAt);

		// Desplaçament lateral amb clic del mig del ratolí
		if (Input.GetMouseButton(2))
		{
			float panSpeed = cameraOffset.magnitude * 0.012f;

			float deltaX = Input.GetAxisRaw("Mouse X");
			float deltaY = Input.GetAxisRaw("Mouse Y");

			Vector3 moveCam = (-mainCamera.transform.right * deltaX + -mainCamera.transform.up * deltaY) * panSpeed;

			lookAt += moveCam;
		}

		// Moviment amb teclat WASD
		Vector3 move = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) move += mainCamera.transform.forward;
		if (Input.GetKey(KeyCode.S)) move -= mainCamera.transform.forward;
		if (Input.GetKey(KeyCode.A)) move -= mainCamera.transform.right;
		if (Input.GetKey(KeyCode.D)) move += mainCamera.transform.right;

		if (move != Vector3.zero)
		{
			lookAt += move * moveSpeed * Time.deltaTime;
		}
	}
}
