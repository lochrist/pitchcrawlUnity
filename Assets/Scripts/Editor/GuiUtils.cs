using UnityEngine;
using System.Collections;
using UnityEditor;

public static class GuiUtils {

    public static Color DividerColor = new Color(0f, 0f, 0f, 0.25f);
    public static GUIStyle DividerStyle = new GUIStyle()
    {    
        name = "PLYDivider",    
        border = new RectOffset(0, 0, 0, 0),    
        padding = new RectOffset(0, 0, 0, 0),    
        margin = new RectOffset(0, 0, 0, 0),    
        normal = { background = EditorGUIUtility.whiteTexture },    
    }; 
    
    public static void DrawHorizontalLine(float thickness, Color color, float paddingTop = 0f, float paddingBottom = 0f, float width = 0f)
    {    
        GUILayoutOption[] options = new GUILayoutOption[2]    
        {         
            GUILayout.ExpandHeight(false),         
            (width > 0.0f ? GUILayout.Width(width) : GUILayout.ExpandWidth(true))
            
        };    
        Color prevColor = GUI.backgroundColor;    
        GUI.backgroundColor = color;    
        GUILayoutUtility.GetRect(0f, (thickness + paddingTop + paddingBottom), options);    
        Rect r = GUILayoutUtility.GetLastRect();    
        r.y += paddingTop;   
        r.height = thickness;    
        GUI.Box(r, "", DividerStyle);    
        GUI.backgroundColor = prevColor;    
    }
    
    
    
    public static void DrawVerticalLine(float thickness, Color color, float paddingLeft = 0f, float paddingRight = 0f, float height = 0f)    
    {    
        GUILayoutOption[] options = new GUILayoutOption[2]   
        {        
            GUILayout.ExpandWidth(false),        
            (height > 0.0f ? GUILayout.Height(height) : GUILayout.ExpandHeight(true))        
        };
        
        Color prevColor = GUI.backgroundColor;    
        GUI.backgroundColor = color;    
        GUILayoutUtility.GetRect((thickness + paddingLeft + paddingRight), 0f, options);    
        Rect r = GUILayoutUtility.GetLastRect();    
        r.x += paddingLeft;    
        r.width = thickness;    
        GUI.Box(r, "", DividerStyle);    
        GUI.backgroundColor = prevColor;    
    }
}
