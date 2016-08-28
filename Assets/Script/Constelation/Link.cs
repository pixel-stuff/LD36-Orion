using UnityEngine;
using System.Collections;

public class Link : MonoBehaviour {
	public ConstelationStar targetStar;
	public ConstelationStar sourceStar;

	public ParticleSystem destruct;
	public ParticleSystem reussite;
	public ParticleSystem activate;

	// Use this for initialization
	public void Init () {
		this.transform.position = new Vector3((targetStar.transform.position.x + sourceStar.transform.position.x)/2f,
			(targetStar.transform.position.y + sourceStar.transform.position.y)/2f,
		(targetStar.transform.position.z + sourceStar.transform.position.z)/2f);

		Vector3 vector = targetStar.transform.position - sourceStar.transform.position;
		float size = vector.magnitude;

		UnityEditor.SerializedObject so = new UnityEditor.SerializedObject(activate);

		so.FindProperty("ShapeModule.boxX").floatValue = size;
		so.ApplyModifiedProperties ();
		so = new UnityEditor.SerializedObject(reussite);

		so.FindProperty("ShapeModule.boxX").floatValue = size;
		so.ApplyModifiedProperties ();
		so = new UnityEditor.SerializedObject(destruct);

		so.FindProperty("ShapeModule.boxX").floatValue = size;
		so.ApplyModifiedProperties ();

		float alpha = Vector3.Angle (vector, new Vector3 (1, 0, 0));
		alpha = (vector.y < 0) ? -alpha : alpha;
		this.transform.Rotate (new Vector3(0,0,alpha));
	}

	public void Destruct() {
		Debug.Log ("DESTRUCT");
		destruct.Play ();
		activate.Stop ();
		StartCoroutine(DestructIn (4f));
	}

	public void Reussite() {
		Debug.Log ("Reussite");
		reussite.Play ();
		activate.Stop ();
		StartCoroutine(DestructIn (4f));
	}

	IEnumerator DestructIn( float aTime)
	{
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			yield return null;
		}
		Debug.Log ("DESTROY");
		Destroy (this.gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
