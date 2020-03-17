using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupPromptScript : MonoBehaviour
{
    public int Type;
    public AudioClip BoxSound;
    public Image PromptImage;
    public GameObject GreenExplosion;
    public GameObject RedExplosion;
    public GameObject EverythingElse;

    private bool IsWeaponPrompt;
    private AudioSource BoxPlayer;
    private GameObject SpawnedExplosion;
    
    void Start()
    {
        BoxPlayer = gameObject.AddComponent<AudioSource>();
        BoxPlayer.clip = BoxSound;
        BoxPlayer.loop = false;
    }

    private void OnEnable()
    {
        EverythingElse.SetActive(true);
        PromptImage.preserveAspect = true;
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
    
    public void ClosePrompt(bool Equip)
    {
        PlayerController PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerReference.SetPlayerActivityPermission(true);
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<UIInventory>().InventoryButtonOn();
        EverythingElse.SetActive(false);
        ShowExplosion();
        if (Equip)
        {
            if (IsWeaponPrompt)
            {
                PlayerReference.LateEquipWeapon();
            }
            else
            {
                PlayerReference.LateEquipSupermove();
            }
        }
    }

    public void SetIsWeapon(bool IsWeapon)
    {
        IsWeaponPrompt = IsWeapon;
    }

    void ShowExplosion()
    {
        BoxPlayer.Play();
        GameObject ToSpawn = Type == 0 ? GreenExplosion : RedExplosion;
        Transform PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnedExplosion = ObjectPooler.CentralObjectPool.SpawnFromPool(ToSpawn.name, PlayerTransform.position + new Vector3(0, 1, 0), PlayerTransform.rotation);
        Invoke("DestroyEverything", 1);
    }

    void DestroyEverything()
    {
        SpawnedExplosion.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
