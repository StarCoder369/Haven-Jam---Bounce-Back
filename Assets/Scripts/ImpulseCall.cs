using Unity.Cinemachine;
using UnityEngine;

public class ImpulseCall : MonoBehaviour
{
    public CinemachineImpulseSource source;

    void Start()
    {
        source = GetComponent<CinemachineImpulseSource>();
        source.GenerateImpulse();
    }
}
