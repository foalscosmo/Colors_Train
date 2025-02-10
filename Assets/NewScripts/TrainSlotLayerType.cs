using UnityEngine;

public class TrainSlotLayerType : MonoBehaviour
{
    [SerializeField] private LayerEnum trainSlotEnum;

    public LayerEnum TrainSlotEnum
    {
        get => trainSlotEnum;
        set => trainSlotEnum = value;
    }
}