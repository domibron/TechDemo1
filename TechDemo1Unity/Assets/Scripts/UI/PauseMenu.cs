using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
	public static PauseMenu instance;

	public GameObject PauseMenuScreen;

	public GameObject DialogWindow;

	public TMP_Text DialogText;

	private PlayerController playerController;
	private bool playerControllerPrevState = false;
	private JetPack jetPack;
	private bool jetPackPrevState = false;
	private LadderMovementController ladderMovementController;
	private bool ladderMovementPrevState = false;

	private bool isQuitting = false;

	private bool isPaused = false;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		GameObject player = GameObject.Find("Player");

		if (player == null)
		{
			throw new Exception("Failed to locate player, exited system for safty!");
		}

		playerController = player.GetComponent<PlayerController>();
		jetPack = player.GetComponent<JetPack>();
		ladderMovementController = player.GetComponent<LadderMovementController>();

		DialogWindowClose();
		Resume();

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused) Resume();
			else Pause();


		}
	}

	public void ReturnToMainMenu()
	{
		isQuitting = false;
		OpenDialogWindow(isQuitting);
	}

	public void QuitGame()
	{
		isQuitting = true;
		OpenDialogWindow(isQuitting);
	}

	public void OpenDialogWindow(bool isQuitting)
	{
		DialogWindow.SetActive(true);

		DialogText.text = "Are you sure you want to " + (isQuitting ? "quit the game?" : "return to main menu?");
	}

	public void DialogYes()
	{
		if (isQuitting)
		{
			Application.Quit();
		}
		else
		{
			SceneManager.LoadScene(0);
		}
	}

	public void DialogWindowClose()
	{
		DialogWindow.SetActive(false);
	}

	public void Pause()
	{
		Time.timeScale = 0f;

		playerControllerPrevState = playerController.LockMovement;
		jetPackPrevState = jetPack.Locked;

		ladderMovementPrevState = ladderMovementController.IsLadderMovementLocked;

		playerController.LockMovement = true;
		jetPack.Locked = true;

		ladderMovementController.IsLadderMovementLocked = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		PauseMenuScreen.SetActive(true);

		isPaused = true;
	}

	public void Resume()
	{
		Time.timeScale = 1.0f;

		playerController.LockMovement = playerControllerPrevState;
		jetPack.Locked = jetPackPrevState;

		ladderMovementController.IsLadderMovementLocked = ladderMovementPrevState;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		DialogWindowClose();

		PauseMenuScreen.SetActive(false);

		isPaused = false;


	}
}
