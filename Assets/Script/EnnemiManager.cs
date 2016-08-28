using UnityEngine;
using System.Collections;

public class EnnemiManager : MonoBehaviour {


	[SerializeField]
	private Ennemi[] m_ennemis;
	private int m_currentEnnemi = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_ennemis.Length; i++) {
			if (i == 0) {
				m_ennemis [m_currentEnnemi].StarEnnemi ();
				this.GetComponent<AudioSource> ().Play ();
			} else {
				m_ennemis [i].gameObject.SetActive (false);
			}
			m_ennemis [i].OnDeadEvent += GoToNextEnnemi;
		}
	}

	public void GoToNextEnnemi(){
		m_ennemis [m_currentEnnemi].gameObject.SetActive (false);
		m_ennemis [m_currentEnnemi].transform.localScale = Vector3.one;
		m_currentEnnemi++;
		if (m_currentEnnemi >= m_ennemis.Length) {
			m_currentEnnemi = 0;
		}
		m_ennemis [m_currentEnnemi].gameObject.SetActive (true);
		m_ennemis [m_currentEnnemi].StarEnnemi ();
		this.GetComponent<AudioSource> ().Play ();
	}

	void OnDestroy(){
		for (int i = 0; i < m_ennemis.Length; i++) {
			m_ennemis [i].OnDeadEvent -= GoToNextEnnemi;
		}
	}
}
