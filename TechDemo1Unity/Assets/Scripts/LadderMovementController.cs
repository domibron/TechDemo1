using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovementController : MonoBehaviour
{
	public bool IsPlayerOnLadder = false;

	private PlayerController playerController;

	private JetPack jetPack;

	private Rigidbody2D attachedRigidbody;

	private float playerHeight;

	private Vector3 ladderPos;
	private float ladderHeight;

	private bool stopDismount = false;

	// Start is called before the first frame update
	void Start()
	{
		playerController = GetComponent<PlayerController>();

		attachedRigidbody = GetComponent<Rigidbody2D>();

		jetPack = GetComponent<JetPack>();

		playerHeight = GetComponent<CapsuleCollider2D>().size.y;
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsPlayerOnLadder) return;

		if (jetPack.UsingJetPack)
		{
			DismountLadder();
			return;
		}

		Vector2 moveDir = PlayerController.GetInputAsVector2();

		moveDir.Normalize();


		if (moveDir.x != 0)
		{
			DismountLadder();
		}

		if (((transform.position.y > (ladderPos.y + playerHeight / 2f) + (ladderHeight / 2f) && !Input.GetKey(KeyCode.S)) || (transform.position.y < (ladderPos.y + playerHeight / 2f) - (ladderHeight / 2f))) && IsPlayerOnLadder)
		{
			DismountLadder();
		}

		if (moveDir.y != 0)
		{
			attachedRigidbody.AddForce(((Vector2.up * moveDir.y * playerController.MaxSpeed) - attachedRigidbody.velocity) * playerController.AccelRate);
		}
		else
		{
			attachedRigidbody.AddForce(new Vector2(0, -attachedRigidbody.velocity.y) * playerController.DeaccelRate);
		}


	}

	public void MountLadder(Vector3 posToTeleport, Vector3 ladderPosition, float ladderFullHeight)
	{
		if (jetPack.UsingJetPack) return;

		if (stopDismount == false)
		{
			StartCoroutine(StopDismontDelay());
		}


		transform.position = posToTeleport;

		IsPlayerOnLadder = true;

		playerController.LockMovement = true;

		attachedRigidbody.gravityScale = 0;

		attachedRigidbody.velocity = Vector2.zero;

		jetPack.Locked = true;

		ladderPos = ladderPosition;

		ladderHeight = ladderFullHeight;
	}

	IEnumerator StopDismontDelay()
	{
		stopDismount = true;
		yield return new WaitForSeconds(0.05f);
		stopDismount = false;
	}

	public void DismountLadder()
	{
		if (stopDismount) return;

		IsPlayerOnLadder = false;

		playerController.LockMovement = false;

		attachedRigidbody.gravityScale = 1;

		jetPack.Locked = false;

	}
}
