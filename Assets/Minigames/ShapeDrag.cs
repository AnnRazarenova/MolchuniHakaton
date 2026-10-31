
using UnityEngine;
using UnityEngine.EventSystems;

public class ShapeDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler {
    public RectTransform targetHole;
    public float snapDistance = 80f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public bool isPlaced = false;
    private Vector2 startPosition;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (isPlaced) return;

        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = true;

        float distance = Vector2.Distance(
            rectTransform.position,
            targetHole.position
        );

        if (distance <= snapDistance) {
            PlaceShape();
        }
    }

    private void PlaceShape() {
        isPlaced = true;

        rectTransform.position = targetHole.position;

        ShapeGameManager.singleton.placedCount++;
    }
}
