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

	public AudioSource jetPackBeepAudioSource;

	public AudioClip jetPackBeep;

	public JetPackGuage jetPackGuage;

	public float BeepInterval = 0.2f;

	public AudioSource jetPackAudioSource;

	public bool UsingJetPack = false;



	private Rigidbody2D attachedRigidBody;

	private float fuel;

	private bool allowedToUseJetPack = false;

	private float countDown = 0f;

	private float beepDelay = 0;

	// Start is called before the first frame update
	void Start()
	{
		attachedRigidBody = GetComponent<Rigidbody2D>();

		fuel = MaxFuel;

		jetPackGuage.MaxTankSize = MaxFuel;

		jetPackAudioSource.loop = true;
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

		if ((CanUseJetPack && countDown <= 0) || UsingJetPack)
		{
			allowedToUseJetPack = true;



		}
		else if (!CanUseJetPack)
		{
			allowedToUseJetPack = false;


		}


		if (beepDelay >= 0)
		{
			beepDelay -= Time.deltaTime;

		}
		else if (fuel < MaxFuel * 0.25f && beepDelay <= 0 && allowedToUseJetPack && fuel > 0)
		{
			beepDelay = Mathf.Lerp(0.1f, BeepInterval, fuel / (MaxFuel * 0.25f));

			jetPackBeepAudioSource.PlayOneShot(jetPackBeep);
		}

		

		if (Input.GetKey(KeyCode.W) && allowedToUseJetPack && fuel > 0)
		{
			attachedRigidBody.AddForce(((Vector2)transform.up * ForceOfJetPack), ForceMode2D.Force);

			fuel -= Time.deltaTime * FuelUsageRate;



			if (!jetPackAudioSource.isPlaying) jetPackAudioSource.Play();

			UsingJetPack = true;
		}
		else
		{
			if (jetPackAudioSource.isPlaying) jetPackAudioSource.Stop();

			UsingJetPack = false;
		}




		jetPackGuage.TankFill = fuel;

		if (fuel < MaxFuel * 0.25f)
		{
			jetPackGuage.LowFuel = true;
		}
	}
}
