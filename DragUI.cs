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

    private float x;
    private float y;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragObjectPosition = rectTransform.position;
        beginDragMousePosition = eventData.pressPosition;
        m_PositionCorrection = beginDragMousePosition - beginDragObjectPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        x = eventData.position.x - m_PositionCorrection.x;
        y = eventData.position.y - m_PositionCorrection.y;

        if (AnchorsCheck(0,1,1,1)) // TOP
        {
            m_PosX = Screen.width / 2f;
            m_PosY = Mathf.Clamp(y, rect.height, Screen.height);
        }
        else if (AnchorsCheck(0,0,1,0)) // BOTTOM
        {
            m_PosX = Screen.width / 2f;
            m_PosY = Mathf.Clamp(y, 0f, Screen.height - rect.height);
        }
        else if(AnchorsCheck(0,0,0,1)) // LEFT
        {
            m_PosX = Mathf.Clamp(x, 0f, Screen.width - rect.width);
            m_PosY = Screen.height / 2f;
        }
        else if (AnchorsCheck(1,0,1,1))
        {
            m_PosX = Mathf.Clamp(x, rect.width, Screen.width);
            m_PosY = Screen.height / 2f;
        }
        else
        {
            if (rectTransform.anchorMin.x == 1)
            {
                minX = rect.width;
                maxX = Screen.width;
            }
            else if (rectTransform.anchorMin.x == 0)
            {
                minX = 0f;
                maxX = Screen.width - rect.width;
            }
            else
            {
                minX = rect.width / 2f;
                maxX = Screen.width - minX;
            }

            if (rectTransform.anchorMin.y == 0)
            {
                minY = 0;
                maxY = Screen.height - rect.height;
            }
            else if (rectTransform.anchorMin.y == 1)
            {
                minY = rect.height;
                maxY = Screen.height;
            }
            else
            {
                minY = rect.height / 2f;
                maxY = Screen.height - (minY);
            }

            m_PosX = Mathf.Clamp(x, minX, maxX);
            m_PosY = Mathf.Clamp(y, minY, maxY);
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

    private bool AnchorsCheck(int minX, int minY, int maxX, int maxY)
    {
        return (rectTransform.anchorMin.x == minX &&
            rectTransform.anchorMin.y == minY &&
            rectTransform.anchorMax.x == maxX &&
            rectTransform.anchorMax.y == maxY);
    }
}
