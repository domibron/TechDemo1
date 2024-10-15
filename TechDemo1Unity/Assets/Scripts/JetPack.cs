using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JetPack : MonoBehaviour
{
	public bool CanUseJetPack = false;

	public float ForceOfJetPack = 3f;

	public float MaxFuel = 100f;

	public float FuelUsageRate = 1f;

	public float TimeToAllowUse = 0.1f;

	public float AccelRateFromDecent = 0.8f;
	public float ReductionFromMaxSpeed = 0.8f;

	public AudioSource jetPackBeepAudioSource;

	public AudioClip jetPackBeep;

	public JetPackGuage jetPackGuage;

	public float BeepInterval = 0.2f;

	public AudioSource jetPackAudioSource;

	public bool UsingJetPack = false;

	public Light2D light2D;

	public bool Locked = false;

	private Animator playerAnimator;
	private Rigidbody2D attachedRigidBody;
	private PlayerController playerController;
	private SpriteRenderer spriteRenderer;

	public float Fuel;

	public bool SetToMaxFuel = false;

	private bool allowedToUseJetPack = false;

	private float countDown = 0f;

	private float beepDelay = 0;

	private Vector3 lightPos;

	// Start is called before the first frame update
	void Start()
	{
		playerAnimator = GetComponent<Animator>();

		attachedRigidBody = GetComponent<Rigidbody2D>();

		playerController = GetComponent<PlayerController>();

		spriteRenderer = GetComponent<SpriteRenderer>();

		lightPos = light2D.transform.localPosition;

		if (SetToMaxFuel) Fuel = MaxFuel;

		jetPackGuage.MaxTankSize = MaxFuel;

		jetPackAudioSource.loop = true;

	}

	// Update is called once per frame
	void Update()
	{
		if (Locked)
		{
			UsingJetPack = false;
			allowedToUseJetPack = false;
			return;
		}

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
		else if (Fuel < MaxFuel * 0.25f && beepDelay <= 0 && allowedToUseJetPack && Fuel > 0)
		{
			beepDelay = Mathf.Lerp(0.1f, BeepInterval, Fuel / (MaxFuel * 0.25f));

			jetPackBeepAudioSource.PlayOneShot(jetPackBeep);
		}


		//      if (Input.GetKeyDown(KeyCode.W) && allowedToUseJetPack && fuel > 0)
		//{
		//          if (attachedRigidBody.velocity.normalized.y < 0)
		//          {
		//              attachedRigidBody.AddForce((Vector2.up * (-attachedRigidBody.velocity.y)) * AccelRateFromDecent, ForceMode2D.Force);
		//          }
		//      }

		if (Input.GetKey(KeyCode.W) && allowedToUseJetPack && Fuel > 0)
		{
			Vector2 additionalForce = Vector2.zero;

			if (attachedRigidBody.velocity.y > (Vector2.up * ForceOfJetPack).y)
			{
				additionalForce.y = ((Vector2.up * ForceOfJetPack).y - (attachedRigidBody.velocity.y + ForceOfJetPack)) * ReductionFromMaxSpeed;

			}

			attachedRigidBody.AddForce((Vector2.up * ForceOfJetPack + additionalForce), ForceMode2D.Impulse);


			Fuel -= Time.deltaTime * FuelUsageRate;



			if (!jetPackAudioSource.isPlaying) jetPackAudioSource.Play();

			UsingJetPack = true;
		}
		else
		{
			if (jetPackAudioSource.isPlaying) jetPackAudioSource.Stop();

			UsingJetPack = false;
		}

		HandleAnimations();

		light2D.enabled = UsingJetPack;

		light2D.transform.localPosition = new Vector3((spriteRenderer.flipX ? -lightPos.x : lightPos.x), light2D.transform.localPosition.y, light2D.transform.localPosition.z);


		jetPackGuage.TankFill = Fuel;

		if (Fuel < MaxFuel * 0.25f)
		{
			jetPackGuage.LowFuel = true;
		}
	}

	private void HandleAnimations()
	{
		playerAnimator.SetBool("IsFlying", UsingJetPack);


	}
}
