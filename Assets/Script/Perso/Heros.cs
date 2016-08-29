using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Heros : Perso {

	[Header("Heros")]
	[SerializeField]
	private Image m_bracelet;

	public Sprite[] m_braceletState;

	public AudioClip m_apparitionSound;
	public AudioClip m_attackSound;
	public AudioClip m_healSound;
	public AudioClip m_defenseSound;

	public GameObject m_particleAttack;
	public GameObject m_particleDefense;
	public GameObject m_particleHeal;

	void Start(){
		if (AudioManager.m_instance != null) {
			AudioManager.m_instance.PlayFightMusic ();
		}
		this.GetComponent<Image> ().enabled = true;
		this.GetComponent<AudioSource> ().clip = m_apparitionSound;
		this.GetComponent<AudioSource> ().Play ();

	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.StartAction (12,ConstelationType.ATTACK);
		}
	}
	#region Controls
	public void StartAction(int value, ConstelationType type){
		switch (type) {
		case ConstelationType.ATTACK:
			this.GetComponent<AudioSource> ().clip = m_attackSound;
			this.GetComponent<AudioSource> ().Play ();
			StartCoroutine (CoroutAttack (value));
			break;
		case ConstelationType.DEFENCE:
			this.GetComponent<AudioSource> ().clip = m_defenseSound;
			this.GetComponent<AudioSource> ().Play ();
			m_particleDefense.SetActive (true);
			this.m_coeffDef = 2;
		break;
		case ConstelationType.HEAL:
			m_particleHeal.SetActive (true);
			this.GetComponent<AudioSource> ().clip = m_healSound;
			this.GetComponent<AudioSource> ().Play ();
			this.m_pv += value;
			StartCoroutine (CoroutHeal ());
			break;
		}
	}
	#endregion Controls

	#region Coroutine

	public IEnumerator CoroutHeal(){
		yield return new WaitForSeconds(0.3f);
		m_particleHeal.SetActive (false);
	}

	public IEnumerator CoroutAttack(int value){
		m_particleAttack.GetComponent<ParticleSystem> ().Play ();
		this.StartAttack ();
		yield return new WaitForSeconds (0.3f);
		FindObjectOfType<EnnemiManager> ().EnnemiBeingAttack(value);
	}

	public override IEnumerator CoroutBeingAttack(){

		int iteration = 4;
		float timeToWait = 0.10f;
		do{
			if(iteration%2 == 0){
				this.GetComponent<HeroBlink> ().SetBlinkColorAndOpacity (Color.white, 1.0f);
				timeToWait = 0.20f;
			}else{
				this.GetComponent<HeroBlink> ().SetBlinkColorAndOpacity (Color.white, 0.0f);
				timeToWait = 0.10f;
			}
			iteration--;
			yield return new WaitForSeconds(timeToWait);
		}while(iteration > 0);

		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (this.GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0)[0].clip.length);

		float newPercent = m_pv * 100 / m_pvMax;

		int lastIndex = m_braceletState.Length - 1 - (int)m_currentPercentLife/((int)m_pvMax / m_braceletState.Length);
		int newIndex = m_braceletState.Length - 1 - (int)newPercent/((int)m_pvMax / m_braceletState.Length);

		if (lastIndex != newIndex && newIndex >= 0) {
			m_bracelet.sprite = m_braceletState [newIndex];
		}
		m_currentPercentLife = newPercent;


		if (m_pv <= 0) {
			GameStateManager.setGameState (GameState.GameOver);
		}
		m_coeffDef = 1;
		m_particleDefense.SetActive (false);
	}

	#endregion Coroutine
}
