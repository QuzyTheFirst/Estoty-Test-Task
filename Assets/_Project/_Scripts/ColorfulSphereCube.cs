using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulSphereCube : MonoBehaviour
{
    public struct CubePart
    {
        public Transform Tf;
        public MeshRenderer Renderer;
    }
    
    [SerializeField] private int _cubeSize = 15;
    [SerializeField] private float _positionModifier = 1.2f;
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;
    [SerializeField] private float _animationSpeed = 10;
    [SerializeField] private float _cosCurveDivider = 15;

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
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.isStatic = true;
                    sphere.transform.position = new Vector3(x, y, z) * _positionModifier;
                    _cubeParts.Add(new CubePart()
                    {
                        Tf = sphere.transform,
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
            float lerpValue = Mathf.InverseLerp(-1, 1, (float)Math.Cos((cubePart.Tf.position.x + Time.time * _animationSpeed) / _cosCurveDivider));
            Color color = Color.Lerp(_firstColor, _secondColor, lerpValue);
            
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(_colorPropertyId, color);
            cubePart.Renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
