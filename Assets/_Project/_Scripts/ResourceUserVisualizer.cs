using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUserVisualizer : MonoBehaviour
{
    [Header("Visuals")] 
    [SerializeField] private GameObject _visuals;
    
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
    
    public void UseResource(ResourceSO resourceSo, Transform player, Vector3 targetPos)
    {
        GameObject resource = ObjectPooler.Instance.SpawnFromPool(resourceSo, player.position, Quaternion.identity);
        Rigidbody rig = resource.GetComponent<Rigidbody>();
        Vector3 forward = (targetPos - player.position).normalized;
        Vector3 right = (Vector3.Cross(forward, Vector3.up)).normalized;
        rig.velocity = Vector3.up * _dropUpPower + right * (Random.Range(-1f, 1f) * _dropSidePower) + forward * (Random.Range(0.5f, 1f) * _dropTowardPower);
        rig.angularVelocity = Vector3.one * Random.Range(_dropMinRotationPower, _dropMaxRotationPower);
        StartCoroutine(GoToTarget(rig, targetPos));
    }
    
    IEnumerator GoToTarget(Rigidbody rig, Vector3 targetPos)
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

    public void BuiltIsDone()
    {
        _visuals.SetActive(false);
        enabled = false;
    }
}
