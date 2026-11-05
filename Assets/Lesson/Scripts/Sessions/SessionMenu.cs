using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace NetLesson
{
    public class SessionMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _startPanel;

        [SerializeField]
        private Button _createButton;

        [SerializeField]
        private Button _joinButton;

        [SerializeField]
        private TMP_InputField _codeInput;

        [SerializeField]
        private Button _leaveButton;

        [SerializeField]
        private TMP_Text _statusText;

        [SerializeField]
        private int _maxPlayers = 8;

        private ISession _session;

        private async void Start()
        {
            _createButton.onClick.AddListener(CreateSession);
            _joinButton.onClick.AddListener(JoinSession);
            _leaveButton.onClick.AddListener(LeaveSession);
            NetworkManager.Singleton.OnClientStopped += OnClientStopped;

            ShowStartPanel(true);
            SetButtonsInteractable(false);
            _statusText.text = "Вход в Unity Services...";

            try
            {
                var options = new InitializationOptions().SetProfile($"player{UnityEngine.Random.Range(0, 1000000)}");
                await UnityServices.InitializeAsync(options);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                _statusText.text = "Создайте игру или введите код";
                SetButtonsInteractable(true);
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }

        private void OnDestroy()
        {
            _createButton.onClick.RemoveAllListeners();
            _joinButton.onClick.RemoveAllListeners();
            _leaveButton.onClick.RemoveAllListeners();

            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
        }

        private async void CreateSession()
        {
            SetButtonsInteractable(false);
            _statusText.text = "Создаём игру...";

            try
            {
                var options = new SessionOptions { MaxPlayers = _maxPlayers }.WithRelayNetwork();
                _session = await MultiplayerService.Instance.CreateSessionAsync(options);

                _statusText.text = $"Код: {_session.Code}";
                ShowStartPanel(false);
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }

        private async void JoinSession()
        {
            SetButtonsInteractable(false);
            _statusText.text = "Подключаемся...";

            try
            {
                _session = await MultiplayerService.Instance.JoinSessionByCodeAsync(_codeInput.text.Trim());

                _statusText.text = $"Код: {_session.Code}";
                ShowStartPanel(false);
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }

        private async void LeaveSession()
        {
            if (_session == null)
                return;

            try
            {
                await _session.LeaveAsync();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void OnClientStopped(bool wasHost)
        {
            _session = null;
            _statusText.text = "Создайте игру или введите код";
            ShowStartPanel(true);
            SetButtonsInteractable(true);
        }

        private void ShowError(Exception exception)
        {
            Debug.LogException(exception);
            _statusText.text = exception.Message;
            SetButtonsInteractable(true);
        }

        private void ShowStartPanel(bool isVisible)
        {
            _startPanel.SetActive(isVisible);
            _leaveButton.gameObject.SetActive(!isVisible);
        }

        private void SetButtonsInteractable(bool isInteractable)
        {
            _createButton.interactable = isInteractable;
            _joinButton.interactable = isInteractable;
        }
    }
}
