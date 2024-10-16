using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCollectable : MonoBehaviour
{
	public float FuelAmmount = 10;

	public bool UsePercentageInstead = false;

	[Range(0, 1)]
	public float PercentageFill = 0.1f;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			JetPack jetPack = other.transform.GetComponent<JetPack>();


			if (!UsePercentageInstead)
			{
				if (jetPack.Fuel + FuelAmmount > jetPack.MaxFuel)
				{
					jetPack.Fuel = jetPack.MaxFuel;
				}
				else jetPack.Fuel += FuelAmmount;
			}
			else
			{
				if (jetPack.Fuel + (jetPack.MaxFuel * PercentageFill) > jetPack.MaxFuel)
				{
					jetPack.Fuel = jetPack.MaxFuel;
				}
				else jetPack.Fuel += jetPack.MaxFuel * PercentageFill;
			}

			Destroy(gameObject);
		}
	}
}
