using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Rigidbody _rig;

    public Rigidbody Rig => _rig;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }
}
