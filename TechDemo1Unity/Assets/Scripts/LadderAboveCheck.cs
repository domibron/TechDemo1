using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAboveCheck : MonoBehaviour
{
	private Ladder ladder;

	private float playerHeight;

	private Transform player;

	// Start is called before the first frame update
	void Start()
	{
		ladder = GetComponentInParent<Ladder>();
	}

	// Update is called once per frame
	void Update()
	{
		if (player == null) return;


		if (player.position.y - (playerHeight / 2f) > ladder.transform.position.y + (ladder.HeightOfLadder / 2f))
		{
			ladder.playerAbove = true;
		}
		else
		{
			ladder.playerAbove = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (ladder.ladderMovementController == null) ladder.ladderMovementController = other.transform.GetComponent<LadderMovementController>();
			if (ladder.ladderMovementController != null) ladder.playerTransform = ladder.ladderMovementController.transform;

			player = other.transform;
			playerHeight = player.GetComponent<CapsuleCollider2D>().size.y;

			if (player.position.y - (playerHeight / 2f) > ladder.transform.position.y + (ladder.HeightOfLadder / 2f))
			{
				ladder.playerAbove = true;
			}
			else
			{
				ladder.playerAbove = false;
			}

		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (player.position.y - (playerHeight / 2f) > ladder.transform.position.y + (ladder.HeightOfLadder / 2f) && Vector3.Distance(transform.position, player.position) < 0.75f)
			{
				ladder.playerAbove = true;
			}
			else
			{
				ladder.playerAbove = false;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			ladder.playerAbove = false;
		}
	}
}
