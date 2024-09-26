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

	void Awake()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}


	void Update()
	{
		Vector2 InputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


		Vector2 forceToApply = new Vector2();


		if (InputVector.x != 0)
		{
			forceToApply = ((new Vector2(InputVector.normalized.x, 0) * MaxSpeed) - new Vector2(attachedRigidBody.velocity.x, 0)) * AccelRate;

		}
		else
		{
			forceToApply = new Vector2(-attachedRigidBody.velocity.x, 0) * DeaccelRate;
		}


		attachedRigidBody.AddForce(forceToApply);



		bool isGrounded = Physics2D.Raycast(transform.position, -transform.up, 1f, ~(1 << 3));



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


}
