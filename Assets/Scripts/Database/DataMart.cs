using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Xml.Serialization;
using Better.StreamingAssets;
using System.Collections;

namespace GameDataDictionary {
    public static class DataMart {

        //Streams and Paths
        private static BinaryFormatter formatter = new BinaryFormatter();
        private static string path = Application.persistentDataPath;

        //Databases
        private static Dictionary<int, ItemData> itemDatabase = new Dictionary<int, ItemData>();
        private static Dictionary<int, ConversationData> conversationDatabase = new Dictionary<int, ConversationData>();
        private static Dictionary<int, NPCData> npcDatabase = new Dictionary<int, NPCData>();

        //Properties
        public static Dictionary<int, ConversationData> ConversationDatabase {
            get {
                return conversationDatabase;
            }
        }
        public static Dictionary<int, ItemData> ItemDatabase {
            get {
                return itemDatabase;
            }
        }

        public static Dictionary<int, NPCData> NPCDatabase {
            get {
                return npcDatabase;
            }
        }

        #region Item Methods
        public static ItemData GetItem(int _id) {
            if (itemDatabase.ContainsKey(_id)) {
                return itemDatabase[_id];
            }

            Debug.LogError("No item with this ID found in Item Database"+_id);
            return null;

        }

        public static void AddItem(ItemData _item) {
            if (itemDatabase.ContainsKey(_item.ID)) {
                Debug.LogError("Cannot add duplicate item to item database.");
            }
            itemDatabase.Add(_item.ID, _item);
        }

        public static void RemoveItem(ItemData _item) {
            if (itemDatabase.ContainsKey(_item.ID)) {
                itemDatabase.Remove(_item.ID);
            } else {
                Debug.LogError("Cannot Remove Item. Doesnt Exist." + _item.ID +" : "+ _item.Name);
            }
            
        }

        public static void ClearItemDatabase() {
            itemDatabase.Clear();
        }
        public static int GetItemDatabaseCount() {
            return itemDatabase.Count;
        }

        public static bool CheckItemDataBase(int id) {
            if (itemDatabase.ContainsKey(id))
                return true;
            else
                return false;
        }
        public static int GetItemID() {
            if (itemDatabase.Count > 0) {
                for (int i = 0; i < itemDatabase.Count; i++) {
                    if (!itemDatabase.ContainsKey(i)) {
                        return i;
                    }
                }
                return itemDatabase.Count;
            } else {
                return 0;
            }
        }
        #endregion

        #region Conversation Methods
        public static ConversationData GetConversation(int _id) {
            if (conversationDatabase.ContainsKey(_id)) {
                return conversationDatabase[_id];
            }

            Debug.LogError("Could not find conversation ID: " + _id);
            return null;

        }

        public static bool CheckConversationDataBase(int id) {
            if (conversationDatabase.ContainsKey(id))
                return true;
            else
                return false;
        }

        public static void AddConversation(ConversationData _conversation) {
            if (conversationDatabase.ContainsKey(_conversation.ID)) {
                Debug.LogError("Cannot add duplicate item to conversation database.");
            }
            conversationDatabase.Add(_conversation.ID, _conversation);
        }

        public static void RemoveConversation(int id) {
            if (!conversationDatabase.ContainsKey(id)) {
                Debug.LogError("Cannot remove item that doesnt exist.");
            }
            conversationDatabase.Remove(id);
        }

        public static void ClearConversationDatabase() {
            conversationDatabase.Clear();
        }

        public static int GetConversationDatabaseCount() {
            return conversationDatabase.Count;
        }

        public static int GetConversationID() {
            if(conversationDatabase.Count > 0) {
                for (int i = 0; i < conversationDatabase.Count; i++) {
                    if (!conversationDatabase.ContainsKey(i)) {
                        return i;
                    }
                }
                return conversationDatabase.Count;
            } else {
                return 0;
            }
        }

        #endregion

        #region NPC Methods
        public static bool CheckNPC(int id) {
            if (npcDatabase.ContainsKey(id))
                return true;
            else
                return false;
        }

        public static NPCData GetNPC(int id) {
            if (npcDatabase.ContainsKey(id)) {
                return npcDatabase[id];
            } else {
                Debug.LogError("Cannot get NPC. doesnt exist.");
                return null;
            }
        }

