using UnityEngine;
using Unity.Netcode;

namespace LindoNoxStudio.Network.Input
{
    public class ClientInputState : INetworkSerializable
    {
        public uint Tick;
        public float PlayerRotation;
        private byte _essentialKeys;
        public bool IsSprinting => (_essentialKeys & (1 << 4)) != 0;
        public bool IsJumping => (_essentialKeys & (1 << 5)) != 0;
        public bool IsCrouching => (_essentialKeys & (1 << 6)) != 0;

        public void SetUp(uint tick, Vector2 moveInput, bool isSprinting, bool isJumping, bool isCrouching, float playerRotation)
        {
            Tick = tick;
            PlayerRotation = playerRotation;

            _essentialKeys = 0;
            if (moveInput.y > 0) _essentialKeys |= 1 << 0;
            if (moveInput.x < 0) _essentialKeys |= 1 << 1;
            if (moveInput.y < 0) _essentialKeys |= 1 << 2;
            if (moveInput.x > 0) _essentialKeys |= 1 << 3;
            if (isSprinting) _essentialKeys |= 1 << 4;
            if (isJumping) _essentialKeys |= 1 << 5;
            if (isCrouching) _essentialKeys |= 1 << 6;
            //if (isCrouching) essentialKeys |= 1 << 7; Empty Slot to use in the future
        }

        public Vector2 GetMoveInput()
        {
            return new Vector2(
                (_essentialKeys & (1 << 1)) != 0 ? -1 :
                (_essentialKeys & (1 << 3)) != 0 ? 1 : 0,
                (_essentialKeys & (1 << 0)) != 0 ? 1 :
                (_essentialKeys & (1 << 2)) != 0 ? -1 : 0
            );
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref PlayerRotation);
            serializer.SerializeValue(ref _essentialKeys);
        }
    }
}