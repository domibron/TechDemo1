using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformParenting : MonoBehaviour
{
	private Transform player;
	private Rigidbody2D playerRB;
	private PlayerController playerController;

	private MovingPlatformSystem movingPlatformSystem;

	private bool hasPlayer = false;

	// private Vector3 oldPos;

	private Vector2 velocity;

	// Start is called before the first frame update
	void Start()
	{
		movingPlatformSystem = GetComponentInParent<MovingPlatformSystem>();

		// oldPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		// velocity = movingPlatformSystem.Velocity;


		// if (oldPos != transform.localPosition)
		// {
		// 	velocity = (transform.localPosition - oldPos) / Time.deltaTime;
		// 	oldPos = transform.localPosition;
		// }
		// else
		// {
		// 	// oldPos = transform.position;s
		// 	velocity = Vector2.zero;
		// }

		// print($"{playerRB.velocity} = {playerController.VelocityForFrame} + {movingPlatformSystem.Velocity}");

		if (hasPlayer)
		{
			velocity = movingPlatformSystem.Velocity.normalized * movingPlatformSystem.Speed;
			velocity.y = 0;

			// playerController.VelocityForFrame += velocity;

			//playerRB.velocity = playerController.VelocityForFrame;


		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			player = other.transform;

			playerRB = player.GetComponent<Rigidbody2D>();

			playerRB.interpolation = RigidbodyInterpolation2D.None;

			// playerRB.gravityScale = 0;

			player.parent = transform;

			playerController = player.GetComponent<PlayerController>();

			//playerController.IgnoreRBVelocity = true;

			hasPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			hasPlayer = false;

			// playerRB.gravityScale = 1;

			player.parent = null;

			playerRB.interpolation = RigidbodyInterpolation2D.Interpolate;


			//playerController.IgnoreRBVelocity = false;

			//playerController.NewVelocityForFrame += velocity - playerController.VelocityForFrame;

			//playerRB.velocity = new Vector2(playerController.VelocityForFrame.x, playerController.VelocityForFrame.y);
		}
	}
}
