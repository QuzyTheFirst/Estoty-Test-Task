using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceGiver : MonoBehaviour
{
    [SerializeField] private ObjectPooler.ResourceType _resourceType;
    [SerializeField] private LayerMask _playerMask;
    
    [Header("Drop")]
    [SerializeField] private float _intervalBetweenDrop = .2f;
    [SerializeField] private float _dropUpPower = 3;
    [SerializeField] private float _dropSidePower = 3;
    [SerializeField] private float _dropMinRotationPower = 2;
    [SerializeField] private float _dropMaxRotationPower = 5;
    [SerializeField] private float _sinCosCurveMultiplier = 8;

    [Header("Going To Player")]
    [SerializeField] private float _timeBeforeGoingToPlayer = .5f;
    [SerializeField] private float _timeTravelingToPlayer = .2f;

    private Coroutine _playerInTriggerCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (( _playerMask & (1 << other.gameObject.layer)) != 0)
        {
            _playerInTriggerCoroutine = StartCoroutine(DropResources(other.transform));
        }
    }

    IEnumerator DropResources(Transform player)
    {
        while (true)
        {
            GameObject resource = ObjectPooler.Instance.SpawnFromPool(_resourceType, transform.position, Quaternion.identity);
            Rigidbody rig = resource.GetComponent<Rigidbody>();
            rig.velocity = Vector3.up * _dropUpPower + (Vector3.right * Mathf.Cos(Time.time * _sinCosCurveMultiplier) + Vector3.forward * (float)Math.Sin(Time.time * _sinCosCurveMultiplier)) * _dropSidePower;
            rig.angularVelocity = Vector3.one * Random.Range(_dropMinRotationPower, _dropMaxRotationPower);
            StartCoroutine(GoToPlayer(rig, player));
            yield return new WaitForSeconds(_intervalBetweenDrop);
        }
    }

    IEnumerator GoToPlayer(Rigidbody rig, Transform player)
    {
        yield return new WaitForSeconds(_timeBeforeGoingToPlayer);
        rig.velocity = rig.velocity * 0.5f;

        float timer = 0;
        Vector3 startPosition = rig.transform.position;
        while (timer < _timeTravelingToPlayer)
        {
            timer += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(startPosition, player.position, timer / _timeTravelingToPlayer);
            rig.transform.position = currentPosition;
            yield return null;
        }
        rig.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (( _playerMask & (1 << other.gameObject.layer)) != 0)
        {
            StopCoroutine(_playerInTriggerCoroutine);
        }
    }
}