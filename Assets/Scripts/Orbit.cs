using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Shield shieldScript;

    public Transform center;
    public float radius = 1.5f;
    public float orbitSpeed = 180f;
    public float startAngle = 0f;
    public bool clockwise = false;
    public float selfSpin = 0f;

    public Vector2 position;

    private float angle;

    private void Start()
    {
        angle = startAngle;
    }

    private void LateUpdate()
    {
        if (center == null)
        {
            return;
        }

        float direction = 1f;

        if (clockwise)
        {
            direction = -1f;
        }

        angle += orbitSpeed * direction * Time.deltaTime;

        float radians = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
        position = center.position + offset;

        if (shieldScript.State == ThrowState.Held)
        {
            transform.position = center.position + offset;
            transform.Rotate(0f, 0f, selfSpin * Time.deltaTime);
        }
    }
}