using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject PauseMenuScreen;

	private PlayerController playerController;
	private bool playerControllerPrevState = false;
	private JetPack jetPack;
	private bool jetPackPrevState = false;
	private LadderMovementController ladderMovementController;
	private bool ladderMovementPrevState = false;

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

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Resume()
	{
		Time.timeScale = 1.0f;

		playerController.LockMovement = playerControllerPrevState;
		jetPack.Locked = jetPackPrevState;

		ladderMovementController.IsLadderMovementLocked = ladderMovementPrevState;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PauseMenuScreen.SetActive(false);
	}
}
