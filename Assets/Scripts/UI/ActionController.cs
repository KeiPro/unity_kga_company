using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : NetworkBehaviour
{
    [SerializeField]
    private float range; // ���� ������ �ִ� �Ÿ�

    private bool pickupActivated = false; // ���� ������ �� true

    private RaycastHit hitInfo; // �浹ü ���� ����

    // ������ ���̾�� �����ϵ��� ���̾� ����ũ�� ����
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory m_inventory;

    private GameObject itemPrefab; // ������ ������ ���� ������ ������Ʈ

    GameObject m_currentItem;

    GameObject m_reactItem;

    [SerializeField]
    Transform[] m_weaponPrefabsArray;


    [SerializeField]
    GameObject m_rightHand;
    [SerializeField]
    GameObject m_leftHand;

    bool m_isPossibleToReact;

    void Start()
    {
        m_inventory.onCurrentSlotChanged += LoadItem;
    }

    void Update()
    {
        CheckItem();
        ReactInput();
    }



    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().itemData.itemName + " ȹ���߽��ϴ�");
                m_inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().itemData);
                //Destroy(hitInfo.transform.gameObject);
                SetItem();
                LoadItem();
                ItemInfoDisappear();
            }
        }
    }






    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
                m_isPossibleToReact = true;
                m_reactItem = hitInfo.transform.gameObject;
            }
        }
        else
        {
            ItemInfoDisappear();
            m_isPossibleToReact = false;
            m_reactItem = null;
        }
    }

    //������ ���氡������ text �����ֱ�
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().itemData.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";
    }

    //������ ������ text ������
    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    void SetItem()
    {
        if (m_currentItem != null)
        {
            if (m_currentItem.GetComponent<Weapon>() is IWeapon || m_currentItem.GetComponent<Item>() is IWeapon)
            {
                m_inventory.SetItem(m_currentItem.GetComponent<Weapon>() as IItem);
            }
            else if (m_currentItem.GetComponent<Item>() is IItem)
            {
                m_inventory.SetItem(m_currentItem.GetComponent<Item>() as IItem);
            }
        }
    }

    void LoadItem()
    {
        if (m_inventory.LoadItem() is Weapon)
        {
            m_currentItem = (m_inventory.LoadItem() as Weapon).gameObject;
        }
        else if (m_inventory.LoadItem() is Item)
        {
            m_currentItem = (m_inventory.LoadItem() as Item).gameObject;
        }
        else
        {
            m_currentItem = null;
        }
    }

    void ReactInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_isPossibleToReact == true)
            {

                if (m_currentItem != null)
                {
                    RemoveItem();   //아이템 버리기

                }
                CanPickUp();
                m_currentItem = m_reactItem; // 인벤토리에 아이템 할당


                SetItem();

                if (m_currentItem.GetComponent<Weapon>() is IWeapon || m_currentItem.GetComponent<Item>() is IWeapon)
                {
                    //Debug.Log("무기입니다.");
                    Weapon weapon = m_currentItem.GetComponent<Weapon>();
                    weapon.SetColider(false);

                    if (weapon.weaponSort == WeaponSort.Axe)
                    {
                        weapon.transform.eulerAngles = m_weaponPrefabsArray[0].transform.eulerAngles;
                    }
                    else if (weapon.weaponSort == WeaponSort.Shield)
                    {
                        weapon.transform.eulerAngles = m_weaponPrefabsArray[1].transform.eulerAngles;
                    }
                    else if (weapon.weaponSort == WeaponSort.Sword)
                    {
                        weapon.transform.eulerAngles = m_weaponPrefabsArray[2].transform.eulerAngles;
                    }
                    else if (weapon.weaponSort == WeaponSort.Staff)
                    {
                        weapon.transform.eulerAngles = m_weaponPrefabsArray[3].transform.eulerAngles;
                    }

                    if (weapon.weaponType == WeaponType.rightHand)
                    {
                        m_reactItem.transform.SetParent(m_rightHand.transform, false);
                        m_reactItem.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        m_reactItem.transform.SetParent(m_leftHand.transform, false);
                        m_reactItem.transform.localPosition = Vector3.zero;
                    }
                }
                else if (m_currentItem.GetComponent<Item>() is IItem)
                {
                    //Debug.Log("그냥 프롭입니다.");
                    m_currentItem.GetComponent<Item>().SetColider(false);
                    m_reactItem.transform.SetParent(m_leftHand.transform, false);
                    m_reactItem.transform.localPosition = Vector3.zero;
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveItem();
        }
    }



    void RemoveItem()
    {
        if (m_currentItem != null)
        {
            if (m_currentItem.GetComponent<Weapon>() is IWeapon || m_currentItem.GetComponent<Item>() is IWeapon)
            {
                m_currentItem.GetComponent<Weapon>().SetColider(true);
                m_currentItem.GetComponent<Weapon>().LocToGround();
            }
            else if (m_currentItem.GetComponent<Item>() is IItem)
            {
                m_currentItem.GetComponent<Item>().SetColider(true);
                m_currentItem.GetComponent<Item>().LocToGround();
            }

            m_currentItem.transform.SetParent(null);
            m_inventory.LoseItem();
            m_currentItem = null;

            m_inventory.SetItem(null);
        }
    }
}
