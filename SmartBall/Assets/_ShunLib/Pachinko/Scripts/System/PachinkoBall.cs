using UnityEngine;

using SoftGear.Strix.Unity.Runtime;

using ShunLib.Strix.Manager.Room;

namespace Pachinko.Ball
{
    public class PachinkoBall : StrixBehaviour
    {
        [Header("RigidBody")]
        [SerializeField] private Rigidbody _rb = default;

        private bool _isActive = default;

        public void SetActiveAllClone(bool b)
        {
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoBall.SetActive),
                    (handler) => { },
                    (handler) => { Debug.Log(handler.cause.Message); },
                    b
                );
            }
            else
            {
                SetActive(b);
            }
        }

        [StrixRpc]
        public void SetActive(bool b)
        {
            _isActive = b;
            this.gameObject.SetActive(_isActive);
        }

        public Rigidbody RB
        {
            get { return _rb; }
        }
    }
}

