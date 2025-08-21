using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    XmlDocument xmlDoc;
    public void LoadDialogue(TextAsset xmlFile)
    {
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
    }
    public string GetDialogue(string lanugage, int DialogueId,out int nextDialogueID)
    {
        XmlNode targetNode = xmlDoc.SelectSingleNode("/Dialogues/dialogue[@id='" + DialogueId + "']");
        string text = targetNode["lan_" + lanugage].InnerText;
        nextDialogueID = int.Parse(targetNode["next_dialogue"].InnerText);
        return text;
        
    }
}
