using AssetsTools.NET;
using AssetsTools.NET.Extra;
using GameObjectHierarchyTool.XResources;
using System.IO;

namespace GameObjectHierarchyTool
{
    public class GameObjectHelper
    {
        private AssetsManager _manager;
        private AssetsFileInstance _fileInstance;
        private AssetsFile _assetsFile;

        public GameObjectHelper(AssetsManager manager, AssetsFileInstance fileInstance)
        {
            _manager = manager;
            _fileInstance = fileInstance;
            _assetsFile = fileInstance.file;
        }

        public GameObject GetGameObject(long pathId, CrossResources crossResources)
        {
            GameObject gameObject = new();
            var gameObjectBase = _manager.GetBaseField(_fileInstance, pathId);
            gameObject.name = gameObjectBase["m_Name"].AsString;
            gameObject.active = gameObjectBase["m_IsActive"].AsBool;
            gameObject.data = gameObjectBase.WriteToByteArray();
            gameObject.pathID = pathId;
            var componentsPPtrs = GetComponentsPPtrs(pathId);
            for (int i = 0; i < componentsPPtrs.Count; i++)
            {
                var componentExtInfo = _manager.GetExtAsset(_fileInstance, componentsPPtrs[i]);
                int componentType = componentExtInfo.info.TypeId;

                if (componentType != (int)AssetClassID.Transform && componentType != (int)AssetClassID.RectTransform)
                {
                    if (MainWindow.IsCollectCrossResourcesEnabled)
                    {
                        if (!crossResources.dependencyTable.ContainsKey(componentExtInfo.file.name))
                        {
                            crossResources.dependencyTable.Add(componentExtInfo.file.name, CrossResources.GetDependencyArray(componentExtInfo.file));
                        }
                        crossResources.LoadAllCrossAssets(componentExtInfo, true);
                    }
                }

                byte[] componentData;
                if (componentType == (int)AssetClassID.MonoBehaviour)
                {
                    componentData = GetMonoBehaviourAssetBytes(_assetsFile, componentExtInfo.info);
                }
                else
                {
                    componentData = componentExtInfo.baseField.WriteToByteArray();
                }
                string componentResourceName = componentExtInfo.file.name;
                gameObject.components.Add(new Component(componentType, componentData, componentResourceName));
            }

            return gameObject;
        }

        public List<GameObjectHierarchy> GetChildren(GameObject gameObject, CrossResources crossResources)
        {
            var transformBase = _manager.GetBaseField(_fileInstance, GetTransformPathId(gameObject.pathID));
            var childrenTransform = transformBase["m_Children.Array"];
            List<GameObjectHierarchy> children = new();
            for (int i = 0; i < childrenTransform.Children.Count; i++)
            {
                var childTransformPointer = childrenTransform.Children[i];
                var childTransformExtInfo = _manager.GetExtAsset(_fileInstance, childTransformPointer);
                long childGameObjectPathId = childTransformExtInfo.baseField["m_GameObject.m_PathID"].AsLong;
                children.Add(new GameObjectHierarchy(GetGameObject(childGameObjectPathId, crossResources), new(), crossResources));
            }
            return children;
        }

        public List<long> GetChildrenPathIds(long gameObjectPathId)
        {
            long transformPathId = GetTransformPathId(gameObjectPathId);
            var transformBase = _manager.GetBaseField(_fileInstance, transformPathId);
            var childrenTransform = transformBase["m_Children.Array"];
            List<long> childrenPathIds = new();
            for (int i = 0; i < childrenTransform.Children.Count; i++)
            {
                var childTransformPointer = childrenTransform.Children[i];
                long childTransformPathId = childTransformPointer["m_PathID"].AsLong;
                long childGameObjectPathId = GetGameObjectPathId(childTransformPathId);
                childrenPathIds.Add(childGameObjectPathId);
            }
            return childrenPathIds;
        }

