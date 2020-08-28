using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GameDataDictionary;

public class Conversation {

    public List<ConversationStepData> conversationSteps;

    public Conversation() {
        conversationSteps = new List<ConversationStepData>();
    }

    public void AddStep(ConversationStepData _step) {
        if (_step == null)
            return;

        conversationSteps.Add(_step);

        _step.stepID = conversationSteps.IndexOf(_step);
    }
    public void AddOption(ConversationStepData _destStep, string _text, ConversationStepData _parentStep) {
        if (!conversationSteps.Contains(_parentStep))
            AddStep(_parentStep);

        if (!conversationSteps.Contains(_destStep))
            AddStep(_destStep);

        ConversationOptionData newOption;

        if (_destStep == null)
            newOption = new ConversationOptionData(_text, -1);
        else
            newOption = new ConversationOptionData(_text, _destStep.stepID);

        _parentStep.myOptions.Add(newOption);
    }
    public void AddOption(ConversationStepData _destStep, string _text, ConversationStepData _parentStep, CompanyValue _value, OptionTrigger _trigger) {
        if (!conversationSteps.Contains(_parentStep))
            AddStep(_parentStep);

        if (!conversationSteps.Contains(_destStep))
            AddStep(_destStep);

        ConversationOptionData newOption;

        if (_destStep == null)
            newOption = new ConversationOptionData(_text, -1, _value, _trigger);
        else
            newOption = new ConversationOptionData(_text, _destStep.stepID, _value, _trigger);

        _parentStep.myOptions.Add(newOption);
    }
}
