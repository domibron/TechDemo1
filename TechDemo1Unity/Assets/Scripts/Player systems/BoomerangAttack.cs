using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAttack : MonoBehaviour
{
	public Transform Boomerang;

	public float Speed = 3f;

	public float Range = 5f;

	public float Tolerance = 0.3f;

	public float PickUpRange = 0.5f;

	private Rigidbody2D attachedRigidBody;

	private Rigidbody2D boomerangRB;

	private SpriteRenderer spriteRenderer;

	private Vector3 target;

	[HideInInspector]
	public bool Returning = true;

	[HideInInspector]
	public bool hasBoomerang = true;

	// Start is called before the first frame update
	void Start()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();
		boomerangRB = Boomerang.GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector3.Distance(Boomerang.position, target) <= 0.1f && !Returning)
		{
			Returning = true;
		}

		if (Returning)
		{
			target = transform.position;
		}

		boomerangRB.velocity = ((target - Boomerang.position).normalized * Speed);

		if (Vector3.Distance(Boomerang.position, transform.position) <= PickUpRange && Returning)
		{
			hasBoomerang = true;
		}

		if (Input.GetKey(KeyCode.Space) && hasBoomerang)
		{
			Attack();
		}

		Boomerang.gameObject.SetActive(!hasBoomerang);
	}




	public void Attack()
	{
		Boomerang.position = transform.position;

		Vector2 velocity = attachedRigidBody.velocity.normalized;

		if (velocity.x > +Tolerance)
		{
			target.x = 1;
		}
		else if (velocity.x < -Tolerance)
		{
			target.x = -1;
		}
		else if (!spriteRenderer.flipX)
		{
			target.x = 1f;
		}
		else if (spriteRenderer.flipX)
		{
			target.x = -1f;
		}


		if (velocity.y > +Tolerance)
		{
			target.y = 1;
		}
		else if (velocity.y < -Tolerance)
		{
			target.y = -1;
		}
		else
		{
			target.y = 0f;
		}

		target.z = 0f;

		target = transform.position + target.normalized * Range;

		Returning = false;

		hasBoomerang = false;
	}
}
