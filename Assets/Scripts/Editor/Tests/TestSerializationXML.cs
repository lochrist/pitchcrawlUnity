using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;


public class Foin {
    public int i;
}

public class DerivedFoin : Foin {
    public float pow;
}

public class Pow {
    // [XmlArray("list")]
    // [XmlArrayItem("Foin", typeof(Foin))]
    // [XmlArrayItem("DerivedFoin", typeof(DerivedFoin))]
    public List<Foin> list = new List<Foin> ();
}


static public class TestSerializationXML {

    // [MenuItem("Assets/Create/TestSerialization")]
    public static void TestXmlSerialization () {
        
        Pow p = new Pow ();
        p.list.Add (new Foin());
        p.list.Add (new DerivedFoin());
        
        // Create XmlAttributeOverrides and XmlAttributes objects.
        XmlAttributeOverrides xOver = new XmlAttributeOverrides();
        XmlAttributes xAttrs = new XmlAttributes();
        
        // Add an override for the XmlArrayItem.    
        
        XmlArrayItemAttribute xArrayItem = 
            new XmlArrayItemAttribute(typeof(Foin));
        xAttrs.XmlArrayItems.Add(xArrayItem);
        
        
        // Add a second override.
        XmlArrayItemAttribute xArrayItem2 = 
            new XmlArrayItemAttribute(typeof(DerivedFoin));
        xAttrs.XmlArrayItems.Add(xArrayItem2);
        
        // Add all overrides to XmlAttribueOverrides object.
        xOver.Add(typeof(Pow), "list", xAttrs);
        
        XmlSerializer serializer = new XmlSerializer(p.GetType(), xOver/*, attrOverrides*/);
        TextWriter textWriter = new StreamWriter(@"C:\Users\lochrist\Documents\work\PitchCrawlReboot\Assets\testSerialization.xml");
        
        serializer.Serialize(textWriter, p);
        textWriter.Close();
    }
}
