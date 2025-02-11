using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NextObjectMovement : MonoBehaviour
{
    [SerializeField] private MapSpawner mapSpawner;
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private FruitsSpawner fruitsSpawner;
    [SerializeField] private BuildingSpawner buildingSpawner;
    [SerializeField] private float moveDistance = 52f;
    [SerializeField] private float moveDuration = 7f;
    [SerializeField] private Transform tunnel;
    private Vector2 tunnelStartPosition = new Vector2(32,-1.5f);
 
    private int currentIndexOfObject;

    private void OnEnable()
    {
        mapSpawner.OnStartMoving += StartMovingPassengers;
    }

    private void OnDisable()
    {
        mapSpawner.OnStartMoving -= StartMovingPassengers;
    }

    private void StartMovingPassengers()
    {
        switch (currentIndexOfObject)
        {
            case 0:
                MovePassengers();
                MoveTunnel();
                MoveBuilding(0);
                currentIndexOfObject++;
                break;
            case 1:
                ResetTunnel();
                MoveTunnel();
                foreach (var pig in passengerSpawner.DraggedObjects)
                {
                    pig.SnapOffsetX = 1.5f;
                    pig.SnapOffsetY = -1.5f;
                    pig.OriginOffsetY = 1.1f;
                }
                MoveBuildingWithReset(0);
                MoveBuilding(1);
                currentIndexOfObject++;
                break;
            case 2:
                ResetTunnel();
                MoveTunnel();
                MoveBuildingWithReset(1);
                MoveBuilding(2);
                currentIndexOfObject++;
                break;
            case 3:
                ResetTunnel();
                MoveTunnel();
                MoveBuildingWithReset(2);
                MoveBuilding(3);
                currentIndexOfObject++;
                break;
            case 4:
                ResetTunnel();
                MoveTunnel();
                MoveBuildingWithReset(3);
                MoveBuilding(0);
                MoveFruits();
                currentIndexOfObject++;
                break;
            case 5:
                ResetTunnel();
                MoveTunnel();
                foreach (var fruits in fruitsSpawner.DraggedObjects)
                {
                    fruits.SnapOffsetX = -1.5f;
                    fruits.SnapOffsetY = -1.5f;
                    fruits.OriginOffsetY = 1f;
                }
                MoveBuildingWithReset(0);
                MoveBuilding(1);
                currentIndexOfObject++;
                break;
            case 6:
                ResetTunnel();
                MoveTunnel();
                MoveBuildingWithReset(1);
                MoveBuilding(2);
                currentIndexOfObject++;
                break;
            case 7:
                ResetTunnel();
                MoveTunnel();
                MoveBuildingWithReset(2);
                MoveBuilding(3);
                currentIndexOfObject++;
                break;
        }
    }

    private void MovePassengers()
    {
        foreach (var passenger in passengerSpawner.DraggedObjects)
        {
            Transform transform1;
            passenger.transform.DOMove(
                new Vector3((transform1 = passenger.transform).position.x - moveDistance, transform1.position.y),
                moveDuration)
                .SetEase(Ease.Linear);
        }
    }

    private void MoveBuilding(int buildingIndex)
    {
        if (buildingIndex >= 0 && buildingIndex < buildingSpawner.BuildingObjects.Count)
        {
            buildingSpawner.BuildingObjects[buildingIndex].transform.DOMove(
                new Vector2(buildingSpawner.BuildingObjects[buildingIndex].transform.position.x - moveDistance,
                            buildingSpawner.BuildingObjects[buildingIndex].transform.position.y),
                moveDuration)
                .SetEase(Ease.Linear);
        }
    }

    private void MoveBuildingWithReset(int buildingIndex)
    {
        if (buildingIndex >= 0 && buildingIndex < buildingSpawner.BuildingObjects.Count)
        {
            buildingSpawner.BuildingObjects[buildingIndex].transform.DOMove(
                new Vector2(buildingSpawner.BuildingObjects[buildingIndex].transform.position.x - moveDistance,
                            buildingSpawner.BuildingObjects[buildingIndex].transform.position.y),
                moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ResetBuildingPosition(buildingIndex);
                });
        }
    }

    private void ResetBuildingPosition(int buildingIndex)
    {
        var building = buildingSpawner.BuildingObjects[buildingIndex];
        if (buildingIndex == 0)
        {
             building.transform.position = buildingSpawner.SpawnPoint.transform.position;
        }
        else
        {
            building.transform.position = new Vector2(buildingSpawner.SpawnPoint.transform.position.x,
                buildingSpawner.SpawnPoint.transform.position.y + 1.3f);
        }
        
        var child = building.transform.GetChild(0);
        var transform1 = child.transform;
        var position = transform1.position;
        position = new Vector2(
            position.x - 1.5f, position.y);
        transform1.position = position;
    }

    private void MoveFruits()
    {
        foreach (var passenger in fruitsSpawner.DraggedObjects)
        {
            Transform transform1;
            passenger.transform.DOMove(
                    new Vector3((transform1 = passenger.transform).position.x - moveDistance, transform1.position.y),
                    moveDuration)
                .SetEase(Ease.Linear);
        }
    }

    private void MoveTunnel()
    {
        tunnel.DOMove(new Vector2(tunnel.transform.position.x - 52, tunnel.transform.position.y), moveDuration).SetEase(Ease.Linear);

    }

    private void ResetTunnel()
    {
        tunnel.position = tunnelStartPosition;
    }
}