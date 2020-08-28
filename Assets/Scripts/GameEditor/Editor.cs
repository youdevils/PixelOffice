using System.Collections;
using GameDataDictionary;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{

    [Header("Colour Scheme")]
    public Color Base1; //4558CC
    public Color Base2; //383E3F
    public Color Highlight1; //47AB56
    public Color Highlight2; //F5AF00

    [Header("Tab Menu Buttons")]
    public Button LoadDatabaseButton;
    public Button SaveDatabaseButton;
    public Button ConversationsButton;
    public Button ItemsButton;
    public Button NPCButton;
    public Button QuestsButton;

    [Header("Windows and Containers")]
    public Transform ConversationWindow;
    public Transform ItemsWindow;
    public Transform NPCWindow;
    public Transform QuestWindow;

    private void Awake() {
        //Load Conversation Files Into Database
        StartCoroutine(DataMart.LoadAllDatabasesFromFile());

        //Add Tab Bar Events
        LoadDatabaseButton.onClick.AddListener(delegate { StartCoroutine(DataMart.LoadAllDatabasesFromFile()); });
        SaveDatabaseButton.onClick.AddListener(delegate { StartCoroutine(DataMart.SaveAllDatabasesToFile()); });
    }

    public void SetConversationsActive() {
        //Set window active.
        ConversationWindow.gameObject.SetActive(true);

        //Set all other windows false
        ItemsWindow.gameObject.SetActive(false);
        NPCWindow.gameObject.SetActive(false);
        QuestWindow.gameObject.SetActive(false);
    }

    public void SetItemsActive() {
        //Set window active.
        ItemsWindow.gameObject.SetActive(true);

        //Set all other windows false
        ConversationWindow.gameObject.SetActive(false);
        NPCWindow.gameObject.SetActive(false);
        QuestWindow.gameObject.SetActive(false);
    }

    public void SetNPCActive() {
        //Set window active.
        NPCWindow.gameObject.SetActive(true);

        //Set all other windows false
        ItemsWindow.gameObject.SetActive(false);
        ConversationWindow.gameObject.SetActive(false);
        QuestWindow.gameObject.SetActive(false);
    }

    public void SetQuestActive() {
        //Set window active.
        QuestWindow.gameObject.SetActive(true);

        //Set all other windows false
        ItemsWindow.gameObject.SetActive(false);
        NPCWindow.gameObject.SetActive(false);
        ConversationWindow.gameObject.SetActive(false);
    }
}
