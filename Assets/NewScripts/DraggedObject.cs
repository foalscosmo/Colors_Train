using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DraggedObject : MonoBehaviour
{
   private Camera camera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D objCollider;
    [SerializeField] private float moveSpeed;
    private bool isDragging;
    private Vector2 touchOffset;
    private Vector2 currentTouchPosition;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private bool isSnapped;
    public event Action OnCorrectSnap;
    [SerializeField] private List<Transform> targetTransforms = new();
    [SerializeField] private bool canDrag;
    [SerializeField] private float snapOffsetY;
    [SerializeField] private float snapOffsetX;
    [SerializeField] private float originOffsetY;
    [SerializeField] private float snapRange;
    private int activeTouchID = -1;

    public float OriginOffsetY
    {
        get => originOffsetY;
        set => originOffsetY = value;
    }
    public float SnapOffsetY
    {
        get => snapOffsetY;
        set => snapOffsetY = value;
    }
    
    public float SnapOffsetX
    {
        get => snapOffsetX;
        set => snapOffsetX = value;
    }
    
    public bool CanDrag
    {
        get => canDrag;
        set => canDrag = value;
    }

    public List<Transform> TargetTransforms
    {
        get => targetTransforms;
        set => targetTransforms = value;
    }

    public bool IsSnapped
    {
        get => isSnapped;
        set => isSnapped = value;
    }

    public Vector2 StartPosition
    {
        get => startPosition;
        set => startPosition = value;
    }

    public bool IsDragging
    {
        get => isDragging;
        set => isDragging = value;
    }

    public SpriteRenderer SpriteRenderer
    {
        get => spriteRenderer;
        set => spriteRenderer = value;
    }

    public Camera MainCamera
    {
        get => camera;
        set => camera = value;
    }

    [SerializeField] private LayerEnum draggedLayer;

    public LayerEnum DraggedLayer
    {
        get => draggedLayer;
        set => draggedLayer = value;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0) 
        {
            Touch lastTouch = Input.GetTouch(Input.touchCount - 1); // Get the last touch

            switch (lastTouch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchDown(lastTouch);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    HandleTouchMove(lastTouch);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    HandleTouchUp(lastTouch);
                    break;
            }
        }
    }

    private void HandleTouchDown(Touch touch)
    {
        Vector2 touchWorldPos = camera.ScreenToWorldPoint(touch.position);

        if (objCollider.OverlapPoint(touchWorldPos) && canDrag)
        {
            activeTouchID = touch.fingerId; // Assign the touch ID
            transform.DOScale(1.2f, 0.2f);
            spriteRenderer.sortingOrder = 10;
            isDragging = true;
            currentTouchPosition = touchWorldPos;
            touchOffset = (Vector2)transform.position - touchWorldPos;
        }
    }

    private void HandleTouchUp(Touch touch)
    {
        if (touch.fingerId == activeTouchID) // Only process if it’s the right touch
        {
            if (canDrag)
            {
                foreach (var target in targetTransforms)
                {
                    float distanceToTarget = Vector2.Distance(
                        new Vector2(transform.position.x, transform.position.y),
                        target.position
                    );

                    var trainSlot = target.GetComponent<MatchingLayerType>();
                    if (distanceToTarget <= snapRange && trainSlot != null && DraggedLayer == trainSlot.MatchingLayer)
                    {
                        SnapToTarget(target);
                        break;
                    }
                }
                isDragging = false;
                spriteRenderer.sortingOrder = 8;
            }

            if (!IsSnapped && canDrag)
            {
                ReturnToOriginalPosition();
            }

            activeTouchID = -1; // Reset the active touch ID
        }
    }

    private void HandleTouchMove(Touch touch)
    {
        if (touch.fingerId == activeTouchID) // Ensure we're tracking the correct touch
        {
            Vector2 touchWorldPos = camera.ScreenToWorldPoint(touch.position);
            Vector2 targetPosition = touchWorldPos + touchOffset;
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed);
        }
    }

    private void SnapToTarget(Transform target)
    {
        var position = target.position;
        Vector3 targetPosition = new Vector3(position.x + snapOffsetX, position.y + snapOffsetY);
        spriteRenderer.sortingOrder = 5;
        transform.DOMove(targetPosition, 0.35f).SetEase(Ease.OutBack);
        IsSnapped = true;
        canDrag = false;
        transform.SetParent(target);
        transform.DOScale(1f, 0.2f);
        OnCorrectSnap?.Invoke();
    }

    private void ReturnToOriginalPosition()
    {
        spriteRenderer.sortingOrder = 5;
        transform.DOScale(1f, 0.2f);
        transform.DOMove(new Vector2(startPosition.x, startPosition.y + originOffsetY), 0.35f);
    }

    // Draw snap range in editor
    private void OnDrawGizmos()
    {
        if (targetTransforms == null) return;

        Gizmos.color = Color.green;
        foreach (var target in targetTransforms)
        {
            if (target == null) continue;
            Gizmos.DrawWireSphere(target.position, snapRange);
        }
    }
}