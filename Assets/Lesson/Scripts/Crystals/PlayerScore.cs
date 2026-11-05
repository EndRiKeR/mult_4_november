using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NetLesson
{
    public class PlayerScore : NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text _scoreText;

        private NetworkVariable<int> _score = new(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _score.OnValueChanged += UpdateView;

            UpdateView(0, _score.Value);
        }

        public override void OnNetworkDespawn()
        {
            _score.OnValueChanged -= UpdateView;
            base.OnNetworkDespawn();
        }

        private void LateUpdate()
        {
            _scoreText.transform.rotation = Camera.main.transform.rotation;
        }

        public void AddPoints(int points)
        {
            if (!IsServer)
                return;

            _score.Value += points;
        }

        private void UpdateView(int oldValue, int newValue)
        {
            _scoreText.text = newValue.ToString();
        }
    }
}
