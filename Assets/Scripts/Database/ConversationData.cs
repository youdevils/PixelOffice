using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDataDictionary {
    public enum OptionTrigger {
        QuestAcceptance,
        ObtainQuestReward,
        None
    }

    public enum CompanyValue {
        ValueOne,
        ValueTwo,
        ValueThree,
        NULL
    }

    [System.Serializable]
    public class OptionData {
        [TextArea(2,3)]
        public string Text;
        public int DestinationID;
        public CompanyValue ValueCheck;
        public int ValueCheckAmount;
        public OptionTrigger Trigger;
    }

    [System.Serializable]
    public class StepData {
        [TextArea(3, 5)]
        public string Text;
        public List<OptionData> OptionData;
    }
    [System.Serializable]
    public class ConversationData {
        public int NPCID;
        public int ID;
        public List<StepData> StepData;
    }
}