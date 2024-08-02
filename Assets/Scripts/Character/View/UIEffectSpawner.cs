using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectSpawner : MonoBehaviour
{
    [SerializeField]
    UIRadar m_radarPref;
    [SerializeField]
    GameObject m_canvas;

    Dictionary<int, UIRadar> m_uiradarDict = new Dictionary<int, UIRadar>();

    public void OnRadarDetected(Vector3 vec, Vector3 vec2, int key)
    {
        UIRadar uIRadar = m_uiradarDict[key];
        uIRadar.SetWorldTransfrom(vec2);

        if (uIRadar != null)
        {
            uIRadar.transform.position = vec;
        }
    }

    public void OnRadarDetected2(int key)
    {
        UIRadar radar = GameObject.Instantiate(m_radarPref, m_canvas.transform, false).GetComponent<UIRadar>();
        if (m_uiradarDict.ContainsKey(key))
        {
            m_uiradarDict.Remove(key);
        }
        m_uiradarDict.Add(key, radar);
    }
}
