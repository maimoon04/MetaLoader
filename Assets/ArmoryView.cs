using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryView : MonoBehaviour
{
    [SerializeField] private ArmoryContainer ArmoryContainer;
    [SerializeField] private MyMetaLoader MyMetaLoader;
    [SerializeField] private WeaponButtons prefab;
    [SerializeField] private List<WeaponButtons> weaponButtonslist;
    [SerializeField] private GameObject WeaponContainer;
    [SerializeField] private ArmoryObjects armoryObject;

    [Header("BuyButton")]
    [SerializeField] private Button BuyButton;
    [SerializeField] private TextMeshProUGUI PriceText;
    [SerializeField] private TextMeshProUGUI InventoryText;

    public int Gems = 2000;
    public int Coins = 3000;
    // Start is called before the first frame update
    void Start()
    {
        MyMetaLoader.LoadCSV();
        ArmoryContainer.Deserialize();
       StartCoroutine(ShowInventory());
        BuyButton.onClick.AddListener(BuyInventory);
    }

    private IEnumerator ShowInventory()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0;i<ArmoryContainer.armoryObjects.Count;i++)
        {
            WeaponButtons obj = Instantiate(prefab, WeaponContainer.transform);
            ArmoryObjects temp = ArmoryContainer.armoryObjects[i];
            Debug.Log(temp.WeaponType);
            obj.init(temp.WeaponType, WeaponImage(temp.path));
            weaponButtonslist.Add(obj);
            obj.Button.onClick.AddListener(() => onClickButton(obj.WeaponsType));
        }
    }

    private void onClickButton(WeaponsType type) 
    {
        armoryObject = ArmoryContainer.FetchArmoryObj(type);
        UpdateText();
    }

    private void UpdateText()
    {
        PriceText.text = $"{armoryObject.Amount} {armoryObject.Currency}";
        InventoryText.text = $"{armoryObject.WeaponType} {ArmoryContainer.ReturnWeaponCountInInventory(armoryObject.WeaponType)}";
    }

    private void BuyInventory()
    {
        switch (armoryObject.Currency)
        {
            case Currency.Gems:
                if (armoryObject.Amount <= Gems)
                {
                    ArmoryContainer.SetWeaponCountInInventory(armoryObject.WeaponType);
                    Gems -=  armoryObject.Amount;
                }
                   
                break;
            case Currency.Coins:
                if (armoryObject.Amount <= Coins)
                {
                    ArmoryContainer.SetWeaponCountInInventory(armoryObject.WeaponType);
                    Coins -= armoryObject.Amount;
                }
                break;
        }
        UpdateText();
    }

    private Sprite WeaponImage(string Path)
    {
        return Resources.Load<Sprite>(Path);
    }
    // Update is called once per frame

}
