using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitScreenSize : MonoBehaviour
{
	Camera _camera;

	// Start is called before the first frame update
	void Start()
	{
		_camera = GetComponent<Camera>();




		AdjustScreenRatio();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void AdjustScreenRatio()
	{
		float targetAspect = 3f / 2f;

		float windowAspect = (float)Screen.width / (float)Screen.height;

		float scaleHeight = windowAspect / targetAspect;



		if (scaleHeight < 1f)
		{
			Rect rect = _camera.rect;

			rect.width = 1.0f;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (1f - scaleHeight) / 2f;

			_camera.rect = rect;
		}
		else
		{
			float scaleWidth = 1f / scaleHeight;

			Rect rect = _camera.rect;

			rect.width = scaleWidth;
			rect.height = 1f;
			rect.x = (1f - scaleWidth) / 2f;
			rect.y = 0;

			_camera.rect = rect;
		}
	}
}
