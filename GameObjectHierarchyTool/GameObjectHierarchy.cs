using AssetsTools.NET;

namespace GameObjectHierarchyTool
{
    public class GameObjectHierarchy
    {
        public GameObject gameObject = new();
        public GameObjectHierarchy? father;
        public List<GameObjectHierarchy> children = new();

        public GameObjectHierarchy(GameObject gameObject, List<GameObjectHierarchy> children)
        {
            this.gameObject = gameObject;
            this.children = children;
        }

        public GameObjectHierarchy() { }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);

            byte[] gameObjectBytes = gameObject.Serialize();
            writer.Write(gameObjectBytes.Length);
            writer.Write(gameObjectBytes);
            writer.Align();
            writer.Write(children.Count);

            for (int i = 0; i < children.Count; i++)
            {
                byte[] childBytes = children[i].Serialize();
                writer.Write(childBytes.Length);
                writer.Write(childBytes);
            }

            return stream.ToArray();
        }

        static public GameObjectHierarchy Deserialize(byte[] bytes)
        {
            GameObjectHierarchy gameObjectHierarchy = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            int gameObjectBytesLength = reader.ReadInt32();
            byte[] gameObjectBytes = reader.ReadBytes(gameObjectBytesLength);
            gameObjectHierarchy.gameObject = GameObject.Deserialize(gameObjectBytes);
            reader.Align();
            int childrenCount = reader.ReadInt32();

            for (int i = 0; i < childrenCount; i++)
            {
                int childBytesLength = reader.ReadInt32();
                byte[] childBytes = reader.ReadBytes(childBytesLength);
                gameObjectHierarchy.children.Add(Deserialize(childBytes));
                gameObjectHierarchy.children[i].father = gameObjectHierarchy;
            }

            return gameObjectHierarchy;
        }
    }
}
