using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGiverVisualizer : MonoBehaviour
{
    [Header("Drop Animation")]
    [SerializeField] private float _dropUpPower = 6;
    [SerializeField] private float _dropSidePower = 3;
    [SerializeField] private float _dropMinRotationPower = 4;
    [SerializeField] private float _dropMaxRotationPower = 7;

    [Header("Going To Player Animation")]
    [SerializeField] private float _timeBeforeGoingToPlayer = .5f;
    [SerializeField] private float _timeTravelingToPlayer = .2f;

    public void DropResource(ResourceSO resourceSo, Transform target)
    {
        GameObject resource = ObjectPooler.Instance.SpawnFromPool(resourceSo, transform.position, Quaternion.identity);
        Rigidbody rig = resource.GetComponent<Rigidbody>();
        rig.velocity = Vector3.up * _dropUpPower + (Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f)) * _dropSidePower;
        rig.angularVelocity = Vector3.one * Random.Range(_dropMinRotationPower, _dropMaxRotationPower);
        StartCoroutine(GoToTarget(rig, target));
    }
    
    IEnumerator GoToTarget(Rigidbody rig, Transform target)
    {
        yield return new WaitForSeconds(_timeBeforeGoingToPlayer);
        rig.velocity *= 0.5f;
        rig.angularVelocity *= 0.5f;
        yield return new WaitForSeconds(.2f);
        float timer = 0;
        Vector3 startPosition = rig.transform.position;
        while (timer < _timeTravelingToPlayer)
        {
            timer += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(startPosition, target.position, timer / _timeTravelingToPlayer);
            rig.transform.position = currentPosition;
            yield return null;
        }
        rig.gameObject.SetActive(false);
    }
    
}
