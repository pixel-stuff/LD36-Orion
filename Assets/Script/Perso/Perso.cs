using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Perso : MonoBehaviour {

	private int m_pvMax = 100;
	public int m_pv = 0;
	private float m_currentPercentLife;

	[SerializeField]
	private Image m_bracelet;

	public Sprite[] m_braceletState;



	void Awake(){
		m_pv = m_pvMax;
		m_currentPercentLife = 100 * m_pv / m_pvMax;
	}

	// Use this for initialization
	void Start () {
		this.GetComponent<CanvasRenderer> (). SetColor (Color.black);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartAttack(){
		this.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	public void StartBeingAttack(int degat){
		m_pv -= degat;
		this.GetComponent<Animator> ().SetTrigger ("BeingAttack");
		StartCoroutine (BeingAttack ());
	}

	#region Coroutine
	public IEnumerator BeingAttack(){
		
		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0).Length);

		float newPercent = m_pv * 100 / m_pvMax;

		int lastIndex = m_braceletState.Length - 1 - (int)m_currentPercentLife/((int)m_pvMax / m_braceletState.Length);
		int newIndex = m_braceletState.Length - 1 - (int)newPercent/((int)m_pvMax / m_braceletState.Length);

		if (lastIndex != newIndex) {
			m_bracelet.sprite = m_braceletState [newIndex];
		}
		m_currentPercentLife = newPercent;
	}

	#endregion Coroutine
}
