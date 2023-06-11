using System;
using UnityEngine;

using Pachinko.Const;
using Pachinko.Ball;

namespace Pachinko
{
    public class PachiSphereChucker : MonoBehaviour
    {
        public StartChuckerState state = StartChuckerState.NONE;
        public Action Callback { get; set; }
        void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == PachinkoConst.PACHINKO_SPHERE_TAG)
            {
                PachinkoBall ball = collision.gameObject.GetComponent<PachinkoBall>();
                if (ball.isLocal)
                {
                    ball.SetActiveAllClone(false);
                    ball.gameObject.transform.position = Vector3.zero;
                    ball.gameObject.transform.localPosition = Vector3.zero;
                    ball.gameObject.transform.rotation = Quaternion.identity;
                    ball.gameObject.transform.localRotation = Quaternion.identity;
                    ball.gameObject.transform.localEulerAngles = Vector3.zero;
                    Debug.Log(state);
                    Callback?.Invoke();
                }
            }
        }
    }
}