        public static void AddNPC(NPCData _data) {
            if (!npcDatabase.ContainsKey(_data.ID)) {
                npcDatabase.Add(_data.ID, _data);
            } else {
                Debug.LogError("Cannot add duplicate NPC.");
            }

        }

        public static void RemoveNPC(NPCData _data) {
            if (!npcDatabase.ContainsKey(_data.ID)) {
                npcDatabase.Remove(_data.ID);
            } else {
                Debug.LogError("Cannot remove. No NPC Found.");
            }
        }

        public static void ClearNPCDatabase() {
            npcDatabase.Clear();
        }

        public static int GetNPCID() {
            if (npcDatabase.Count > 0) {
                for (int i = 0; i < npcDatabase.Count; i++) {
                    if (!npcDatabase.ContainsKey(i)) {
                        return i;
                    }
                }
                return npcDatabase.Count;
            } else {
                return 0;
            }
        }
        #endregion

        #region Save and Load Player
        public static void Save_PlayerData(PlayerData _data) {
            string thispath = path + "/player.gdd";
            FileStream stream = new FileStream(thispath, FileMode.Create);
            formatter.Serialize(stream, _data);
            stream.Close();
        }

        public static PlayerData Load_PlayerData() {
            string thispath = path + "/player.gdd";
            if (File.Exists(path)) {
                FileStream stream = new FileStream(thispath, FileMode.Open);
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return data;
            }
            else {
                Debug.LogError("Player Data File Not Found. Path: " + thispath);
                return null;
            }
        }
        #endregion

        #region Individual Save/Load Methods
        public static ConversationData Load_ConversationData(string _filename) {
            if(_filename == "") {
                Debug.LogError("No Filename Supplied - Conversation Load");
                return null;
            }
            string thispath = path + "/" + _filename + ".gdd";
            if (File.Exists(thispath)) {
                FileStream stream = new FileStream(thispath, FileMode.Open);
                ConversationData data = formatter.Deserialize(stream) as ConversationData;
                stream.Close();
                return data;
            } else {
                Debug.LogError("Conversation Data File Not Found. Path: " + thispath);
                return null;
            }
        }
        
        public static void Save_ConversationData(ConversationData _data, string _filename) {
            if (_filename == "") {
                Debug.LogError("No Filename Supplied - Conversation Save");
                return;
            }
            string thispath = path + "/" + _filename + ".gdd";
            FileStream stream = new FileStream(thispath, FileMode.Create);
            formatter.Serialize(stream, _data);
            stream.Close();
        }

        public static ItemData Load_ItemData(string _filename) {
            if (_filename == "") {
                Debug.LogError("No Filename Supplied - Item Load");
                return null;
            }
            string thispath = path + "/" + _filename + ".gdd";
            if (File.Exists(thispath)) {
                FileStream stream = new FileStream(thispath, FileMode.Open);
                ItemData data = formatter.Deserialize(stream) as ItemData;
                stream.Close();
                return data;
            } else {
                Debug.LogError("Item Data File Not Found. Path: " + thispath);
                return null;
            }
        }

        public static void Save_ItemData(ItemData _data, string _filename) {
            if (_filename == "") {
                Debug.LogError("No Filename Supplied - Item Save");
                return;
            }
            string thispath = path + "/" + _filename + ".gdd";
            FileStream stream = new FileStream(thispath, FileMode.Create);
            formatter.Serialize(stream, _data);
            stream.Close();
        }
        #endregion

        #region Test Methods
        public static ItemData CreateTestItemData(string _name, string _uri, string _description) {
            ItemData data = new ItemData();
            data.ID = 0;
            data.Name = _name;
            data.SpriteURI = _uri;
            data.Value = 350;
            data.Description = _description;
            return data;
        }
        #endregion


