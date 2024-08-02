using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

public class RosterItem : MonoBehaviour
{
    // Player specific items.
    public VivoxParticipant Participant { get; private set; }
    [SerializeField] private TextMeshProUGUI _playerNameText;

    [SerializeField] public Image _chatStateImage;
    [SerializeField] public Sprite _mutedImage;
    [SerializeField] public Sprite _speakingImage;
    [SerializeField] public Sprite _notSpeakingImage;
    [SerializeField] public Slider _participantVolumeSlider;
    [SerializeField] public Button _muteButton;

    const float _minSliderVolume = -50;
    const float _maxSliderVolume = 10;
    readonly Color _MutedColor = new Color(1, 0.624f, 0.624f, 1);

    private void UpdateChatStateImage()
    {
        if (Participant.IsMuted)
        {
            _chatStateImage.sprite = _mutedImage;
            _chatStateImage.gameObject.transform.localScale = Vector3.one;
        }
        else
        {
            if (Participant.SpeechDetected)
            {
                _chatStateImage.sprite = _speakingImage;
                _chatStateImage.gameObject.transform.localScale = Vector3.one;
            }
            else
            {
                _chatStateImage.sprite = _notSpeakingImage;
            }
        }
    }

    public void SetupRosterItem(VivoxParticipant participant)
    {
        Participant = participant;
        _playerNameText.text = Participant.DisplayName;
        UpdateChatStateImage();
        Participant.ParticipantMuteStateChanged += UpdateChatStateImage;
        Participant.ParticipantSpeechDetected += UpdateChatStateImage;

        _muteButton.onClick.AddListener(() =>
        {
            // If already muted, unmute, and vice versa.
            if (Participant.IsMuted)
            {
                participant.UnmutePlayerLocally();
                _muteButton.image.color = Color.white;
            }
            else
            {
                participant.MutePlayerLocally();
                _muteButton.image.color = _MutedColor;
            }
        });

        if (participant.IsSelf)
        {
            // Can't change our own participant volume, so turn off the slider
            _participantVolumeSlider.gameObject.SetActive(false);
        }
        else
        {
            _participantVolumeSlider.minValue = _minSliderVolume;
            _participantVolumeSlider.maxValue = _maxSliderVolume;
            _participantVolumeSlider.value = participant.LocalVolume;
            _participantVolumeSlider.onValueChanged.AddListener((val) =>
            {
                OnParticipantVolumeChanged(val);
            });
        }
    }

    void OnDestroy()
    {
        Participant.ParticipantMuteStateChanged -= UpdateChatStateImage;
        Participant.ParticipantSpeechDetected -= UpdateChatStateImage;
        _muteButton.onClick.RemoveAllListeners();
        _participantVolumeSlider.onValueChanged.RemoveAllListeners();
    }

    void OnParticipantVolumeChanged(float volume)
    {
        if (!Participant.IsSelf)
        {
            Participant.SetLocalVolume((int)volume);
        }
    }
}