using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrigger : MonoBehaviour
{
   public enum PlaceType
    {
        combine,
        altar,
        shop,
    }

    public GameObject interactiveUI;

    public PlaceType placeType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            interactiveUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactiveUI.SetActive(false);
        }
    }
}
