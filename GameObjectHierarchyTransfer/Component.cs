using AssetsTools.NET;

namespace GameObjectHierarchyTransfer
{
    public class Component
    {
        public int typeId;
        public byte[] data = new byte[0];

        public Component(int typeId, byte[] data)
        {
            this.typeId = typeId;
            this.data = data;
        }

        public Component() { }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);

            writer.Write(typeId);
            writer.Write(data.Length);
            writer.Write(data);

            return stream.ToArray();
        }

        public static Component Deserialize(byte[] bytes)
        {
            Component component = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            component.typeId = reader.ReadInt32();
            int dataLength = reader.ReadInt32();
            component.data = reader.ReadBytes(dataLength);

            return component;
        }
    }
}
