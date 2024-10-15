using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Ladder : MonoBehaviour
{
	public Transform AboveCheck;

	public LadderMovementController ladderMovementController;

	public Transform playerTransform;

	public bool playerAbove = false;

	private SpriteRenderer spriteRenderer;

	private BoxCollider2D boxCollider;

	private float heightOfLadder = 0f;

	private bool playerNearLadder = false;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();

		boxCollider.size = spriteRenderer.size;
		boxCollider.offset = Vector2.zero;

		boxCollider.isTrigger = true;

		float height = AboveCheck.GetComponent<BoxCollider2D>().size.y;

		height /= 2f;

		height += boxCollider.size.y / 2f;

		AboveCheck.localPosition = new Vector3(0, height, 0);

		heightOfLadder = boxCollider.size.y;
	}

	// Update is called once per frame
	void Update()
	{
		if (ladderMovementController == null) return;


		if (playerAbove && boxCollider.isTrigger && !ladderMovementController.IsPlayerOnLadder)
		{
			boxCollider.isTrigger = false;
		}
		else if (playerAbove && Input.GetKeyDown(KeyCode.S) && !ladderMovementController.IsPlayerOnLadder)
		{
			ladderMovementController.MountLadder(CalcPlayerTargetPositionOnLadder());
		}
		else if (!boxCollider.isTrigger)
		{
			boxCollider.isTrigger = true;
		}


		if (playerNearLadder && Input.GetKeyUp(KeyCode.W) && !ladderMovementController.IsPlayerOnLadder)
		{
			ladderMovementController.MountLadder(CalcPlayerTargetPositionOnLadder());
		}
	}

	private Vector3 CalcPlayerTargetPositionOnLadder()
	{
		Vector3 targ = playerTransform.position - transform.position;

		targ.y -= 1f;

		if (targ.y < transform.position.y - (heightOfLadder / 2f) + 1f)
		{
			targ.y = transform.position.y - (heightOfLadder / 2f) + 1f;
		}
		else if (targ.y > transform.position.y + (heightOfLadder / 2f) + 1f)
		{
			targ.y = transform.position.y + (heightOfLadder / 2f) + 1f;
		}
		else
		{
			targ.y = playerTransform.position.y;
		}

		targ.x = transform.position.x;

		targ.z = playerTransform.position.z;

		return targ;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - (heightOfLadder / 2f) + 1f, transform.position.z), 0.5f);
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + (heightOfLadder / 2f) + 1f, transform.position.z), 0.5f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (ladderMovementController == null) ladderMovementController = other.GetComponent<LadderMovementController>();
			if (ladderMovementController != null) playerTransform = ladderMovementController.transform;

			playerNearLadder = true;
			// ladderMovementController.MountLadder(CalcPlayerTargetPositionOnLadder());
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			playerNearLadder = false;
			// ladderMovementController.DismountLadder();
		}
	}
}
