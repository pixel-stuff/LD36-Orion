using UnityEngine;
using System.Collections;

public class UIMenuManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (a != null) {
			Debug.Log ("LOADING : " + a.progress);
			Debug.Log ("is done : " + a.isDone + "(" + a.progress*100f +"%)" );
		}
		if (Time.time - timeStartLoading >= 10f) {
			a.allowSceneActivation = true;
		}*/
	}

	AsyncOperation a;
	float timeStartLoading;

	public void GoToLevelScene(){
        StartCoroutine(DelayGoToLevelScene(2.0f));
    }

    IEnumerator DelayGoToLevelScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameStateManager.setGameState(GameState.Playing);
        a = Application.LoadLevelAsync("SceneMathias");
        //a.allowSceneActivation = false;
        timeStartLoading = Time.time;
    }
}
