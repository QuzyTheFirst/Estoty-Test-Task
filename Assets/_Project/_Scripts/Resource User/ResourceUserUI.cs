using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceUserUI : MonoBehaviour
{
    [SerializeField] private GameObject _resourceUserRequirementsPanel;
    [SerializeField] private ResourceUIRequirement _resourceUIRequirementPf;

    private List<ResourceUIRequirement> _uiRequirements;

    private ResourceUser _resourceUser;
    
    private void Awake()
    {
        _resourceUser = GetComponent<ResourceUser>();
        _uiRequirements = new List<ResourceUIRequirement>();
        InitializeRequirements();
    }

    private void InitializeRequirements()
    {
        foreach (ResourceUser.BuildRequirement buildRequirement in _resourceUser.BuildRequirements)
        {
            ResourceUIRequirement resourceUIRequirement = Instantiate(_resourceUIRequirementPf, _resourceUserRequirementsPanel.transform);
            resourceUIRequirement.Initialize(buildRequirement.ResourceSO, buildRequirement.Amount);
            _uiRequirements.Add(resourceUIRequirement);
        }
    }

    private void OnEnable()
    {
        _resourceUser.ResourceUsed += ResourceUserOnResourceUsed;
    }

    private void ResourceUserOnResourceUsed(object sender, ResourceUser.ResourceUsedEventData e)
    {
        ResourceUIRequirement resourceUIRequirement =
            _uiRequirements.First(item => item.CurrentResourceSO == e.ResourceSO);
        
        resourceUIRequirement.UpdateCollectedAmount(_resourceUser.ResourcesAmountCollected[e.ResourceSO]);
    }

    private void OnDisable()
    {
        _resourceUser.ResourceUsed -= ResourceUserOnResourceUsed;
    }
}
