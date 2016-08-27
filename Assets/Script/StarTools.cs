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

[RequireComponent(typeof(Camera))]
public class StarTools : MonoBehaviour {
    
    private static int m_nLinesMax = 20;
    public Material mat = null;

    private static ArrayList m_lines = new ArrayList();

    // Use this for initialization
    void Start () {
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
        m_lines.Clear();
    }

    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        if(m_lines==null)
        {
            Debug.LogError("Lines is null");
            return;
        }
        //GL.PushMatrix();
        mat.SetPass(0);
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
