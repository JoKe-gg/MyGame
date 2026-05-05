using System;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] private float length;
    [SerializeField, Min(3)] int amountOfRays;
    [SerializeField] private bool DrawGizmos;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] int maxRays = 20;

    public event Action<Vector2> OnCoughtPlayer;

    void OnValidate()
    {
        amountOfRays = Mathf.Clamp(amountOfRays, 3, maxRays);
    }
    public void VisionCast()
    {
        float step = 360 / (float)amountOfRays;
        Debug.Log($"step = {step}");
        for (int i = 0; i < amountOfRays; i++)
        {
            float angleRad = i * step * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, length, targetLayer);
            if (hit.collider != null)
            {
                OnCoughtPlayer?.Invoke(hit.point);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!DrawGizmos) { return; }
        Gizmos.color = Color.red;
        float step = 360 / (float)amountOfRays;
        Debug.Log($"step = {step}");
        for (int i = 0; i < amountOfRays; i++)
        {
            float angleRad = i * step * Mathf.Deg2Rad;
            Vector2 dir = new Vector2 (Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            Gizmos.DrawRay(transform.position, dir*length);
            Debug.Log($"Target vector2 = {dir.x} , {dir.y}");
        }
    }
}
