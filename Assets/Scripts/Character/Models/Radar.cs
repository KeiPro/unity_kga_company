using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Radar : MonoBehaviour
{
    public Action<Vector3, Vector3, int> onRadarDetected;

    public Action<int> onRadarDetected2;

    bool m_isWorking;

    [SerializeField]
    float m_maxRadius;
    [SerializeField]
    float m_radarSpeed;

    float m_currentRadius = 0;

    Transform m_parentTrans;

    List<Collider> m_colList = new List<Collider>();

    CharacterMovement m_characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        GetComponent<Renderer>().material.color = Color.blue;
        m_parentTrans = gameObject.transform.parent;
        m_characterMovement = GameObject.FindWithTag("Player").GetComponent<CharacterMovement>();

    }
    public void StartRadar()
    {
        if (m_isWorking == false)
            StartCoroutine(RadarRoutine());
    }

    IEnumerator RadarRoutine()
    {

        SphereCollider col = this.gameObject.transform.GetComponent<SphereCollider>();
        this.gameObject.transform.SetParent(null);
        m_isWorking = true;
        while (!Mathf.Approximately(Mathf.Ceil(transform.localScale.x), m_maxRadius))
        {
            m_currentRadius = Mathf.Lerp(m_currentRadius, m_maxRadius, m_radarSpeed * Time.deltaTime);
            transform.localScale = new Vector3(m_currentRadius, m_currentRadius, m_currentRadius);
            yield return null;
        }
        transform.localScale = Vector3.zero;
        m_isWorking = false;
        m_currentRadius = 0;
        this.transform.position = Vector3.zero;
        this.gameObject.transform.SetParent(m_parentTrans, false);
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.layer == 10 || !m_characterMovement.IsInFieldOfView(col.transform))
            return;

        if (col.GetComponent<Weapon>() is IWeapon || col.GetComponent<Item>() is IWeapon)
        {
            m_colList.Add(col);
            col.GetComponent<Weapon>().OnRadarDetected += onRadarDetected;
            col.GetComponent<Weapon>().SetKey(m_colList.IndexOf(col));
            int idn = m_colList.IndexOf(col);
            onRadarDetected2(idn);
            col.GetComponent<Weapon>().StartRayToScreen();

        }
        else if (col.GetComponent<Item>() is IItem)
        {
            m_colList.Add(col);
            col.GetComponent<Item>().OnRadarDetected += onRadarDetected;
            col.GetComponent<Item>().SetKey(m_colList.IndexOf(col));
            int idn = m_colList.IndexOf(col);
            onRadarDetected2(idn);
            col.GetComponent<Item>().StartRayToScreen();
        }

    }

}
