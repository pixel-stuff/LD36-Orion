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
public class StarTools : MonoBehaviour
{

    private static int m_nLinesMax = 20;
    private static bool _drawIt = false;
    public float m_size = 0.05f;
    public Material mat = null;

    // star to mouse
    static Vector3 _Start = Vector3.zero;
    static Vector3 _End = Vector3.zero;

    private static ArrayList m_lines = new ArrayList();

    public Color _MainColor = Color.white;
    public Color _SecondaryColor = Color.white;

    // Use this for initialization
    void Start()
    {
    }

    public static void AddLine(ConstelationStar start, ConstelationStar end)
    {
        m_lines.Add(new Line(start, end, 2.0f));
    }

    public static void ClearLinksRelatedTo(ConstelationStar start)
    {
        foreach (Line l in m_lines)
        {
            if (l.start == start || l.end == start)
            {
                m_lines.Remove(l);
            }
        }
    }

    public static void Clear()
    {
        m_lines.Clear();
    }

    public static void DrawFromStarToMouse(bool drawIt, Vector3 start, Vector3 end)
    {
        _drawIt = drawIt;
        _Start = start;
        _End = end;
    }

    /*public void Update()
    {

        foreach (Line l in m_lines)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = l.end.transform.position-l.start.transform.position;
        }
    }*/

    static Quaternion QuatFromV3xV3(Vector3 v0, Vector3 v1, Vector3 fallbackAxis)
    {
        Quaternion q = Quaternion.identity;
        v0.Normalize();
        v1.Normalize();

        float d = Vector3.Dot(v0, v1);
        // If dot == 1, vectors are the same
        if (d >= 1.0f)
        {
            return Quaternion.identity;
        }
        if (d < (1e-6f - 1.0f))
        {
            if (fallbackAxis != Vector3.zero)
            {
                // rotate 180 degrees about the fallback axis
                q = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.PI, fallbackAxis);
            }
            else
            {
                // Generate an axis
                Vector3 axis = Vector3.Cross(Vector3.right, v0);
                if (axis.magnitude < 0.0001f) // pick another if colinear
                    axis = Vector3.Cross(Vector3.up, v0);
                axis.Normalize();
                q = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.PI, axis);
            }
        }
        else
        {
            float s = Mathf.Sqrt((1 + d) * 2);
            float invs = 1.0f / s;

            Vector3 c = Vector3.Cross(v0, v1);

            q.x = c.x * invs;
            q.y = c.y * invs;
            q.z = c.z * invs;
            q.w = s * 0.5f;
        }
        return q;
    }


    private void GLine(Vector3 start, Vector3 end)
    {
        float length = (end - start).magnitude;
        /*Quaternion q = Quaternion.identity;
        Vector3 a = Vector3.Cross(start, end);
        q = QuatFromV3xV3(start, end, Vector3.right);
        GL.modelview.SetTRS(Vector3.zero, q, Vector3.one); // Change Translation, but leave the other parts of the matrix alone.*/
        Vector2 a = new Vector2(start.x, start.y);
        a.Normalize();
        Vector2 b = new Vector2(end.x, end.y);
        b.Normalize();
        //float angle = Mathf.Acos(Vector2.Dot(a, b));
        float angle = Mathf.Acos(Vector3.Dot(a.normalized, b.normalized));
        float sign = a.x * b.y - a.y * b.x;
        float sinA = Mathf.Asin(Vector3.Cross(a.normalized, b.normalized).magnitude);
        //Matrix4x4 rot = Matrix4x4.TRS(new Vector3((start.y - end.y) / 2.0f, (start.y-end.y)/2.0f, 0.0f), Quaternion.AngleAxis(Mathf.Rad2Deg*angle, Vector3.up), Vector3.one);
        Vector3 dir = (end - start).normalized;
        angle = Vector3.Angle(Vector3.right, dir);
        //GL.LoadIdentity();
        Quaternion q = Quaternion.Euler(0, 0, angle * Mathf.Sign(dir.y));
        Matrix4x4 trs = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
        //GL.MultMatrix(trs);
        //q = Quaternion.identity;=
        GL.Vertex((start + q * (new Vector3(0.0f, -m_size, 0.0f))));
        GL.Vertex((start + q * (new Vector3(length, -m_size, 0.0f))));
        GL.Vertex((start + q * (new Vector3(length, m_size, 0.0f))));
        GL.Vertex((start + q * (new Vector3(0.0f, m_size, 0.0f))));
        /*GL.Vertex((start + q*(new Vector3(length, m_size, 0.0f))));
        GL.Vertex((start + q * (new Vector3(length, -m_size, 0.0f))));*/

        /*GL.Vertex((start + (new Vector3(0.0f, -m_size, 0.0f))));
        GL.Vertex((start + (new Vector3(0.0f, m_size, 0.0f))));
        GL.Vertex((end + (new Vector3(0.0f, m_size, 0.0f))));

        GL.Vertex((end + (new Vector3(0.0f, -m_size, 0.0f))));
        GL.Vertex((end + (new Vector3(0.0f, m_size, 0.0f))));
        GL.Vertex((start + (new Vector3(0.0f, -m_size, 0.0f))));*/
        /*GL.Vertex(l.start.transform.position);
        GL.Vertex(l.end.transform.position);*/
    }

    void OnPostRender()
    {
        //rest of GL functions
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        if (m_lines == null)
        {
            Debug.LogError("Lines is null");
            return;
        }
        mat.SetPass(0);
        GL.PushMatrix();
        GL.Color(Color.white);
        //GL.LoadIdentity();
        if (Application.platform == RuntimePlatform.WindowsEditor)
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
        else
        {
            //GL.MultMatrix(Matrix4x4.TRS(Camera.main.transform.position, Camera.main.transform.rotation, Camera.main.transform.localScale));
        }
        //GL.LoadPixelMatrix();
        //GL.MultMatrix(MVP);
        // GL.MultMatrix(GL.GetGPUProjectionMatrix(Camera.current.projectionMatrix, false) );
        //GL.LoadIdentity();
        //GL.MultMatrix(GL.modelview);
        //GL.LoadOrtho();
        GL.Begin(GL.QUADS);
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
        //GL.Color(_MainColor);
        foreach (Line l in m_lines)
        {
            GLine(l.start.transform.position, l.end.transform.position);
        }
        if (_drawIt)
        {
            //GL.Color(_SecondaryColor);
            GLine(_Start, _End);
        }
        GL.PopMatrix();
        GL.End();
    }
}