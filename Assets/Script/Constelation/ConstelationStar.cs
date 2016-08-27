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

	private IEnumerator rotationEnumeration = null;
	public ParticleSystem overParticule;
	public StarStates state { 
		get{return _state;}
		set{ 
			switch (value) {
			case StarStates.ACTIVATE:
				this.GetComponent < UnityEngine.UI.Image> ().color = Color.red;
				this.transform.localScale = new Vector3 (1, 1, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				rotationEnumeration = Rotate (10.0f);
				StartCoroutine (rotationEnumeration);
				break;
			case StarStates.IDLE:
				this.GetComponent < UnityEngine.UI.Image> ().color = Color.white;
				this.transform.localScale = new Vector3 (1, 1, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				break;
			case StarStates.SHOW:
				this.GetComponent < UnityEngine.UI.Image> ().color = Color.white;
				this.transform.localScale = new Vector3 (1.5f, 1.5f, 1);
				if (rotationEnumeration != null) {
					StopCoroutine (rotationEnumeration);
				}
				break;
			case StarStates.OVER:
				this.GetComponent < UnityEngine.UI.Image> ().color = Color.blue;
				StartCoroutine (Pulse (2f, 0.3f, 0.8f));
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
			this.transform.Rotate( newRotation);
			yield return null;
		}
	}

	IEnumerator Pulse(float aValue, float aTime,float bTime)
	{
		float scale = this.transform.localScale.x;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Vector3 newScale = new Vector3 (Mathf.Lerp (scale, aValue, t),Mathf.Lerp (scale, aValue, t),1);
			this.transform.localScale = newScale;
			yield return null;
		}

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / bTime)
		{
			Vector3 newScale = new Vector3 (Mathf.Lerp (aValue, scale, t),Mathf.Lerp (aValue, scale, t),1);
			this.transform.localScale = newScale;
			yield return null;
		}
	}

	// Use this for initialization
	void Start () {
	
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
