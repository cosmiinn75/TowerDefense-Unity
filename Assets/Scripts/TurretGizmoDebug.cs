using UnityEngine;

public class TurretGizmoDebug: MonoBehaviour
{
    public float range = 15f; // Raza ta de acțiune
    private LineRenderer lineRenderer;

    void Start()
    {
        // Configurăm un LineRenderer simplu
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true;

        // Setăm culoarea cercului (de exemplu, un roșu semi-transparent)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);

        DrawCircle();
    }

    void DrawCircle()
    {
        int segments = 50;
        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (i / (float)segments) * 2 * Mathf.PI;
            float x = Mathf.Cos(angle) * range;
            float y = Mathf.Sin(angle) * range;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}