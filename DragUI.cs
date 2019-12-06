using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float m_X;
    private float m_Y;
    private float m_PosX;
    private float m_PosY;
    private float m_MinX;
    private float m_MaxX;
    private float m_MinY;
    private float m_MaxY;

    private Vector3 m_PositionCorrection;
    private Vector2 m_BeginDragMousePosition;
    private Vector2 m_BeginDragObjectPosition;

    private Rect m_Rect;
    private RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = this.transform.GetComponent<RectTransform>();
        m_Rect = m_RectTransform.rect;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_BeginDragObjectPosition = m_RectTransform.position;
        m_BeginDragMousePosition = eventData.pressPosition;
        m_PositionCorrection = m_BeginDragMousePosition - m_BeginDragObjectPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_X = eventData.position.x - m_PositionCorrection.x;
        m_Y = eventData.position.y - m_PositionCorrection.y;

        if (AnchorsCheck(0, 1, 1, 1)) // TOP
        {
            m_PosX = Screen.width / 2f;
            m_PosY = Mathf.Clamp(m_Y, m_Rect.height, Screen.height);
        }
        else if (AnchorsCheck(0, 0, 1, 0)) // BOTTOM
        {
            m_PosX = Screen.width / 2f;
            m_PosY = Mathf.Clamp(m_Y, 0f, Screen.height - m_Rect.height);
        }
        else if (AnchorsCheck(0, 0, 0, 1)) // LEFT
        {
            m_PosX = Mathf.Clamp(m_X, 0f, Screen.width - m_Rect.width);
            m_PosY = Screen.height / 2f;
        }
        else if (AnchorsCheck(1, 0, 1, 1)) // RIGHT
        {
            m_PosX = Mathf.Clamp(m_X, m_Rect.width, Screen.width);
            m_PosY = Screen.height / 2f;
        }
        else // CENTER
        {
            if (m_RectTransform.anchorMin.x == 1)
            {
                m_MinX = m_Rect.width;
                m_MaxX = Screen.width;
            }
            else if (m_RectTransform.anchorMin.x == 0)
            {
                m_MinX = 0f;
                m_MaxX = Screen.width - m_Rect.width;
            }
            else
            {
                m_MinX = m_Rect.width / 2f;
                m_MaxX = Screen.width - m_MinX;
            }

            if (m_RectTransform.anchorMin.y == 0)
            {
                m_MinY = 0;
                m_MaxY = Screen.height - m_Rect.height;
            }
            else if (m_RectTransform.anchorMin.y == 1)
            {
                m_MinY = m_Rect.height;
                m_MaxY = Screen.height;
            }
            else
            {
                m_MinY = m_Rect.height / 2f;
                m_MaxY = Screen.height - (m_MinY);
            }

            m_PosX = Mathf.Clamp(m_X, m_MinX, m_MaxX);
            m_PosY = Mathf.Clamp(m_Y, m_MinY, m_MaxY);
        }

        transform.position = new Vector2(m_PosX, m_PosY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    private bool AnchorsCheck(int minX, int minY, int maxX, int maxY)
    {
        return (m_RectTransform.anchorMin.x == minX &&
            m_RectTransform.anchorMin.y == minY &&
            m_RectTransform.anchorMax.x == maxX &&
            m_RectTransform.anchorMax.y == maxY);
    }
}