using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed, float mass)
    {
        _line.enabled = true;
        Vector3[] points = new Vector3[1000];
        _line.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * .01f;
            points[i] = origin + speed * time + Physics.gravity * Mathf.Pow(mass, 2) * Mathf.Pow(time, 2) / 2f;
            if (points[i].y <= 0)
            {
                _line.positionCount = i;
                break;
            }
        }

        _line.SetPositions(points);
    }

    public void HideTrajectory()
    {
        _line.enabled = false;
    }
}
