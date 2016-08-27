using UnityEngine;
using System.Collections;

class Line
{
    public ConstelationStar start { set; get; }
    public ConstelationStar end { set; get; }
    public Line(ConstelationStar _start, ConstelationStar _end)
    {
        start = _start;
        end = _end;
    }
};

public class StarTools : MonoBehaviour {
    
    private static int m_nLinesMax = 20;
    public Material mat;

    private static ArrayList m_lines;

    // Use this for initialization
    void Start () {
        m_lines = new ArrayList();
	}

    public static void DrawLine(ConstelationStar start, ConstelationStar end, Color colorStart, Color colorEnd, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(colorStart, colorEnd);
        lr.SetVertexCount(m_nLinesMax);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start.transform.position);
        lr.SetPosition(1, end.transform.position);
        lr.SetPosition(2, Vector3.Cross(start.transform.position, end.transform.position));
        lr.SetPosition(3, end.transform.position);
        if (duration>0.0f)
            GameObject.Destroy(myLine, duration);
    }

    public static void AddLine(ConstelationStar start, ConstelationStar end)
    {
        m_lines.Add(new Line(start, end));
    }

    public static void ClearLinksRelatedTo(ConstelationStar start)
    {
        foreach(Line l in m_lines)
        {
            if(l.start==start ||l.end==start)
            {
                m_lines.Remove(l);
            }
        }
    }

    public static void Clear()
    {
        m_lines.Clear());
    }

    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        //GL.PushMatrix();
        //mat.SetPass(0);
        //GL.LoadIdentity();
        //GL.MultMatrix(GL.modelview);
        //GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.white);
        foreach(Line l in m_lines)
        {
            GL.Vertex(l.start.transform.position);
            GL.Vertex(l.end.transform.position);
        }
        GL.End();
        //GL.PopMatrix();
    }

    public static void draw()
    {

    }
}
