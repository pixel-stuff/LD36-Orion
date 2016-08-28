using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public ConstelationStar starOne;
    public ConstelationStar starTwo;
    public ConstelationStar star3;
    public ConstelationStar star4;

    public Vector3 mouse = Vector3.zero;

    // Use this for initialization
    void Start () {
        StarTools.AddLine(starOne, starTwo);
        StarTools.AddLine(starOne, star3);
        StarTools.AddLine(starOne, star4);
    }

    void Update()
    {
        StarTools.DrawFromStarToMouse(true, starOne, mouse);
    }
}
