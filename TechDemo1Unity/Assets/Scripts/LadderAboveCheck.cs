using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAboveCheck : MonoBehaviour
{
	private Ladder ladder;

	// Start is called before the first frame update
	void Start()
	{
		ladder = GetComponentInParent<Ladder>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (ladder.ladderMovementController == null) ladder.ladderMovementController = other.transform.GetComponent<LadderMovementController>();
			if (ladder.ladderMovementController != null) ladder.playerTransform = ladder.ladderMovementController.transform;

			ladder.playerAbove = true;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			ladder.playerAbove = true;
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
