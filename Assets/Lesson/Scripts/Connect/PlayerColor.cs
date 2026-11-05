using Unity.Netcode;
using UnityEngine;

namespace NetLesson
{
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private Color[] _colors;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _renderer.material.color = _colors[(int)(OwnerClientId % (ulong)_colors.Length)];
        }
    }
}
