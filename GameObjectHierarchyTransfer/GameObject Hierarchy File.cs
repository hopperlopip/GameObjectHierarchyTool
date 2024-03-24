using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTransfer
{
    internal class GameObject_Hierarchy_File
    {
        public byte[] GameObject = new byte[0];
        public List<int> componentsTypeIDs = new();
        public List<byte[]> components = new();
        public GameObject_Hierarchy_File? father = null;
        public List<GameObject_Hierarchy_File> children = new();
        public long pathID;

        public GameObject_Hierarchy_File(byte[] GameObject, List<byte[]> components)
        {
            this.GameObject = GameObject;
            this.components = components;
        }

        public GameObject_Hierarchy_File() { }
    }
}
