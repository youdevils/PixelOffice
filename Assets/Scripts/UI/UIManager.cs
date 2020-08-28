using UnityEngine;
using GameDataDictionary;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    private Sprite perceptionIcon;
    private Sprite managementIcon;
    private Sprite coWorkerIcon;
    private Sprite valueIcon;
    private Sprite unknown;

    [Header("Globals")]
    [SerializeField]
    private Color32 ValueOneColor;
    [SerializeField]
    private Color32 ValueTwoColor;
    [SerializeField]
    private Color32 ValueThreeColor;
    [SerializeField]
    private Color32 Menu1Color;
    [SerializeField]
    private Color32 Menu2Color;
    [SerializeField]
    private Color32 Menu3Color;
    [SerializeField]
    private Color32 Menu4Color;
    [SerializeField]
    private Color32 NotSelected;
    [SerializeField]
    private Color32 Selected;

    #region Conversation Variables


    [Header("Conversation UI Components")]
    [SerializeField]
    private GameObject conversationUI;
    [SerializeField]
    private Image conversationPlayerPortrait;
    [SerializeField]
    private TextMeshProUGUI conversationPlayerName;
    [SerializeField]
    private Image conversationOtherPortrait;
    [SerializeField]
    private TextMeshProUGUI conversationOtherName;
    [SerializeField]
    private TextMeshProUGUI conversationMainText;
    [SerializeField]
    private GameObject conversationOptionPrefab;
    [SerializeField]
    private Transform conversationOptionContainer;

    private int maxStepTextCharacters = -1;
    private float maxCharactersCounter = 0;
    #endregion

    #region Character Menu Variables
    [Header("Base Character Menu Components")]
    [SerializeField]
    private GameObject characterMenuUI;
    [SerializeField]
    private Button inventoryTabButton;
    [SerializeField]
    private Button performanceTabButton;
    [SerializeField]
    private Button questLogTabButton;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerJobTitle;
    [SerializeField]
    private TextMeshProUGUI playerHistory;


    #endregion

    #region Inventory Variables
    private Dictionary<int, Transform> inventoryItemUIObjects = new Dictionary<int, Transform>();
    [Header("Inventory UI Components")]
    [SerializeField]
    private Transform InventoryPanel;
    [SerializeField]
    private Transform ItemContainer;
    [SerializeField]
    private GameObject ItemPrefab;
    [SerializeField]
    private Image ItemDetailImage;
    [SerializeField]
    private TextMeshProUGUI ItemDetailName;
    [SerializeField]
    private TextMeshProUGUI ItemDetailType;
    [SerializeField]
    private TextMeshProUGUI ItemDetailDescription;
    [SerializeField]
    private Button ItemDropButton;
    private int selectedItemID = -1;
    private int selectedSlotID = -1;

    #endregion

    #region Quest Variables
    private Dictionary<int, Transform> inventoryQuestUIObjects = new Dictionary<int, Transform>();
    [Header("Quest UI Components")]
    [SerializeField]
    private Transform QuestPanel;
    [SerializeField]
    private Transform QuestContainer;
    [SerializeField]
    private GameObject QuestPrefab;
    #endregion

    #region Performance Variables
    [Header("Performance UI Components")]
    [SerializeField]
    private Transform MyStatsPanel;
    [SerializeField]
    private TextMeshProUGUI PerceptionText;
    [SerializeField]
    private TextMeshProUGUI ManagementText;
    [SerializeField]
    private TextMeshProUGUI CoWorkersText;
    #endregion

    #region Notification Variables
    [Header("Notification UI Components")]
    [Tooltip("Object that will contain all other notification UI objects")]
    [SerializeField]
    private Transform notificationUI;
    [Tooltip("Object that will contains the icon Sprite")]
    [SerializeField]
    private SpriteRenderer notificationIcon;
    [Tooltip("Object that will contains the notification Text")]
    [SerializeField]
    private TextMeshProUGUI notificationText;
    [Tooltip("Object that has the notification acknowledgement button")]
    [SerializeField]
    private Button notificationButton;
    #endregion

    #region Controller Variables
    [Header("Controller UI Components")]
    [SerializeField]
    public GameObject controllerUI;
    [SerializeField]
    public Button upButton;
    [SerializeField]
    public Button downButton;
    [SerializeField]
    public Button leftButton;
    [SerializeField]
    public Button rightButton;
    [SerializeField]
    public Button aButton;
    [SerializeField]
    public Button bButton;
    #endregion


    public UIManager() {
        if(instance != null) {
            Debug.LogError("Cannot have more than 1 UI Manager");
            return;
        }
        instance = this;
    }

    private void Start() {
        perceptionIcon = Resources.Load<Sprite>("Icons/value1");
        managementIcon = Resources.Load<Sprite>("Icons/value2");
        coWorkerIcon = Resources.Load<Sprite>("Icons/value3");
        valueIcon = Resources.Load<Sprite>("Icons/value");
        unknown = Resources.Load<Sprite>("Icons/Card/unknown");
        ResetItemDetail();
    }

    private void Update() {
        CheckMovement();

        //Run character reval code
        if(maxCharactersCounter <= maxStepTextCharacters) {
            maxCharactersCounter += Time.deltaTime * 20f;
            conversationMainText.maxVisibleCharacters = (int)maxCharactersCounter;
        }
    }

    #region Conversation Methods
    public void SetConversationEnabled(bool _val) {
        conversationUI.SetActive(_val);
        if (_val) {
            controllerUI.SetActive(false);
        } else {
            controllerUI.SetActive(true);
        }
    }

    public void SetupNewConversation(string _othername, string _otherspriteurl, string _playername) {
        SetConversationEnabled(true);
        conversationOtherName.text = _othername;
        conversationPlayerName.text = _playername;
        Sprite sprite = Resources.Load<Sprite>(_otherspriteurl);
        conversationOtherPortrait.sprite = sprite;
    }

    public void UpdateConversation(string _maintext) {
        conversationMainText.text = _maintext;
        TMP_TextInfo info = conversationMainText.GetTextInfo(_maintext);
        maxStepTextCharacters = info.characterCount;
        maxCharactersCounter = 0;
        Transform[] oldOptions = conversationOptionContainer.GetComponentsInChildren<Transform>();
        for (int i = 1; i < oldOptions.Length; i++) {
            Destroy(oldOptions[i].gameObject);
        }
    }

    public void AddConversationResponse(string _responsetext, int _responseID, CompanyValue _trait, OptionTrigger _type, Quest _quest) {
        GameObject btn = Instantiate(conversationOptionPrefab, conversationOptionContainer) as GameObject;
        btn.GetComponentInChildren<TextMeshProUGUI>().text = _responsetext;
        btn.GetComponent<Button>().onClick.AddListener(delegate { ConversationManager.instance.NextConversationStep(_responseID); });
        if(_type == OptionTrigger.QuestAcceptance) {
            btn.GetComponent<Button>().onClick.AddListener(delegate { _quest.Accept(); });
        }else if(_type == OptionTrigger.ObtainQuestReward) {
            btn.GetComponent<Button>().onClick.AddListener(delegate { _quest.ClaimReward(); });
        }
        switch (_trait) {
            case CompanyValue.NULL:
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
                break;
            case CompanyValue.ValueOne:
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = perceptionIcon;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = ValueOneColor;
                break;
            case CompanyValue.ValueTwo:
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = managementIcon;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = ValueTwoColor;
                break;
            case CompanyValue.ValueThree:
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = true;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = coWorkerIcon;
                btn.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = ValueThreeColor;
                break;
            default:
                Debug.LogError("Unknown Trait Type");
                break;
        }
    }
    #endregion

    #region Character Menu Methods
    public void ToggleCharacterMenu() {
        if (characterMenuUI.activeSelf) {
            characterMenuUI.SetActive(false);
            SetControllerUIInteractable(true);
            GameStateManager.instance.SetState(GameStateManager.GameState.Playing);
        } else {
            characterMenuUI.SetActive(true);
            SetControllerUIInteractable(false);
            GameStateManager.instance.SetState(GameStateManager.GameState.DisplayingUI);
        }
    }
    public void SetCharacterMenuPlayerDetails(PlayerData _data) {
        Sprite sprite = Resources.Load<Sprite>(_data.PortraitURI);
        playerImage.sprite = sprite;
        playerName.text = _data.Name;
        playerJobTitle.text = _data.JobTitle;
        playerHistory.text = _data.BackStory;
        PerceptionText.text = _data.CompanyValueOne.ToString();
        ManagementText.text = _data.CompanyValueTwo.ToString();
        CoWorkersText.text = _data.CompanyValueThree.ToString();
    }

    public void SetInventoryTabActive() {
        performanceTabButton.GetComponent<Image>().color = Menu2Color;
        performanceTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        questLogTabButton.GetComponent<Image>().color = Menu2Color;
        questLogTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        inventoryTabButton.GetComponent<Image>().color = Menu4Color;
        inventoryTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu2Color;
        InventoryPanel.gameObject.SetActive(true);
        MyStatsPanel.gameObject.SetActive(false);
        QuestPanel.gameObject.SetActive(false);

    }
    public void SetPerformanceTabActive() {
        performanceTabButton.GetComponent<Image>().color = Menu4Color;
        performanceTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu2Color;
        questLogTabButton.GetComponent<Image>().color = Menu2Color;
        questLogTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        inventoryTabButton.GetComponent<Image>().color = Menu2Color;
        inventoryTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        InventoryPanel.gameObject.SetActive(false);
        MyStatsPanel.gameObject.SetActive(true);
        QuestPanel.gameObject.SetActive(false);
    }
    public void SetQuestTabActive() {
        performanceTabButton.GetComponent<Image>().color = Menu2Color;
        performanceTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        questLogTabButton.GetComponent<Image>().color = Menu4Color;
        questLogTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu2Color;
        inventoryTabButton.GetComponent<Image>().color = Menu2Color;
        inventoryTabButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Menu4Color;
        InventoryPanel.gameObject.SetActive(false);
        MyStatsPanel.gameObject.SetActive(false);
        QuestPanel.gameObject.SetActive(true);
    }
    #endregion

    #region Inventory Methods
    public void AddInventoryItem(ItemData _item, int _slot) {
        GameObject inventoryitem = Instantiate(ItemPrefab) as GameObject;
        inventoryitem.transform.SetParent(ItemContainer);
        Sprite sprite = Resources.Load<Sprite>(_item.SpriteURI);
        if (sprite != null) {
            inventoryitem.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        } else {
            Debug.LogError("Item Sprite File Not Found: " + _item.SpriteURI);
        }
        inventoryitem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _item.Name;
        inventoryitem.transform.GetChild(2).GetComponent<Image>().sprite = valueIcon;
        inventoryitem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _item.Value.ToString();
        inventoryitem.GetComponent<Button>().onClick.AddListener(delegate { OpenItemDetail(_item.ID, _slot); });
        inventoryitem.transform.localScale = new Vector3(1, 1, 1);
        OpenItemDetail(_item.ID, _slot);
        inventoryItemUIObjects.Add(_slot, inventoryitem.transform);
    }

    public void RemoveItemFromSlot(int _slotID) {
        if (inventoryItemUIObjects.ContainsKey(_slotID)) {

            Destroy(inventoryItemUIObjects[_slotID].gameObject);

            inventoryItemUIObjects.Remove(_slotID);
            if(_slotID == selectedSlotID) {
                selectedItemID = -1;
                selectedSlotID = -1;
            }
            ResetItemDetail();
        } else
            Debug.LogError("Cant remove item, doesnt exist." + _slotID);
    }

    public void ClearAllItemObjects() {
        for (int i = 0; i < ItemContainer.childCount; i++) {
            Destroy(ItemContainer.transform.GetChild(i).gameObject);
        }
        inventoryItemUIObjects.Clear();
    }

    public void OpenItemDetail(int _itemID, int _slotID) {
        ItemData _data = DataMart.GetItem(_itemID);
        Sprite sprite = Resources.Load<Sprite>(_data.SpriteURI);
        ResetItemDetail();
        ItemDetailDescription.text = _data.Description;
        ItemDetailImage.sprite = sprite;
        ItemDetailName.text = _data.Name;
        ItemDetailType.text = _data.Type.ToString();
        if(_data.Type == ItemData.ItemType.Valuable) {
            ItemDropButton.interactable = true;
            ItemDropButton.onClick.AddListener(delegate { Player.instance.RemoveInventoryItem(_slotID); });
        } else if (_data.Type == ItemData.ItemType.Quest) {
        } else if (_data.Type == ItemData.ItemType.KeyItem) {
        }

    }

    private void ResetItemDetail() {
        ItemDetailImage.sprite = unknown;
        ItemDetailDescription.text = "";
        ItemDetailName.text = "";
        ItemDetailType.text = "";
        ItemDropButton.onClick.RemoveAllListeners();
        ItemDropButton.interactable = false;
    }
    #endregion

    #region Notification Methods
    private void SetNotificationEnabled(bool _val) {
        if (_val) {
            GameStateManager.instance.SetState(GameStateManager.GameState.DisplayingUI);
            controllerUI.SetActive(false);
        } else { 
            GameStateManager.instance.SetState(GameStateManager.GameState.Playing);
            controllerUI.SetActive(true);
        }
        notificationUI.gameObject.SetActive(_val);
    }

    public void PostNotification(string _iconuri, string _text) {
        notificationButton.onClick.RemoveAllListeners();
        Sprite icon = Resources.Load<Sprite>(_iconuri);
        if(icon == null) {
            Debug.LogError("Sprite not loaded.");
            return;
        }
        notificationIcon.sprite = icon;
        notificationText.text = _text; 
        notificationButton.onClick.AddListener(delegate { SetNotificationEnabled(false); });
        SetNotificationEnabled(true);
    }

    public void PostNotification(string _text) {
        notificationButton.onClick.RemoveAllListeners();
        notificationIcon.sprite = null;
        notificationText.text = _text;
        notificationButton.onClick.AddListener(delegate { SetNotificationEnabled(false); });
        SetNotificationEnabled(true);
    }

    #endregion

    #region Controller Methods
    private void SetControllerUIInteractable(bool _val) {
        if (_val) {
            leftButton.interactable = true;
            rightButton.interactable = true;
            upButton.interactable = true;
            downButton.interactable = true;
            aButton.interactable = true;
            bButton.interactable = true;
        } else {
            leftButton.interactable = false;
            rightButton.interactable = false;
            upButton.interactable = false;
            downButton.interactable = false;
            aButton.interactable = false;
            bButton.interactable = false;
        }
    }
    private void CheckMovement() {
        if (GameStateManager.instance.GetState != GameStateManager.GameState.Playing)
            return;

        if (!upButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed &&
            !downButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed &&
            !leftButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed &&
            !rightButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed) {
            Player.instance.StopAllMovementAnimation();
        } else {
            if (upButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed) {
                Player.instance.MoveUp();
            } else if (downButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed) {
                Player.instance.MoveDown();
            } else if (leftButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed) {
                Player.instance.MoveLeft();
            } else if (rightButton.GetComponent<Button_HeldBehaviour>().ButtonIsPressed) {
                Player.instance.MoveRight();
            }
        }
    }
    #endregion
}
