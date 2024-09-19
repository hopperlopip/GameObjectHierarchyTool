using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool
{
    [Serializable]
    internal class GameObjectHierarchyImportException : Exception
    {
        public GameObjectHierarchyImportException() { }

        public GameObjectHierarchyImportException(string message) : base(message) { }
    }
}
