using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float m_PosX;
    private float m_PosY;

    private Vector3 m_PositionCorrection;

    private Vector2 beginDragMousePosition;
    private Vector2 beginDragObjectPosition;

    private Rect rect;
    private RectTransform rectTransform;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragObjectPosition = rectTransform.position;
        beginDragMousePosition = eventData.pressPosition;
        m_PositionCorrection = beginDragMousePosition - beginDragObjectPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(rectTransform.anchorMin.x == 1)
        {
            m_PosX = Mathf.Clamp(eventData.position.x - m_PositionCorrection.x, rect.width, Screen.width);
        }
        else if(rectTransform.anchorMin.x == 0)
        {
            m_PosX = Mathf.Clamp(eventData.position.x - m_PositionCorrection.x, 0f, Screen.width - rect.width);
        }
        else
        {
            m_PosX = Mathf.Clamp(eventData.position.x - m_PositionCorrection.x, (rect.width / 2f), Screen.width - (rect.width/2f));
        }

        if (rectTransform.anchorMin.y == 0)
        {
            m_PosY = Mathf.Clamp(eventData.position.y - m_PositionCorrection.y, 0, Screen.height - rect.height);
        }
        else if(rectTransform.anchorMin.y == 1)
        {
            m_PosY = Mathf.Clamp(eventData.position.y - m_PositionCorrection.y, rect.height, Screen.height);
        }
        else
        {
            m_PosY = Mathf.Clamp(eventData.position.y - m_PositionCorrection.y, (rect.height / 2f), Screen.height - (rect.height/2f));
        }

        transform.position = new Vector2(m_PosX, m_PosY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    private void Awake()
    {
        rectTransform = this.transform.GetComponent<RectTransform>();
        rect = rectTransform.rect;
    }
}
