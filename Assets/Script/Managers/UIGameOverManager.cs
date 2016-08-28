using UnityEngine;
using System.Collections;

public class UIGameOverManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioManager.m_instance.PlayMenuMusic ();
	}

	public void ReturnToSceneMenu(){
		GameStateManager.setGameState (GameState.Menu);
		Application.LoadLevelAsync ("MenuScene");
	}
	
	public void ReturnToLevelScene(){
		GameStateManager.setGameState (GameState.Playing);
		Application.LoadLevelAsync ("SceneMathias");
		
	}
}
