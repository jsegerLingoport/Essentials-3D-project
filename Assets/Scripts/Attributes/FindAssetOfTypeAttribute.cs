using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FindAssetOfTypeAttribute : Attribute
    {
        public Type AssetType { get; }

        public FindAssetOfTypeAttribute(Type assetType)
        {
            AssetType = assetType;
        }
    }
}