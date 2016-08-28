using UnityEngine;
using System.Collections;

public class EnnemiManager : MonoBehaviour {


	[SerializeField]
	private Ennemi[] m_ennemis;
	private int m_currentEnnemi = -1;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_ennemis.Length; i++) {
			m_ennemis [i].OnDeadEvent += DisplayNextEnnemi;
		}
		DisplayNextEnnemi ();
	}



	public void DisplayNextEnnemi(){
		if (m_currentEnnemi > -1) {
			m_ennemis [m_currentEnnemi].gameObject.SetActive (false);
			m_ennemis [m_currentEnnemi].transform.localScale = Vector3.one;
		}
		m_currentEnnemi++;
		if (m_currentEnnemi >= m_ennemis.Length) {
			m_currentEnnemi = 0;
		}
		m_ennemis [m_currentEnnemi].gameObject.SetActive (true);
		m_ennemis [m_currentEnnemi].StarEnnemi ();
	}

	public void EnnemiBeingAttack(int value){
		m_ennemis [m_currentEnnemi].StartBeingAttack (value);
	}


	void OnDestroy(){
		for (int i = 0; i < m_ennemis.Length; i++) {
			m_ennemis [i].OnDeadEvent -= DisplayNextEnnemi;
		}
	}
}
