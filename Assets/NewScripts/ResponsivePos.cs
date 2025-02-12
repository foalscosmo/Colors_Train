using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResponsivePosition : MonoBehaviour
{
    [SerializeField]  private Camera mainCamera;
    private readonly Vector2 baseResolution = new(1920,1080);
    private float baseAspect;

        
    private void Start()
    {
        FitScaleToResolution();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FitScaleToResolution();
        }
    }

    private void FitScaleToResolution()
    {
        var transform1 = transform;
        transform1.localScale = Vector3.one;
        var position = mainCamera.transform.position;
        transform1.position = new Vector3(position.x, position.y, 0);
        var targetAspect = (float)Screen.width / Screen.height;
        baseAspect = baseResolution.x / baseResolution.y;
        var scaleFactor = targetAspect / baseAspect;
        if (scaleFactor > 1f)
            scaleFactor = 1f;
        transform.localScale *= scaleFactor;
    }
}