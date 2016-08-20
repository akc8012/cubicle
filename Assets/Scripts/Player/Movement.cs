// Andrew
using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	CharacterController controller;
	Transform cam;
	Animator anim;

	Vector3 velocity = Vector3.zero;
	float moveSpeed = 10;            // what to increment velocity by
	float maxVel = 5;       // maximum velocity in any direction
	float rotSmooth = 20;    // smoothing on the lerp to rotate towards stick direction
	bool walking = false;
	bool crouching = false;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		SetCam(GameObject.FindWithTag("MainCamera").transform);
		anim = GetComponentInChildren<Animator>();
	}

	void Update()
	{
		if (!controller.enabled) return;

		float speed = 0.0f;
		Vector3 moveDir = GetInput(ref speed);
		Rotate(moveDir, speed);
		speed *= moveSpeed;
		
		velocity = transform.forward * speed;
		velocity = Vector3.ClampMagnitude(velocity, maxVel);

		controller.Move(velocity * Time.deltaTime);
	}

	Vector3 GetInput(ref float speed)
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector3 stickDir = new Vector3(horizontal, 0, vertical);

		speed = Mathf.Clamp(Vector3.Magnitude(stickDir), 0, 1);     // make sure we can't exceed 1 (diagonals)

		anim.SetFloat("Walking", speed);
		anim.SetBool("Crouching", Input.GetButton("Stealth"));
		
		// get camera rotation
		Vector3 cameraDir = cam.forward;
		cameraDir.y = 0.0f;                 // cameraDir is the camera's forward vector, with the y removed
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDir);    // creates a rotation that describes how to take forward (0,0,1), and rotate it so that it is facing the same direction as the camera
																								// we do this so that in the next line, no matter where the camera is, pushing up will always move the player forward
																								// convert joystick input in Worldspace coordinates

		Vector3 moveDir = referentialShift * stickDir;                              // multiplying a quaternion by a vector applies that rotation to the vector
																					// here, we end up rotating the stick direction by the rotation it takes from forward to reach the camera's rotation (referentialShift)
																					// WHY?: This rotates the stickDir around so that moveDir will be relative to the camera, not to the world

		// fixes bug when the camera forward is exactly -forward (opposite to Vector3.forward) by flipping the x around
		if (Vector3.Dot(Vector3.forward, cameraDir.normalized) == -1) moveDir = new Vector3(-moveDir.x, moveDir.y, moveDir.z);

		Debug.DrawRay(transform.position, stickDir * 2, Color.blue);
		Debug.DrawRay(transform.position + Vector3.up, moveDir * 2, Color.green);

		return moveDir;
	}

	void Rotate(Vector3 moveDir, float speed)
	{
		if (Vector3.Angle(moveDir, transform.forward) > 135)        // if the difference is above a certain angle,
			transform.forward = moveDir;                            // we'll want to snap right to it, instead of lerping
		else
		{
			Vector3 targetRotation = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotSmooth);
			if (targetRotation != Vector3.zero) transform.rotation = Quaternion.LookRotation(targetRotation);
		}
	}

	public void SetCam(Transform camera)
	{
		cam = camera;
	}

	public void SetMovement(bool enable)
	{
		controller.enabled = enable;
	}
}
