using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Pickup : MonoBehaviour
{

    public GameObject Prompt;

    protected string WeaponName;
    protected string[] WeaponDescription;
    protected string[] UsefulAgainst;

    private Transform CanvasObject;

    // Use this for initialization
    void Start()
    {
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas").transform;
        SetWeaponName("Healthy");
    }

    public string GetWeaponName()
    {
        return WeaponName;
    }
    
    public void SetWeaponDesc(string[] Desc)
    {
        WeaponDescription = Desc;
    }

    public void SetUsefulAgainst(string[] Uses)
    {
        UsefulAgainst = Uses;
    }

    public void SetWeaponIcon(Sprite WeaponIcon)
    {
        GetComponent<SpriteRenderer>().sprite = WeaponIcon;
    }
    
    public void SetWeaponName(string Name)
    {
        WeaponName = Name;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Tutorial_PlayerController>().PickedUpHealth();
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void MakePrompt()
    {
        GameObject DrawnPrompt = Instantiate(Prompt, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)), transform.rotation, CanvasObject);
        //DrawnPrompt.GetComponent<PickupPromptScript>().WeaponIcon.sprite = GetComponent<SpriteRenderer>().sprite;
        //DrawnPrompt.GetComponent<PickupPromptScript>().WeaponName.text = GetWeaponName();

        /*
        string ParsedUses = "";

        foreach (string CurrentUse in UsefulAgainst)
        {
            ParsedUses += CurrentUse + "\n";
        }
        DrawnPrompt.GetComponent<PickupPromptScript>().WeaponUses.text = ParsedUses;

        string ParsedDescription = "";
        foreach (string CurrentDesc in WeaponDescription)
        {
            ParsedDescription += CurrentDesc + "\n";
        }
        DrawnPrompt.GetComponent<PickupPromptScript>().WeaponDescription.text = ParsedDescription;
        */
        Time.timeScale = 0.3f;
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<UIInventory>().InventoryButtonOff();
    }

}
