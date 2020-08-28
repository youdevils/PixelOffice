namespace GameDataDictionary {
    [System.Serializable]
    public class ItemData {

        public enum ItemType {
            Quest,
            Valuable,
            KeyItem
        }

        public int ID;
        public ItemType Type;
        public string Name;
        public string Description;
        public int Value;
        public string SpriteURI;

        public ItemData() {}

        public ItemData(int _id, string _name, string _description, int _value, string _spriteuri, ItemType _type) {
            ID = _id;
            Name = _name;
            Description = _description;
            Value = _value;
            SpriteURI = _spriteuri;
            Type = _type;
        }
    }
}