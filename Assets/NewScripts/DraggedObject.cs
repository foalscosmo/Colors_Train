using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DraggedObject : MonoBehaviour
{
        private new Camera camera;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D objCollider;
        [SerializeField] private float moveSpeed;
        private Finger movementFinger;
        private Vector2 touchOffset;
        private Vector2 currentTouchPosition;
        [SerializeField] private bool isDragging;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private bool isSnapped;
        public event Action OnCorrectSnap;
        public event Action OnInCorrectSnap;
        [SerializeField] private List<Transform> targetTransforms = new();
        [SerializeField] private bool canDrag;

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
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += HandleFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += HandleFingerUp;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += HandleFingerMove;
        }

        private void OnDisable()
        {
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= HandleFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= HandleFingerUp;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= HandleFingerMove;
            EnhancedTouchSupport.Disable();
        }


        private void HandleFingerDown(Finger touchedFinger)
        {
            if (movementFinger == null && IsTouchingScreen(touchedFinger) && canDrag)
            {
                transform.DOScale(1.2f, 0.5f);
                spriteRenderer.sortingOrder = 4;
                isDragging = true;
                movementFinger = touchedFinger;
                currentTouchPosition = touchedFinger.screenPosition;
                Vector2 worldTouchPosition = camera.ScreenToWorldPoint(currentTouchPosition);
                touchOffset = (Vector2)transform.position - worldTouchPosition;
            }
        }

        private void HandleFingerUp(Finger liftedFinger)
        {
            if (liftedFinger == movementFinger && canDrag)
            {
                foreach (var target in from target in targetTransforms
                         let distanceToTarget = Vector2.Distance
                             (new Vector2(transform.position.x, transform.position.y+2.5f), target.position)
                         where distanceToTarget <= 3f
                         where gameObject.CompareTag(target.gameObject.tag)
                         select target)
                {
                    SnapToTarget(target);
                    break;
                }
                movementFinger = null;
                spriteRenderer.sortingOrder = 3;
                isDragging = false;
            }
            
            if (!IsSnapped && canDrag)
            {
                ReturnToOriginalPosition();
                OnInCorrectSnap?.Invoke();
            }
            
            transform.DOScale(1f, 0.2f);
        }
        
        private void SnapToTarget(Transform target)
        {
            var position = target.position;
            Vector3 targetPosition = new Vector3(position.x, position.y);
            transform.DOMove(targetPosition, 0.2f).SetEase(Ease.OutBack);
            IsSnapped = true;
            canDrag = false;
            transform.SetParent(target);
            OnCorrectSnap?.Invoke();
        }
        
        private void ReturnToOriginalPosition()
        {
            transform.DOMove(StartPosition, 0.35f).OnComplete(() =>
            {
                spriteRenderer.sortingOrder = 5;
            });
        }

        private void HandleFingerMove(Finger movedFinger)
        {
            if (movedFinger == movementFinger && canDrag)
            {
                currentTouchPosition = movedFinger.screenPosition;
                Vector2 worldTouchPosition = camera.ScreenToWorldPoint(currentTouchPosition);
                Vector2 targetPosition = worldTouchPosition + touchOffset;
                transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed);
            }
        }

        private bool IsTouchingScreen(Finger touchedFinger)
        {
            Vector2 touchPosition = touchedFinger.screenPosition;
            Vector2 worldTouchPosition = camera.ScreenToWorldPoint(touchPosition);
            return objCollider.OverlapPoint(worldTouchPosition);
        }
}