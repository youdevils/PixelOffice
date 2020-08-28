
namespace GameDataDictionary {
    public class ConversationOptionData {
        public string optionText;
        public CompanyValue optionTrait;
        public int optionDestinationStepID;
        public OptionTrigger optionType;

        public ConversationOptionData() { }

        public ConversationOptionData(string _text, int _id) {
            optionText = _text;
            optionType = OptionTrigger.None;
            optionTrait = CompanyValue.NULL;
            optionDestinationStepID = _id;
        }

        public ConversationOptionData(string _text, int _id, CompanyValue _trait, OptionTrigger _type) {
            optionText = _text;
            optionDestinationStepID = _id;
            optionTrait = _trait;
            optionType = _type;
        }
    }
}
