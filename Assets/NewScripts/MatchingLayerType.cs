using UnityEngine;

public class MatchingLayerType : MonoBehaviour
{
    [SerializeField] private LayerEnum matchingLayer;

    public LayerEnum MatchingLayer
    {
        get => matchingLayer;
        set => matchingLayer = value;
    }
}