﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Ennemi : Perso {

	[Header("Ennemi")]
	public Sprite[] m_ennemiState;
	public int m_powerAttack = 51;
	public Action OnDeadEvent;

	public void StarEnnemi(){
		m_pv = m_pvMax;
		m_currentPercentLife = 100 * m_pv / m_pvMax;
		StartCoroutine (CoroutSpawnAnimation ());
	}
	#region Coroutine
	public override IEnumerator CoroutBeingAttack(){

		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0).Length);

		float newPercent = m_pv * 100 / m_pvMax;

		int lastIndex = m_ennemiState.Length - 1 - (int)m_currentPercentLife/((int)m_pvMax / m_ennemiState.Length);
		int newIndex = m_ennemiState.Length - 1 - (int)newPercent/((int)m_pvMax / m_ennemiState.Length);

		/*if (lastIndex != newIndex && newIndex >= 0) {
			this.GetComponent<Image>().sprite = m_ennemiState [newIndex];
		}*/
		m_currentPercentLife = newPercent;
	}

	public IEnumerator CoroutSpawnAnimation(){
		StartSpawn ();
		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo(0) [0].clip.length);

		StartCoroutine (CoroutLogiqueEnnemi());

	}

	public IEnumerator CoroutLogiqueEnnemi(){
		float lastAttack = Time.time;
		do{
			if( (Time.time - lastAttack) >= 3.0f){
				lastAttack = Time.time;
				this.StartAttack();
				yield return new WaitForSeconds(0.3f);
				FindObjectOfType<Heros>().StartBeingAttack(m_powerAttack);
			}
			yield return new WaitForEndOfFrame();
		}while(GameStateManager.getGameState() != GameState.GameOver && m_pv > 0);

		if (m_pv <= 0) {
			StartDeath ();
			yield return new WaitForEndOfFrame ();
			yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0) [0].clip.length);

			if (OnDeadEvent != null) {
				OnDeadEvent ();
			}
		}
	}
	#endregion Coroutine
}
