using UnityEngine;
using System.Collections;

// maintains position offset while orbiting around target
public class OrbitCamera : MonoBehaviour
{
	[SerializeField] private Transform target;

	public float rotSpeed = 1.5f;
	public float minVertical = -30.0f;
	public float maxVertical = 60.0f;
	public float rightShoulderOffset = 1.0f;
	public float lookTargetOffset = -2.0f;

	[Header("Ground Detection")]
	public bool preventGroundClipping = true;
	public float groundOffset = 0.2f;
	public LayerMask groundLayers;

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
		_rotX += mouseY;

		// Clamp vertical rotation
		_rotX = Mathf.Clamp(_rotX, minVertical, maxVertical);

		// Apply both horizontal and vertical rotation
		Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);

		// Position the camera based on the offset and rotation
		Vector3 desiredPosition = target.position - (rotation * _offset);

		// Add right offset for over-the-shoulder view
		Vector3 rightOffset = rotation * Vector3.right * rightShoulderOffset;
		desiredPosition += rightOffset;

		// Create a look target that's also offset to make player appear on left side
		Vector3 lookDirection = rotation * Vector3.forward;
		Vector3 lookOffset = rotation * Vector3.right * lookTargetOffset;
		Vector3 lookTarget = target.position + new Vector3(0, 1.0f, 0) + lookOffset;

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
		transform.LookAt(lookTarget);
	}
}