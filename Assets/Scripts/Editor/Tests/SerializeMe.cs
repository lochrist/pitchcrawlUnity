
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class MyBaseClass
{
    public int m_IntField = 12;
}

[System.Serializable]
public class ChildClass : MyBaseClass
{
    public float m_FloatField = 9.0f;
}

[System.Serializable]
public class SerializeMe : ScriptableObject
{    
    public List<MyBaseClass> m_Instances;          


    public List<ChildClass> m_Children;          

    public void OnEnable ()
    {        
        if (m_Instances == null)           
            m_Instances = new List<MyBaseClass> ();

        if (m_Children == null)           
            m_Children = new List<ChildClass> ();
    }
           
}

public static class SerializeMenu {
    // [MenuItem("Assets/Create/CreateSerializeMe")]
    public static void TestXmlSerialization () {
        ScriptableObjectUtils.CreateAsset<SerializeMe> ();
    }
}

/*
[InitializeOnLoad]
//Make sure this code runs every time the editor is updated
public class RegisterCustomInspector : EditorWindow
{
    static RegisterCustomInspector ()
    {
        Debug.Log ("Called");
    }
}

*/