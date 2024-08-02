using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRadarPresenter : MonoBehaviour
{
    [SerializeField]
    Radar m_radar;
    [SerializeField]
    UIEffectSpawner m_uiEffectSpawner;

    void Start()
    {
        m_radar.onRadarDetected += OnRadarDetected;
        m_radar.onRadarDetected2 += OnRadarDetected2;
    }

    void OnRadarDetected(Vector3 vec, Vector3 vec2, int key)
    {
        m_uiEffectSpawner.OnRadarDetected(vec, vec2, key);
    }

    void OnRadarDetected2(int key)
    {
        m_uiEffectSpawner.OnRadarDetected2(key);
    }


}
