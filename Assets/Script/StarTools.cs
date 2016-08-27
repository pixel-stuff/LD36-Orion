using UnityEngine;
using System.Collections;

public class StarTools : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	
	}

    public static void DrawLine(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(colorStart, colorEnd);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        if(duration>0.0f)
            GameObject.Destroy(myLine, duration);
    }
}
