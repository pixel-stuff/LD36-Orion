using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ennemi : Perso {

	[Header("Ennemi")]
	public Sprite[] m_ennemiState;

	#region Coroutine
	public override IEnumerator CoroutBeingAttack(){

		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0).Length);

		float newPercent = m_pv * 100 / m_pvMax;

		int lastIndex = m_ennemiState.Length - 1 - (int)m_currentPercentLife/((int)m_pvMax / m_ennemiState.Length);
		int newIndex = m_ennemiState.Length - 1 - (int)newPercent/((int)m_pvMax / m_ennemiState.Length);

		/*if (lastIndex != newIndex) {
			this.GetComponent<Image>().sprite = m_ennemiState [newIndex];
		}*/
		m_currentPercentLife = newPercent;
	}

	#endregion Coroutine
}
