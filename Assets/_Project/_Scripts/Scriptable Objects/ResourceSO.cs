using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "ScriptableObjects/ResourceSO")]
public class ResourceSO : ScriptableObject
{
    [Header("Game")]
    public GameObject Prefab;

    [Header("UI")]
    public Color UIColor;
}
