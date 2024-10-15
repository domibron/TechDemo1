using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool LockMovement = false;

	public float MaxSpeed = 5f;
	public float AccelRate = 0.7f;
	public float DeaccelRate = 0.7f;



	public float JumpForce = 10f;

	private Rigidbody2D attachedRigidBody;

	private JetPack jetPack;

	private bool isGrounded = false;


	private Animator animator;

	private SpriteRenderer spriteRenderer;


	void Start()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();

		jetPack = GetComponent<JetPack>();

		animator = GetComponent<Animator>();

		spriteRenderer = GetComponent<SpriteRenderer>();
	}



	void Update()
	{
		if (LockMovement) return;


		if (CameraMovement.Instance.CameraIsMoving)
		{
			attachedRigidBody.simulated = false;
			return;
		}
		else if (!attachedRigidBody.simulated)
		{
			attachedRigidBody.simulated = true;
		}

		isGrounded = CheckIfGrounded();

		// HandleGravity();

		jetPack.CanUseJetPack = !isGrounded;

		Vector2 InputVector = GetInputAsVector2();

		ApplyMovementForces(InputVector);

		HandleJumping();

		PassPlayerPositionToCameraMovement();

		HandleAnimations(InputVector);
	}

	// private void HandleGravity()
	// {
	// 	Vector2 gravForce = Vector2.zero;

	// 	if (!isGrounded)
	// 	{
	// 		gravForce = new Vector2(0, Physics2D.gravity.y);
	// 	}
	// 	else if (isGrounded && attachedRigidBody.velocity.y > -2f)
	// 	{
	// 		gravForce = new Vector2(0, -2f);
	// 	}

	// 	gravForce *= 1f;

	// 	NewVelocityForFrame += gravForce;

	// 	attachedRigidBody.AddForce(gravForce);
	// }

	private void HandleAnimations(Vector2 InputVector)
	{
		animator.SetBool("IsMoving", InputVector.x != 0 && isGrounded);

		if (InputVector.x < 0)
		{
			spriteRenderer.flipX = true;
		}
		else if (InputVector.x > 0)
		{
			spriteRenderer.flipX = false;
		}

		animator.SetBool("Falling", attachedRigidBody.velocityY < 0 && !isGrounded);


	}

	private void PassPlayerPositionToCameraMovement()
	{
		CameraMovement.Instance?.CheckPlayerIsAtBounds(transform.position);
	}

	private void HandleJumping()
	{
		if (Input.GetKeyDown(KeyCode.W) && isGrounded)
		{
			Vector2 counterGrav = attachedRigidBody.velocity;

			counterGrav.x = 0;

			if (counterGrav.y > 0) counterGrav.y = 0;
			else
			{
				counterGrav.y = MathF.Abs(counterGrav.y);
			}

			// if (!IgnoreRBVelocity) 
			attachedRigidBody.AddForce(new Vector2(0, JumpForce) + counterGrav, ForceMode2D.Impulse);

			animator.SetTrigger("Jump");
		}
	}

	public static Vector2 GetInputAsVector2()
	{
		return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
	}

	private void ApplyMovementForces(Vector2 InputVector)
	{
		Vector2 forceToApply = new Vector2();


		if (InputVector.x != 0)
		{
			forceToApply = ((new Vector2(InputVector.normalized.x, 0) * MaxSpeed) - new Vector2(attachedRigidBody.velocity.x, 0)) * AccelRate;

		}
		else if (isGrounded)
		{
			forceToApply = new Vector2(-attachedRigidBody.velocity.x, 0) * DeaccelRate;
		}



		// if (InputVector.x != 0 && IgnoreRBVelocity)
		// {
		// 	forceToApply = ((new Vector2(InputVector.normalized.x, 0) * MaxSpeed) - new Vector2(VelocityForFrame.x, 0)) * AccelRate;

		// }
		// else if (isGrounded && IgnoreRBVelocity)
		// {
		// 	forceToApply = new Vector2(-VelocityForFrame.x, 0) * DeaccelRate;
		// }



		// if (!IgnoreRBVelocity) 
		attachedRigidBody.AddForce(forceToApply);

	}

	private bool CheckIfGrounded()
	{
		return Physics2D.Raycast(transform.position, -transform.up, (GetComponent<CapsuleCollider2D>().size.y / 2f) + .5f, ~(1 << 3));
	}
}
