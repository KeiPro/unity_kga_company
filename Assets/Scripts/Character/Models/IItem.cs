
using System;
using UnityEngine;

public interface IItem
{

    Action<Vector3> onRadarDetected { get; set; }
    ItemType itemType { get; set; }

    Vector3 originEulerAngle { get; set; }

    void SetColider(bool b);

    void LocToGround();


}

public enum ItemType
{
    HeavyItem, lightItem
}

