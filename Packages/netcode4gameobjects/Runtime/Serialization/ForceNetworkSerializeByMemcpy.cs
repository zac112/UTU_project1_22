using System;

namespace Unity.Netcode
{
    /// <summary>
    /// This is a wrapper that adds `INetworkSerializeByMemcpy` support to existing structs that the developer
    /// doesn't have the ability to modify (for example, external structs like `Guid`).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ForceNetworkSerializeByMemcpy<T> : INetworkSerializeByMemcpy, IEquatable<ForceNetworkSerializeByMemcpy<T>> where T : unmanaged, IEquatable<T>
    {
        public T Value;

        public ForceNetworkSerializeByMemcpy(T value)
        {
            Value = value;
        }

        public static implicit operator T(ForceNetworkSerializeByMemcpy<T> container) => container.Value;
        public static implicit operator ForceNetworkSerializeByMemcpy<T>(T underlyingValue) => new ForceNetworkSerializeByMemcpy<T> { Value = underlyingValue };

        public bool Equals(ForceNetworkSerializeByMemcpy<T> other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is ForceNetworkSerializeByMemcpy<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
