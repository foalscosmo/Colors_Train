using UnityEngine;

public class BuildingLayerType : MonoBehaviour
{
    [SerializeField] public LayerEnum buildingLayer;

    public LayerEnum BuildingLayer
    {
        get => buildingLayer;
        set => buildingLayer = value;
    }
}