using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox.AudioTaps;
using UnityEngine;

public class VivoxVoiceSourceTap : MonoBehaviour
{
    void Start()
    {
        UGSServiceManager.Instance.VivoxService.SetEndCallback(() =>
        {
            var vivoxAudioTap = GetComponent<VivoxChannelAudioTap>();
            if (vivoxAudioTap == null)
            {
                gameObject.AddComponent<VivoxChannelAudioTap>();
                Debug.Log("Added VivoxChannelAudioTap");
            }
        });
    }
}
