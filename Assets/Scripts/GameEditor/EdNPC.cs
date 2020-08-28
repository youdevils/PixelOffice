using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameDataDictionary;

public class EdNPC : MonoBehaviour {

    //NPC Sidebar
    public Button CreateNPC;
    public TMP_InputField LoadValueField;
    public Button LoadButton;
    public Button RefreshEntriesButton;
    public Transform DatabaseEntryContainer;
    public GameObject entryPrefab;

    //NPC
    public TextMeshProUGUI ID;
    public TMP_InputField NPCName;
    public TMP_InputField PortraitURI;
    public Button Delete;
    public Button Save;

    private void Awake() {
        //Add Side Bar Events
        RefreshEntriesButton.onClick.AddListener(delegate { RefreshDatabaseEntries(); });
        LoadButton.onClick.AddListener(delegate { LoadNPC(int.Parse(LoadValueField.text)); });
    }

    public void ClearNPCUI() {
        NPCName.text = "";
        PortraitURI.text = "";
    }

    public void CreateNewNPC() {
        NPCData npcData = new NPCData();

        npcData.ID = DataMart.GetNPCID();

        ID.text = npcData.ID.ToString();

        Delete.onClick.RemoveAllListeners();
        Save.onClick.RemoveAllListeners();

        Delete.onClick.AddListener(delegate { DeleteNPC(npcData); } );
        Save.onClick.AddListener(delegate { SaveNPC(npcData); });

        ClearNPCUI();
    }

    private void ClearUI() {
        NPCName.text = "";
        PortraitURI.text = ""; 
    }

    public  void LoadNPC(int id) {
        if (DataMart.CheckNPC(id)) {
            NPCData data = DataMart.GetNPC(id);
            RefreshUI(data);
        } else {
            Debug.LogError("Cannot load, NPC not found.");
        }
    }

    public void RefreshUI(NPCData _data) {
        NPCName.text = _data.Name;
        PortraitURI.text = _data.URI;
    }

    private void DeleteNPC(NPCData _npc) {
        if (DataMart.CheckNPC(_npc.ID)) {
            DataMart.RemoveNPC(_npc);
        } else {
            Debug.LogError("NPC doesnt exist");
        }
    }

    private void SaveNPC(NPCData _npc) {
        _npc.Name = NPCName.text;
        _npc.URI = PortraitURI.text;

        if (DataMart.CheckNPC(_npc.ID)) {
            DataMart.RemoveNPC(_npc);
            DataMart.AddNPC(_npc);
        } else {
            DataMart.AddNPC(_npc);
        }

    }

    public void RefreshDatabaseEntries() {
        //Clear existing entries
        if (DatabaseEntryContainer.childCount > 0) {
            for (int step = 0; step < DatabaseEntryContainer.childCount; step++) {
                Destroy(DatabaseEntryContainer.GetChild(step).gameObject);
            }
        }

        //Read Database and Load Entries
        foreach (NPCData npc in DataMart.NPCDatabase.Values) {
            GameObject entry = Instantiate(entryPrefab, DatabaseEntryContainer);
            entry.GetComponent<TextMeshProUGUI>().text = "ID: " + npc.ID + " | Name: " + npc.Name;
            entry.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
