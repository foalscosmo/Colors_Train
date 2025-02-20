using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private FruitsSpawner fruitsSpawner;
    [SerializeField] private MapSpawner mapSpawner;
    [SerializeField] private GameObject finishPanel;
    
    private void Awake()
    {
        Application.targetFrameRate = 300;
    }

    private void OnEnable()
    {
        passengerSpawner.OnAllCorrectSnap += StartMovingMap;
        passengerSpawner.OnBuildingCorrectSnap += mapSpawner.MoveMapWithDelay;
        
        fruitsSpawner.OnAllCorrectSnapFruits += StartMovingMapSecondStage;
        fruitsSpawner.OnBuildingCorrectSnapFruits += mapSpawner.MoveMapWithDelay;

        mapSpawner.OnAllFinish += FinishGame;
    }
    
    private void OnDisable()
    {
        passengerSpawner.OnAllCorrectSnap -= StartMovingMap;
        passengerSpawner.OnBuildingCorrectSnap -= mapSpawner.MoveMapWithDelay;
        
        fruitsSpawner.OnAllCorrectSnapFruits -= StartMovingMapSecondStage;
        fruitsSpawner.OnBuildingCorrectSnapFruits -= mapSpawner.MoveMapWithDelay;
        
        mapSpawner.OnAllFinish -= FinishGame;

    }

    private void StartMovingMap()
    {
        mapSpawner.MoveMapWithDelay();
        SetTargetTransformToBuildings();
    }

    private void FinishGame()
    {
       // finishPanel.SetActive(true);
       
       //finish game
    }

    private void SetTargetTransformToBuildings()
    {
        if(mapSpawner.CurrentIndexOfMap > 1) return;
        StartCoroutine(DelayAfterResetTransform());
    }

    private IEnumerator DelayAfterResetTransform()
    {
        yield return new WaitForSecondsRealtime(1f);
        foreach (var draggedObj in passengerSpawner.DraggedObjects)
        {
            draggedObj.TargetTransforms = buildingSpawner.BuildingTransforms;
            draggedObj.IsSnapped = false;
            var parentTransform = draggedObj.transform.parent;

            if (parentTransform != null)
            {
                draggedObj.StartPosition = parentTransform.position;
            }
        }
    }
    
    private void StartMovingMapSecondStage()
    {
        mapSpawner.MoveMapWithDelay();
        SetFruitsTargetTransformBuildings();
    }
    
    private void SetFruitsTargetTransformBuildings()
    {
        if(mapSpawner.CurrentIndexOfMap > 5) return;
        StartCoroutine(DelayAfterResetFruits());
    }
    
    private IEnumerator DelayAfterResetFruits()
    {
        yield return new WaitForSecondsRealtime(1f);
        foreach (var draggedObj in fruitsSpawner.DraggedObjects)
        {
            draggedObj.TargetTransforms = buildingSpawner.BuildingTransforms;
            draggedObj.IsSnapped = false;
            var parentTransform = draggedObj.transform.parent;

            if (parentTransform != null)
            {
                draggedObj.StartPosition = parentTransform.position;
            }
        }
    }
}