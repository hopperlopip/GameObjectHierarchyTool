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

        public byte[] SerializeAll(GameObject_Hierarchy_File file)
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);

            writer.Write(FILE_SIGNATURE);
            writer.Align();

            writer.Write(SerializeGameObjectWithChildren(file));

            return stream.ToArray();
        }

        public GameObject_Hierarchy_File DeserializeAll(byte[] bytes)
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

            long dataLength = stream.Length - reader.Position;
            file = DeserializeGameObjectWithChildren(reader.ReadBytes(Convert.ToInt32(dataLength)));

            return file;
        }


        public byte[] SerializeGameObjectWithChildren(GameObject_Hierarchy_File file)
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);
            
            byte[] GameObject = SerializeGameObject(file);
            writer.Write(GameObject.Length);
            writer.Write(GameObject);
            writer.Align();
            writer.Write(file.children.Count);
            for (int i = 0; i < file.children.Count; i++)
            {
                byte[] child = SerializeGameObjectWithChildren(file.children[i]);
                writer.Write(child.Length);
                writer.Write(child);
                writer.Align();
            }

            return stream.ToArray();
        }

        public GameObject_Hierarchy_File DeserializeGameObjectWithChildren(byte[] bytes)
        {
            GameObject_Hierarchy_File file = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            int GameObjectLength = reader.ReadInt32();
            file = DeserializeGameObject(reader.ReadBytes(GameObjectLength));
            reader.Align();
            int childrenCount = reader.ReadInt32();
            for (int i = 0; i < childrenCount; i++)
            {
                int childLength = reader.ReadInt32();
                GameObject_Hierarchy_File child = DeserializeGameObjectWithChildren(reader.ReadBytes(childLength));
                child.father = file;
                file.children.Add(child);
                reader.Align();
            }

            return file;
        }

        public byte[] SerializeGameObject(GameObject_Hierarchy_File file)
        {
            MemoryStream stream = new MemoryStream();
            AssetsFileWriter writer = new AssetsFileWriter(stream);
            
            writer.Write(file.GameObject.Length);
            writer.Write(file.GameObject);
            writer.Align();
            writer.Write(file.components.Count);
            for (int i = 0;  i < file.components.Count; i++)
            {
                writer.Write(file.componentsTypeIDs[i]);
                writer.Write(file.components[i].Length);
                writer.Write(file.components[i]);
                writer.Align();
            }

            return stream.ToArray();
        }

        public GameObject_Hierarchy_File DeserializeGameObject(byte[] bytes)
        {
            GameObject_Hierarchy_File file = new();
            MemoryStream stream = new MemoryStream(bytes);
            AssetsFileReader reader = new AssetsFileReader(stream);

            int lengthOfGameObject = reader.ReadInt32();
            file.GameObject = reader.ReadBytes(lengthOfGameObject);
            reader.Align();
            int childrenCount = reader.ReadInt32();
            for (int i = 0; i < childrenCount; i++)
            {
                file.componentsTypeIDs.Add(reader.ReadInt32());
                int componentLength = reader.ReadInt32();
                file.components.Add(reader.ReadBytes(componentLength));
                reader.Align();
            }

            return file;
        }
    }
}
