using GameDataDictionary;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    public static ConversationManager instance;
    private delegate void PlayDelegate(int _id);
    private Conversation currentConversation;
    private Quest quest;

    // Use this for initialization
    public ConversationManager() {
        if (instance != null) {
            Debug.LogError("Cannot have more than 1 conversation manager");
            return;
        }
        instance = this;
    }


    public void PlayConversation(string playerName, string otherName, string otherURI, Conversation _conversation, Quest _quest) {
        currentConversation = _conversation;
        quest = _quest;
        GameStateManager.instance.SetState(GameStateManager.GameState.DisplayingUI);
        UIManager.instance.SetupNewConversation(otherName, otherURI, playerName);
        NextConversationStep(0);
    }

    private void PlayConversationStep(ConversationStepData _step) {
        UIManager.instance.UpdateConversation(_step.stepText);
        foreach (ConversationOptionData optionResponse in _step.myOptions) {
            UIManager.instance.AddConversationResponse(
                        optionResponse.optionText,
                        optionResponse.optionDestinationStepID,
                        optionResponse.optionTrait,
                        optionResponse.optionType,
                        quest);
        }
    }

    public void NextConversationStep(int _id) {
        if (_id <= -1) {
            //Is Null no more conversation steps
            UIManager.instance.SetConversationEnabled(false);
            GameStateManager.instance.SetState(GameStateManager.GameState.Playing);
        } else {
            PlayConversationStep(currentConversation.conversationSteps[_id]);
        }
    
    }


    public Conversation CreateConversation(ConversationData _data) {
        Conversation converse = new Conversation();
        for (int i = 0; i < _data.StepData.Count; i++) {
            ConversationStepData step = new ConversationStepData(_data.StepData[i].Text);
            converse.AddStep(step);
        }

        for (int i = 0; i < _data.StepData.Count; i++) {
            for (int z = 0; z < _data.StepData[i].OptionData.Count; z++) {
                if(_data.StepData[i].OptionData[z].DestinationID <= -1) {
                    converse.AddOption(
                    null,
                    _data.StepData[i].OptionData[z].Text,
                    converse.conversationSteps[i],
                    _data.StepData[i].OptionData[z].ValueCheck,
                    _data.StepData[i].OptionData[z].Trigger);
                } else {
                    converse.AddOption(
                    converse.conversationSteps[_data.StepData[i].OptionData[z].DestinationID],
                    _data.StepData[i].OptionData[z].Text,
                    converse.conversationSteps[i],
                    _data.StepData[i].OptionData[z].ValueCheck,
                    _data.StepData[i].OptionData[z].Trigger);
                }
            }
        }
        return converse;
    }

    public ConversationData CreateConversationData(Conversation _data) {
        //Create new Data
        ConversationData outputData = new ConversationData();

        //Set 
        for (int step = 0; step < _data.conversationSteps.Count; step++) {
            outputData.StepData[step].Text = _data.conversationSteps[step].stepText;
            for (int option = 0; option < _data.conversationSteps[step].myOptions.Count; option++) {
                outputData.StepData[step].OptionData[option].Text = _data.conversationSteps[step].myOptions[option].optionText;
                outputData.StepData[step].OptionData[option].DestinationID = _data.conversationSteps[step].myOptions[option].optionDestinationStepID;
                outputData.StepData[step].OptionData[option].ValueCheck = _data.conversationSteps[step].myOptions[option].optionTrait;
            }
        }
        return outputData;
    }
}