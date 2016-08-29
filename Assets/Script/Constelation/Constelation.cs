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
	public bool IsTuto = false;
	public List<ConstelationNode> constelation;
	// Use this for initialization
	public GameObject LinkPrefab;
	public List<Link> listLinks;

	public ConstelationStar startStar= null;

	public UnityEngine.UI.Image ConstelationImage;

	public ConstelationType constelationType = ConstelationType.ATTACK;
	public int constelationStrength = 2;

	public float maxAlpha= 30f;
	public float NBActivateStar = 0;
	public float NBStar = 0;

	public float TimeAtOver = 2f;

	public AudioClip CastFailSound;
	public List<AudioClip> starsSound;

	private int NBTouchStar;
	private IEnumerator MouseSoundEnumerator;

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
			StarTools.Clear ();
			foreach (Link c in listLinks) {
				c.Destruct ();
			}
			StopMouseSound ();
			PlayCastFail ();
			listLinks.Clear ();

		}
		if (startStar != null && Input.GetMouseButton (0) ) {
			if (NBActivateStar != NBStar) {
				Vector3 mousePosition = Input.mousePosition;
				Vector3 pz = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				pz.z = startStar.transform.position.z;
				StarTools.DrawFromStarToMouse (true, startStar.transform.position, pz);
			} else {
				StarTools.DrawFromStarToMouse (false, Vector3.zero, Vector3.zero);
			}

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
		StarTools.Clear ();
		foreach (Link c in listLinks) {
			c.Destruct ();
		}

		listLinks.Clear ();
	}

	IEnumerator ClearLineIn( float aTime)
	{
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			yield return null;
		}
		foreach (Link c in listLinks) {
			c.Destruct ();
		}
		listLinks.Clear ();
		StarTools.Clear ();
		destructConstelation ();
	}



	void ConstelationFinish() {
		foreach (Link c in listLinks) {
			c.Reussite ();
		}
		foreach (ConstelationNode c in constelation) {
			c.star.state = StarStates.OVER;
		}
		StopMouseSound ();

		StartCoroutine(ClearLineIn( TimeAtOver));
		if (IsTuto) {
		}
		GameObject.FindObjectOfType<Heros>().StartAction(constelationStrength,constelationType);
	}

	void OnDestroy() {
	
		destructConstelation ();
		StarTools.Clear ();
		foreach (Link c in listLinks) {
			c.Destruct ();
		}

		listLinks.Clear ();
	}

	void destructConstelation (){
		
		StarTools.DrawFromStarToMouse (false, Vector3.zero, Vector3.zero);

		foreach (ConstelationNode c in constelation) {
			c.star.state = StarStates.IDLE;
		}
		startStar = null;
		NBActivateStar = 0;
		UpdateActivateStar ();
		StopMouseSound ();
	}
		
	public List<ConstelationStar> GetLinkForStar(ConstelationStar star){
		foreach (ConstelationNode c in constelation) {
			if (c.star == star) {
				return c.links;
			}
		}
		return null;
	}

	public void PlayStarSound(){
		AudioSource audio = this.GetComponents<AudioSource> ()[0];
		audio.PlayOneShot (starsSound [NBTouchStar % starsSound.Count]);
		NBTouchStar++;
		StopMouseSound ();
		StartMouseSound ();
	}

	public void PlayCastFail(){
		AudioSource audio = this.GetComponents<AudioSource> ()[0];
		audio.PlayOneShot (CastFailSound);

	}

	/*
	public void StartMouseSound(){
		AudioSource audio = this.GetComponents<AudioSource> ()[1];// play start sound
		audio.Play ();
		MouseSoundEnumerator = AsyncStartMouseEffect ();
		StartCoroutine (MouseSoundEnumerator);
	}

	private IEnumerator AsyncStartMouseEffect(){
		float aTime = 0.475f;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			yield return null;
		}
		AudioSource audio = this.GetComponents<AudioSource> ()[2];
		if(startStar != null){
			audio.mute = false;
			audio.Play ();
		}
	}*/

	public void StartMouseSound(){
		AudioSource audio = this.GetComponents<AudioSource> ()[1];
		if(startStar != null){
			audio.mute = false;
			audio.Play ();
		}
	}

	public void StopMouseSound(){
		AudioSource audio = this.GetComponents<AudioSource> ()[1];
		audio.mute = true;
		audio.Stop ();
	//	StopCoroutine (MouseSoundEnumerator);
	}
		

	public void OnStarDown(ConstelationStar star){
		
		startStar = star;
		startStar.state = StarStates.ACTIVATE;
		NBActivateStar = 1;
		NBTouchStar = 0;
		PlayStarSound ();
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
					PlayStarSound ();
					StarTools.AddLine (startStar, star);
					var link = Instantiate (LinkPrefab);
					var linkScript = link.GetComponent<Link> ();
					linkScript.sourceStar = startStar;
					linkScript.targetStar = star;
					linkScript.Init ();
					listLinks.Add (linkScript);
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
					var link = Instantiate (LinkPrefab);
					var linkScript = link.GetComponent<Link> ();
					linkScript.sourceStar = startStar;
					linkScript.targetStar = star;
					linkScript.Init ();
					linkScript.Destruct ();
					destructConstelation ();
					PlayCastFail ();
					StarTools.Clear ();
					foreach (Link c in listLinks) {
						c.Destruct ();
					}

					listLinks.Clear ();
				}
			}
				
		}
	}
}
