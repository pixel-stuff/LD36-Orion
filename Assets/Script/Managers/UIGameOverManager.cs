using UnityEngine;
using System.Collections;

public class UIGameOverManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioManager.m_instance.PlayMenuMusic ();
	}

	public void ReturnToSceneMenu(){
        // Playing sound for the button click event
        AudioManager.m_instance.PlayMenuButtonSound();
        StartCoroutine(DelayGoToSceneMenu(1.5f));
	}
	
	public void ReturnToLevelScene(){
        // Playing sound for the button click event
        AudioManager.m_instance.PlayMenuButtonSound();

        StartCoroutine(DelayGoToLevelScene(1.5f));
    }

    IEnumerator DelayGoToSceneMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameStateManager.setGameState(GameState.Menu);
        Application.LoadLevelAsync("MenuScene");
    }

    IEnumerator DelayGoToLevelScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameStateManager.setGameState(GameState.Playing);
        Application.LoadLevelAsync("SceneMathias");
    }
}
