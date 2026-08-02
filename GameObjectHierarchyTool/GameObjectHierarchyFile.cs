using AssetsTools.NET;
using GameObjectHierarchyTool.XResources;
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

            byte[] gameObjectHierarchyBytes = gameObjectHierarchy.Serialize();
            writer.Write(gameObjectHierarchyBytes.Length);
            writer.Write(gameObjectHierarchyBytes);

            byte[] crossResourcesBytes = gameObjectHierarchy.crossResources.Serialize();
            writer.Write(crossResourcesBytes.Length);
            writer.Write(crossResourcesBytes);

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

            int gameObjectHierarchyBytesLength = reader.ReadInt32();
            byte[] gameObjectHierarchyBytes = reader.ReadBytes(gameObjectHierarchyBytesLength);

            int crossResourcesBytesLength = reader.ReadInt32();
            byte[] crossResourcesBytes = reader.ReadBytes(crossResourcesBytesLength);
            CrossResources crossResources = CrossResources.Deserialize(crossResourcesBytes);

            gameObjectHierarchy = GameObjectHierarchy.Deserialize(gameObjectHierarchyBytes, crossResources);

            return gameObjectHierarchy;
        }
    }
}