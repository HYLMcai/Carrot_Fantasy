using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XmlTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void XmlTest()
    {
        XmlDocument doc = new XmlDocument();
        XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(declaration);

        XmlElement store = doc.CreateElement("Sotre");
        XmlNode root = doc.AppendChild(store);

        XmlElement shelf1 = doc.CreateElement("shelf1");
        shelf1.InnerText = "¿ÉÀÖ";
        shelf1.SetAttribute("X", "1");
        store.AppendChild(shelf1);

        XmlElement shelf2 = doc.CreateElement("shelf2");
        shelf2.InnerText = "±ùºì²è";
        shelf2.SetAttribute("Y", "2");
        store.AppendChild(shelf2);
        
        doc.Save("123.xml");
    }
}
