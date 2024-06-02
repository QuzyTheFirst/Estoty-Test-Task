using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceUserVisualizer : MonoBehaviour
{
    [Header("Visuals")] 
    [SerializeField] private GameObject _visuals;

    [SerializeField] private GameObject _openingContent;
    
    [Header("Drop Animation")]
    [SerializeField] private float _dropUpPower = 6;
    [SerializeField] private float _dropSidePower = 6;
    [SerializeField] private float _dropTowardPower = 6;
    [SerializeField] private float _dropMinRotationPower = 4;
    [SerializeField] private float _dropMaxRotationPower = 7;

    [Header("Going To Target Animation")]
    [SerializeField] private float _timeBeforeGoingToPlayer = .5f;
    [SerializeField] private float _timeTravelingToPlayer = .2f;
    [SerializeField] private float _endingPositionModifier = .4f;

    private ResourceUser _resourceUser;

    private GameObject _lastUsedResource;

    private void Awake()
    {
        _resourceUser = GetComponent<ResourceUser>();
    }

    private void UseResource(ResourceSO resourceSo, Transform player, Vector3 targetPos)
    {
        _lastUsedResource = ObjectPooler.Instance.SpawnFromPool(resourceSo, player.position, Quaternion.identity);
        Rigidbody rig = _lastUsedResource.GetComponent<Rigidbody>();
        Vector3 forward = (targetPos - player.position).normalized;
        Vector3 right = (Vector3.Cross(forward, Vector3.up)).normalized;
        rig.velocity = Vector3.up * _dropUpPower + right * (Random.Range(-1f, 1f) * _dropSidePower) + forward * (Random.Range(0.5f, 1f) * _dropTowardPower);
        rig.angularVelocity = Vector3.one * Random.Range(_dropMinRotationPower, _dropMaxRotationPower);
        StartCoroutine(GoToTarget(rig, targetPos));
    }
    
    private IEnumerator GoToTarget(Rigidbody rig, Vector3 targetPos)
    {
        yield return new WaitForSeconds(_timeBeforeGoingToPlayer);
        rig.velocity *= 0.5f;
        rig.angularVelocity *= 0.5f;
        yield return new WaitForSeconds(.2f);
        float timer = 0;
        Vector3 startPosition = rig.transform.position;
        targetPos += new Vector3(
            Random.Range(-_endingPositionModifier, _endingPositionModifier),
            Random.Range(-_endingPositionModifier, _endingPositionModifier), 
            Random.Range(-_endingPositionModifier, _endingPositionModifier)
            );
        while (timer < _timeTravelingToPlayer)
        {
            timer += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPos, timer / _timeTravelingToPlayer);
            rig.transform.position = currentPosition;
            yield return null;
        }
        rig.gameObject.SetActive(false);
    }

    private void BuiltIsDone()
    {
        if (_lastUsedResource == null || !_lastUsedResource.activeSelf)
        {
            _visuals.SetActive(false);
            _openingContent.SetActive(true);
            enabled = false;
        }
        else
        {
            _visuals.SetActive(false);
            StartCoroutine(WaitForLastUsedResourceToDisappear());
        }
    }

    private IEnumerator WaitForLastUsedResourceToDisappear()
    {
        while (true)
        {
            yield return null;
            if (!_lastUsedResource.activeSelf)
            {
                _openingContent.SetActive(true);
                enabled = false;
                yield break;
            }
        }
    }
    
    private void OnEnable()
    {
        _resourceUser.ResourceUsed += ResourceUserOnResourceUsed;
        _resourceUser.BuildCompleted += ResourceUserOnBuildCompleted;
    }

    private void ResourceUserOnResourceUsed(object sender, ResourceUser.ResourceUsedEventData resourceUsedEventData)
    {
        UseResource(resourceUsedEventData.ResourceSO, resourceUsedEventData.PlayerTf, _openingContent.transform.position);
    }
    
    private void ResourceUserOnBuildCompleted(object sender, EventArgs e)
    {
        BuiltIsDone();
    }

    private void OnDisable()
    {
        _resourceUser.ResourceUsed -= ResourceUserOnResourceUsed;
        _resourceUser.BuildCompleted -= ResourceUserOnBuildCompleted;
    }
}
