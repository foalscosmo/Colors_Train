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
                currentIndexOfObject++;
                break;

            case 1:
                MoveBuilding(0);
                currentIndexOfObject++;
                break;

            case 2:
                MoveBuildingWithReset(0);
                MoveBuilding(1);
                currentIndexOfObject++;
                break;
            case 3:
                MoveBuildingWithReset(1);
                MoveBuilding(2);
                currentIndexOfObject++;
                break;
            case 4:
                MoveBuildingWithReset(2);
                MoveFruits();
                currentIndexOfObject++;
                break;
            case 5:
                MoveBuilding(0);
                currentIndexOfObject++;
                break;
            case 6:
                MoveBuildingWithReset(0);
                MoveBuilding(1);
                currentIndexOfObject++;
                break;
            case 7:
                MoveBuildingWithReset(1);
                MoveBuilding(2);
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
        building.transform.position = buildingSpawner.SpawnPoint.transform.position;
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
}