using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string Name;
    
    [Header("Game")]
    public GameObject Prefab;

    [Header("UI")] 
    public Color UIColor;
}
