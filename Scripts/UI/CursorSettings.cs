using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSettings : MonoBehaviour
{
    public enum CursorType
    {
        Default,
        Combat,
    }

    [System.Serializable]
    struct CursorMapping
    {
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [SerializeField] private CursorMapping[] m_CursorMapping = null;

    // Start is called before the first frame update
    void Start()
    {
        SetCursor(CursorType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetMouseRay(), out hit))
        {
            if(!hit.collider.gameObject.tag.Equals("Enemy"))
                SetCursor(CursorType.Default);
            else SetCursor(CursorType.Combat);
        }
    }

    public void SetCursor(CursorType type)
    {
        CursorMapping mapping = GetCursorMapping(type);
        Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type)
    {
        foreach (CursorMapping mapping in m_CursorMapping)
        {
            if (mapping.type == type)
            {
                return mapping;
            }
        }

        return m_CursorMapping[0];
    }

    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}