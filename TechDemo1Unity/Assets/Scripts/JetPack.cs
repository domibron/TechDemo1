using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
	public bool CanUseJetPack = false;

	public float ForceOfJetPack = 3f;

	public float MaxFuel = 100f;

	public float FuelUsageRate = 1f;

	public float TimeToAllowUse = 0.1f;


	public JetPackGuage jetPackGuage;

	private Rigidbody2D attachedRigidBody;

	private float fuel;

	private bool allowedToUseJetPack = false;

	private float countDown = 0f;

	// Start is called before the first frame update
	void Start()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();

		fuel = MaxFuel;

		jetPackGuage.MaxTankSize = MaxFuel;
	}

	// Update is called once per frame
	void Update()
	{
		// from here

		if (CanUseJetPack && !allowedToUseJetPack && countDown <= 0)
		{
			countDown = TimeToAllowUse;
		}
		else if (!CanUseJetPack)
		{
			countDown = 0;
		}


		if (countDown >= 0)
		{
			countDown -= Time.deltaTime;
		}

		if (CanUseJetPack && countDown <= 0)
		{
			allowedToUseJetPack = true;
		}
		else if (!CanUseJetPack)
		{
			allowedToUseJetPack = false;
		}

		// to here
		// done at 3am :3


		if (Input.GetKey(KeyCode.W) && allowedToUseJetPack && fuel > 0)
		{
			attachedRigidBody.AddForce(transform.up * ForceOfJetPack, ForceMode2D.Force);

			fuel -= Time.deltaTime * FuelUsageRate;
		}

		jetPackGuage.TankFill = fuel;

		if (fuel < MaxFuel * 0.25f)
		{
			jetPackGuage.LowFuel = true;
		}
	}
}
