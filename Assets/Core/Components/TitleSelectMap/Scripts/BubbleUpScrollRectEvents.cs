using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BubbleUpScrollRectEvents: MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler {
	ScrollRect mainScroll;

	void Start() {
		mainScroll = transform.GetComponentInParent<ScrollRect>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		mainScroll.OnBeginDrag(eventData);
	}


	public void OnDrag(PointerEventData eventData) {
		mainScroll.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData) {
		mainScroll.OnEndDrag(eventData);
	}


	public void OnScroll(PointerEventData data) {
		mainScroll.OnScroll(data);
	}
}