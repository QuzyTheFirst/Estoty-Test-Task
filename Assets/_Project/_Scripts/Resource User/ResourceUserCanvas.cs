using UnityEngine;

public class ResourceUserCanvas : MonoBehaviour
{
    private void Awake()
    {
        transform.rotation = Quaternion.LookRotation(-(Camera.main.transform.position - transform.position).normalized, Vector3.up);
    }
}
