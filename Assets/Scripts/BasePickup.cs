using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum PickupClass
{
    Weapon,
    Supermove,
    Health
};

public class BasePickup : MonoBehaviour {

    public GameObject Prompt;
    public Image PromptIm;
    protected string WeaponName;
    protected PickupClass PickupType;

    private Sprite WeaponImage;
    private Transform CanvasObject;
    
    // Use this for initialization
	void Awake () {
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas").transform;
    }
    
    public string GetWeaponName()
    {
        return WeaponName;
    }
    
    public void SetWeaponImage(Sprite WI)
    {
        WeaponImage = WI;
    }

    public void SetPromptImage(Image PI)
    {
        PromptIm = PI;
    }

    public Sprite GetWeaponImage()
    {
        return WeaponImage;
    }

    public void SetPickupType(PickupClass Type)
    {
        PickupType = Type;
    }
    
    public void SetWeaponIcon(Sprite WeaponIcon)
    {
        GetComponent<SpriteRenderer>().sprite = WeaponIcon;
    }
    
    public void SetWeaponName(string Name)
    {
        WeaponName = Name;
    }
    
    public PickupClass GetPickupType()
    {
        return PickupType;
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (PickupType)
            {
                case PickupClass.Weapon:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerActivityPermission(false);
                    collision.gameObject.GetComponent<PlayerController>().PickedUpWeapon(WeaponName);
                    MakePrompt(true);
                    this.gameObject.SetActive(false);
                    break;
                case PickupClass.Supermove:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerActivityPermission(false);
                    collision.gameObject.GetComponent<PlayerController>().PickedUpSupermove(WeaponName);
                    MakePrompt(false);
                    this.gameObject.SetActive(false);
                    break;
                case PickupClass.Health:
                    collision.gameObject.GetComponent<PlayerController>().PickedUpHealth();
                    this.gameObject.SetActive(false);
                    break;
            }
        }
    }
    */
    public void MakePrompt(bool IsWeapon)
    {
        GameObject DrawnPrompt = ObjectPooler.CentralObjectPool.SpawnFromPool(Prompt.name, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)), transform.rotation);
        DrawnPrompt.transform.SetParent(CanvasObject.transform, false);
        DrawnPrompt.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        DrawnPrompt.GetComponent<PickupPromptScript>().SetIsWeapon(IsWeapon);
        DrawnPrompt.GetComponent<PickupPromptScript>().PromptImage.sprite = PromptIm.sprite;

        if (PickupType == PickupClass.Weapon)
        {
            DrawnPrompt.GetComponent<PickupPromptScript>().Type = 0;
        }
        else
        {
            DrawnPrompt.GetComponent<PickupPromptScript>().Type = 1;
        }

        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<UIInventory>().InventoryButtonOff();
    }

}
