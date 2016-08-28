using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public ConstelationStar starOne;
    public ConstelationStar starTwo;
    public ConstelationStar star3;
    public ConstelationStar star4;

    // Use this for initialization
    void Start () {
        StarTools.AddLine(starOne, starTwo);
        StarTools.AddLine(starOne, star3);
        StarTools.AddLine(starOne, star4);
    }
}
