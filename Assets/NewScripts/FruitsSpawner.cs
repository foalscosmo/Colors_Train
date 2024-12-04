using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FruitsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fruitsPrefab;
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private List<Transform> alignPoints = new();
    [SerializeField] private List<DraggedObject> draggedObjects = new();
    [SerializeField] private List<Transform> trainSlots = new();
    [SerializeField] private MapSpawner mapSpawner;
    [SerializeField] private Camera mainCamera;
    public event Action OnAllCorrectSnapFruits;
    public event Action OnBuildingCorrectSnapFruits;


    [SerializeField] private int snapIndexOfFruits;
    public List<DraggedObject> DraggedObjects
    {
        get => draggedObjects;
        set => draggedObjects = value;
    }

    private void OnEnable()
    {
        mapSpawner.OnSecondStageEnter += SetDragPermissionToFruits;
        mapSpawner.OnStopMoving += SetDragPermissionToFruits;
        
        foreach (var dragObject in draggedObjects) dragObject.OnCorrectSnap += SnapCounterOnFirstStageFruits;
        foreach (var dragObject in draggedObjects) dragObject.OnCorrectSnap += SnapCounterOnSecondStageFruits;
    }

    private void OnDisable()
    {
        mapSpawner.OnSecondStageEnter -= SetDragPermissionToFruits;
        mapSpawner.OnStopMoving -= SetDragPermissionToFruits;

        foreach (var dragObject in draggedObjects) dragObject.OnCorrectSnap -= SnapCounterOnFirstStageFruits;
        foreach (var dragObject in draggedObjects) dragObject.OnCorrectSnap -= SnapCounterOnSecondStageFruits;

    }

    public void Awake()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
           var passenger = Instantiate(fruitsPrefab, spawnPoints[i].position, quaternion.identity);
           passenger.transform.SetParent(spawnPoints[i]);
           passenger.GetComponent<DraggedObject>().MainCamera = mainCamera;
           draggedObjects[i] = passenger.GetComponent<DraggedObject>();
           draggedObjects[i].StartPosition = alignPoints[i].position;
           draggedObjects[i].TargetTransforms = trainSlots;
           draggedObjects[i].CanDrag = false;
        }
    }
    
    private void SetDragPermissionToFruits()
    {
        if (mapSpawner.CurrentIndexOfMap >= 5)
        {
            foreach (var dragObj in draggedObjects)
            {
                dragObj.CanDrag = true;
            }
        }
    }
    
    
    private void SnapCounterOnFirstStageFruits()
    {
        if(snapIndexOfFruits == 3) return;
        snapIndexOfFruits++;
        if (snapIndexOfFruits == 3)
        {
            OnAllCorrectSnapFruits?.Invoke();
            
        }
    }

    private void SnapCounterOnSecondStageFruits()
    {
        if (mapSpawner.CurrentIndexOfMap is >= 6 and <= 8)
        {
            OnBuildingCorrectSnapFruits?.Invoke();
        }
    }
}