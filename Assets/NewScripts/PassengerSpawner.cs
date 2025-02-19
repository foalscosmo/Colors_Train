using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{ 
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private List<Transform> alignPoints = new();
    [SerializeField] private List<DraggedObject> draggedObjects = new();
    [SerializeField] private List<Transform> trainSlots = new();
    [SerializeField] private MapSpawner mapSpawner;
    [SerializeField] private Camera mainCamera;
    public event Action OnAllCorrectSnap;
    public event Action OnBuildingCorrectSnap;
    private int snapIndex;
    public List<DraggedObject> DraggedObjects
    {
        get => draggedObjects;
        set => draggedObjects = value;
    }
    
    private void OnEnable()
    {
        mapSpawner.OnStopMoving += SetDragPermission;
        mapSpawner.OnStartMoving += SetDragPermissionFalse;
        
        foreach (var dragObject in draggedObjects)
        {
            dragObject.OnCorrectSnap += Test;
            dragObject.OnCorrectSnap += SnapCounterOnFirstStage;
            dragObject.OnCorrectSnap += SnapCounterOnSecondStage;
            dragObject.OnDragStarted += HandleDraggingOnlyOne;
            dragObject.OnIncorrectSnap += HandleDraggingAfterReturn;
            dragObject.OnCorrectSnapWithInt += HandleDraggingAfterCorrect;
        }
    }

    private void OnDisable()
    {
        mapSpawner.OnStopMoving -= SetDragPermission;
        mapSpawner.OnStartMoving -= SetDragPermissionFalse;
        
        foreach (var dragObject in draggedObjects)
        {
            dragObject.OnCorrectSnap -= Test;
            dragObject.OnCorrectSnap -= SnapCounterOnFirstStage;
            dragObject.OnCorrectSnap -= SnapCounterOnSecondStage;
            dragObject.OnDragStarted -= HandleDraggingOnlyOne;
            dragObject.OnIncorrectSnap -= HandleDraggingAfterReturn;
            dragObject.OnCorrectSnapWithInt -= HandleDraggingAfterCorrect;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
           var passenger = Instantiate(passengerPrefab, spawnPoints[i].position, quaternion.identity);
           passenger.transform.SetParent(spawnPoints[i]);
           passenger.GetComponent<DraggedObject>().MainCamera = mainCamera;
           draggedObjects[i] = passenger.GetComponent<DraggedObject>();
           draggedObjects[i].StartPosition = alignPoints[i].position;
           draggedObjects[i].TargetTransforms = trainSlots;
           draggedObjects[i].SnapOffsetY = 1.1f;
           draggedObjects[i].CanDrag = false;
           draggedObjects[i].ObjIndex = i;
        }
    }
    
    private void SetDragPermission()
    {
        foreach (var dragObj in draggedObjects)
        {
            dragObj.CanDrag = true;
        }
    }
    
    private void HandleDraggingOnlyOne(int index)
    {
        for (var i = 0; i < draggedObjects.Count; i++)
        {
            draggedObjects[i].CanDrag = (i == index && !draggedObjects[i].IsSnapped);
        }
    }
    
    private void HandleDraggingAfterReturn(int index)
    {
        foreach (var obj in draggedObjects)
        {
            obj.CanDrag = !obj.IsSnapped;

        }
    }

    
    private void HandleDraggingAfterCorrect(int index)
    {
        if (currentIndex < 3)
        {
            for (var i = 0; i < draggedObjects.Count; i++)
            {
                var isSnapped = draggedObjects[i].IsSnapped;
                draggedObjects[i].CanDrag = !isSnapped && (i != index);
            }
        }else if (currentIndex == 3)
        {
            for (var i = 0; i < draggedObjects.Count; i++)
            {
                draggedObjects[i].CanDrag = false;
            }
        }
        
    }
    
    private int currentIndex;

    private void Test()
    {
        currentIndex++;
    }

    private void SetDragPermissionFalse()
    {
        foreach (var dragObj in draggedObjects)
        {
            dragObj.CanDrag = false;
        }
    }

    private void SnapCounterOnFirstStage()
    {
        if(snapIndex == 3) return;
        snapIndex++;
        if (snapIndex == 3)
        {
            OnAllCorrectSnap?.Invoke();
        }
    }

    private void SnapCounterOnSecondStage()
    {
        if (mapSpawner.CurrentIndexOfMap is >= 2 and <= 5)
        {
            OnBuildingCorrectSnap?.Invoke();
        }
    }
}