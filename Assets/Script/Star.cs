using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StarTools.DrawLine(Vector3.zero, new Vector3(10.0f, 10.0f, 10.0f), Color.black, 0.0f);
    }
}
