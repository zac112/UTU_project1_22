using System;

namespace Unity.Netcode
{
    /// <summary>
    /// Interface for network value containers
    /// </summary>
    public abstract class NetworkVariableBase : IDisposable
    {
        /// <summary>
        /// The delivery type (QoS) to send data with
        /// </summary>
        internal const NetworkDelivery Delivery = NetworkDelivery.ReliableFragmentedSequenced;

        private protected NetworkBehaviour m_NetworkBehaviour;

        public void Initialize(NetworkBehaviour networkBehaviour)
        {
            m_NetworkBehaviour = networkBehaviour;
        }

        public const NetworkVariableReadPermission DefaultReadPerm = NetworkVariableReadPermission.Everyone;
        public const NetworkVariableWritePermission DefaultWritePerm = NetworkVariableWritePermission.Server;

        protected NetworkVariableBase(
            NetworkVariableReadPermission readPerm = DefaultReadPerm,
            NetworkVariableWritePermission writePerm = DefaultWritePerm)
        {
            ReadPerm = readPerm;
            WritePerm = writePerm;
        }

        private protected bool m_IsDirty;

        /// <summary>
        /// Gets or sets the name of the network variable's instance
        /// (MemberInfo) where it was declared.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The read permission for this var
        /// </summary>
        public readonly NetworkVariableReadPermission ReadPerm;

        public readonly NetworkVariableWritePermission WritePerm;

        /// <summary>
        /// Sets whether or not the variable needs to be delta synced
        /// </summary>
        public virtual void SetDirty(bool isDirty)
        {
            m_IsDirty = isDirty;
        }

        /// <summary>
        /// Resets the dirty state and marks the variable as synced / clean
        /// </summary>
        public virtual void ResetDirty()
        {
            m_IsDirty = false;
        }

        /// <summary>
        /// Gets Whether or not the container is dirty
        /// </summary>
        /// <returns>Whether or not the container is dirty</returns>
        public virtual bool IsDirty()
        {
            return m_IsDirty;
        }

        public bool CanClientRead(ulong clientId)
        {
            switch (ReadPerm)
            {
                default:
                case NetworkVariableReadPermission.Everyone:
                    return true;
                case NetworkVariableReadPermission.Owner:
                    return clientId == m_NetworkBehaviour.NetworkObject.OwnerClientId;
            }
        }

        public bool CanClientWrite(ulong clientId)
        {
            switch (WritePerm)
            {
                default:
                case NetworkVariableWritePermission.Server:
                    return clientId == NetworkManager.ServerClientId;
                case NetworkVariableWritePermission.Owner:
                    return clientId == m_NetworkBehaviour.NetworkObject.OwnerClientId;
            }
        }

        /// <summary>
        /// Returns the ClientId of the owning client
        /// </summary>
        internal ulong OwnerClientId()
        {
            return m_NetworkBehaviour.NetworkObject.OwnerClientId;
        }

        /// <summary>
        /// Writes the dirty changes, that is, the changes since the variable was last dirty, to the writer
        /// </summary>
        /// <param name="writer">The stream to write the dirty changes to</param>
        public abstract void WriteDelta(FastBufferWriter writer);

        /// <summary>
        /// Writes the complete state of the variable to the writer
        /// </summary>
        /// <param name="writer">The stream to write the state to</param>
        public abstract void WriteField(FastBufferWriter writer);

        /// <summary>
        /// Reads the complete state from the reader and applies it
        /// </summary>
        /// <param name="reader">The stream to read the state from</param>
        public abstract void ReadField(FastBufferReader reader);

        /// <summary>
        /// Reads delta from the reader and applies them to the internal value
        /// </summary>
        /// <param name="reader">The stream to read the delta from</param>
        /// <param name="keepDirtyDelta">Whether or not the delta should be kept as dirty or consumed</param>
        public abstract void ReadDelta(FastBufferReader reader, bool keepDirtyDelta);

        public virtual void Dispose()
        {
        }
    }
}
