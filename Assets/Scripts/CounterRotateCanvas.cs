using UnityEngine;

public class CounterRotateCanvas : MonoBehaviour
{
    private Quaternion localRotation;

    void Awake()
    {
        localRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity * localRotation;
    }
}
