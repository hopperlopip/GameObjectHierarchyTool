using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace GameObjectHierarchyTool.ComponentEditor
{
    internal class Component
    {
        readonly AssetsManager _manager;
        readonly AssetsFileInstance _fileInstance;
        private AssetExternal _componentExt;
        private byte[]? _monoBehaviourBytes = null;

        public bool AllowEnable
        {
            get => !ComponentBase["m_Enabled"].IsDummy;
        }

        public bool IsTransform
        {
            get => TypeId == (int)AssetClassID.Transform || TypeId == (int)AssetClassID.RectTransform;
        }

        private bool IsMonoBehaviour => TypeId == (int)AssetClassID.MonoBehaviour;

        public bool IsEnabled
        {
            get
            {
                if (AllowEnable)
                    return ComponentBase["m_Enabled"].AsBool;
                return true;
            }

            set
            {
                if (AllowEnable)
                    if (IsMonoBehaviour)
                        ChangeMonoBehaviourEnabledState(ComponentBase, value);
                    else
                        ComponentBase["m_Enabled"].AsBool = value;
            }
        }

        public AssetFileInfo ComponentInfo
        {
            get => _componentExt.info;
        }

        public AssetTypeValueField ComponentBase
        {
            get => _componentExt.baseField;
        }

        public int TypeId
        {
            get => ComponentInfo.TypeId;
        }

        public long PathId
        {
            get => ComponentInfo.PathId;
        }

        public string TypeIdName
        {
            get => Enum.GetName(typeof(AssetClassID), TypeId) ?? string.Empty;
        }

        public string Name
        {
            get
            {
                if (TypeId == (int)AssetClassID.MonoBehaviour)
                {
                    var monoBehaviourBase = ComponentBase;
                    string? monoScriptName = GetMonoBehaviourScriptName(monoBehaviourBase);
                    if (monoScriptName != null)
                        return $"{TypeIdName} ({monoScriptName})";
                    else
                        return TypeIdName;
                }
                else
                    return TypeIdName;
            }
        }

        public Component(AssetsManager manager, AssetsFileInstance fileInstance, AssetExternal componentExt)
        {
            _manager = manager;
            _fileInstance = fileInstance;
            _componentExt = componentExt;
        }

        public Component(AssetsManager manager, AssetsFileInstance fileInstance, long componentPathId)
        {
            AssetExternal componentExt = new AssetExternal();
            componentExt.file = fileInstance;
            componentExt.info = fileInstance.file.GetAssetInfo(componentPathId);
            componentExt.baseField = manager.GetBaseField(fileInstance, componentExt.info);

            _manager = manager;
            _fileInstance = fileInstance;
            _componentExt = componentExt;
        }

        private string? GetMonoBehaviourScriptName(AssetTypeValueField monoBehaviourBase)
        {
            var monoScriptPPtr = monoBehaviourBase["m_Script"];
            AssetExternal monoScriptExt = _manager.GetExtAsset(_fileInstance, monoScriptPPtr);
            var monoScriptBase = monoScriptExt.baseField;
            if (monoScriptBase == null)
                return null;
            string monoScriptName = monoScriptBase["m_Name"].AsString;
            return monoScriptName;
        }

        private void ChangeMonoBehaviourEnabledState(AssetTypeValueField monoBehaviourBase, bool enabledState)
        {
            if (_monoBehaviourBytes == null || _monoBehaviourBytes.Length == 0)
                _monoBehaviourBytes = GameObjectHelper.GetMonoBehaviourAssetBytes(_fileInstance.file, ComponentInfo);
            _monoBehaviourBytes[12] = (byte)(enabledState ? 1 : 0);
        }

        public void ChangeAllFileIDFields(AssetTypeValueField baseField, int? oldFileId, int newFileId, bool skipGameObjectField = true)
        {
            foreach (var child in baseField.Children)
            {
                string childName = child.FieldName;
                AssetValueType childType = child.TemplateField.ValueType;
                if (skipGameObjectField && childName == "m_GameObject" && childType == AssetValueType.None)
                    continue;
                if (childName == "m_FileID" && childType == AssetValueType.Int32)
                {
                    if (oldFileId == null || child.AsInt == oldFileId)
                    {
                        child.AsInt = newFileId;
                    }
                }
                ChangeAllFileIDFields(child, oldFileId, newFileId, false);
            }
        }

        public void SaveChanges()
        {
            if (IsMonoBehaviour && _monoBehaviourBytes != null)
                ComponentInfo.SetNewData(_monoBehaviourBytes);
            else
                ComponentInfo.SetNewData(ComponentBase);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
