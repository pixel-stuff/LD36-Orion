using UnityEngine;
using System.Collections;

class Line
{
    public ConstelationStar start { set; get; }
    public ConstelationStar end { set; get; }
    public Line(ConstelationStar _start, ConstelationStar _end, float lineWidth)
    {
        start = _start;
        end = _end;
        Vector3 pstart = _start.transform.position;
        Vector3 pend = _end.transform.position;
        Vector3 normal = Vector3.Cross(pstart, pend);
        Vector3 side = Vector3.Cross(normal, pend - pstart);
        side.Normalize();
        Vector3 a = pstart + side * (lineWidth / 2);
        Vector3 b = pstart + side * (lineWidth / -2);
        Vector3 c = pend + side * (lineWidth / 2);
        Vector3 d = pend + side * (lineWidth / -2);
    }
};

[RequireComponent(typeof(Camera))]
public class StarTools : MonoBehaviour {
    
    private static int m_nLinesMax = 20;
    public float m_size = 0.05f;
    public Material mat = null;

    private static ArrayList m_lines = new ArrayList();

    // Use this for initialization
    void Start () {
	}
    
    public static void AddLine(ConstelationStar start, ConstelationStar end)
    {
        m_lines.Add(new Line(start, end, 2.0f));
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

    /*public void Update()
    {

        foreach (Line l in m_lines)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = l.end.transform.position-l.start.transform.position;
        }
    }*/

    void OnPostRender()
    {
        //rest of GL functions
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
        mat.SetPass(0);
        GL.Color(Color.white);
        GL.PushMatrix();
        //GL.LoadIdentity();
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            bool d3d = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
            Matrix4x4 M = transform.localToWorldMatrix;
            Matrix4x4 V = Camera.current.worldToCameraMatrix;
            Matrix4x4 P = Camera.current.projectionMatrix;
            if (d3d)
            {
                // Invert Y for rendering to a render texture
                for (int i = 0; i < 4; i++) { P[1, i] = -P[1, i]; }
                // Scale and bias from OpenGL -> D3D depth range
                for (int i = 0; i < 4; i++) { P[2, i] = P[2, i] * 0.5f + P[3, i] * 0.5f; }
            }
            Matrix4x4 MVP = P * V * M;
            //GL.MultMatrix(Matrix4x4.TRS(Camera.main.transform.position, Camera.main.transform.rotation, Camera.main.transform.localScale));
            GL.LoadProjectionMatrix(MVP);
        }
        //GL.LoadPixelMatrix();
        //GL.MultMatrix(MVP);
        // GL.MultMatrix(GL.GetGPUProjectionMatrix(Camera.current.projectionMatrix, false) );
        //GL.LoadIdentity();
        //GL.MultMatrix(GL.modelview);
        //GL.LoadOrtho();
        GL.Begin(GL.TRIANGLE_STRIP);
        /*GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(Vector3.zero);
        GL.Vertex(Vector3.up);
        GL.Color(Color.green);
        GL.Vertex(Vector3.zero);
        GL.Vertex(Vector3.right);
        GL.Color(Color.blue);
        GL.Vertex(Vector3.zero);
        GL.Vertex(Vector3.forward);
        GL.Color(Color.white);*/
        //Matrix4x4 mat = Matrix4x4();
        foreach (Line l in m_lines)
        {
            GL.Vertex(l.start.transform.position + new Vector3(0.0f, -m_size, 0.0f));
            GL.Vertex(l.start.transform.position + new Vector3(0.0f, m_size, 0.0f));
            GL.Vertex(l.end.transform.position + new Vector3(0.0f, m_size, 0.0f));

            GL.Vertex(l.end.transform.position + new Vector3(0.0f, -m_size, 0.0f));
            GL.Vertex(l.end.transform.position + new Vector3(0.0f, m_size, 0.0f));
            GL.Vertex(l.start.transform.position + new Vector3(0.0f, -m_size, 0.0f));
            /*GL.Vertex(l.start.transform.position);
            GL.Vertex(l.end.transform.position);*/
        }
        GL.End();
        GL.PopMatrix();
    }
}
