using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NetLesson
{
    public class PlayerPing : NetworkBehaviour
    {
        [SerializeField]
        private InputActionReference _pingAction;

        [SerializeField]
        private Transform _body;

        [SerializeField]
        private float _cooldown = 1f;

        private float _lastPingTime = float.MinValue;

        private void Update()
        {
            if (!IsOwner)
                return;

            if (_pingAction.action.WasPressedThisFrame())
                PingServerRpc();
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
        private void PingServerRpc()
        {
            if (Time.time - _lastPingTime < _cooldown)
                return;

            _lastPingTime = Time.time;
            ShowPingRpc();
        }

        [Rpc(SendTo.Everyone)]
        private void ShowPingRpc()
        {
            StopAllCoroutines();
            StartCoroutine(PlayPing());
        }

        private IEnumerator PlayPing()
        {
            const float duration = 0.3f;

            for (float time = 0f; time < duration; time += Time.deltaTime)
            {
                _body.localScale = Vector3.one * (1f + Mathf.Sin(time / duration * Mathf.PI) * 0.6f);
                yield return null;
            }

            _body.localScale = Vector3.one;
        }
    }
}
