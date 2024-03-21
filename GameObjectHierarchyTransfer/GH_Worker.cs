using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTransfer
{
    internal class GH_Worker
    {
        private const string FILE_SIGNATURE = "GH";

        public byte[] Serialize(GameObject_Hierarchy_File file)
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);
            writer.Write(FILE_SIGNATURE);
            writer.Align();
            writer.Write(file.parentGameObject.Length);
            writer.Write(file.parentGameObject);
            writer.Align();
            writer.Write(file.children.Count);
            for (int i = 0;  i < file.children.Count; i++)
            {
                writer.Write(file.childrenTypeIDs[i]);
                writer.Write(file.children[i].Length);
                writer.Write(file.children[i]);
                writer.Align();
            }
            return stream.ToArray();
        }

        public GameObject_Hierarchy_File Deserialize(byte[] bytes)
        {
            GameObject_Hierarchy_File file = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);
            string signature = reader.ReadString();
            if (signature != FILE_SIGNATURE)
            {
                throw new Exception("Wrong file or file is corrupted");
            }
            reader.Align();
            int lengthOfParent = reader.ReadInt32();
            file.parentGameObject = reader.ReadBytes(lengthOfParent);
            reader.Align();
            int childrenCount = reader.ReadInt32();
            for (int i = 0; i < childrenCount; i++)
            {
                file.childrenTypeIDs.Add(reader.ReadInt32());
                int componentLength = reader.ReadInt32();
                file.children.Add(reader.ReadBytes(componentLength));
                reader.Align();
            }
            return file;
        }
    }
}
