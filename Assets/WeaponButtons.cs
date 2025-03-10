using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Image SPrite;
    [SerializeField] public WeaponsType WeaponsType;
    [SerializeField] public Button Button;

    public void init(WeaponsType WeaponType,Sprite sprite)
    {
        Text.text = WeaponType.ToString();
        WeaponsType = WeaponType;
        SPrite.sprite = sprite;
    }

    // Start is called before the first frame update
  
}
