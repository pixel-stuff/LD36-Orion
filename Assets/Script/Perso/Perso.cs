using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Perso : MonoBehaviour {

	[Header("Perso")]
	protected int m_pvMax = 100;
	public int m_pv = 0;
	protected float m_currentPercentLife;

	void Awake(){
		m_pv = m_pvMax;
		m_currentPercentLife = 100 * m_pv / m_pvMax;
	}

	public void StartAttack(){
		this.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	public void StartBeingAttack(int degat){
		m_pv -= degat;
		this.GetComponent<Animator> ().SetTrigger ("BeingAttack");
		StartCoroutine (CoroutBeingAttack ());
	}

	#region Coroutine
	public abstract IEnumerator CoroutBeingAttack ();
	#endregion Coroutine
}
