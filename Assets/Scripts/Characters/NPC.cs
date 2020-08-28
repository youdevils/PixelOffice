using GameDataDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //General Data
    public int IDKEY = -1;
    private string npcname = "New";
    public string NPCName {
        get {
            return npcname;
        }
    }
    private string uri = "TBA/TBA";
    public string NPCURI {
        get {
            return uri;
        }
    }

    private void Start() {
        if(IDKEY >= 0) {
            //Is not a negative number
            if (DataMart.CheckNPC(IDKEY)) {
                NPCData data = DataMart.GetNPC(IDKEY);
                npcname = data.Name;
                uri = data.URI;
            }
        } else {
            Debug.LogError("ID is a negative number");
        }
    }
}
