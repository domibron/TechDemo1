using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MaxSpeed = 5f;
	public float AccelRate = 0.7f;
	public float DeaccelRate = 0.7f;


	public float JumpForce = 10f;


	private Rigidbody2D attachedRigidBody;

	private Camera cam;


	private bool cameraIsMoving = false;

	void Awake()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();
		cam = Camera.main;
	}

	// Start is called before the first frame update
	void Start()
	{

	}


	void Update()
	{
		Vector2 InputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


		Vector2 forceToApply = new Vector2();


		if (InputVector.x != 0)
		{
			forceToApply = ((new Vector2(InputVector.normalized.x, 0) * MaxSpeed) - new Vector2(attachedRigidBody.velocity.x, 0)) * AccelRate;

		}
		else
		{
			forceToApply = new Vector2(-attachedRigidBody.velocity.x, 0) * DeaccelRate;
		}


		attachedRigidBody.AddForce(forceToApply);



		bool isGrounded = Physics2D.Raycast(transform.position, -transform.up, 1f, ~(1 << 3));



		if (Input.GetKeyDown(KeyCode.W) && isGrounded)
		{
			Vector2 counterGrav = attachedRigidBody.velocity;

			counterGrav.x = 0;

			if (counterGrav.y > 0) counterGrav.y = 0;
			else
			{
				counterGrav.y = +counterGrav.y;
			}

			attachedRigidBody.AddForce(new Vector2(0, JumpForce) + counterGrav, ForceMode2D.Impulse);
		}

		// for player
		//print(cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0), Camera.MonoOrStereoscopicEye.Mono));

		// for camera

		// print(MoveCamera(Vector2Int.left, cam.transform.position, Vector2.zero, cam));

		Vector2Int camMoveDir = Vector2Int.zero;

		if (cam.WorldToScreenPoint(transform.position).x > Screen.width)
		{
			camMoveDir.x = 1;
		}
		else if (cam.WorldToScreenPoint(transform.position).x < 0)
		{
			camMoveDir.x = -1;
		}


		if (cam.WorldToScreenPoint(transform.position).y > Screen.height)
		{
			camMoveDir.y = 1;
		}
		else if (cam.WorldToScreenPoint(transform.position).y < 0)
		{
			camMoveDir.y = -1;
		}

		// print(MoveCamera(camMoveDir, cam));

		if (camMoveDir.magnitude != 0 && !cameraIsMoving)
		{
			StartCoroutine(MoveCameraFancy(MoveCamera(camMoveDir, cam), cam, 4));
		}
	}

	private IEnumerator MoveCameraFancy(Vector3 targetPosition, Camera targetCamera, float aniSpeed = 2f)
	{
		cameraIsMoving = true;

		Vector3 startPosition = cam.transform.position;

		float localTime = 0;

		// freez player

		while (targetCamera.transform.position != targetPosition)
		{
			targetCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, localTime);

			localTime += Time.deltaTime * aniSpeed;

			yield return null;
		}

		// unfreez

		cameraIsMoving = false;
	}

	private Vector3 MoveCamera(Vector2Int direction, Camera cameraToBaseOff, Vector2 buffer = new())
	{
		if (direction.x > 1) direction.x = 1;
		else if (direction.x < -1) direction.x = -1;

		if (direction.y > 1) direction.y = 1;
		else if (direction.y < -1) direction.y = -1;

		if (cameraToBaseOff == null)
		{
			cameraToBaseOff = Camera.main;
		}

		Vector3 returnedPosition = cameraToBaseOff.ScreenToWorldPoint(new Vector3(((Screen.width + buffer.x) * direction.x) + Screen.width / 2f, ((Screen.height + buffer.y) * direction.y) + Screen.height / 2f, 0), Camera.MonoOrStereoscopicEye.Mono);



		returnedPosition.z = cameraToBaseOff.transform.position.z;

		return returnedPosition;
	}

}
