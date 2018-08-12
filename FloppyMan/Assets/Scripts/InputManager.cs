using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	public static InputManager Instance { get; private set; }

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy (gameObject);
		
		Instance = this;
		
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//// If there exists a player, then it checks for key input.
		//if (GameManager.Instance.GetPlayer ()) 
		//{
		//	// Checks for pressed keys and makes the player do
		//	// the action it needs to do based on the pressed key.
		//	if (Input.GetKeyDown (KeyCode.Space))
		//		GameManager.Instance.GetPlayer ().Jump ();
		
		//	if (Input.GetKeyDown (KeyCode.DownArrow))
		//		GameManager.Instance.GetPlayer ().Slide ();
		
		//	if (Input.GetKeyDown (KeyCode.A))
		//		GameManager.Instance.GetPlayer ().ChangeColor ("Red");

		//	if (Input.GetKeyDown (KeyCode.S))
		//		GameManager.Instance.GetPlayer ().ChangeColor ("Blue");

		//	if (Input.GetKeyDown (KeyCode.Escape)) 
		//	{
		//		GameManager.Instance.PauseGame ();
		//		GUIManager.Instance.ShowInGameMenu ();
		//	}
		//}
	}
}