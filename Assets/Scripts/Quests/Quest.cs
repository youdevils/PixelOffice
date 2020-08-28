using UnityEngine;
using System.Collections;
using GameDataDictionary;

public class Quest : MonoBehaviour {

    public enum QuestType {
        HaveQuestItemInInventory
    }

    public enum QuestState {
        NotAccepted,
        Accepted,
        ObjectiveMet,
        RewardObtained
    }

    private QuestState state = QuestState.NotAccepted;

    [Header("Quest Conversations")]
    public ConversationTrigger trigger;
    public int RewardAlreadyObtainedID;
    public int NotYetAcceptedID;
    public int HaveAcceptedNotMetID;
    public int ObjectiveMetID;

    [Header("Quest Type")]
    public QuestType questType = QuestType.HaveQuestItemInInventory;

    [Header("QuestVariables")]
    public int itemNeededID = -1;
    private int itemInventorySlotID = -1; //Holds the Players Inventory Slot ID of the Quest Item required so it can be swapped for the reward.

    [Header("Reward")]
    public int rewardItemID = -1;


    private bool GetIsComplete() {
        switch (questType) {
            case QuestType.HaveQuestItemInInventory:
                return CheckPlayerHasItem();
            default:
                Debug.LogError("Check Quest Type Error.");
                break;
        }
        return false;
    }

    public void SetConversationandCheckIfComplete() {
        //If quest has not been accepted yet
        if(state == QuestState.NotAccepted) {

            //Offer the quest
            trigger.SetID(NotYetAcceptedID);

            //If quest has been accepted
        } else if (state == QuestState.Accepted) {

            //Check if quest has been completed
            if (GetIsComplete()) {

                //If it has, set NPCs conversation to 'objective met'
                trigger.SetID(ObjectiveMetID);
            } else {

                //Else, set NPCs conversation to 'go meet objective'
                trigger.SetID(HaveAcceptedNotMetID);
            }

            //If objective is met already
        } else if (state == QuestState.ObjectiveMet) {

            //Set objective met ID (YOU SHOULD NEVER GET HERE AS THIS IS ALWAYS CHECKED MID CONVERSATION)
            trigger.SetID(ObjectiveMetID);

            //If quest has been completed (and reward obtained) then set conversation to this.
        } else if (state == QuestState.RewardObtained) {
            trigger.SetID(RewardAlreadyObtainedID);
        }
    }

    private bool CheckPlayerHasItem() {
        foreach (int id in Player.instance.Inventory.slots.Keys) {
            if (Player.instance.Inventory.slots[id].ID == itemNeededID) {
                itemInventorySlotID = id;
                state = QuestState.ObjectiveMet;
                return true;
            }
        }
        return false;
    }


    /*************/
    // Triggered Off Conversation Option Click Events
    /*************/
    public void Accept() {
        state = QuestState.Accepted;
    }
    public void ClaimReward() {
        switch (questType) {
            case QuestType.HaveQuestItemInInventory:
                Player.instance.Inventory.RemoveInventoryItem(itemInventorySlotID);
                ItemData item = DataMart.GetItem(rewardItemID);
                Player.instance.Inventory.AddInventoryItem(item);
                string message = "You have found a <color=#C2C34D>" + item.Name + "</color>!";
                UIManager.instance.PostNotification(item.SpriteURI, message);
                break;
            default:
                Debug.LogError("error in switch statement");
                break;
        }
        state = QuestState.RewardObtained;
    }
}
