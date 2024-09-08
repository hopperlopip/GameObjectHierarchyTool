using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool.GameObjectEditor
{
    internal class GameObjectEditorParams
    {
        AssetTypeValueField _gameObjectBase;

        public AssetTypeValueField GameObjectBase
        {
            get => _gameObjectBase;
        }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_gameObjectBase["m_Name"].AsString))
                {
                    return _gameObjectBase["m_Name"].AsString;
                }
                else
                {
                    return "GameObject";
                }
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _gameObjectBase["m_Name"].AsString = value;
                }
                else
                {
                    _gameObjectBase["m_Name"].AsString = "GameObject";
                }
            }
        }
        public uint Layer
        {
            get => _gameObjectBase["m_Layer"].AsUInt;
            set => _gameObjectBase["m_Layer"].AsUInt = value;
        }
        public ushort Tag
        {
            get => _gameObjectBase["m_Tag"].AsUShort;
            set => _gameObjectBase["m_Tag"].AsUShort = value;
        }
        public bool IsActive
        {
            get => _gameObjectBase["m_IsActive"].AsBool;
            set => _gameObjectBase["m_IsActive"].AsBool = value;
        }

        public GameObjectEditorParams(AssetTypeValueField gameObjectBase)
        {
            _gameObjectBase = gameObjectBase;
        }
    }
}
