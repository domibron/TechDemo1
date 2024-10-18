using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public GameObject AreYouSurePopUp;

	// Start is called before the first frame update
	void Start()
	{
		ClosePopUp();

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartGame(int sceneID)
	{
		SceneManager.LoadScene(sceneID);
	}

	public void QuitButtonPressed()
	{
		AreYouSurePopUp.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ClosePopUp()
	{
		AreYouSurePopUp.SetActive(false);
	}


}
