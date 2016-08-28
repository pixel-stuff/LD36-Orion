using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Perso : MonoBehaviour {

	[Header("Perso")]
	protected int m_pvMax = 100;
	public int m_pv = 0;
	protected float m_currentPercentLife;
	protected int m_coeffDef = 1;
	void Awake(){
		m_pv = m_pvMax;
		m_currentPercentLife = 100 * m_pv / m_pvMax;
	}

	public void StartAttack(){
		this.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	public void StartHeal(int value){
		m_pv += value;
		this.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	public void StartBeingAttack(int degat){
		m_pv -= (degat/m_coeffDef);
		this.GetComponent<Animator> ().SetTrigger ("BeingAttack");
		StartCoroutine (CoroutBeingAttack ());
	}

	public void StartSpawn(){
		this.GetComponent<Animator> ().SetTrigger ("Spawn");
	}

	public void StartDeath(){
		this.GetComponent<Animator> ().SetTrigger ("Death");
	}
	#region Coroutine
	public abstract IEnumerator CoroutBeingAttack ();
	#endregion Coroutine
}
