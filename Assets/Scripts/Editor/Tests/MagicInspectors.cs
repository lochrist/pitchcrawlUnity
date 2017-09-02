using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

/*
[InitializeOnLoad]
//Make sure this code runs every time the editor is updated
public class MagicInspectors : EditorWindow
{
    static MagicInspectors ()
    {
        //Get access to the UnityEditor assembly
        var asm = Assembly.GetAssembly (typeof(UnityEditor.CustomEditor));
        //Use Linq to find the CustomEditorAttribute type
        var cea = asm.GetTypes ().FirstOrDefault (t => t.Name == "CustomEditorAttributes");
        //Get access to the method that is called to find a custom editor for a type - this 
        //caches the results, so it has to happen before we play with the lists
        var findCustomEditor = cea.GetMethod ("FindCustomEditorType", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        //Call it
        findCustomEditor.Invoke (null, new object [] {
            new UnityEngine.Object (),
            false
        });
        //Find the MonoEditorType class so we can make instances of it later
        var next = asm.GetTypes ().FirstOrDefault (t => t.Name .Contains ("MonoEditorType"));
        var inst = Activator.CreateInstance (next);
        //Get the field in that class which is the type of the inspector to use
        var inspectorType = next.GetField ("inspectorType");
        //Get the field in that class which is the type to inspect using this inspector
        var inspectedType = next.GetField ("inspectedType");
        //Get the custom editors field which is the cache in CustomEditorAttribute
        var editorsField = cea.GetField ("m_CustomEditors", BindingFlags.Static | BindingFlags.NonPublic);
        //Get the current list (it's an ArrayList)
        var editors = editorsField.GetValue (null) as ArrayList;
        //Get the current list of multi item editors
        var multiEditorsField = cea.GetField ("m_CustomMultiEditors", BindingFlags.Static | BindingFlags.NonPublic);
        var multiEditors = multiEditorsField.GetValue (null) as ArrayList;
        
        //Now its time to get all of the inspectors we've defined
        //Get all of the current assemblies loaded
        var types = AppDomain.CurrentDomain
            .GetAssemblies ()
        //Get all of the types in those assemblies
                .SelectMany (a => a.GetTypes ())
        //Which have a CustomEditor attribute
                .Where (t => t.IsDefined (typeof(CustomEditor), true))
        //Get the type that this CustomEditor edits
                .Select (t => new { editor = t, inspected = (Attribute.GetCustomAttribute (t, typeof(CustomEditor), false) as CustomEditor).m_InspectedType}).ToList ();
        
        //Now look for types that are the type edited or its subclasses
        var usableTypes = AppDomain.CurrentDomain
            .GetAssemblies ()
                .SelectMany (a => a.GetTypes ())
        //For all of the types, get the custom inspector for which they are assignable
        //In other words find an inspector (or null) for which this type can be downcast to
                .Select (t => new { editable = t, custom = types.FirstOrDefault (e => e.inspected.IsAssignableFrom (t)) })
        //Make sure we only have valid ones
                .Where (r => r.custom != null);
        
        //Now update the internal cache with these new types
        
        foreach (var newEditor in usableTypes) {
            //Create a new instance of the internal structure that represents the relationship
            var editorInstance = Activator.CreateInstance (next);
            //tell it which inspector to use
            inspectorType.SetValue (editorInstance, newEditor.custom.editor);
            //tell it which type to inspect from the list we made above
            inspectedType.SetValue (editorInstance, newEditor.editable);
            //Add it to multiEditors if it supports it
            if (newEditor.custom.editor.GetType ().IsDefined (typeof(CanEditMultipleObjects), false))
                multiEditors.Add (editorInstance);
            //Add it to ordinary editors always
            editors.Add (editorInstance);
        }
        
        
    }
    
}
*/