using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CharacterManager m_player;
    private Transform m_playerTransform;
    
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterManager>();
    }
    
    void OnEnable()
    {
        CharacterManager.OnPlayerSpawned += HandlePlayerSpawned;
    }

    void OnDisable()
    {
        CharacterManager.OnPlayerSpawned -= HandlePlayerSpawned;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player == null && m_playerTransform == null)
            return;
            
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // TODO : 추후에 네트워크 전용으로 바뀌어야함.
        Vector3 playerloc = m_player != null ? m_player.transform.position : m_playerTransform.position;
        this.gameObject.transform.position = new Vector3(playerloc.x,playerloc.y+ 1.546f,playerloc.z);
    }
    
    private void HandlePlayerSpawned(Transform player)
    {
        m_playerTransform = player;
    }
}
