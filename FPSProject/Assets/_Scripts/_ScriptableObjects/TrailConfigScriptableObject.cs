using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapon/Gun Trail Config", order = 4)]
public class TrailConfigScriptableObject : ScriptableObject
{
    [Header("Trail Configuration Settings")]
    public Material _material;
    public AnimationCurve _widthCurve;
    public float _duration = 0.5f;
    public float _minVertexDistance = 0.1f;
    public Gradient _color;

    [Header("Simulation Settings")]
    public float _missDistance = 100f;
    public float _simulationSpeed = 100f;

}
