using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FindAllOfInterfaceAttribute : Attribute
    {
        public Type InterfaceType { get; }

        public FindAllOfInterfaceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}