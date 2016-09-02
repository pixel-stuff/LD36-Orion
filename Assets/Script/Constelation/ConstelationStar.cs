using UnityEngine;
using System.Collections;

public enum StarStates {
	IDLE,
	SHOW,
	ACTIVATE,
	OVER

}
public class ConstelationStar : MonoBehaviour {

	public Constelation starConstelation;

	public StarStates _state = StarStates.IDLE;
	public GameObject roundStar;
	public GameObject roundStarShow;
	public GameObject starStar;
	private IEnumerator rotationEnumeration = null;
	private IEnumerator scaleEnumeration = null;
	public ParticleSystem overParticule;
	private float InitialScale = 0;
	public float starScale = 1;
	public float ShowStateScaleAddition = 0.2f;
	public float OverStateScaleAddition = 1f;
	public float minBlink = 0.1f;
	public float durationBlinkDown = 1.2f;
	public float durationBlinkUp = 0.5f;
	public StarStates state { 
		get{return _state;}
		set{ 
			switch (value) {
			case StarStates.ACTIVATE:
				//star.GetComponent < UnityEngine.UI.Image> ().sprite = ActivateSprite;
				roundStar.SetActive (false);
				roundStarShow.SetActive (false);
				starStar.SetActive (true);
				starStar.transform.localScale = new Vector3 (starScale, starScale, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
			
				rotationEnumeration = Rotate (3.0f);
				StartCoroutine (rotationEnumeration);
				break;
			case StarStates.IDLE:
				//star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				roundStar.SetActive (true);
				roundStarShow.SetActive (false);
				starStar.SetActive (false);
				roundStar.transform.localScale = new Vector3 (InitialScale, InitialScale, 1);
				if (scaleEnumeration == null) {
					scaleEnumeration = Pulse (minBlink, durationBlinkDown, durationBlinkUp, Random.Range (10, 100) / 100f);
					StartCoroutine (scaleEnumeration);
				}
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				break;
			case StarStates.SHOW:
				roundStarShow.SetActive (true);
				roundStar.SetActive (false);
				starStar.SetActive (false);
				//star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				roundStarShow.transform.localScale = new Vector3 (InitialScale + ShowStateScaleAddition, InitialScale + ShowStateScaleAddition, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}

				break;
			case StarStates.OVER:
				roundStar.SetActive (true);
				roundStarShow.SetActive (false);
				starStar.SetActive (false);
				//star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				//StartCoroutine (Pulse (InitialScale + OverStateScaleAddition, 0.3f, 0.8f));
				roundStar.transform.localScale = new Vector3 (InitialScale, InitialScale, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}

				overParticule.Emit (1);
				break;
			}
			_state = value;
		}
	}
	IEnumerator Rotate(float aValue){
		while(true)
		{
			Vector3 newRotation = new Vector3 (0,0,aValue);
			starStar.transform.Rotate( newRotation);
			yield return null;
		}
	}

	IEnumerator Pulse(float aValue, float aTime,float bTime,float waitTime)
	{
		while (true) {
			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / waitTime) {
				yield return null;
			}	
			float scale = InitialScale;
			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
				Vector3 newScale = new Vector3 (Mathf.Lerp (scale, aValue, t), Mathf.Lerp (scale, aValue, t), 1);
				roundStar.transform.localScale = newScale;
				yield return null;
			}

			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / bTime) {
				Vector3 newScale = new Vector3 (Mathf.Lerp (aValue, scale, t), Mathf.Lerp (aValue, scale, t), 1);
				roundStar.transform.localScale = newScale;
				yield return null;
			}
		}

	}

	// Use this for initialization
	void Start () {
		InitialScale = roundStar.transform.localScale.x;
		//IdleSprite = star.GetComponent < UnityEngine.UI.Image> ().sprite;
		state = StarStates.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTouchDown() {
		starConstelation.OnStarDown (this);
	}

	public void OnTouchEnter() {
		starConstelation.OnStarEnter (this);
	}


}
