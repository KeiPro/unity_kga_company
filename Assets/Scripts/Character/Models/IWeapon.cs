
using UnityEngine;

public interface IWeapon
{
    WeaponType weaponType{get;set;}
    WeaponSort weaponSort{get;set;}

}

public enum WeaponType
{
    rightHand,leftHand
}

public enum WeaponSort
{
    Axe,Sword,Shield,Staff
}

