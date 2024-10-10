using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
	public BoomerangAttack boomerangAttack;

	public float TimeUntilGiveBack = 5f;

	public int MaxCollisionCount = 15;

	public float SpintSpeed = 15f;

	private int collisionCount = 0;

	private float collisionTimeCounter = 0f;

	private float count = 0;

	private bool StuckCheck = false;


	void Start()
	{
		count = TimeUntilGiveBack;
	}

	void Update()
	{
		if (boomerangAttack.Returning && collisionCount >= MaxCollisionCount)
		{
			StuckCheck = true;
		}

		// we reset collision count every second.
		if (boomerangAttack.Returning && collisionTimeCounter >= 0)
		{
			collisionTimeCounter -= Time.deltaTime;
		}
		else if (boomerangAttack.Returning)
		{
			collisionTimeCounter = 1f;

			if (collisionCount < 3f)
			{
				StuckCheck = false;
			}

			collisionCount = 0;
		}

		if (StuckCheck && count >= 0)
		{
			count -= Time.deltaTime;
		}
		else if (!StuckCheck)
		{
			count = TimeUntilGiveBack;
		}

		// if time expires, then give this back.
		if (count <= 0)
		{
			boomerangAttack.hasBoomerang = true;

			transform.position = boomerangAttack.transform.position;
		}

		// reset
		if (boomerangAttack.hasBoomerang)
		{
			count = TimeUntilGiveBack;

			collisionCount = 0;
		}

		transform.Rotate(Vector3.forward * SpintSpeed);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!boomerangAttack.Returning)
		{
			boomerangAttack.Returning = true;
		}
		else
		{
			collisionCount++;
		}

	}

	void OnCollisionStay2D(Collision2D other)
	{
		if (boomerangAttack.Returning)
		{
			collisionCount++;
		}
	}
}
