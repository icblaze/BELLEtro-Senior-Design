using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform  _rect;
    private CanvasGroup    _canvasGroup;
    private Canvas         _rootCanvas;
    private Transform      _originalParent;

    void Awake()
    {
        _rect        = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // Find your root Canvas (assumes card is under a Canvas somewhere)
        _rootCanvas = GetComponentInParent<Canvas>();
        if (_rootCanvas == null)
            Debug.LogError("DraggableCard: no Canvas in parent hierarchy!");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1) remember where we came from
        _originalParent = transform.parent;

        // 2) lift up into the canvas root so we’re always on top
        transform.SetParent(_rootCanvas.transform, true);

        // 3) allow raycasts to pass through this card while dragging
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move with the pointer
        _rect.anchoredPosition += eventData.delta / _rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // re-enable blocking
        _canvasGroup.blocksRaycasts = true;

        // see what we’re hovering over
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Transform newParent = null;

        foreach (var r in results)
        {
            // 1) if we find a Slot, snap there
            if (r.gameObject.CompareTag("Slot"))
            {
                newParent = r.gameObject.transform;
                break;
            }

            // 2) (optional) if we find the central PlayArea, snap there too
            if (r.gameObject.CompareTag("PlayArea"))
            {
                newParent = r.gameObject.transform;
                break;
            }
        }

        // 3) if no valid drop target, go back to where we came from
        if (newParent == null)
            newParent = _originalParent;

        transform.SetParent(newParent, false);
        _rect.anchoredPosition = Vector2.zero;
    }
}