        public List<AssetTypeValueField> GetComponentsPPtrs(long gameObjectPathId)
        {
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectPathId);
            var components = gameObjectBase["m_Component.Array"];
            List<AssetTypeValueField> componentsPPtrs = new();
            for (int i = 0; i < components.Children.Count; i++)
            {
                var componentArrayData = components.Children[i];
                var componentPointer = componentArrayData["component"];
                componentsPPtrs.Add(componentPointer);
            }
            return componentsPPtrs;
        }

        public List<AssetExternal> GetComponents(long gameObjectPathId)
        {
            var componentsPPtrs = GetComponentsPPtrs(gameObjectPathId);
            List<AssetExternal> components = new();
            foreach (var componentPPtr in componentsPPtrs)
            {
                var componentExtInfo = _manager.GetExtAsset(_fileInstance, componentPPtr);
                components.Add(componentExtInfo);
            }
            return components;
        }

        public void ReplaceChildrenPathIds(long gameObjectPathId, List<long> childrenPathIds)
        {
            long transformPathId = GetTransformPathId(gameObjectPathId);
            var transformInfo = _assetsFile.GetAssetInfo(transformPathId);
            var transformBase = _manager.GetBaseField(_fileInstance, transformInfo);
            var childrenTransform = transformBase["m_Children.Array"];
            childrenTransform.Children.Clear();
            for (int i = 0; i < childrenPathIds.Count; i++)
            {
                var newChildrenArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(childrenTransform);
                newChildrenArrayItem["m_FileID"].AsInt = 0;
                newChildrenArrayItem["m_PathID"].AsLong = GetTransformPathId(childrenPathIds[i]);
                childrenTransform.Children.Add(newChildrenArrayItem);
            }
            transformInfo.SetNewData(transformBase);
        }

        public long GetFatherPathId(long gameObjectPathId)
        {
            long transformPathId = GetTransformPathId(gameObjectPathId);
            var transformBase = _manager.GetBaseField(_fileInstance, transformPathId);
            long fatherTransformPathId = transformBase["m_Father.m_PathID"].AsLong;
            long fatherPathId = GetGameObjectPathId(fatherTransformPathId);
            return fatherPathId;
        }

        public void ReplaceFatherPathId(long gameObjectPathId, long newFatherPathId)
        {
            long transformPathId = GetTransformPathId(gameObjectPathId);
            var transformInfo = _assetsFile.GetAssetInfo(transformPathId);
            var transformBase = _manager.GetBaseField(_fileInstance, transformInfo);
            transformBase["m_Father.m_PathID"].AsLong = GetTransformPathId(newFatherPathId);
            transformInfo.SetNewData(transformBase);
        }

        public GameObjectHierarchy GetHierarchy(GameObject gameObject, CrossResources crossResources)
        {
            GameObjectHierarchy gameObjectHierarchy = new GameObjectHierarchy(gameObject, GetChildren(gameObject, crossResources), crossResources);
            for (int i = 0; i < gameObjectHierarchy.children.Count; i++)
            {
                if (gameObjectHierarchy.children[i].gameObject.pathID == gameObject.pathID)
                {
                    throw new Exception("Child PathID is same for Farther PathID. Endless loop.");
                }
                gameObjectHierarchy.children[i] = GetHierarchy(gameObjectHierarchy.children[i].gameObject, crossResources);
            }
            return gameObjectHierarchy;
        }

        public AssetFileInfo CreateAsset(long pathId, AssetClassID typeId)
        {
            return CreateAsset(_fileInstance, _manager, pathId, typeId);
        }

        public static AssetFileInfo CreateAsset(AssetsFileInstance fileInstance, AssetsManager manager, long pathId, AssetClassID typeId)
        {
            var assetsFile = fileInstance.file;
            var assetInfo = AssetFileInfo.Create(assetsFile, pathId, (int)typeId, manager.ClassDatabase);
            var assetBase = manager.CreateValueBaseField(fileInstance, (int)typeId);
            assetInfo.SetNewData(assetBase);
            assetsFile.Metadata.AddAssetInfo(assetInfo);
            return assetInfo;
        }

