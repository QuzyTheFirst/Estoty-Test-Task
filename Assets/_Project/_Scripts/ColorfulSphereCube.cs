using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ColorfulSphereCube : MonoBehaviour
{
    public struct CubePart
    {
        public Transform Tf;
        public MeshRenderer Renderer;
    }

    [Header("Mesh")]
    [SerializeField] private Transform _spherePf;
    
    [Header("Parameters")]
    [SerializeField] private int _cubeSize = 15;
    [SerializeField] private float _positionModifier = 1.5f;
    [SerializeField] private float _animationSpeed = 20;
    [SerializeField] private float _cosCurveLength = 5;
    
    [Header("Colors")]
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    private List<CubePart> _cubeParts;
    
    private readonly int _colorPropertyId = Shader.PropertyToID("_Color");

    private void Awake()
    {
        _cubeParts = new List<CubePart>();
        
        for (int x = 0; x < _cubeSize; x++)
        {
            for (int y = 0; y < _cubeSize; y++)
            {
                for(int z = 0; z < _cubeSize; z++)
                {
                    Transform sphere = Instantiate(_spherePf);
                    sphere.gameObject.isStatic = true;
                    sphere.position = new Vector3(x, y, z) * _positionModifier;
                    _cubeParts.Add(new CubePart()
                    {
                        Tf = sphere,
                        Renderer = sphere.GetComponent<MeshRenderer>(),
                    });
                }
            }
        }
    }

    private void Update()
    {
        foreach (CubePart cubePart in _cubeParts)
        {
            float lerpValue = Mathf.InverseLerp(-1, 1, (float)Math.Cos((cubePart.Tf.position.x + Time.time * _animationSpeed) / _cosCurveLength));
            Color color = Color.Lerp(_firstColor, _secondColor, lerpValue);
            
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(_colorPropertyId, color);
            cubePart.Renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
