using GameDataDictionary;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Player() {
        if(instance != null) {
            Debug.LogError("This is a single player game.");
            return;
        }
        instance = this;
    }

    //Core Attributes
    private PlayerData _myData;
    private string myName = "Mr. Samuel Green";
    private string portraitURI = "Portraits/PlayerPortrait";
    private string backStory = "Im just test data...nothing else...:(";
    private string jobTitle = "Senior Business Analyst";

    //Animation Controller
    [SerializeField]
    private Animator animator;

    //Company Values
    private int valueOne = 0;
    private int valueTwo = 0;
    private int valueThree = 0;

    //Movement
    private float speed = 2f;

    //Inventory
    public Inventory Inventory;

    //Interaction
    private float interactableDistance = 0.6f;
    [SerializeField]
    private Transform interactionPoint;

    //Shader Effect Test
    private bool isBlurred = false;
    private float currentBlur = 0f;

    //KPI
    private int PersonalKPIID = -1;
    public int PersonalKPI {
        get { return PersonalKPIID; }
    }
    private int CompanyKPIID = -1;
    public int CompanyKPI {
        get { return CompanyKPIID; }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactableDistance);
    }

    private void Start() {
        SavePlayer();
        Inventory = new Inventory();
        UIManager.instance.SetCharacterMenuPlayerDetails(_myData);
        
    }
    // Update is called once per frame
    void Update()
    {
        #region Test
        if (isBlurred) {
            currentBlur += (1f * Time.deltaTime);
            currentBlur = Mathf.Clamp(currentBlur, 0f, 1f);
            GetComponent<SpriteRenderer>().material.SetFloat("_BlurOut", currentBlur);
        } else {
            currentBlur -= (1f * Time.deltaTime);
            currentBlur = Mathf.Clamp(currentBlur, 0f, 1f);
            GetComponent<SpriteRenderer>().material.SetFloat("_BlurOut", currentBlur);
        }
        #endregion
    }
    #region Command Methods
    public void FindInteractable() {
        CustomTags[] tagobjects = GameObject.FindObjectsOfType<CustomTags>();
        if(tagobjects.Length <= 0) {
            Debug.LogError("No Tag Objects Objects Found");
            return;
        }

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach(CustomTags tagobject in tagobjects) {
            float dist = Vector3.Distance(tagobject.transform.position, interactionPoint.position);
            if (closestObject == null) {
                closestObject = tagobject.gameObject;
                closestDistance = dist;
            } else {
                if (dist < closestDistance) {
                    closestDistance = dist;
                    closestObject = tagobject.gameObject;
                }
            }
        }

        if(closestDistance <= interactableDistance) {
            Quest temporaryQuest = null;
            if(closestObject.GetComponent<CustomTags>().Tags.Contains("Quest")) {
                closestObject.GetComponent<Quest>().SetConversationandCheckIfComplete();
                temporaryQuest = closestObject.GetComponent<Quest>();
            } 
            
            if (closestObject.GetComponent<CustomTags>().Tags.Contains("Conversation")) {
                if (closestObject.GetComponent<ConversationTrigger>() != null) {
                    try {
                        Conversation conversation = closestObject.GetComponent<ConversationTrigger>().GetConversation;
                        string otherName = closestObject.GetComponent<NPC>().NPCName;
                        string otherURI = closestObject.GetComponent<NPC>().NPCURI;
                        ConversationManager.instance.PlayConversation(myName, otherName, otherURI, conversation, temporaryQuest);
                    }
                    catch {
                        Debug.LogError("Failure to play Conversation");
                        return;
                    }
                    return;
                }
            } 
            
            if (closestObject.GetComponent<CustomTags>().Tags.Contains("Chest")) {
                if (closestObject.GetComponent<Chest>() != null) {
                    closestObject.GetComponent<Chest>().Open();
                    return;
                }
            }
        } else {
            Debug.Log("No Interaction Points in Range");
            return;
        }
    }
    #endregion


    public PlayerData CreatePlayerData() {
        PlayerData playerData = new PlayerData();
        playerData.PersonalKPIID = PersonalKPI;
        playerData.CompanyKPIID = CompanyKPI;
        playerData.CompanyValueOne = valueOne;
        playerData.CompanyValueTwo = valueTwo;
        playerData.CompanyValueThree = valueThree;
        playerData.Name = myName;
        playerData.BackStory = backStory;
        playerData.JobTitle = jobTitle;
        playerData.PortraitURI = portraitURI;
        return playerData;
    }

    public void SavePlayer() {
        _myData = CreatePlayerData();
        DataMart.Save_PlayerData(_myData);
    }

    public void RemoveInventoryItem(int _slotID) {
        Inventory.RemoveInventoryItem(_slotID);
    }

    public void MoveUp() {
        transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);
        animator.SetInteger("GoUp", 1);
        animator.SetInteger("GoDown", 0);
        animator.SetInteger("GoLeft", 0);
        animator.SetInteger("GoRight", 0);

    }
    public void MoveDown() {
        transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime * -1), transform.position.z);
        animator.SetInteger("GoUp", 0);
        animator.SetInteger("GoDown", 1);
        animator.SetInteger("GoLeft", 0);
        animator.SetInteger("GoRight", 0);

    }
    public void MoveLeft() {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime * -1), transform.position.y, transform.position.z);
        animator.SetInteger("GoUp", 0);
        animator.SetInteger("GoDown", 0);
        animator.SetInteger("GoLeft", 1);
        animator.SetInteger("GoRight", 0);

    }
    public void MoveRight() {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);
        animator.SetInteger("GoUp", 0);
        animator.SetInteger("GoDown", 0);
        animator.SetInteger("GoLeft", 0);
        animator.SetInteger("GoRight", 1);
    }

    public void StopAllMovementAnimation() {
        animator.SetInteger("GoUp", 0);
        animator.SetInteger("GoDown", 0);
        animator.SetInteger("GoLeft", 0);
        animator.SetInteger("GoRight", 0);
    }
}
