using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Texture2D cursor_texture;
    [SerializeField] private Vector2 cursor_hotspot = Vector2.zero;
    void Start()
    {
        Cursor.SetCursor(cursor_texture, cursor_hotspot, CursorMode.Auto);
    }
}
