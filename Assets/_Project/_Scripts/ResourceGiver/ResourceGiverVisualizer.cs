using System.Collections;
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

    private ResourceGiver _resourceGiver;

    private void Awake()
    {
        _resourceGiver = GetComponent<ResourceGiver>();
    }

    public void DropResource(ResourceSO resourceSo, Transform target)
    {
        Resource resource = ResourcePooler.Instance.SpawnFromPool(resourceSo, transform.position, Quaternion.identity);
        resource.Rig.velocity = Vector3.up * _dropUpPower + (Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f)) * _dropSidePower;
        resource.Rig.angularVelocity = Vector3.one * Random.Range(_dropMinRotationPower, _dropMaxRotationPower);
        StartCoroutine(GoToTarget(resource.Rig, target));
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

    private void OnEnable()
    {
        _resourceGiver.ResourceGiven += ResourceGiverOnResourceGiven;
    }

    private void ResourceGiverOnResourceGiven(object sender, ResourceGiver.ResourceGivenEventData data)
    {
        DropResource(data.ResourceSO, data.PlayerTf);
    }

    private void OnDisable()
    {
        _resourceGiver.ResourceGiven -= ResourceGiverOnResourceGiven;
    }
}
