using System;
using System.Collections.Generic;

namespace GameDataDictionary {
    public class ConversationStepData {
        public string stepText;
        public int stepID = -1;
        public List<ConversationOptionData> myOptions;

        public ConversationStepData() { }

        public ConversationStepData(string _text) {
            stepText = _text;
            myOptions = new List<ConversationOptionData>();
        }
    }
}

