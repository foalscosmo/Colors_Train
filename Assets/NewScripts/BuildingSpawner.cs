using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingObjects = new();
    [SerializeField] private List<SpriteRenderer> buildingSpriteRenderers = new();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Transform> buildingTransforms = new();
    
    public List<SpriteRenderer> BuildingSpriteRenderers
    {
        get => buildingSpriteRenderers;
        set => buildingSpriteRenderers = value;
    }

    public Transform SpawnPoint
    {
        get => spawnPoint;
    }

    public List<GameObject> BuildingObjects
    {
        get => buildingObjects;
        set => buildingObjects = value;
    }
    public List<Transform> BuildingTransforms
    {
        get => buildingTransforms;
        set => buildingTransforms = value;
    }
    
}