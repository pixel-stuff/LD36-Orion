using UnityEngine;
using System.Collections;

public class TutoConstelationStar : MonoBehaviour {

	public GameObject target;
	Vector3 direction= Vector3.zero;
	Vector3 startPoint= Vector3.zero;
	IEnumerator drawTutoEnumerator;
	// Use this for initialization

	IEnumerator drawTuto( float aTime)
	{
		float t;
		while (true) {
			for (t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
				StarTools.DrawFromStarToMouse (true, startPoint, startPoint + (direction * t));
				yield return null;
			}
			StarTools.DrawFromStarToMouse (false, Vector3.zero, Vector3.zero);
			t = 0;
			yield return null;
		}
	}

	// Update is called once per frame
	void Update () {
		if(direction == Vector3.zero){
			ConstelationStar thisStar = this.GetComponent<ConstelationStar> ();
			ConstelationStar targetStar = thisStar.starConstelation.GetLinkForStar (thisStar) [0];
			startPoint = thisStar.transform.position;
			direction = targetStar.transform.position - startPoint;
			drawTutoEnumerator = drawTuto (2);
			StartCoroutine (drawTutoEnumerator);

		}
		ConstelationStar thisStar2 = this.GetComponent<ConstelationStar> ();
		if (thisStar2 != null) {
			if(thisStar2.state != StarStates.IDLE){
				StopCoroutine (drawTutoEnumerator);
				StarTools.DrawFromStarToMouse (false, Vector3.zero, Vector3.zero);
				target.gameObject.SetActive (false);
			}

		}
	}
}
