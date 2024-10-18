using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public static CameraMovement Instance { get; private set; }

	public float CameraMoveSpeed = 4f;

	public bool CameraIsMoving = false;

	private Camera cam;

	int screenWidth = 1500;
	int screenHeight = 1000;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;


	}

	// Update is called once per frame
	void Update()
	{

	}


	private IEnumerator MoveCameraFancy(Vector3 targetPosition, Camera targetCamera, float aniSpeed = 2f)
	{
		CameraIsMoving = true;

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

		CameraIsMoving = false;
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




		Vector3 returnedPosition = cameraToBaseOff.ScreenToWorldPoint(new Vector3(((screenWidth + buffer.x) * direction.x) + screenWidth / 2f, ((screenHeight + buffer.y) * direction.y) + screenHeight / 2f, 0), Camera.MonoOrStereoscopicEye.Mono);



		returnedPosition.z = cameraToBaseOff.transform.position.z;

		return returnedPosition;
	}

	public void CheckPlayerIsAtBounds(Vector3 PlayerPosition)
	{
		Vector2Int camMoveDir = Vector2Int.zero;

		Vector3 posAtScreen = cam.WorldToScreenPoint(PlayerPosition);

		if (posAtScreen.x > screenWidth)
		{
			camMoveDir.x = 1;
		}
		else if (posAtScreen.x < 0)
		{
			camMoveDir.x = -1;
		}


		if (posAtScreen.y > screenHeight)
		{
			camMoveDir.y = 1;
		}
		else if (posAtScreen.y < 0)
		{
			camMoveDir.y = -1;
		}

		// print(MoveCamera(camMoveDir, cam));

		if (camMoveDir.magnitude != 0 && !CameraIsMoving)
		{
			StartCoroutine(MoveCameraFancy(MoveCamera(camMoveDir, cam), cam, CameraMoveSpeed));
		}
	}

}
