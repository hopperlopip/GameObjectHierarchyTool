using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool.XResources
{
    public struct AssetPptr
    {
        public int fileId = 0;
        public long pathId = 0L;
        public bool IsNullReference => fileId == 0 && pathId == 0L;
        public static AssetPptr NullPptr => new AssetPptr(0, 0L);

        public AssetPptr(int fileId, long pathId)
        {
            this.fileId = fileId;
            this.pathId = pathId;
        }
    }
}
