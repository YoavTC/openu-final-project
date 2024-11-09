using UnityEngine;

public class DebugPainter : MonoBehaviour
{
    private static Color defaultRectColour;
    private static Color defaultLineColour;
    private static Color defaultCircleColour;
    
    private static float defaultCircleRadius = 3f;
    private static int defaultCircleSegments = 16;
    
    private void Awake()
    {
        defaultRectColour = Color.green;
        defaultLineColour = Color.red;
        defaultCircleColour = Color.cyan;
    }

    //Draw Rect
    public static void DrawRect(RectTransform rectTransform, Color colour) => DrawRect(rectTransform.rect, colour);
    public static void DrawRect(RectTransform rectTransform) => DrawRect(rectTransform.rect, defaultRectColour);
    public static void DrawRect(Rect rect) => DrawRect(rect, defaultRectColour);
    
    public static void DrawRect(Rect rect, Color colour)
    {
        Vector3 bottomLeft = new Vector3(rect.xMin, rect.yMin, 0);
        Vector3 topLeft = new Vector3(rect.xMin, rect.yMax, 0);
        Vector3 topRight = new Vector3(rect.xMax, rect.yMax, 0);
        Vector3 bottomRight = new Vector3(rect.xMax, rect.yMin, 0);
        
        Debug.DrawLine(bottomLeft, topLeft, colour);
        Debug.DrawLine(topLeft, topRight, colour);
        Debug.DrawLine(topRight, bottomRight, colour);
        Debug.DrawLine(bottomRight, bottomLeft, colour);
    }
    
    //Draw Line
    public static void DrawLineBetween(Transform A, Transform B, Color colour) => DrawLineBetween(A.position, B.position, colour);
    public static void DrawLineBetween(Transform A, Transform B) => DrawLineBetween(A.position, B.position, defaultLineColour);
    public static void DrawLineBetween(Vector3 A, Vector3 B) => DrawLineBetween(A, B, defaultLineColour);
    
    public static void DrawLineBetween(Vector3 A, Vector3 B, Color colour)
    {
        Debug.DrawLine(A, B, colour);
    }
    
    
    //Draw Circle
    public static void DrawCircle(Vector3 center) => DrawCircle(center, defaultCircleRadius, defaultCircleColour, defaultCircleSegments);
    public static void DrawCircle(Vector3 center, float radius) => DrawCircle(center, radius, defaultCircleColour, defaultCircleSegments);
    public static void DrawCircle(Vector3 center, float radius, Color colour) => DrawCircle(center, radius, colour, defaultCircleSegments);
    
    public static void DrawCircle(Vector3 center, float radius, Color colour, int segments)
    {
        float angleStep = 360f / segments;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.forward);

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 localPoint1 = new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * radius;
            Vector3 localPoint2 = new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0) * radius;

            Vector3 worldPoint1 = center + rotation * localPoint1;
            Vector3 worldPoint2 = center + rotation * localPoint2;

            Debug.DrawLine(worldPoint1, worldPoint2, colour);
        }
    }
    
    //Draw Arrow
    public static void DrawArrow(Transform A, Transform B, Color colour) => DrawArrow(A.position, B.position, colour);
    public static void DrawArrow(Transform A, Transform B) => DrawArrow(A.position, B.position, defaultLineColour);
    public static void DrawArrow(Vector3 A, Vector3 B) => DrawArrow(A, B, defaultLineColour);
    
    public static void DrawArrow(Vector3 A, Vector3 B, Color colour)
    {
        DrawLineBetween(A, B, colour);
        DrawCircle(B, defaultCircleRadius ,colour);
    }
}