using AssetsTools.NET;

namespace GameObjectHierarchyTool
{
    public class GameObject
    {
        public string name = string.Empty;
        public long pathID;
        public bool active;
        public byte[] data = new byte[0];
        public List<Component> components = new();

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);

            writer.Write(name);
            writer.Align();
            writer.Write(active);
            writer.Align();
            writer.Write(data.Length);
            writer.Write(data);
            writer.Align();
            writer.Write(components.Count);
            for (int i = 0; i < components.Count; i++)
            {
                byte[] componentBytes = components[i].Serialize();
                writer.Write(componentBytes.Length);
                writer.Write(componentBytes);
                writer.Align();
            }

            return stream.ToArray();
        }

        public static GameObject Deserialize(byte[] bytes)
        {
            GameObject gameObject = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            gameObject.name = reader.ReadString();
            reader.Align();
            gameObject.active = reader.ReadBoolean();
            reader.Align();
            int dataLength = reader.ReadInt32();
            gameObject.data = reader.ReadBytes(dataLength);
            reader.Align();
            int componentsCount = reader.ReadInt32();
            for (int i = 0; i < componentsCount; i++)
            {
                int componentBytesLength = reader.ReadInt32();
                byte[] componentBytes = reader.ReadBytes(componentBytesLength);
                gameObject.components.Add(Component.Deserialize(componentBytes));
                reader.Align();
            }

            return gameObject;
        }
    }
}
