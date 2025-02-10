using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> mapObjects = new();
    [SerializeField] private List<Transform> spawnPoint = new();
    [SerializeField] private float moveDuration;
    [SerializeField] private float spawnOffset = 52f; 
    [SerializeField] private List<GameObject> cloneObjects = new();
    [SerializeField] private int mapIndex;
    [SerializeField] private int currentIndexOfMap;
    
    private bool isMoving;
    public event Action OnStartMoving;
    public event Action OnStopMoving;
    public event Action OnSecondStageEnter;
    public event Action OnAllFinish;

    public int CurrentIndexOfMap
    { 
        get => currentIndexOfMap;
    }
    private void Start()
    {
        InitializeObjects();
        MoveMapWithDelay();
    }
    
    private void InitializeObjects()
    {
        for (int i = 0; i < mapObjects.Count; i++)
        {
            var cloned = Instantiate(mapObjects[i], spawnPoint[i].position, Quaternion.identity);
            var cloned1 = Instantiate(mapObjects[i],
                new Vector2(spawnPoint[i].transform.position.x + 52, spawnPoint[i].transform.position.y),
                quaternion.identity);
            cloned.SetActive(true);
            cloned1.SetActive(true);
            cloneObjects.Add(cloned);
            cloneObjects.Add(cloned1);
        }
    }

    public void MoveMapWithDelay()
    {
        if (currentIndexOfMap == 8)
        {
            OnAllFinish?.Invoke();
            return;
        }
        StartCoroutine(MoveMapObjects());
    }

    private IEnumerator MoveMapObjects()
    {
        yield return new WaitForSecondsRealtime(1f);
        OnStartMoving?.Invoke();
        for (var index = 0; index < cloneObjects.Count; index++)
        {
            int currentIndex = index;
            var index1 = index;
            cloneObjects[currentIndex].transform
                .DOMove(
                    new Vector2(cloneObjects[currentIndex].transform.position.x - spawnOffset,
                        cloneObjects[currentIndex].transform.position.y),
                    moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (currentIndex == cloneObjects.Count - 1) 
                    {
                        if (mapIndex == 0)
                        {
                            cloneObjects[0].transform.position = new Vector2(spawnPoint[0].position.x + spawnOffset, spawnPoint[0].transform.position.y);
                            cloneObjects[2].transform.position = new Vector2(spawnPoint[1].position.x + spawnOffset, spawnPoint[1].transform.position.y);
                            mapIndex++;
                        }
                        else if (mapIndex == 1)
                        {
                            cloneObjects[1].transform.position = new Vector2(spawnPoint[0].position.x + spawnOffset, spawnPoint[0].transform.position.y);
                            cloneObjects[3].transform.position = new Vector2(spawnPoint[1].position.x + spawnOffset, spawnPoint[1].transform.position.y);
                            mapIndex--;
                        }
                    }

                    if (index1 == 2)
                    {
                        OnStopMoving?.Invoke();
                        currentIndexOfMap++;
                        if (currentIndexOfMap == 5)
                        {
                           OnSecondStageEnter?.Invoke();
                        }
                    }
                });
        }
    }
}