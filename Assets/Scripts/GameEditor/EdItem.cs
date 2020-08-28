using GameDataDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EdItem : MonoBehaviour {

    //Item Sidebar
    public Button CreateItem;
    public TMP_InputField LoadValueField;
    public Button LoadButton;
    public Button RefreshEntriesButton;
    public Transform DatabaseEntryContainer;
    public GameObject entryPrefab;

    //Item Window
    public TextMeshProUGUI ID;
    public Button SaveButton;
    public Button DeleteButton;
    public TMP_InputField NameField;
    public TMP_InputField DescriptionField;
    public TMP_InputField ValueField;
    public TMP_InputField SpriteURIField;
    public TMP_Dropdown TypeField;

    private void Awake() {

        //Add Side Bar Events
        RefreshEntriesButton.onClick.AddListener(delegate { RefreshDatabaseEntries(); });
        LoadButton.onClick.AddListener(delegate { LoadItem(int.Parse(LoadValueField.text)); });
    }

    public void CreateNewItem() {
        //Create new Item
        ItemData itemData = new ItemData();

        //Get an ID 
        itemData.ID = DataMart.GetItemID();

        //Set UI - ID
        ID.text = itemData.ID.ToString();

        //Clear listeners
        SaveButton.onClick.RemoveAllListeners();
        DeleteButton.onClick.RemoveAllListeners();

        //Setup Save button
        SaveButton.onClick.AddListener(delegate { SaveItem(itemData); });

        //Setup Delete button
        DeleteButton.onClick.AddListener(delegate { DeleteItem(itemData); });

        ClearItemUI();
    }

   
    private void ClearItemUI() {
        NameField.text = "";
        DescriptionField.text = "";
        ValueField.text = "0";
        SpriteURIField.text = "";
        TypeField.value = 0;
    }

    private void RefreshItemUI(ItemData _item) {
        //Clear all UI event listeners
        SaveButton.onClick.RemoveAllListeners();
        DeleteButton.onClick.RemoveAllListeners();

        //Add event to Save Item Button
        SaveButton.onClick.AddListener(delegate { SaveItem(_item); });
        
        //Setup Delete button
        DeleteButton.onClick.AddListener(delegate { DeleteItem(_item); });

        //Set Item ID
        ID.text = _item.ID.ToString();

        //Set Item Name
        NameField.text = _item.Name;

        //Set Item Description
        DescriptionField.text = _item.Description;

        //Set Item Value
        ValueField.text = _item.Value.ToString();
        
        //Set Item Sprite URI
        SpriteURIField.text = _item.SpriteURI;

        switch (_item.Type) {
            case ItemData.ItemType.Valuable:
                TypeField.value = 0;
                break;
            case ItemData.ItemType.Quest:
                TypeField.value = 1;
                break;
            case ItemData.ItemType.KeyItem:
                TypeField.value = 2;
                break;
            default:
                Debug.LogError("Unknown Item Type.");
                break;
        }
    }

    public void ItemHasChanged() {
        SaveButton.GetComponent<Image>().color = Color.red;
    }

    public void RefreshDatabaseEntries() {
        //Clear existing entries
        if (DatabaseEntryContainer.childCount > 0) {
            for (int step = 0; step < DatabaseEntryContainer.childCount; step++) {
                Destroy(DatabaseEntryContainer.GetChild(step).gameObject);
            }
        }

        //Read Database and Load Entries
        foreach (ItemData item in DataMart.ItemDatabase.Values) {
            GameObject entry = Instantiate(entryPrefab, DatabaseEntryContainer);
            entry.GetComponent<TextMeshProUGUI>().text = "ID: " + item.ID + " | Name: " + item.Name;
            entry.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SaveItem(ItemData itemData) {

        //Set Item Name
        itemData.Name = NameField.text;

        //Set Item Description
        itemData.Description = DescriptionField.text;

        //Set Item Value
        itemData.Value = int.Parse(ValueField.text);

        //Set Item Sprite URI
        itemData.SpriteURI = SpriteURIField.text;

        switch (TypeField.value) {
            case 0:
                itemData.Type = ItemData.ItemType.Valuable;
                break;
            case 1:
                itemData.Type = ItemData.ItemType.Quest;
                break;
            case 2:
                itemData.Type = ItemData.ItemType.KeyItem;
                break;
            default:
                Debug.LogError("Unknown Item Type.");
                break;
        }


        if (DataMart.CheckItemDataBase(itemData.ID)) {
            DataMart.RemoveItem(itemData);
        }
        DataMart.AddItem(itemData);
        SaveButton.GetComponent<Image>().color = Color.green;
    }

    public void LoadItem(int id) {
        if (DataMart.CheckItemDataBase(id)) {
            ItemData data = DataMart.GetItem(id);

            RefreshItemUI(data);
        } else {
            Debug.LogError("Load Error: No item with that ID");
        }
    }

    public void DeleteItem(ItemData item) {
        if (DataMart.CheckItemDataBase(item.ID)) {
            DataMart.RemoveItem(item);
            ClearItemUI();
        } else {
            Debug.LogError("Delete Error: No item with that ID");
        }
    }
}


