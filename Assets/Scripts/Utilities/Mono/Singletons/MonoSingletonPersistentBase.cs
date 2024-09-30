using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Mono.Singletons
{
    public abstract class MonoSingletonPersistentBase : SerializedMonoBehaviour
    {
        protected static readonly object Lock = new();
    }
}