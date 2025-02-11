using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnimator : MonoBehaviour
{
    [SerializeField] private List<Animator> wheelAnimators = new();
    [SerializeField] private MapSpawner mapSpawner;
    private int currentIndex;


    private void OnEnable()
    {
        mapSpawner.OnStartMoving += WheelAnimationPlay;
        mapSpawner.OnStopMoving += WheelAnimationStop;
    }

    private void OnDisable()
    {
        mapSpawner.OnStartMoving -= WheelAnimationPlay;
        mapSpawner.OnStopMoving -= WheelAnimationStop; 
    }

    private void Awake()
    {
        foreach (var wheel in wheelAnimators)
        {
            wheel.speed = 0;
        }
        
    }

    private void WheelAnimationPlay()
    {
        switch (currentIndex)
        {
            case 0:
                StartCoroutine(FirstRotationTimer());
                currentIndex++;
                break;
            case 1:
                foreach (var wheel in wheelAnimators)
                {
                    wheel.speed = 1;
                }
                break;
        }
        
    }

    private IEnumerator FirstRotationTimer()
    {
        yield return new WaitForSecondsRealtime(0.04f);
        foreach (var wheel in wheelAnimators)
        {
            wheel.speed = 1;
        }
    }

    private void WheelAnimationStop()
    {
        foreach (var wheel in wheelAnimators)
        {
            wheel.speed = 0;
        }
    }
}