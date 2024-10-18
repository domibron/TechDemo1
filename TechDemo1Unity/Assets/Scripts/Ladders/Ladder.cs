using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
	public Transform AboveCheck;

	public LadderMovementController ladderMovementController;

	public Transform playerTransform;

	public bool playerAbove = false;

	public float HeightOfLadder = 0f;
	public float WidthOfLadder = 0f;

	private SpriteRenderer spriteRenderer;

	private BoxCollider2D boxCollider;

	private bool playerNearLadder = false;

	private CapsuleCollider2D playerCollider;

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

		HeightOfLadder = boxCollider.size.y;
		WidthOfLadder = boxCollider.size.x;
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
			ladderMovementController.MountLadder(CalcPlayerTargetPositionOnLadder(), transform.position, boxCollider.size.y);
			boxCollider.isTrigger = true;
		}
		else if ((!playerAbove) || ladderMovementController.IsPlayerOnLadder)
		{
			boxCollider.isTrigger = true;
		}


		if (Input.GetKeyUp(KeyCode.W) && !ladderMovementController.IsPlayerOnLadder && !playerAbove)
		{
			playerNearLadder = PlayerInLadderBounds();
		}

		if (playerNearLadder && Input.GetKeyUp(KeyCode.W) && !ladderMovementController.IsPlayerOnLadder && !playerAbove)
		{
			ladderMovementController.MountLadder(CalcPlayerTargetPositionOnLadder(), transform.position, boxCollider.size.y);
		}



	}

	private bool PlayerInLadderBounds()
	{
		if (playerCollider == null)
		{
			return false;
		}

		Vector3 playerPos = playerTransform.position;
		playerPos.y -= playerCollider.size.y / 2f;

		float localHeight = HeightOfLadder / 2f;
		float localWidth = WidthOfLadder / 2f;

		if (playerPos.y < transform.position.y + localHeight && playerPos.y > transform.position.y - localHeight &&
		playerPos.x < transform.position.x + localWidth && playerPos.x > transform.position.x - localWidth)
		{
			print("T");
			return true;
		}
		else
		{
			print("F");
			return false;
		}
	}

	private Vector3 CalcPlayerTargetPositionOnLadder()
	{
		Vector3 targ = playerTransform.position - transform.position;

		if (playerTransform.position.y < transform.position.y - (HeightOfLadder / 2f))
		{
			targ.y = transform.position.y - (HeightOfLadder / 2f) + .9f;
		}
		else if (playerTransform.position.y > transform.position.y + (HeightOfLadder / 2f))
		{
			targ.y = transform.position.y + (HeightOfLadder / 2f) + .9f;
		}
		else
		{
			targ.y = playerTransform.position.y;
		}

		targ.x = transform.position.x;

		targ.z = playerTransform.position.z;

		return targ;
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (ladderMovementController == null) ladderMovementController = other.GetComponent<LadderMovementController>();
			if (playerTransform == null) playerTransform = other.transform;
			if (playerTransform != null) playerCollider = playerTransform.GetComponent<CapsuleCollider2D>();

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
