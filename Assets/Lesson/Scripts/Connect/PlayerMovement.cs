using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NetLesson
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField]
        private InputActionReference _moveAction;

        [SerializeField]
        private float _speed = 6f;

        [SerializeField]
        private float _arenaHalfSize = 9f;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
                transform.position = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            var input = _moveAction.action.ReadValue<Vector2>();
            var direction = new Vector3(input.x, 0f, input.y);
            if (direction.sqrMagnitude < 0.01f)
                return;

            var position = transform.position + direction.normalized * (_speed * Time.deltaTime);
            position.x = Mathf.Clamp(position.x, -_arenaHalfSize, _arenaHalfSize);
            position.z = Mathf.Clamp(position.z, -_arenaHalfSize, _arenaHalfSize);

            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
