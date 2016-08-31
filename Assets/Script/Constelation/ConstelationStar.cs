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
	public GameObject star;
	private IEnumerator rotationEnumeration = null;
	private IEnumerator scaleEnumeration = null;
	public ParticleSystem overParticule;
	private float InitialScale = 0;
	public float starScale = 1;
	public float ShowStateScaleAddition = 0.2f;
	public float OverStateScaleAddition = 1f;
	public Sprite ActivateSprite;
	private Sprite IdleSprite;
	public StarStates state { 
		get{return _state;}
		set{ 
			switch (value) {
			case StarStates.ACTIVATE:
				star.GetComponent < UnityEngine.UI.Image> ().sprite = ActivateSprite;
				star.transform.localScale = new Vector3 (starScale, starScale, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				if (scaleEnumeration != null) {
					StopCoroutine (scaleEnumeration);
				}
				rotationEnumeration = Rotate (3.0f);
				StartCoroutine (rotationEnumeration);
				break;
			case StarStates.IDLE:
				star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				star.transform.localScale = new Vector3 (InitialScale, InitialScale, 1);
				scaleEnumeration = Pulse (0.01f, 1.2f, 0.5f,Random.Range(10,100)/100f);
				StartCoroutine (scaleEnumeration);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				break;
			case StarStates.SHOW:
				star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				star.transform.localScale = new Vector3 (InitialScale + ShowStateScaleAddition, InitialScale + ShowStateScaleAddition, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				if (scaleEnumeration != null) {
					StopCoroutine (scaleEnumeration);
				}
				break;
			case StarStates.OVER:
				star.GetComponent < UnityEngine.UI.Image> ().sprite = IdleSprite;
				//StartCoroutine (Pulse (InitialScale + OverStateScaleAddition, 0.3f, 0.8f));
				star.transform.localScale = new Vector3 (InitialScale, InitialScale, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				if (scaleEnumeration != null) {
					StopCoroutine (scaleEnumeration);
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
			star.transform.Rotate( newRotation);
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
				star.transform.localScale = newScale;
				yield return null;
			}

			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / bTime) {
				Vector3 newScale = new Vector3 (Mathf.Lerp (aValue, scale, t), Mathf.Lerp (aValue, scale, t), 1);
				star.transform.localScale = newScale;
				yield return null;
			}
		}

	}

	// Use this for initialization
	void Start () {
		InitialScale = star.transform.localScale.x;
		IdleSprite = star.GetComponent < UnityEngine.UI.Image> ().sprite;
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
