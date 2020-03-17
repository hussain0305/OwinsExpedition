using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingComponent : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        if(collision.gameObject.GetComponent<BasePickup>())
        {
            switch (collision.gameObject.GetComponent<BasePickup>().GetPickupType())
            {
                case PickupClass.Weapon:
                    transform.parent.GetComponent<PlayerController>().SetPlayerActivityPermission(false);
                    transform.parent.GetComponent<PlayerController>().PickedUpWeapon(collision.gameObject.GetComponent<BasePickup>().GetWeaponName());
                    collision.gameObject.GetComponent<BasePickup>().MakePrompt(true);
                    collision.gameObject.SetActive(false);
                    break;
                case PickupClass.Supermove:
                    transform.parent.GetComponent<PlayerController>().SetPlayerActivityPermission(false);
                    transform.parent.GetComponent<PlayerController>().PickedUpSupermove(collision.gameObject.GetComponent<BasePickup>().GetWeaponName());
                    collision.gameObject.GetComponent<BasePickup>().MakePrompt(false);
                    collision.gameObject.SetActive(false);
                    break;
                case PickupClass.Health:
                    transform.parent.GetComponent<PlayerController>().PickedUpHealth();
                    collision.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
