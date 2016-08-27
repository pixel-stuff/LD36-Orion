using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public ConstelationStar starOne;
    public ConstelationStar starTwo;

	// Use this for initialization
	void Start () {
        StarTools.AddLine(starOne, starTwo);
    }
}