        /************
        * EDITOR
        * **********/
        public static IEnumerator LoadAllDatabasesFromFile() {

            //Clear Databases
            conversationDatabase.Clear();
            itemDatabase.Clear();
            npcDatabase.Clear();

            //Initialise BSA
            BetterStreamingAssets.Initialize();

            //Get File Headers
            string[] conversationHeaders = BetterStreamingAssets.GetFiles("conversations", "*.xml", SearchOption.AllDirectories);
            string[] itemHeaders = BetterStreamingAssets.GetFiles("items", "*.xml", SearchOption.AllDirectories);
            string[] npcHeaders = BetterStreamingAssets.GetFiles("npcs", "*.xml", SearchOption.AllDirectories);

            //Load Conversations
            if (conversationHeaders.Length <= 0) {
                Debug.LogError("No XML Files to Load.");
            }
            
            for (int i = 0; i < conversationHeaders.Length; i++) {
                Stream stream = BetterStreamingAssets.OpenRead(conversationHeaders[i]);
                XmlSerializer serializer = new XmlSerializer(typeof(ConversationData));
                ConversationData conversationdata = (ConversationData)serializer.Deserialize(stream);
                stream.Close();
                conversationDatabase.Add(conversationdata.ID, conversationdata);
            }

            Debug.Log("Load converastions to database complete.");

            //Load Items
            if (itemHeaders.Length <= 0) {
                Debug.LogError("No XML Files to Load.");
            }

            for (int i = 0; i < itemHeaders.Length; i++) {
                Stream stream = BetterStreamingAssets.OpenRead(itemHeaders[i]);
                XmlSerializer serializer = new XmlSerializer(typeof(ItemData));
                ItemData itemdata = (ItemData)serializer.Deserialize(stream);
                stream.Close();
                itemDatabase.Add(itemdata.ID, itemdata);
            }

            Debug.Log("Load items to database complete.");

            //Load NPCs
            if (npcHeaders.Length <= 0) {
                Debug.LogError("No XML Files to Load.");
            }

            for (int i = 0; i < npcHeaders.Length; i++) {
                Stream stream = BetterStreamingAssets.OpenRead(npcHeaders[i]);
                XmlSerializer serializer = new XmlSerializer(typeof(NPCData));
                NPCData npcdata = (NPCData)serializer.Deserialize(stream);
                stream.Close();
                npcDatabase.Add(npcdata.ID, npcdata);
            }

            Debug.Log("Load NPCs to database complete.");

            yield break;

        }

        public static IEnumerator SaveAllDatabasesToFile() {

            //Save Conversations
            if (conversationDatabase.Count > 0) {
                string path = Path.Combine("Assets/StreamingAssets/", "conversations");
                Directory.CreateDirectory(path);

                foreach (int id in conversationDatabase.Keys) {
                    string namedpath = Path.Combine(path, id.ToString()+".xml");
                    XmlSerializer serializer = new XmlSerializer(typeof(ConversationData));    
                    FileStream stream = new FileStream(namedpath, FileMode.Create);
                    serializer.Serialize(stream, conversationDatabase[id]);
                    stream.Close();
                }
                Debug.Log("Save conversation to file complete.");
            }

            //Save Items
            if (itemDatabase.Count > 0) {
                string path = Path.Combine("Assets/StreamingAssets/", "items");
                Directory.CreateDirectory(path);

                foreach (int id in itemDatabase.Keys) {
                    string namedpath = Path.Combine(path, id.ToString() + ".xml");
                    XmlSerializer serializer = new XmlSerializer(typeof(ItemData));
                    FileStream stream = new FileStream(namedpath, FileMode.Create);
                    serializer.Serialize(stream, itemDatabase[id]);
                    stream.Close();
                }
                Debug.Log("Save items to file complete.");
            }

            //Save NPCs
            if (npcDatabase.Count > 0) {
                string path = Path.Combine("Assets/StreamingAssets/", "npcs");
                Directory.CreateDirectory(path);

                foreach (int id in npcDatabase.Keys) {
                    string namedpath = Path.Combine(path, id.ToString() + ".xml");
                    XmlSerializer serializer = new XmlSerializer(typeof(NPCData));
                    FileStream stream = new FileStream(namedpath, FileMode.Create);
                    serializer.Serialize(stream, npcDatabase[id]);
                    stream.Close();
                }
                Debug.Log("Save NPCs to file complete.");
            }

            yield break;
        }

        /************
         * XML
         * **********/
        //Save the streamingDataObject to xml
        public static void SaveConversationXML(ConversationData _data) {
            //Create new xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ConversationData));             //Create serializer
            string filepath = path + "/XML/NPC" + _data.NPCID + "Conversation" + _data.ID + ".xml";
            FileStream stream = new FileStream(filepath, FileMode.CreateNew); //Create file at this path
            serializer.Serialize(stream, _data);//Write the data in the xml file
            stream.Close();//Close the stream
        }
    }
}