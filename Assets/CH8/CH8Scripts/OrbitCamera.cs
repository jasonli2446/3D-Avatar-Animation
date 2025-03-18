using UnityEngine;
using System.Collections;

// maintains position offset while orbiting around target
public class OrbitCamera : MonoBehaviour
{
	[SerializeField] private Transform target;

	public float rotSpeed = 1.5f;
	public float minVertical = -30.0f;  // Changed from -45.0f to prevent looking too far down
	public float maxVertical = 60.0f;   // Increased to allow looking further up

	[Header("Ground Detection")]
	public bool preventGroundClipping = true;
	public float groundOffset = 0.2f;   // How far above ground to keep camera
	public LayerMask groundLayers;      // Set this to your ground layers

	private float _rotY;
	private float _rotX;
	private Vector3 _offset;

	// Use this for initialization
	void Start()
	{
		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// Store initial rotation
		_rotY = transform.eulerAngles.y;
		_rotX = transform.eulerAngles.x;

		// Calculate the initial offset
		_offset = target.position - transform.position;

		// Set default ground layers if not set
		if (groundLayers.value == 0)
			groundLayers = LayerMask.GetMask("Default");
	}

	// Update is called once per frame
	void LateUpdate()
	{
		// Only use mouse input for camera control
		float mouseX = Input.GetAxis("Mouse X") * rotSpeed * 3;
		float mouseY = Input.GetAxis("Mouse Y") * rotSpeed * 3;

		// Update rotation angles
		_rotY += mouseX;
		_rotX -= mouseY; // Inverted to match expected mouse behavior

		// Clamp vertical rotation
		_rotX = Mathf.Clamp(_rotX, minVertical, maxVertical);

		// Apply both horizontal and vertical rotation
		Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);

		// Position the camera based on the offset and rotation
		Vector3 desiredPosition = target.position - (rotation * _offset);

		// Prevent ground clipping if enabled
		if (preventGroundClipping)
		{
			// Check if there's ground below the camera
			RaycastHit hit;
			if (Physics.Raycast(target.position, Vector3.down, out hit, 100f, groundLayers))
			{
				// Don't let camera go below ground level plus offset
				float groundLevel = hit.point.y + groundOffset;
				if (desiredPosition.y < groundLevel)
				{
					desiredPosition.y = groundLevel;
				}
			}
		}

		transform.position = desiredPosition;
		transform.LookAt(target);
	}
}