using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour, IWeapon, IItem
{
    public Action<Vector3, Vector3, int> OnRadarDetected;

    [SerializeField]
    WeaponType m_currentWeaponType;
    [SerializeField]
    WeaponSort m_currentWeaponSort;

    [SerializeField]
    ItemType m_currentItemType;

    Vector3 m_originEulerAngle;

    int m_privateKey;

    private void Start()
    {
        m_originEulerAngle = this.transform.eulerAngles;
    }

    public void SetColider(bool b)
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = b;
    }

    public void LocToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // 바닥과 충돌한 경우, 충돌 지점을 반환합니다.
            Vector3 groundPoint = hit.point;
            Debug.Log("바닥과 충돌 지점: " + groundPoint);
            transform.position = groundPoint;
        }

        transform.eulerAngles = m_originEulerAngle;
    }

    public void StartRayToScreen()
    {
        StartCoroutine(RayRoutine());
    }

    public void SetKey(int key)
    {
        m_privateKey = key;
    }

    IEnumerator RayRoutine()
    {
        float elapsedTime = 0f;
        float duration = 3f; // 3초간 실행

        while (elapsedTime < duration)
        {
            // 루프 내용
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            OnRadarDetected(screenPoint, transform.position, m_privateKey);

            // 0.1초마다 대기
            yield return new WaitForSeconds(0.01f);

            // 경과 시간 업데이트
            elapsedTime += 0.01f;
        }
    }




    public ItemType itemType
    {
        get { return m_currentItemType; }
        set { m_currentItemType = value; }
    }

    public WeaponType weaponType
    {
        get { return m_currentWeaponType; }
        set { m_currentWeaponType = value; }
    }

    public WeaponSort weaponSort
    {
        get { return m_currentWeaponSort; }
        set { m_currentWeaponSort = value; }
    }

    public Vector3 originEulerAngle
    {
        get { return m_originEulerAngle; }
        set { m_originEulerAngle = value; }
    }

    public Action<Vector3> onRadarDetected
    {
        get { return onRadarDetected; }
        set { onRadarDetected = value; }
    }
}
