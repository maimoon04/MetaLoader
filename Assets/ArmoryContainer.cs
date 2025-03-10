using ProjectCore.JsonSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmoryContainer", menuName = "ScriptableObject/ArmoryContainer")]
public class ArmoryContainer : ScriptableObject
{
    public List<ArmoryObjects> armoryObjects;
    public Dictionary<WeaponsType, int> weaponInventory=  new Dictionary<WeaponsType, int>();
    public List<WeaponsType> weaponInventory1;
    public string weaponinventory;

    
    public ArmoryObjects FetchArmoryObj(WeaponsType weaponType)
    {
        foreach (var armoryObject in armoryObjects)
        {
            if(weaponType == armoryObject.WeaponType)
            {
                return armoryObject;
            }
        }
        return null;
    }

    public int ReturnWeaponCountInInventory(WeaponsType weaponType)
    {
        if (weaponInventory.ContainsKey(weaponType))
        {
            return weaponInventory[weaponType];
        }
         return 0; 
    }

    public void SetWeaponCountInInventory(WeaponsType weaponsType)
    {
        if (weaponInventory.ContainsKey(weaponsType))
        {
             weaponInventory[weaponsType]= weaponInventory[weaponsType]+1;
        }
        else
        {
            weaponInventory[weaponsType] = 1;
        }
        serialize();
    }
    public void serialize()
    {
        weaponinventory = JsonHelper.SerializeDictionary(weaponInventory);
        Debug.Log("Saved Inventory: " + weaponinventory);
    }

    public void Deserialize()
    {
        if(weaponinventory== "")
        {
            Debug.Log("json is null");
            return;
        }
        weaponInventory = JsonHelper.DeserializeDictionary<WeaponsType,int>(weaponinventory);
        foreach (var item in weaponInventory)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }

}

[Serializable]
public class ArmoryObjects
{
    public WeaponsType WeaponType;
    public Currency Currency;
    public int Amount;
    public string path;
}

[Serializable]
public enum WeaponsType
{
    None,
    Blade,
    Sword,
    Knife,
    Gun
}

[Serializable]
public enum Currency
{
    Gems,
    Coins
}