using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutGameOver : MonoBehaviour {


	void Start(){
		GameStateManager.onChangeStateEvent += StartGameOverAnimation;
	}

	public void StartGameOverAnimation (GameState newState) {
		if (newState == GameState.GameOver) {
			StartCoroutine (CoroutFadeOutAnim ());
		}
	}

	#region Coroutine
	public IEnumerator CoroutFadeOutAnim(){
		float alpha = 0;
		Color col = this.GetComponent<Image>().color;
		do{
			alpha += 0.015f;
			col.a = alpha;
			this.GetComponent<Image>().color = col;
			yield return new WaitForEndOfFrame();
		}while(alpha <= 1.0f);
		SceneManager.LoadScene ("GameOverScene");
	}
	#endregion Coroutine
}
