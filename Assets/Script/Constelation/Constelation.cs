using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ConstelationType {
	ATTACK,
	DEFENCE,
	HEAL
}

[System.Serializable]
public struct ConstelationNode  {
	public ConstelationStar star;
	public List<ConstelationStar> links;
}

public class Constelation : MonoBehaviour {

	public List<ConstelationNode> constelation;
	// Use this for initialization

	public ConstelationStar startStar= null;

	public UnityEngine.UI.Image ConstelationImage;

	public ConstelationType constelationType = ConstelationType.ATTACK;
	public int constelationStrength = 2;

	public float maxAlpha= 30f;
	public float NBActivateStar = 0;
	public float NBStar = 0;

	public float TimeAtOver = 2f;

	private IEnumerator FadeEnumeration = null;
	void Start () {
		foreach (ConstelationNode c in constelation) {
			NBStar++;
			c.star.starConstelation = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (startStar != null && !Input.GetMouseButton(0) && NBActivateStar != NBStar) {
			destructConstelation ();
		}
		if (startStar != null && Input.GetMouseButton (0)) {
		
			Vector3 mousePosition = Input.mousePosition;
			Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pz.z = 0;
			// DEBUG startStar.transform.position = pz;

		//TODO draw line between mouse and star
		}
	}

	void UpdateActivateStar(){
		if (NBActivateStar == NBStar) {
			ConstelationFinish ();
		}

		float percent = NBActivateStar / NBStar;
		if (FadeEnumeration != null) {
			StopCoroutine (FadeEnumeration);
		}
		FadeEnumeration = FadeTo (percent*maxAlpha, 1.0f);
		
		StartCoroutine(FadeEnumeration);
	}

	IEnumerator FadeTo(float aValue, float aTime)
	{
		float alpha = ConstelationImage.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			ConstelationImage.color = newColor;
			yield return null;
		}
	}

	IEnumerator DestructIn( float aTime)
	{
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			yield return null;
		}
		destructConstelation ();
	}


	void ConstelationFinish() {
		foreach (ConstelationNode c in constelation) {
			c.star.state = StarStates.OVER;
		}
		StartCoroutine(DestructIn( TimeAtOver));
		//TODO set value to player
	}

	void destructConstelation (){
		//TODO destroyConstellation
		foreach (ConstelationNode c in constelation) {
			c.star.state = StarStates.IDLE;
		}
		startStar = null;
		NBActivateStar = 0;
		UpdateActivateStar ();
	}
		
	List<ConstelationStar> GetLinkForStar(ConstelationStar star){
		foreach (ConstelationNode c in constelation) {
			if (c.star == star) {
				return c.links;
			}
		}
		return null;
	}

	public void OnStarDown(ConstelationStar star){
		startStar = star;
		startStar.state = StarStates.ACTIVATE;
		NBActivateStar = 1;
		UpdateActivateStar ();
		// all new star iddle to SHOW
		var newStarLinks = GetLinkForStar (star);
		foreach (ConstelationStar c in newStarLinks) {
			if (c.state == StarStates.IDLE) {
				c.state = StarStates.SHOW;
			}
		}
	}

	public void OnStarEnter(ConstelationStar star){
		
		if(NBActivateStar != NBStar) {
			if (startStar != null && star != startStar) {
				var links = GetLinkForStar (startStar);
				if (links != null && links.Contains (star)) {//if la star est dans les lien
					StarTools.AddLine (startStar, star);
					Debug.Log ("ADDLINE");
			
					// all link Show to Iddle
					foreach (ConstelationStar c in links) {
						if (c.state == StarStates.SHOW) {
							c.state = StarStates.IDLE;
						}
					}
					// all new star iddle to SHOW
					var newStarLinks = GetLinkForStar (star);
					foreach (ConstelationStar c in newStarLinks) {
						if (c.state == StarStates.IDLE) {
							c.state = StarStates.SHOW;
						}
					}
					var oldState = star.state;
					star.state = StarStates.ACTIVATE;
					startStar = star;

					if (oldState != StarStates.ACTIVATE && NBActivateStar < NBStar) {
						NBActivateStar++;
						UpdateActivateStar ();
					}


				} else {
					destructConstelation ();
				}
			}
				
		}
	}
}
