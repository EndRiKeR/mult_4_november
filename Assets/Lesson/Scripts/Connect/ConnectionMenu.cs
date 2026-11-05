using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace NetLesson
{
    public class ConnectionMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _startPanel;

        [SerializeField]
        private Button _hostButton;

        [SerializeField]
        private Button _clientButton;

        [SerializeField]
        private Button _disconnectButton;

        private void Awake()
        {
            _hostButton.onClick.AddListener(StartHost);
            _clientButton.onClick.AddListener(StartClient);
            _disconnectButton.onClick.AddListener(Disconnect);
            ShowStartPanel(true);
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientStopped += OnClientStopped;
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveAllListeners();
            _clientButton.onClick.RemoveAllListeners();
            _disconnectButton.onClick.RemoveAllListeners();

            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
        }

        private void StartHost()
        {
            if (NetworkManager.Singleton.StartHost())
                ShowStartPanel(false);
        }

        private void StartClient()
        {
            if (NetworkManager.Singleton.StartClient())
                ShowStartPanel(false);
        }

        private void Disconnect()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private void OnClientStopped(bool wasHost)
        {
            ShowStartPanel(true);
        }

        private void ShowStartPanel(bool isVisible)
        {
            _startPanel.SetActive(isVisible);
            _disconnectButton.gameObject.SetActive(!isVisible);
        }
    }
}
