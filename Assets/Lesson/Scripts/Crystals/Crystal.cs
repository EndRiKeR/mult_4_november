using Unity.Netcode;
using UnityEngine;

namespace NetLesson
{
    public class Crystal : NetworkBehaviour
    {
        [SerializeField]
        private int _points = 1;

        [SerializeField]
        private float _rotationSpeed = 90f;

        private void Update()
        {
            transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer || !IsSpawned)
                return;

            if (!other.TryGetComponent<PlayerScore>(out var score))
                return;

            score.AddPoints(_points);
            NetworkObject.Despawn();
        }
    }
}
