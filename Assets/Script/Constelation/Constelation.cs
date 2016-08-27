using UnityEngine;
using System.Collections;

[System.Serializable]
public class ConstelationNode : System.Object {
	public ConstelationStar star;
	public Link[] links;
}

public class Constelation : MonoBehaviour {

	public ConstelationNode[] constelation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
