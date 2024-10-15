using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovementController : MonoBehaviour
{
	public bool IsPlayerOnLadder = false;

	private PlayerController playerController;

	private Rigidbody2D attachedRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		playerController = GetComponent<PlayerController>();

		attachedRigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsPlayerOnLadder) return;

		Vector2 moveDir = PlayerController.GetInputAsVector2();

		moveDir.Normalize();

		if (moveDir.y != 0)
		{
			attachedRigidbody.AddForce(((Vector2.up * moveDir.y * playerController.MaxSpeed) - attachedRigidbody.velocity) * playerController.AccelRate);
		}
		else
		{
			attachedRigidbody.AddForce(new Vector2(0, -attachedRigidbody.velocity.y) * playerController.DeaccelRate);
		}

		// add delay
		if (moveDir.x != 0)
		{
			DismountLadder();
		}
	}

	public void MountLadder(Vector3 posToTeleport)
	{
		IsPlayerOnLadder = true;

		playerController.LockMovement = true;

		attachedRigidbody.gravityScale = 0;

		attachedRigidbody.velocity = Vector2.zero;

		transform.position = posToTeleport;
	}

	public void DismountLadder()
	{
		IsPlayerOnLadder = false;

		playerController.LockMovement = false;

		attachedRigidbody.gravityScale = 1;
	}
}
