using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsablePlatforms : MonoBehaviour
{
	public float WaitTime = 2f;

	public float ResetTime = 5f;

	public float FallSpeed = 5f;

	public float ShakeAmmount = 0.2f;

	public float ShakeFrequancy = 3f;

	private float countDown = 0f;

	private float resetCounting = 0f;

	private bool isPlayerOn = false;

	private bool isReseting = false;

	private Vector3 originalPos;

	// Start is called before the first frame update
	void Start()
	{
		countDown = WaitTime;
		originalPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (isPlayerOn && countDown >= 0)
		{
			countDown -= Time.deltaTime;

			float wave = (Mathf.Sin((countDown / WaitTime) * ShakeFrequancy) + 1) / 2f;

			print(wave);

			transform.position = new Vector3(Mathf.Lerp(originalPos.x - ShakeAmmount, originalPos.x + ShakeAmmount, wave), transform.position.y, transform.position.z);
		}

		if (countDown < 0f && !isReseting)
		{
			isReseting = true;
			resetCounting = ResetTime;

			StartCoroutine(CollapsePlatform());
		}
	}

	IEnumerator CollapsePlatform()
	{
		while (resetCounting > 0)
		{
			transform.Translate(-transform.up * Time.deltaTime * FallSpeed);

			resetCounting -= Time.deltaTime;

			yield return null;
		}

		countDown = WaitTime;

		isReseting = false;

		isPlayerOn = false;

		transform.position = originalPos;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		isPlayerOn = true;
	}
}
