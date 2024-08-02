using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRadar : MonoBehaviour
{
    [SerializeField]
    CharacterMovement m_characterMovement;

    Vector3 m_worldTransfromOfObject;
    // Start is called before the first frame update
    void Start()
    {
        m_characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (!m_characterMovement.IsInFieldOfView(m_worldTransfromOfObject))
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void SetWorldTransfrom(Vector3 worldTransfromOfObject)
    {
        m_worldTransfromOfObject = worldTransfromOfObject;
    }

}
