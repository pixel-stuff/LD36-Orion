using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Heros : Perso {

	[Header("Heros")]
	[SerializeField]
	private Image m_bracelet;

	public Sprite[] m_braceletState;


	void Update(){
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.StartAction (12,ConstelationType.ATTACK);
		}
	}
	#region Controls
	public void StartAction(int value, ConstelationType type){
		Debug.Log ("coucoxhvodshngpzenv");
		switch (type) {
		case ConstelationType.ATTACK:
			this.StartAttack ();
			StartCoroutine (CoroutAttack (value));
			break;
		case ConstelationType.DEFENCE:
			this.m_coeffDef = 2;
		break;
		case ConstelationType.HEAL:
			
			break;
		}
	}
	#endregion Controls

	#region Coroutine
	public IEnumerator CoroutAttack(int value){
		yield return new WaitForSeconds (0.3f);
		FindObjectOfType<EnnemiManager> ().EnnemiBeingAttack(value);
	}

	public override IEnumerator CoroutBeingAttack(){
		
		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0)[0].clip.length);

		float newPercent = m_pv * 100 / m_pvMax;

		int lastIndex = m_braceletState.Length - 1 - (int)m_currentPercentLife/((int)m_pvMax / m_braceletState.Length);
		int newIndex = m_braceletState.Length - 1 - (int)newPercent/((int)m_pvMax / m_braceletState.Length);

		/*if (lastIndex != newIndex && newIndex >= 0) {
			m_bracelet.sprite = m_braceletState [newIndex];
		}*/
		m_currentPercentLife = newPercent;


		if (m_pv <= 0) {
			GameStateManager.setGameState (GameState.GameOver);
		}
	}

	#endregion Coroutine
}
