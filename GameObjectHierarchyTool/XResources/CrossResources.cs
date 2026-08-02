using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace GameObjectHierarchyTool.XResources
{
    public class CrossResources
    {
        private readonly AssetsManager _manager;
        public List<AssetClassID> dontGetPointersTypes = new() { AssetClassID.GameObject, AssetClassID.Transform, AssetClassID.RectTransform, AssetClassID.MonoBehaviour };
        public List<CrossAsset> importedCrossAssets = new List<CrossAsset>();
        public AssetPptr? newMaterialShaderPptr = null;

        public List<CrossAsset> crossAssets = new List<CrossAsset>();
        public Dictionary<string, string[]> dependencyTable = new Dictionary<string, string[]>();

        public CrossResources() : this(MainWindow.Manager) { }

        public CrossResources(AssetsManager manager)
        {
            _manager = manager;
        }

        private CrossResources(List<CrossAsset> crossAssets, Dictionary<string, string[]> dependencyTable) : this()
        {
            this.crossAssets = crossAssets;
            this.dependencyTable = dependencyTable;
        }

        private byte[] SerializeDependencyTable()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(dependencyTable.Keys.Count);
            foreach (var dependencyKey in dependencyTable.Keys)
            {
                writer.Write(dependencyKey);
                string[] dependencyValues = dependencyTable[dependencyKey];
                writer.Write(dependencyValues.Length);
                foreach (var dependencyValue in dependencyValues)
                {
                    writer.Write(dependencyValue);
                }
            }

            return stream.ToArray();
        }

        private static Dictionary<string, string[]> DeserializeDependencyTable(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            Dictionary<string, string[]> dependencyTable = new Dictionary<string, string[]>();

            int dependencyKeysCount = reader.ReadInt32();
            for (int i = 0; i < dependencyKeysCount; i++)
            {
                string dependencyKey = reader.ReadString();
                int dependencyValuesLength = reader.ReadInt32();
                string[] dependencyValues = new string[dependencyValuesLength];
                for (int j = 0; j < dependencyValues.Length; j++)
                {
                    string dependencyValue = reader.ReadString();
                    dependencyValues[j] = dependencyValue;
                }
                dependencyTable.Add(dependencyKey, dependencyValues);
            }

            return dependencyTable;
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(crossAssets.Count);
            foreach (var crossAsset in crossAssets)
            {
                byte[] crossAssetBytes = crossAsset.Serialize();
                writer.Write(crossAssetBytes.Length);
                writer.Write(crossAssetBytes);
            }
            byte[] dependencyTableBytes = SerializeDependencyTable();
            writer.Write(dependencyTableBytes.Length);
            writer.Write(dependencyTableBytes);

            return stream.ToArray();
        }

        public static CrossResources Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            int CrossAssetsCount = reader.ReadInt32();
            List<CrossAsset> crossAssets = new List<CrossAsset>();
            for (int i = 0; i < CrossAssetsCount; i++)
            {
                int crossAssetBytesLength = reader.ReadInt32();
                byte[] crossAssetBytes = reader.ReadBytes(crossAssetBytesLength);
                CrossAsset crossAsset = CrossAsset.Deserialize(crossAssetBytes);
                crossAssets.Add(crossAsset);
            }
            int dependencyTableBytesLength = reader.ReadInt32();
            byte[] dependencyTableBytes = reader.ReadBytes(dependencyTableBytesLength);
            Dictionary<string, string[]> dependencyTable = DeserializeDependencyTable(dependencyTableBytes);

            CrossResources crossResources = new CrossResources(crossAssets, dependencyTable);
            return crossResources;
        }

        public void ImportCrossResources(AssetsFileInstance fileInstance, string externalDataFileName)
        {
            foreach (CrossAsset crossAsset in crossAssets)
            {
                if (crossAsset.IsAssetCreated)
                    continue;
                if (crossAsset.CreateAsset(fileInstance, _manager, GameObjectHelper.GetNewPathId(fileInstance)) == null)
                    throw new Exception("Couldn't create a cross-asset.");
                importedCrossAssets.Add(crossAsset);
            }
            foreach (CrossAsset importedCrossAsset in importedCrossAssets)
            {
                FixPointersInImportedAsset(fileInstance, importedCrossAsset);
            }
            List<CrossAsset> crossAssetsWithExtData = importedCrossAssets.FindAll((crossAsset) => { return crossAsset.HasExternalData; });
            if (crossAssetsWithExtData.Count == 0)
                return;
            using FileStream fileStream = File.OpenWrite(Path.Combine(Path.GetDirectoryName(fileInstance.path) ?? string.Empty, externalDataFileName));
            BinaryWriter writer = new BinaryWriter(fileStream);
            foreach (CrossAsset crossAssetWithExtData in crossAssetsWithExtData)
            {
                if (crossAssetWithExtData.NewCreatedAsset == null)
                    continue;
                AssetTypeValueField baseField = _manager.GetBaseField(fileInstance, crossAssetWithExtData.NewCreatedAsset);
                WriteExternalResources(writer, crossAssetWithExtData.externalData, out long offset, out int size);
                SetExternalResources(baseField, crossAssetWithExtData.typeId, externalDataFileName, offset, size);
                crossAssetWithExtData.NewCreatedAsset.SetNewData(baseField);
            }
            fileStream.Close();
        }

        private void FixPointersInImportedAsset(AssetsFileInstance fileInstance, CrossAsset importedCrossAsset)
        {
            if (importedCrossAsset.NewCreatedAsset != null)
                FixPointersInAsset(_manager, fileInstance, importedCrossAsset.NewCreatedAsset, importedCrossAsset.resourceName);
        }

        public void FixPointersInAsset(AssetsManager manager, AssetsFileInstance fileInstance, AssetFileInfo assetInfo, string resourceName)
        {
            if (assetInfo == null)
                return;
            AssetTypeValueField baseField = manager.GetBaseField(fileInstance, assetInfo);
            FixPointersInAsset(assetInfo, baseField, resourceName);
        }

        public void FixPointersInAsset(AssetFileInfo assetInfo, AssetTypeValueField baseField, string resourceName)
        {
            if (assetInfo == null)
                return;
            FixPointersInAssetBaseField(baseField, resourceName);
            if (assetInfo.TypeId == (int)AssetClassID.Material && newMaterialShaderPptr.HasValue)
            {
                ChangeMaterialShaderPptr(baseField, newMaterialShaderPptr.Value);
            }
            assetInfo.SetNewData(baseField);
        }

        public void FixPointersInAssetBaseField(AssetTypeValueField baseField, string resourceName, bool skipFirstPptr = false)
        {
            if (importedCrossAssets.Count == 0)
                return;
            List<AssetPptr> assetPptrs = GetAssetPptrs(baseField, true);
            if (skipFirstPptr)
                assetPptrs.RemoveAt(0);
            Dictionary<AssetPptr, AssetPptr> changeDict = new Dictionary<AssetPptr, AssetPptr>();
            foreach (var assetPptr in assetPptrs)
            {
                if (changeDict.ContainsKey(assetPptr))
                    continue;
                string pptrResourceName = dependencyTable[resourceName][assetPptr.fileId];
                CrossAsset pptrCrossAsset = new CrossAsset(pptrResourceName, assetPptr.pathId);
                int crossAssetIndex = importedCrossAssets.IndexOf(pptrCrossAsset);
                if (crossAssetIndex != -1)
                {
                    pptrCrossAsset = importedCrossAssets[crossAssetIndex];
                    AssetPptr newAssetPptr = new AssetPptr(0, pptrCrossAsset.NewPathId);
                    changeDict.Add(assetPptr, newAssetPptr);
                }
                else
                {
                    if (MainWindow.IsMakeNullPptrButtonEnabled)
                        changeDict.Add(assetPptr, AssetPptr.NullPptr);
                }
            }
            ChangeAssetPptrs(baseField, changeDict);
        }

        public static string[] GetDependencyArray(AssetsFileInstance fileInstance)
        {
            List<AssetsFileExternal> assetsFiles = fileInstance.file.Metadata.Externals;
            string[] dependencies = new string[assetsFiles.Count + 1];
            dependencies[0] = fileInstance.name;
            for (int i = 1; i < dependencies.Length; i++)
            {
                dependencies[i] = Path.GetFileName(assetsFiles[i - 1].PathName);
            }
            return dependencies;
        }

        public void LoadAllCrossAssets(AssetExternal assetExternal, bool skipFirstPptr)
        {
            int i = crossAssets.Count;
            LoadCrossAssets(assetExternal, skipFirstPptr);
            for (; i < crossAssets.Count; i++)
            {
                if (dontGetPointersTypes.Contains(crossAssets[i].typeId))
                    continue;
                LoadCrossAssets(_manager.GetExtAsset(crossAssets[i].fileInstance, 0, crossAssets[i].pathId), false);
            }
        }

        private void LoadCrossAssets(AssetExternal assetExternal, bool skipFirstPptr)
        {
            LoadCrossAssets(assetExternal.file, assetExternal.baseField, skipFirstPptr);
        }

        private void LoadCrossAssets(AssetsFileInstance fileInstance, AssetTypeValueField assetBaseField, bool skipFirstPptr)
        {
            LoadCrossAssets(fileInstance, GetAssetPptrs(assetBaseField), skipFirstPptr);
        }

        private void LoadCrossAssets(AssetsFileInstance fileInstance, List<AssetPptr> assetPptrs, bool skipFirstPptr)
        {
            int i = 0;
            if (skipFirstPptr)
                i = 1;
            for (; i < assetPptrs.Count; i++)
            {
                AssetPptr assetPptr = assetPptrs[i];
                if (assetPptr.IsNullReference)
                    continue;
                AssetExternal asset = _manager.GetExtAsset(fileInstance, assetPptr.fileId, assetPptr.pathId, true);
                if (asset.info == null)
                    continue;
                CrossAsset crossAsset = new CrossAsset(asset.file.name, assetPptr.pathId, asset.info.TypeId, asset.file);
                if (!crossAssets.Contains(crossAsset))
                {
                    AssetTypeValueField baseField = _manager.GetBaseField(asset.file, asset.info);
                    if (crossAsset.typeId == AssetClassID.Texture2D || crossAsset.typeId == AssetClassID.Mesh)
                    {
                        ImportExternalStreamData(asset.file, crossAsset.typeId, baseField);
                    }
                    if (crossAsset.typeId == AssetClassID.AudioClip || crossAsset.typeId == AssetClassID.VideoClip)
                    {
                        crossAsset.externalData = GetExternalResources(asset.file, crossAsset.typeId, baseField);
                    }
                    byte[] data = baseField.WriteToByteArray();
                    crossAsset.data = data;
                    crossAssets.Add(crossAsset);
                    if (!dependencyTable.ContainsKey(asset.file.name))
                    {
                        dependencyTable.Add(asset.file.name, GetDependencyArray(asset.file));
                    }
                }
            }
        }

        /// <summary>
        /// For texture2D and mesh assets.
        /// </summary>
        /// <param name="fileInstance"></param>
        /// <param name="typeId"></param>
        /// <param name="baseField"></param>
        private void ImportExternalStreamData(AssetsFileInstance fileInstance, AssetClassID typeId, AssetTypeValueField baseField)
        {
            AssetTypeValueField streamData = baseField["m_StreamData"];
            string relPath = streamData["path"].AsString;
            string dirPath = Path.GetDirectoryName(fileInstance.path) ?? string.Empty;
            string filePath = Path.Combine(dirPath, relPath);
            ulong offset = streamData["offset"].AsULong;
            uint size = streamData["size"].AsUInt;
            byte[] streamDataBytes = GetExternalData(filePath, offset, size);
            if (streamDataBytes.Length == 0)
                return;
            if (typeId == AssetClassID.Texture2D)
            {
                baseField["image data"].AsByteArray = streamDataBytes;
            }
            else
            {
                baseField["m_VertexData.m_DataSize"].AsByteArray = streamDataBytes;
            }
            streamData["path"].AsString = string.Empty;
            streamData["offset"].AsULong = 0uL;
            streamData["size"].AsUInt = 0u;
        }

        /// <summary>
        /// For audio and video assets.
        /// </summary>
        /// <param name="fileInstance"></param>
        /// <param name="typeId"></param>
        /// <param name="baseField"></param>
        /// <returns></returns>
        private byte[] GetExternalResources(AssetsFileInstance fileInstance, AssetClassID typeId, AssetTypeValueField baseField)
        {
            AssetTypeValueField streamedResource;
            if (typeId == AssetClassID.AudioClip)
                streamedResource = baseField["m_Resource"];
            else
                streamedResource = baseField["m_ExternalResources"];
            string relPath = streamedResource["m_Source"].AsString;
            string dirPath = Path.GetDirectoryName(fileInstance.path) ?? string.Empty;
            string filePath = Path.Combine(dirPath, relPath);
            ulong offset = streamedResource["m_Offset"].AsULong;
            ulong longSize = streamedResource["m_Size"].AsULong;
            uint size = (uint)longSize;
            byte[] resourceDataBytes = GetExternalData(filePath, offset, size);
            return resourceDataBytes;
        }

        /// <summary>
        /// For audio and video assets.
        /// </summary>
        /// <param name="baseField"></param>
        /// <param name="typeId"></param>
        /// <param name="fileName"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        private void SetExternalResources(AssetTypeValueField baseField, AssetClassID typeId, string fileName, long offset, int size)
        {
            AssetTypeValueField streamedResource;
            if (typeId == AssetClassID.AudioClip)
                streamedResource = baseField["m_Resource"];
            else
                streamedResource = baseField["m_ExternalResources"];
            streamedResource["m_Source"].AsString = fileName;
            streamedResource["m_Offset"].AsULong = (ulong)offset;
            streamedResource["m_Size"].AsULong = (ulong)size;
        }

        private void WriteExternalResources(BinaryWriter writer, byte[] data, out long offset, out int size)
        {
            size = data.Length;
            offset = writer.BaseStream.Position;
            writer.Write(data);
        }

        private static byte[] GetExternalData(string filePath, ulong offset, uint size)
        {
            try
            {
                using FileStream fileStream = File.OpenRead(filePath);
                BinaryReader reader = new BinaryReader(fileStream);
                reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
                byte[] dataBytes = reader.ReadBytes((int)size);
                return dataBytes;
            }
            catch { }
            return Array.Empty<byte>();
        }

        private void ChangeMaterialShaderPptr(AssetTypeValueField materialBaseField, AssetPptr shaderPptr)
        {
            AssetTypeValueField shaderPptrField = materialBaseField["m_Shader"];
            ChangePptrField(shaderPptrField, shaderPptr);
        }

        /// <summary>
        /// Changes pointers in asset's base field.
        /// </summary>
        /// <param name="baseField">Asset's base field.</param>
        /// <param name="changeDict">Change dictionary. The key is an original pointer, the value is a new one.</param>
        private static void ChangeAssetPptrs(AssetTypeValueField baseField, Dictionary<AssetPptr, AssetPptr> changeDict)
        {
            foreach (var child in baseField.Children)
            {
                if (IsPptrField(child, out var assetPptr))
                {
                    if (changeDict.ContainsKey(assetPptr))
                        ChangePptrField(child, changeDict[assetPptr]);
                }
                else
                    ChangeAssetPptrs(child, changeDict);
            }
        }

        private static List<AssetPptr> GetAssetPptrs(AssetTypeValueField baseField, bool skipNullReferencePointers = true)
        {
            List<AssetPptr> assetPptrs = new List<AssetPptr>();
            foreach (var child in baseField.Children)
            {
                if (IsPptrField(child, out var assetPptr))
                {
                    if (assetPptr.IsNullReference && skipNullReferencePointers)
                        continue;
                    assetPptrs.Add(assetPptr);
                }
                else
                    assetPptrs.AddRange(GetAssetPptrs(child));
            }
            return assetPptrs;
        }

        private static void ChangePptrField(AssetTypeValueField pptrField, AssetPptr assetPptr)
        {
            pptrField["m_FileID"].AsInt = assetPptr.fileId;
            pptrField["m_PathID"].AsLong = assetPptr.pathId;
        }

        private static bool IsPptrField(AssetTypeValueField baseField, out AssetPptr assetPptr)
        {
            assetPptr = new AssetPptr();
            if (baseField == null || baseField.IsDummy || baseField.Children.Count != 2)
                return false;
            if (baseField.TypeName.StartsWith("PPtr"))
            {
                assetPptr.fileId = baseField["m_FileID"].AsInt;
                assetPptr.pathId = baseField["m_PathID"].AsLong;
                return true;
            }
            return false;
        }
    }
}
