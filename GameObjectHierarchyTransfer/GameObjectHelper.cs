using AssetsTools.NET.Extra;
using AssetsTools.NET;

namespace GameObjectHierarchyTransfer
{
    public class GameObjectHelper
    {
        AssetsManager manager;
        AssetsFileInstance fileInstance;
        AssetsFile assetsFile;

        public GameObjectHelper(AssetsManager manager, AssetsFileInstance fileInstance)
        {
            this.manager = manager;
            this.fileInstance = fileInstance;
            assetsFile = fileInstance.file;
        }

        public GameObject GetGameObject(long pathId)
        {
            GameObject gameObject = new();
            AssetTypeValueField gameObjectBase = manager.GetBaseField(fileInstance, pathId);
            gameObject.data = gameObjectBase.WriteToByteArray();
            gameObject.pathID = pathId;
            var components = gameObjectBase["m_Component.Array"];

            for (int i = 0; i < components.Children.Count; i++)
            {
                var componentArrayData = components.Children[i];
                var componentPointer = componentArrayData["component"];
                var componentExtInfo = manager.GetExtAsset(fileInstance, componentPointer);
                int componentType = componentExtInfo.info.TypeId;
                byte[] componentData;
                if (componentType == (int)AssetClassID.MonoBehaviour)
                {
                    componentData = GetAssetBytes(assetsFile, componentExtInfo.info);
                }
                else
                {
                    componentData = componentExtInfo.baseField.WriteToByteArray();
                }
                gameObject.components.Add(new Component(componentType, componentData));

                if (componentType == (int)AssetClassID.Transform || componentType == (int)AssetClassID.RectTransform)
                {
                    gameObject.transformPathID = componentExtInfo.info.PathId;
                }
            }

            return gameObject;
        }

        public List<GameObjectHierarchy> GetChildren(GameObject gameObject)
        {
            AssetTypeValueField transformBase = manager.GetBaseField(fileInstance, gameObject.transformPathID);
            var childrenTransform = transformBase["m_Children.Array"];
            List<GameObjectHierarchy> children = new();
            for (int j = 0; j < childrenTransform.Children.Count; j++)
            {
                var childTransformPointer = childrenTransform.Children[j];
                var childTransformExtInfo = manager.GetExtAsset(fileInstance, childTransformPointer);
                long childGameObjectPathId = childTransformExtInfo.baseField["m_GameObject.m_PathID"].AsLong;
                children.Add(new GameObjectHierarchy(GetGameObject(childGameObjectPathId), new()));
            }
            return children;
        }

        public GameObjectHierarchy GetHierarchy(GameObject gameObject)
        {
            GameObjectHierarchy gameObjectHierarchy = new GameObjectHierarchy(gameObject, GetChildren(gameObject));
            for (int i = 0; i < gameObjectHierarchy.children.Count; i++)
            {
                if (gameObjectHierarchy.children[i].gameObject.pathID == gameObject.pathID)
                {
                    throw new Exception("Child PathID is same for Farther PathID. Endless loop.");
                }
                gameObjectHierarchy.children[i] = GetHierarchy(gameObjectHierarchy.children[i].gameObject);
            }
            return gameObjectHierarchy;
        }

        public void CreateGameObject(GameObject gameObject)
        {
            gameObject.pathID = assetsFile.AssetInfos.Count + 1;
            var gameObjectInfo = AssetFileInfo.Create(assetsFile, gameObject.pathID, (int)AssetClassID.GameObject, manager.ClassDatabase, false);
            gameObjectInfo.SetNewData(gameObject.data);
            var gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
            var components = gameObjectBase["m_Component.Array"];
            components.Children.Clear();
            assetsFile.Metadata.AddAssetInfo(gameObjectInfo);

            for (int i = 0; i < gameObject.components.Count; i++)
            {
                Component component = gameObject.components[i];
                int componentPathID = assetsFile.AssetInfos.Count + 1;
                var componentInfo = AssetFileInfo.Create(assetsFile, componentPathID, component.typeId, manager.ClassDatabase, false);
                if (componentInfo.TypeId == (int)AssetClassID.MonoBehaviour)
                {
                    component.data = ChangeMonoBehaviourGameObjectPathId(component.data, gameObject.pathID);
                }
                else
                {
                    componentInfo.SetNewData(component.data);
                    var componentBase = manager.GetBaseField(fileInstance, componentInfo);
                    componentBase["m_GameObject.m_PathID"].AsLong = gameObject.pathID;
                    component.data = componentBase.WriteToByteArray();
                }
                componentInfo.SetNewData(component.data);
                assetsFile.Metadata.AddAssetInfo(componentInfo);

                if (componentInfo.TypeId == (int)AssetClassID.Transform || componentInfo.TypeId == (int)AssetClassID.RectTransform)
                {
                    gameObject.transformPathID = componentInfo.PathId;
                }

                var newArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(components);
                newArrayItem["component.m_FileID"].AsInt = 0;
                newArrayItem["component.m_PathID"].AsLong = componentInfo.PathId;
                components.Children.Add(newArrayItem);
            }
            gameObjectInfo.SetNewData(gameObjectBase);
        }

        public void CreateHierarchy(GameObjectHierarchy gameObjectHierarchy, long fatherPathId)
        {
            CreateGameObject(gameObjectHierarchy.gameObject);
            var transformBase = manager.GetBaseField(fileInstance, gameObjectHierarchy.gameObject.transformPathID);
            transformBase["m_Father.m_PathID"].AsLong = GetTransformPathId(fatherPathId);
            var childrenTransform = transformBase["m_Children.Array"];
            var newChildrenArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(childrenTransform);
            childrenTransform.Children.Clear();

            var children = gameObjectHierarchy.children;
            for (int i = 0; i < children.Count; i++)
            {
                CreateHierarchy(children[i], gameObjectHierarchy.gameObject.pathID);
                GameObject childGameObject = children[i].gameObject;
                newChildrenArrayItem["m_FileID"].AsInt = 0;
                newChildrenArrayItem["m_PathID"].AsLong = childGameObject.pathID;
                childrenTransform.Children.Add(newChildrenArrayItem);
            }
        }

        public static byte[] ChangeMonoBehaviourGameObjectPathId(byte[] data, long gameObjectPathID)
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
            var gameObjectBase = manager.GetBaseField(fileInstance, gameObjectPathId);
            var components = gameObjectBase["m_Component.Array"];
            for (int i = 0; i < components.Children.Count; i++)
            {
                var componentData = components.Children[i];
                var componentPointer = componentData["component"];
                var componentExtInfo = manager.GetExtAsset(fileInstance, componentPointer);
                int componentType = componentExtInfo.info.TypeId;
                long componentPathId = componentExtInfo.info.PathId;
                if (componentType == (int)AssetClassID.Transform || componentType == (int)AssetClassID.RectTransform)
                {
                    return componentPathId;
                }
            }
            return 0;
        }

        private byte[] GetAssetBytes(AssetsFile assetFile, AssetFileInfo assetFileInfo)
        {
            assetFile.Reader.Position = assetFileInfo.GetAbsoluteByteOffset(assetFile);
            return assetFile.Reader.ReadBytes(Convert.ToInt32(assetFileInfo.ByteSize));
        }
    }
}
