using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // reference to linerenderer
    public LineRenderer lineRenderer;

    // store points in line 
    List<Vector2> points;

    // either start new line or continue drawing line
    public void UpdateLine (Vector2 mousePos) {
        if (points == null){
            points = new List<Vector2>();
            SetPoint(mousePos);
            return;
        }

        // Check if mouse has moved enough for us to insert new point
        // if it has, insert point at mouse/touch position?
        // Update: .1f -> .001f to make lines smoother
        if (Vector2.Distance(points.Last(), mousePos) > .001f){
            SetPoint(mousePos);
        }
    }

    void SetPoint(Vector2 point){
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);

    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        // Debug.Log("activeLine's color has been set to" + color);
    }

    public void SetWidth(float width) {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public void SetTexture(string texture) {
        string url = "Materials/" + texture;
        var mat = Resources.Load<Material>(url);
        lineRenderer.material =  mat;
    }
}