        public AssetFileInfo CreateGameObject(long pathId)
        {
            return CreateAsset(pathId, AssetClassID.GameObject);
        }

        public AssetFileInfo CreateTransform(long pathId)
        {
            return CreateAsset(pathId, AssetClassID.Transform);
        }

        public void CreateGameObject(GameObject gameObject, CrossResources crossResources)
        {
            gameObject.pathID = GetNewPathId();
            var gameObjectInfo = CreateGameObject(gameObject.pathID);
            gameObjectInfo.SetNewData(gameObject.data);
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectInfo);
            gameObjectBase["m_Name"].AsString = gameObject.name;
            gameObjectBase["m_IsActive"].AsBool = gameObject.active;
            var components = gameObjectBase["m_Component.Array"];
            components.Children.Clear();

            for (int i = 0; i < gameObject.components.Count; i++)
            {
                Component component = gameObject.components[i];
                long componentPathID = GetNewPathId();
                var componentInfo = AssetFileInfo.Create(_assetsFile, componentPathID, component.typeId, _manager.ClassDatabase, false);
                if (componentInfo.TypeId == (int)AssetClassID.MonoBehaviour && !MainWindow.IsMonoBehaviourParsingEnabled)
                {
                    component.data = ChangeMonoBehaviourGameObjectPathId(component.data, gameObject.pathID);
                }
                else
                {
                    componentInfo.SetNewData(component.data);
                    var componentBase = _manager.GetBaseField(_fileInstance, componentInfo);
                    componentBase["m_GameObject.m_PathID"].AsLong = gameObject.pathID;
                    if (componentInfo.TypeId != (int)AssetClassID.Transform && componentInfo.TypeId != (int)AssetClassID.RectTransform)
                        crossResources.FixPointersInAssetBaseField(componentBase, component.resourceName, true);
                    component.data = componentBase.WriteToByteArray();
                }
                componentInfo.SetNewData(component.data);
                _assetsFile.Metadata.AddAssetInfo(componentInfo);

                var newArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(components);
                newArrayItem["component.m_FileID"].AsInt = 0;
                newArrayItem["component.m_PathID"].AsLong = componentInfo.PathId;
                components.Children.Add(newArrayItem);
            }
            gameObjectInfo.SetNewData(gameObjectBase);
        }

        /// <summary>
        /// Creates GameObject with its hierarchy from the <see cref="GameObjectHierarchy"/>.
        /// </summary>
        /// <param name="gameObjectHierarchy"></param>
        /// <param name="fatherPathId"></param>
        /// <returns>GameObject path ID of created GameObject</returns>
        public long CreateHierarchy(GameObjectHierarchy gameObjectHierarchy, long fatherPathId = 0)
        {
            CreateGameObject(gameObjectHierarchy.gameObject, gameObjectHierarchy.crossResources);
            AddGameObjectToTheFather(gameObjectHierarchy.gameObject.pathID, fatherPathId);
            var transformInfo = _assetsFile.GetAssetInfo(GetTransformPathId(gameObjectHierarchy.gameObject.pathID));
            var transformBase = _manager.GetBaseField(_fileInstance, transformInfo);
            var childrenTransform = transformBase["m_Children.Array"];
            childrenTransform.Children.Clear();
            transformInfo.SetNewData(transformBase);

            var children = gameObjectHierarchy.children;
            for (int i = 0; i < children.Count; i++)
            {
                GameObjectHierarchy child = children[i];
                CreateHierarchy(child, gameObjectHierarchy.gameObject.pathID);
            }
            return gameObjectHierarchy.gameObject.pathID;
        }

        public long GetNewPathId()
        {
            return GetNewPathId(_fileInstance);
        }

        public static long GetNewPathId(AssetsFileInstance fileInstance)
        {
            // Starting with 1 because 0 path ID is reserved as null reference.
            for (long newPathId = 1; newPathId <= long.MaxValue; newPathId++)
            {
                AssetFileInfo assetFileInfo = fileInstance.file.GetAssetInfo(newPathId);
                if (assetFileInfo == null)
                    return newPathId;
            }
            //Trying to check all negative path IDs.
            for (long newPathId = -1; newPathId >= long.MinValue; newPathId--)
            {
                AssetFileInfo assetFileInfo = fileInstance.file.GetAssetInfo(newPathId);
                if (assetFileInfo == null)
                    return newPathId;
            }
            throw new Exception("There is no free PathID. All PathIDs are taken.");
        }

        private void AddGameObjectToTheFather(long gameObjectPathId, long fatherPathId)
        {
            ReplaceFatherPathId(gameObjectPathId, fatherPathId);
            if (fatherPathId == 0)
                return;
            long fatherTransformPathId = GetTransformPathId(fatherPathId);
            var fatherTransformInfo = _assetsFile.GetAssetInfo(fatherTransformPathId);
            var fatherTransformBase = _manager.GetBaseField(_fileInstance, fatherTransformInfo);
            var childrenFatherTransform = fatherTransformBase["m_Children.Array"];
            var newChildrenArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(childrenFatherTransform);
            newChildrenArrayItem["m_FileID"].AsInt = 0;
            newChildrenArrayItem["m_PathID"].AsLong = GetTransformPathId(gameObjectPathId);
            childrenFatherTransform.Children.Add(newChildrenArrayItem);
            fatherTransformInfo.SetNewData(fatherTransformBase);
        }

        public bool HasCldbSpecifiedTypeId(AssetClassID typeId)
        {
            var cldbType = _manager.ClassDatabase.FindAssetClassByID((int)typeId);
            if (cldbType == null)
                return false;
            return true;
        }

        public bool HasCldbTypeRootNodes(AssetClassID typeId)
        {
            var cldbType = _manager.ClassDatabase.FindAssetClassByID((int)typeId);
            if (cldbType.EditorRootNode == null && cldbType.ReleaseRootNode == null)
                return false;
            return true;
        }

        public bool IsValidComponentType(AssetClassID componentTypeId)
        {
            var componentBase = _manager.CreateValueBaseField(_fileInstance, (int)componentTypeId);
            if (componentBase["m_GameObject"].IsDummy)
                return false;
            return true;
        }

        public long AddComponent(long gameObjectPathId, AssetClassID componentTypeId)
        {
            //Creating component
            if (componentTypeId == AssetClassID.Transform || componentTypeId == AssetClassID.RectTransform)
                throw new Exception("You shouldn't create new Transform/RectTransform in a GameObject.");
            long componentPathId = GetNewPathId();
            var componentInfo = CreateAsset(componentPathId, componentTypeId);
            var componentBase = _manager.GetBaseField(_fileInstance, componentInfo);
            if (componentBase["m_GameObject"].IsDummy)
                throw new Exception("Can't add component because it doesn't have the \"m_GameObject\" field.");
            componentBase["m_GameObject.m_PathID"].AsLong = gameObjectPathId;
            componentInfo.SetNewData(componentBase);

            //Adding component to the GameObject
            var gameObjectInfo = _assetsFile.GetAssetInfo(gameObjectPathId);
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectInfo);
            var components = gameObjectBase["m_Component.Array"];

            var newArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(components);
            newArrayItem["component.m_FileID"].AsInt = 0;
            newArrayItem["component.m_PathID"].AsLong = componentPathId;
            components.Children.Add(newArrayItem);

            gameObjectInfo.SetNewData(gameObjectBase);

            return componentPathId;
        }

        public void RemoveComponent(long componentPathId)
        {
            var componentInfo = _assetsFile.GetAssetInfo(componentPathId);
            if (componentInfo.TypeId == (int)AssetClassID.Transform || componentInfo.TypeId == (int)AssetClassID.RectTransform)
                throw new Exception("You shouldn't remove Transform/RectTransform from a GameObject.");
            var componentBase = _manager.GetBaseField(_fileInstance, componentInfo);
            var gameObjectPPtrField = componentBase["m_GameObject"];
            AssetExternal gameObjectExt = _manager.GetExtAsset(_fileInstance, gameObjectPPtrField);
            var gameObjectInfo = gameObjectExt.info;
            var gameObjectBase = gameObjectExt.baseField;
            var components = gameObjectBase["m_Component.Array"];
            foreach ( var component in components.Children)
            {
                if (component["component.m_PathID"].AsLong == componentPathId)
                {
                    components.Children.Remove(component);
                    break;
                }
            }
            gameObjectInfo.SetNewData(gameObjectBase);
            _assetsFile.Metadata.RemoveAssetInfo(componentInfo);
        }

        public void RenameGameObject(long pathId, string newName)
        {
            var gameObjectInfo = _assetsFile.GetAssetInfo(pathId);
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectInfo);
            gameObjectBase["m_Name"].AsString = newName;
            gameObjectInfo.SetNewData(gameObjectBase);
        }

        public string GetGameObjectName(long pathId)
        {
            var gameObjectBase = _manager.GetBaseField(_fileInstance, pathId);
            return gameObjectBase["m_Name"].AsString;
        }

        public void ChangeActiveState(long pathId, bool activeState)
        {
            var gameObjectInfo = _assetsFile.GetAssetInfo(pathId);
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectInfo);
            gameObjectBase["m_IsActive"].AsBool = activeState;
            gameObjectInfo.SetNewData(gameObjectBase);
        }

        public bool GetActiveState(long pathId)
        {
            var gameObjectInfo = _assetsFile.GetAssetInfo(pathId);
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectInfo);
            return gameObjectBase["m_IsActive"].AsBool;
        }

        private static byte[] ChangeMonoBehaviourGameObjectPathId(byte[] data, long gameObjectPathID)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(data, 0, 4);
            binaryWriter.Write(gameObjectPathID);
            int currentPosition = Convert.ToInt32(binaryWriter.BaseStream.Position);
            binaryWriter.Write(data, currentPosition, data.Length - currentPosition);
            return memoryStream.ToArray();
        }

        public long GetTransformPathId(long gameObjectPathId)
        {
            if (gameObjectPathId == 0)
            {
                return 0;
            }
            var gameObjectBase = _manager.GetBaseField(_fileInstance, gameObjectPathId);
            var components = gameObjectBase["m_Component.Array"];
            for (int i = 0; i < components.Children.Count; i++)
            {
                var componentData = components.Children[i];
                var componentPointer = componentData["component"];
                var componentExtInfo = _manager.GetExtAsset(_fileInstance, componentPointer, true);
                int componentType = componentExtInfo.info.TypeId;
                long componentPathId = componentExtInfo.info.PathId;
                if (componentType == (int)AssetClassID.Transform || componentType == (int)AssetClassID.RectTransform)
                {
                    return componentPathId;
                }
            }
            return 0;
        }

        public long GetTransformPathId(AssetTypeValueField gameObjectBase)
        {
            var components = gameObjectBase["m_Component.Array"];
            for (int i = 0; i < components.Children.Count; i++)
            {
                var componentData = components.Children[i];
                var componentPointer = componentData["component"];
                var componentExtInfo = _manager.GetExtAsset(_fileInstance, componentPointer, true);
                int componentType = componentExtInfo.info.TypeId;
                long componentPathId = componentExtInfo.info.PathId;
                if (componentType == (int)AssetClassID.Transform || componentType == (int)AssetClassID.RectTransform)
                {
                    return componentPathId;
                }
            }
            return 0;
        }

        public long GetGameObjectPathId(long transformPathId)
        {
            if (transformPathId == 0)
            {
                return 0;
            }
            var transformBase = _manager.GetBaseField(_fileInstance, transformPathId);
            return transformBase["m_GameObject.m_PathID"].AsLong;
        }

        public long GetGameObjectPathId(AssetTypeValueField transformBase)
        {
            return transformBase["m_GameObject.m_PathID"].AsLong;
        }

        public static byte[] GetAssetBytes(AssetsFile assetsFile, AssetFileInfo assetFileInfo)
        {
            assetsFile.Reader.Position = assetFileInfo.GetAbsoluteByteOffset(assetsFile);
            return assetsFile.Reader.ReadBytes(Convert.ToInt32(assetFileInfo.ByteSize));
        }

        public static byte[] GetMonoBehaviourAssetBytes(AssetsFile assetsFile, AssetFileInfo assetFileInfo)
        {
            byte[] assetData;
            if (!assetFileInfo.IsReplacerPreviewable)
            {
                // Workaround to get valid MonoBehaviour data for already existing GameObject
                assetData = GetAssetBytes(assetsFile, assetFileInfo);
            }
            else
            {
                // Workaround to get valid MonoBehaviour data for imported GameObject
                MemoryStream ms = new();
                assetFileInfo.Replacer.GetPreviewStream().CopyTo(ms);
                assetData = ms.ToArray();
            }
            return assetData;
        }

        public void ChangeGameObjectFather(long gameObjectPathId, long newFatherPathId)
        {
            long oldFatherPathId = GetFatherPathId(gameObjectPathId);

            //Deleting GameObject from FatherGameObject
            if (oldFatherPathId != 0)
            {
                List<long> oldChildrenPathIds = GetChildrenPathIds(oldFatherPathId);
                oldChildrenPathIds.Remove(gameObjectPathId);
                ReplaceChildrenPathIds(oldFatherPathId, oldChildrenPathIds);
            }

            //Adding GameObject to new FatherGameObject
            if (newFatherPathId != 0)
            {
                List<long> newChildrenPathIds = GetChildrenPathIds(newFatherPathId);
                newChildrenPathIds.Add(gameObjectPathId);
                ReplaceChildrenPathIds(newFatherPathId, newChildrenPathIds);
            }

            //Replacing FatherGameObject info in the GameObject
            ReplaceFatherPathId(gameObjectPathId, newFatherPathId);
        }

        public void RemoveGameObject(long gameObjectPathId)
        {
            var gameObjectInfo = _assetsFile.GetAssetInfo(gameObjectPathId);
            var componentsPPtrs = GetComponentsPPtrs(gameObjectPathId);
            for (int i = 0; i < componentsPPtrs.Count; i++)
            {
                var componentExtInfo = _manager.GetExtAsset(_fileInstance, componentsPPtrs[i], true);
                _assetsFile.Metadata.RemoveAssetInfo(componentExtInfo.info);
                //componentExtInfo.info.SetRemoved();
            }
            //Works too slow
            //RemoveFatherChildPPtr(gameObjectPathId);

            _assetsFile.Metadata.RemoveAssetInfo(gameObjectInfo);
            //gameObjectInfo.SetRemoved();
        }

        public void RemoveHierarchy(long gameObjectPathId, bool removeFatherChildPPtr = true)
        {
            if (removeFatherChildPPtr)
                RemoveFatherChildPPtr(gameObjectPathId);
            List<long> childrenPathIds = GetChildrenPathIds(gameObjectPathId);
            for (int i = 0; i < childrenPathIds.Count; i++)
            {
                long childPathId = childrenPathIds[i];
                RemoveHierarchy(childPathId, false);
            }
            RemoveGameObject(gameObjectPathId);
        }

        public void RemoveFatherChildPPtr(long gameObjectPathId)
        {
            long fatherPathId = GetFatherPathId(gameObjectPathId);
            if (fatherPathId != 0)
            {
                List<long> fatherChildrenPathIds = GetChildrenPathIds(fatherPathId);
                fatherChildrenPathIds.Remove(gameObjectPathId);
                ReplaceChildrenPathIds(fatherPathId, fatherChildrenPathIds);
            }
        }
    }
}
