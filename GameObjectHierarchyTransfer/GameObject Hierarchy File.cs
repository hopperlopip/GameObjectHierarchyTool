using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTransfer
{
    internal class GameObject_Hierarchy_File
    {
        public byte[] parentGameObject = new byte[0];
        public List<int> childrenTypeIDs = new();
        public List<byte[]> children = new();

        public GameObject_Hierarchy_File(byte[] parentGameObject, List<byte[]> children)
        {
            this.parentGameObject = parentGameObject;
            this.children = children;
        }

        public GameObject_Hierarchy_File() { }
    }
}
