using GameDataDictionary;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour {
    [SerializeField]
    private int ConversationID;

    private ConversationData Conversation_Data;
    private Conversation conversation;
    public Conversation GetConversation {
        get {
            if (conversation != null) {
                ReloadData();
                return conversation;
            } else {
                conversation = ConversationManager.instance.CreateConversation(Conversation_Data);
                ReloadData();
                return conversation;
            }
        }
    }


    private void Start() {

        Conversation_Data = DataMart.GetConversation(ConversationID);

    }

    public void SetID(int _ID) {
        ConversationID = _ID;
        ReloadData();
    }

    private void ReloadData() {
        Conversation_Data = DataMart.GetConversation(ConversationID);
        conversation = ConversationManager.instance.CreateConversation(Conversation_Data); 
    }
}
