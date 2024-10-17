using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSystem : MonoBehaviour
{
	public Transform ThePlatformItself;

	[SerializeField]
	public Transform[] MovingPlatformPoints;

	public bool PingPong = true;

	public bool ReverseOrder = false;

	public float Speed = 1f;

	public float PauseTime = 1f;

	public Vector2 Velocity;

	private float currentTimeWaiting = 0f;

	private bool isWaiting = false;

	public float Threshold = 0.1f;

	private int currentPoint = 1;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (MovingPlatformPoints.Length < 2) return;


		if (Vector3.Distance(ThePlatformItself.position, MovingPlatformPoints[currentPoint].position) < Threshold)
		{
			if (MovingPlatformPoints.Length - 1 > currentPoint && !ReverseOrder)
			{
				currentPoint++;
			}
			else if (0 < currentPoint && ReverseOrder)
			{
				currentPoint--;
			}
			else if (currentPoint >= MovingPlatformPoints.Length - 1 && !ReverseOrder)
			{
				if (PingPong)
				{
					ReverseOrder = true;
					currentPoint--;
				}
				else
				{
					currentPoint = 0;
				}
			}
			else if (currentPoint <= 0 && ReverseOrder)
			{
				if (PingPong)
				{
					ReverseOrder = false;
					currentPoint++;
				}
				else
				{
					currentPoint = MovingPlatformPoints.Length - 1;
				}
			}

			currentTimeWaiting = PauseTime;
		}

		if (currentTimeWaiting >= 0)
		{
			currentTimeWaiting -= Time.deltaTime;
			isWaiting = true;
		}
		else
		{
			isWaiting = false;
		}


		Vector3 targDir = (MovingPlatformPoints[currentPoint].position - ThePlatformItself.position).normalized;


		if (!isWaiting) ThePlatformItself.Translate(targDir * Time.deltaTime * Speed);

		if (!isWaiting) Velocity = targDir * Time.deltaTime * Speed;
		else Velocity = Vector2.zero;


	}
}
