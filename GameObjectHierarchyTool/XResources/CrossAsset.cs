using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool.XResources
{
    public struct CrossAsset : IEquatable<CrossAsset>
    {
        public long NewPathId { get; private set; } = 0L;
        public AssetFileInfo? NewCreatedAsset { get; set; } = null;
        public bool IsAssetCreated => NewPathId != 0L;
        public bool HasExternalData => externalData.Length > 0;
        public readonly AssetsFileInstance? fileInstance = null;

        public readonly string resourceName = string.Empty;
        public readonly AssetClassID typeId = AssetClassID.Object;
        public readonly long pathId = 0L;
        public byte[] data = Array.Empty<byte>();
        public byte[] externalData = Array.Empty<byte>();

        public CrossAsset(string resourceName, long pathId)
        {
            this.resourceName = resourceName;
            this.pathId = pathId;
        }

        public CrossAsset(string resourceName, long pathId, int typeId) : this(resourceName, pathId)
        {
            this.typeId = (AssetClassID)typeId;
        }

        public CrossAsset(string resourceName, long pathId, int typeId, AssetsFileInstance fileInstance) : this(resourceName, pathId, typeId)
        {
            this.fileInstance = fileInstance;
        }

        public AssetFileInfo CreateAsset(AssetsFileInstance fileInstance, AssetsManager manager, long pathId)
        {
            if (pathId == 0L || typeId == AssetClassID.Object)
                return null;
            NewPathId = pathId;

            var assetsFile = fileInstance.file;
            var assetInfo = AssetFileInfo.Create(assetsFile, NewPathId, (int)typeId, manager.ClassDatabase);
            assetInfo.SetNewData(data);
            assetsFile.Metadata.AddAssetInfo(assetInfo);
            NewCreatedAsset = assetInfo;
            return assetInfo;
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(resourceName);
            writer.Write((int)typeId);
            writer.Write(pathId);
            writer.Write(data.Length);
            writer.Write(data);
            writer.Write(externalData.Length);
            writer.Write(externalData);

            return stream.ToArray();
        }

        public static CrossAsset Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            string resourceName = reader.ReadString();
            int typeId = reader.ReadInt32();
            long pathId = reader.ReadInt64();
            int dataLength = reader.ReadInt32();
            byte[] data = reader.ReadBytes(dataLength);
            int externalDataLength = reader.ReadInt32();
            byte[] externalData = reader.ReadBytes(externalDataLength);

            CrossAsset crossAsset = new CrossAsset(resourceName, pathId, typeId);
            crossAsset.data = data;
            crossAsset.externalData = externalData;

            return crossAsset;
        }

        public static bool operator ==(CrossAsset crossAsset, CrossAsset crossAsset1)
        {
            return Equals(crossAsset, crossAsset1);
        }

        public static bool operator !=(CrossAsset crossAsset, CrossAsset crossAsset1)
        {
            return !(crossAsset == crossAsset1);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is CrossAsset other)
                return Equals(other);
            return base.Equals(obj);
        }

        public readonly bool Equals(CrossAsset other)
        {
            return resourceName == other.resourceName && pathId == other.pathId;
        }
    }
}
