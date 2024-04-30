using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool
{
    internal class GameObjectHierarchyFile
    {
        private const string FILE_SIGNATURE = "GH";

        public static byte[] Serialize(GameObjectHierarchy gameObjectHierarchy)
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);

            writer.Write(FILE_SIGNATURE);
            writer.Align();

            writer.Write(gameObjectHierarchy.Serialize());

            return stream.ToArray();
        }

        public static GameObjectHierarchy Deserialize(byte[] bytes)
        {
            GameObjectHierarchy gameObjectHierarchy;
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            string signature = reader.ReadString();
            if (signature != FILE_SIGNATURE)
            {
                throw new Exception("Wrong file or file is corrupted");
            }
            reader.Align();

            long dataLength = stream.Length - reader.Position;
            byte[] gameObjectHierarchyBytes = reader.ReadBytes(Convert.ToInt32(dataLength));
            gameObjectHierarchy = GameObjectHierarchy.Deserialize(gameObjectHierarchyBytes);

            return gameObjectHierarchy;
        }
    }
}