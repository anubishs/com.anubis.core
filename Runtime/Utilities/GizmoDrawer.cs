using UnityEngine;

[ExecuteAlways]
public class GizmoDrawer : MonoBehaviour
{
    public enum GizmoShape
    {
        Sphere,
        Cube,
        WireSphere,
        WireCube
    }

    public enum SizeSource
    {
        Manual,
        TransformScale,
        Collider,
        Renderer
    }

    [Header("Shape")]
    public GizmoShape shape = GizmoShape.Sphere;

    [Header("Size")]
    public SizeSource sizeSource = SizeSource.Manual;
    public float manualSize = 0.25f;

    [Header("Appearance")]
    public Color color = Color.yellow;

    [Header("Offset")]
    public Vector3 offset = Vector3.zero;

    [Header("Extras")]
    public bool drawLineUp = false;
    public float lineHeight = 1f;

    public bool drawForward = false;
    public float forwardLength = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Vector3 pos = transform.position + offset;
        Vector3 size = GetSize();

        DrawShape(pos, size);

        if (drawLineUp)
        {
            Gizmos.DrawLine(pos, pos + Vector3.up * lineHeight);
        }

        if (drawForward)
        {
            Gizmos.DrawLine(pos, pos + transform.forward * forwardLength);
        }
    }

    Vector3 GetSize()
    {
        switch (sizeSource)
        {
            case SizeSource.TransformScale:
                return transform.lossyScale;

            case SizeSource.Collider:
                Collider col = GetComponent<Collider>();
                if (col != null)
                    return col.bounds.size;
                break;

            case SizeSource.Renderer:
                Renderer rend = GetComponent<Renderer>();
                if (rend != null)
                    return rend.bounds.size;
                break;
        }

        // Default fallback (Manual)
        return Vector3.one * manualSize;
    }

    void DrawShape(Vector3 pos, Vector3 size)
    {
        switch (shape)
        {
            case GizmoShape.Sphere:
                Gizmos.DrawSphere(pos, size.x * 0.5f);
                break;

            case GizmoShape.Cube:
                Gizmos.DrawCube(pos, size);
                break;

            case GizmoShape.WireSphere:
                Gizmos.DrawWireSphere(pos, size.x * 0.5f);
                break;

            case GizmoShape.WireCube:
                Gizmos.DrawWireCube(pos, size);
                break;
        }
    }
}