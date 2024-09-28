using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetPackGuage : MonoBehaviour
{
	public Vector2 FullPosition;
	public Vector2 EmptyPosition;

	public Transform GuageTransform;

	public Image LightImage;

	public Sprite LightOnSprite;
	public Sprite LightOffSprite;

	public float BlinkInterval = .2f;

	public float MaxTankSize = 100f;
	public float TankFill = 100f;

	public bool LowFuel = false;

	private bool blinkingLight = false;

	// Start is called before the first frame update
	void Start()
	{
		LightImage.sprite = LightOffSprite;
	}

	// Update is called once per frame
	void Update()
	{
		TankFill = Mathf.Clamp(TankFill, 0, MaxTankSize);

		GuageTransform.GetComponent<RectTransform>().localPosition = Vector2.Lerp(EmptyPosition, FullPosition, (TankFill / MaxTankSize));

		if (!blinkingLight && LowFuel && TankFill > 0)
		{
			StartCoroutine(BlinkLight());

		}

		if (TankFill <= 0)
		{
			blinkingLight = false;
			StopCoroutine(BlinkLight());
			LightImage.sprite = LightOnSprite;
		}
	}

	private IEnumerator BlinkLight()
	{
		blinkingLight = true;

		while (blinkingLight)
		{
			yield return new WaitForSeconds(BlinkInterval);
			LightImage.sprite = LightOnSprite;
			yield return new WaitForSeconds(BlinkInterval);
			LightImage.sprite = LightOffSprite;
		}

	}
}
