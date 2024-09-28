using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MaxSpeed = 5f;
	public float AccelRate = 0.7f;
	public float DeaccelRate = 0.7f;


	public float JumpForce = 10f;


	private Rigidbody2D attachedRigidBody;

	private JetPack jetPack;

	private bool isGrounded = false;



	void Start()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();

		jetPack = GetComponent<JetPack>();

	}



	void Update()
	{


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

		jetPack.CanUseJetPack = !isGrounded;

		Vector2 InputVector = GetInputAsVector2();

		ApplyMovementForces(InputVector);

		HandleJumping();

		PassPlayerPositionToCameraMovement();
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
				counterGrav.y = +counterGrav.y;
			}

			attachedRigidBody.AddForce(new Vector2(0, JumpForce) + counterGrav, ForceMode2D.Impulse);
		}
	}

	private static Vector2 GetInputAsVector2()
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


		attachedRigidBody.AddForce(forceToApply);
	}

	private bool CheckIfGrounded()
	{
		return Physics2D.Raycast(transform.position, -transform.up, 1f, ~(1 << 3));
	}
}
