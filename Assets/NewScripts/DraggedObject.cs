using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DraggedObject : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D objCollider;
    [SerializeField] private float moveSpeed;
    private bool isDragging;
    private Vector2 touchOffset;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private bool isSnapped;
    public event Action OnCorrectSnap;
    public event Action<int> OnDragStarted;
    public event Action<int> OnIncorrectSnap;
    public event Action<int> OnCorrectSnapWithInt;
    [SerializeField] private int objIndex;
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

    public int ObjIndex
    {
        get => objIndex;
        set => objIndex = value;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && activeTouchID == -1)
            {
                HandleTouchDown(touch);
            }
            else if (touch.fingerId == activeTouchID)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        HandleTouchMove(touch);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        HandleTouchUp(touch);
                        break;
                    case TouchPhase.Began:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void HandleTouchDown(Touch touch)
    {
        if(isSnapped) return;
        Vector2 touchWorldPos = camera.ScreenToWorldPoint(touch.position);

        if (!objCollider.OverlapPoint(touchWorldPos) || !canDrag) return;
        OnDragStarted?.Invoke(objIndex);
        activeTouchID = touch.fingerId;
        Transform transform1;
        (transform1 = transform).DOScale(1.2f, 0.2f);
        spriteRenderer.sortingOrder = 10;
        isDragging = true;
        touchOffset = (Vector2)transform1.position - touchWorldPos;
    }

    private void HandleTouchUp(Touch touch)
    {
        if (touch.fingerId != activeTouchID) return;
        if (canDrag)
        {
            foreach (var target in from target in targetTransforms let distanceToTarget = Vector2.Distance(
                         new Vector2(transform.position.x, transform.position.y),
                         target.position
                     ) let trainSlot = target.GetComponent<MatchingLayerType>() where distanceToTarget <= snapRange && trainSlot != null && DraggedLayer == trainSlot.MatchingLayer select target)
            {
                SnapToTarget(target);
                break;
            }
            isDragging = false;
            spriteRenderer.sortingOrder = 5;
        }

        if (!IsSnapped && canDrag)
        {
            ReturnToOriginalPosition();
        }

        canDrag = false;
        activeTouchID = -1;
    }

    private void HandleTouchMove(Touch touch)
    {
        if (touch.fingerId != activeTouchID) return;
        Vector2 touchWorldPos = camera.ScreenToWorldPoint(touch.position);
        Vector2 targetPosition = touchWorldPos + touchOffset;
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed);
    }

    private void SnapToTarget(Transform target)
    {
        var position = target.position;
        var targetPosition = new Vector3(position.x + snapOffsetX, position.y + snapOffsetY);
        spriteRenderer.sortingOrder = 5;
        transform.DOMove(targetPosition, 0.35f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            OnCorrectSnapWithInt?.Invoke(objIndex);
        });
        IsSnapped = true;
        canDrag = false;
        transform.SetParent(target);
        transform.DOScale(1f, 0.2f);
        OnCorrectSnap?.Invoke();
    }

    private void ReturnToOriginalPosition()
    {
        transform.DOScale(1f, 0.2f);
        transform.DOMove(new Vector2(startPosition.x, startPosition.y + originOffsetY), 0.3f).OnComplete(() =>
        {
            OnIncorrectSnap?.Invoke(objIndex);
            spriteRenderer.sortingOrder = 5;
        });
    }
    
    private void OnDrawGizmos()
    {
        if (targetTransforms == null || targetTransforms.Count == 0)
            return;

        Gizmos.color = Color.red;

        foreach (var target in targetTransforms)
        {
            if (target != null)
            {
                Gizmos.DrawWireSphere(target.position, snapRange);
            }
        }
    }
}