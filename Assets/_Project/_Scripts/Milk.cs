using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;

    private Rigidbody _rig;
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (( _playerMask & (1 << other.gameObject.layer)) != 0)
        {
            transform.parent = null;
            _rig.useGravity = true;

            Vector3 dirFromPlayer = (transform.position - other.transform.position).normalized;
            _rig.velocity = dirFromPlayer * 10 + Vector3.up * 3;
            _rig.angularVelocity = Vector3.one * 4;
        }
    }
}
