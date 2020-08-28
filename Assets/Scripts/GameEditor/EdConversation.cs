using GameDataDictionary;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EdConversation : MonoBehaviour {

    //Conversation Sidebar
    public Button CreateConversation;
    public TMP_InputField LoadValueField;
    public Button LoadButton;
    public Button RefreshEntriesButton;

    //Conversation Window
    public TMP_InputField NPCID;
    public TextMeshProUGUI ID;
    public Button CreateStep;
    public Button SaveButton;
    public Button DeleteButton;

    //Conversation Prefabs
    public GameObject StepPrefab;
    public GameObject OptionPrefab;
    public GameObject EntryPrefab;

    //Conversation Transforms (Containers)
    public Transform StepContainer;
    public Transform DatabaseEntryContainer;

    

    private void Awake() {

        //Add Side Bar Events
        RefreshEntriesButton.onClick.AddListener(delegate { RefreshDatabaseEntries(); });
        LoadButton.onClick.AddListener(delegate { LoadConversation(int.Parse(LoadValueField.text)); });
    }

    private void DeleteConversation(ConversationData conversationData) {
        if (DataMart.CheckConversationDataBase(conversationData.ID)) {
            DataMart.RemoveConversation(conversationData.ID);
        }
        DestroyAllChildUIElements();
    }

    public void CreateNewConvesation() {
        //Create new conversation data class
        ConversationData conversationData = new ConversationData();
        conversationData.StepData = new List<StepData>();

        //Get an ID for conversation
        conversationData.ID = DataMart.GetConversationID();

        //Set any default values for the new conversation
        conversationData.NPCID = -1;

        //Set Conversation UI Field(s) with data from the new conversation
        ID.text = conversationData.ID.ToString();
        NPCID.text = conversationData.NPCID.ToString();

        //Clear any old UI elements
        DestroyAllChildUIElements();

        //Clear listeners
        CreateStep.onClick.RemoveAllListeners();
        SaveButton.onClick.RemoveAllListeners();
        DeleteButton.onClick.RemoveAllListeners();

        //Setup Create Step button
        CreateStep.onClick.AddListener(delegate { CreateNewStep(conversationData); });

        //Setup Save button
        SaveButton.onClick.AddListener(delegate { SaveConversation(conversationData); });

        //Delete button
        DeleteButton.onClick.AddListener(delegate { DeleteConversation(conversationData); });
    }

    private void CreateNewStep(ConversationData _conversationData) {
        //Create new step data class
        StepData stepData = new StepData();

        stepData.OptionData = new List<OptionData>();

        //Set default values
        stepData.Text = "Step Conversation Text..";

        //Create Step UI Object
        GameObject stepPrefab = Instantiate(StepPrefab, StepContainer);

        //Step Conponent Transforms (for easy reference)
        Transform optionContainer = stepPrefab.transform.GetChild(3).GetChild(0).GetChild(0).transform;
        Transform fieldStepText = stepPrefab.transform.GetChild(2);
        Transform fieldStepID = stepPrefab.transform.GetChild(0);
        Transform fieldStepDeleteButton = stepPrefab.transform.GetChild(4);
        Transform fieldStepCreatOptionButton = stepPrefab.transform.GetChild(1);

        //Set Step ID
        fieldStepID.GetComponent<TextMeshProUGUI>().text = "ID: " + stepPrefab.transform.GetSiblingIndex().ToString();

        //Setup Create Option button
        fieldStepCreatOptionButton.GetComponent<Button>().onClick.AddListener(delegate { CreateNewOption(_conversationData, stepData, optionContainer); });

        //Setup Delete Step button
        fieldStepDeleteButton.GetComponent<Button>().onClick.AddListener(delegate { DeleteStep(stepPrefab.gameObject, _conversationData, stepData); });

        //Set Step Text
        fieldStepText.GetComponent<TMP_InputField>().text = stepData.Text;

        //Add new step to conversations step list
        _conversationData.StepData.Add(stepData);

        //Call Create Option (All Steps should have at least 1)
        CreateNewOption(_conversationData, stepData, optionContainer);

    }

    private void DeleteStep(GameObject _object, ConversationData _conversation, StepData _step) {
        if (_conversation.StepData.Contains(_step)) {
            _conversation.StepData.Remove(_step);
            Destroy(_object);
        } else {
            Debug.LogError("No step found. Cannot delete.");
        }
    }

    private void CreateNewOption(ConversationData conversationData, StepData stepdata, Transform parent) {
        //Create Option data class
        OptionData optionData = new OptionData();

        //Create Option UI Object
        GameObject optionPrefab = Instantiate(OptionPrefab, parent);

        //Option Component Transforms (for easy reference)
        Transform fieldDestinationInput = optionPrefab.transform.GetChild(5);
        Transform fieldTextInput = optionPrefab.transform.GetChild(4);
        Transform fieldValueInput = optionPrefab.transform.GetChild(6);
        Transform fieldDeleteButton = optionPrefab.transform.GetChild(7);

        //Set Option Text
        fieldTextInput.GetComponent<TMP_InputField>().text = "Response Text..";

        //Set Option Destination
        fieldDestinationInput.GetComponent<TMP_InputField>().text = "-1";

        //Set Option Value Amount
        fieldValueInput.GetComponent<TMP_InputField>().text = "-1";

        //Setup Delete button
        fieldDeleteButton.GetComponent<Button>().onClick.AddListener(delegate { DeleteOption(optionPrefab.gameObject, stepdata, optionData); });

        //Add Option to Step
        stepdata.OptionData.Add(optionData);
    }

    private void DeleteOption(GameObject _object, StepData _step, OptionData _option) {
        if (_step.OptionData.Contains(_option)) {
            _step.OptionData.Remove(_option);
            Destroy(_object);
        } else {
            Debug.LogError("No option found. Cannot delete.");
        }
    }

    private void DestroyAllChildUIElements() {

        if (StepContainer.childCount > 0) {
            for (int step = 0; step < StepContainer.childCount; step++) {
                for (int option = 0; option < StepContainer.GetChild(step).GetChild(3).GetChild(0).GetChild(0).childCount; option++) {
                    Destroy(StepContainer.GetChild(step).GetChild(3).GetChild(0).GetChild(0).GetChild(option).gameObject);
                }
                Destroy(StepContainer.GetChild(step).gameObject);
            }
        }
    }

    private void RefreshConversationUI(ConversationData _conversation) {
        //Destroy existing UI Elements
        DestroyAllChildUIElements();

        //Clear all UI event listeners
        CreateStep.onClick.RemoveAllListeners();
        SaveButton.onClick.RemoveAllListeners();
        DeleteButton.onClick.RemoveAllListeners();

        //Add event to Create Step Button
        CreateStep.onClick.AddListener(delegate { CreateNewStep(_conversation); });

        //Add event to Save Conversation Button
        SaveButton.onClick.AddListener(delegate { SaveConversation(_conversation); });

        //Delete button
        DeleteButton.onClick.AddListener(delegate { DeleteConversation(_conversation); });

        //Set conversation IDs from data
        ID.text = _conversation.ID.ToString();
        NPCID.text = _conversation.NPCID.ToString();

        foreach (StepData _step in _conversation.StepData) {
            //Create new Step UI Object
            GameObject stepPrefab = Instantiate(StepPrefab, StepContainer);

            //Step Conponent Transforms (for easy reference)
            Transform optionContainer = stepPrefab.transform.GetChild(3).GetChild(0).GetChild(0).transform;
            Transform fieldStepText = stepPrefab.transform.GetChild(2);
            Transform fieldStepID = stepPrefab.transform.GetChild(0);
            Transform fieldStepDeleteButton = stepPrefab.transform.GetChild(4);
            Transform fieldStepCreatOptionButton = stepPrefab.transform.GetChild(1);

            //Set the UI Step ID
            fieldStepID.GetComponent<TextMeshProUGUI>().text = "ID: " + stepPrefab.transform.GetSiblingIndex();

            //Set the UI Step Text
            fieldStepText.GetComponent<TMP_InputField>().text = _step.Text;

            //Setup Delete Step button
            fieldStepDeleteButton.GetComponent<Button>().onClick.AddListener(delegate { DeleteStep(stepPrefab.gameObject, _conversation, _step); });

            //Setup Create Option button
            fieldStepCreatOptionButton.GetComponent<Button>().onClick.AddListener(delegate { CreateNewOption(_conversation, _step, optionContainer); });

            foreach (OptionData _option in _step.OptionData) {
                //Create new Step UI Object
                GameObject optionPrefab = Instantiate(OptionPrefab, optionContainer);

                //Option Component Transforms (for easy reference)
                Transform fieldDestinationInput = optionPrefab.transform.GetChild(5);
                Transform fieldTextInput = optionPrefab.transform.GetChild(4);
                Transform fieldValueDropdown = optionPrefab.transform.GetChild(2);
                Transform fieldValueInput = optionPrefab.transform.GetChild(6);
                Transform fieldTriggerDropdown = optionPrefab.transform.GetChild(0);
                Transform fieldDeleteButton = optionPrefab.transform.GetChild(7);

                //Set the UI Option Text
                fieldTextInput.GetComponent<TMP_InputField>().text = _option.Text;

                //Set the UI Option Destination ID
                fieldDestinationInput.GetComponent<TMP_InputField>().text = _option.DestinationID.ToString();

                //Set the UI Option Value Check Amount
                fieldValueInput.GetComponent<TMP_InputField>().text = _option.ValueCheckAmount.ToString();

                //Setup Delete Option button
                fieldDeleteButton.GetComponent<Button>().onClick.AddListener(delegate { DeleteOption(optionPrefab.gameObject, _step, _option); });

                //Set the UI Option Tigger
                switch (_option.Trigger) {
                    case OptionTrigger.None:
                        fieldTriggerDropdown.GetComponent<TMP_Dropdown>().value = 2;
                        break;
                    case OptionTrigger.ObtainQuestReward:
                        fieldTriggerDropdown.GetComponent<TMP_Dropdown>().value = 1;
                        break;
                    case OptionTrigger.QuestAcceptance:
                        fieldTriggerDropdown.GetComponent<TMP_Dropdown>().value = 0;
                        break;
                }

                //Set the UI Option Check
                switch (_option.ValueCheck) {
                    case CompanyValue.NULL:
                        fieldValueDropdown.GetComponent<TMP_Dropdown>().value = 3;
                        break;
                    case CompanyValue.ValueOne:
                        fieldValueDropdown.GetComponent<TMP_Dropdown>().value = 0;
                        break;
                    case CompanyValue.ValueTwo:
                        fieldValueDropdown.GetComponent<TMP_Dropdown>().value = 1;
                        break;
                    case CompanyValue.ValueThree:
                        fieldValueDropdown.GetComponent<TMP_Dropdown>().value = 2;
                        break;
                }
            }
        }
    }

    public void ConversationHasChange() {
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
        foreach(ConversationData conversation in DataMart.ConversationDatabase.Values) {
            GameObject entryPrefab = Instantiate(EntryPrefab, DatabaseEntryContainer);
            entryPrefab.GetComponent<TextMeshProUGUI>().text = "ID: " + conversation.ID + " NPC: " + conversation.NPCID;
            entryPrefab.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SaveConversation(ConversationData conversationData) {
        //Exit if no steps
        if (StepContainer.childCount <= 0)
            return;

        //Get Conversations NPCID
        conversationData.NPCID = int.Parse(NPCID.text);

        for (int stepObjectID = 0; stepObjectID < StepContainer.childCount; stepObjectID++) {

            //Step Data (for easy reference)
            StepData stepData = conversationData.StepData[stepObjectID];

            //Get Step Text
            conversationData.StepData[stepObjectID].Text = StepContainer.GetChild(stepObjectID).GetChild(2).GetComponent<TMP_InputField>().text;

            //Option Container
            Transform optionContainer = StepContainer.GetChild(stepObjectID).GetChild(3).GetChild(0).GetChild(0).transform;

            for (int option = 0; option < optionContainer.childCount; option++) {

                //Option Data (for easy reference)
                OptionData optionData = stepData.OptionData[option];

                //Get Option ID
                optionData.DestinationID = int.Parse(optionContainer.GetChild(option).GetChild(5).GetComponent<TMP_InputField>().text);

                //Get Option Text
                optionData.Text = optionContainer.GetChild(option).GetChild(4).GetComponent<TMP_InputField>().text;

                //Get Option Trigger
                if (optionContainer.GetChild(option).GetChild(0).GetComponent<TMP_Dropdown>().value == 0) {
                    //Acceptance Trigger
                    optionData.Trigger = OptionTrigger.QuestAcceptance;
                } else if (optionContainer.GetChild(option).GetChild(0).GetComponent<TMP_Dropdown>().value == 1) {
                    //Reward Trigger
                    optionData.Trigger = OptionTrigger.ObtainQuestReward;
                } else if (optionContainer.GetChild(option).GetChild(0).GetComponent<TMP_Dropdown>().value == 2) {
                    //No Trigger
                    optionData.Trigger = OptionTrigger.None;
                }

                //Get Option Value
                if (optionContainer.GetChild(option).GetChild(2).GetComponent<TMP_Dropdown>().value == 0) {
                    //Value 1
                    optionData.ValueCheck = CompanyValue.ValueOne;
                    optionData.ValueCheckAmount = int.Parse(optionContainer.GetChild(option).GetChild(6).GetComponent<TMP_InputField>().text);
                } else if (optionContainer.GetChild(option).GetChild(2).GetComponent<TMP_Dropdown>().value == 1) {
                    //Value 2
                    optionData.ValueCheck = CompanyValue.ValueTwo;
                    optionData.ValueCheckAmount = int.Parse(optionContainer.GetChild(option).GetChild(6).GetComponent<TMP_InputField>().text);
                } else if (optionContainer.GetChild(option).GetChild(2).GetComponent<TMP_Dropdown>().value == 2) {
                    //Value 3
                    optionData.ValueCheck = CompanyValue.ValueThree;
                    optionData.ValueCheckAmount = int.Parse(optionContainer.GetChild(option).GetChild(6).GetComponent<TMP_InputField>().text);
                } else if (optionContainer.GetChild(option).GetChild(2).GetComponent<TMP_Dropdown>().value == 3) {
                    //No Value
                    optionData.ValueCheck = CompanyValue.NULL;
                    optionData.ValueCheckAmount = -1;
                }
            }
        }

        if (DataMart.CheckConversationDataBase(conversationData.ID)) {
            DataMart.RemoveConversation(conversationData.ID);
        }
        DataMart.AddConversation(conversationData);
        SaveButton.GetComponent<Image>().color = Color.green;
    }

    public void LoadConversation(int id) {
        if (DataMart.CheckConversationDataBase(id)) {
            ConversationData data = DataMart.GetConversation(id);

            RefreshConversationUI(data);
        } else {
            Debug.LogError("Load Error: No conversation with that ID");
        }
    }
}

