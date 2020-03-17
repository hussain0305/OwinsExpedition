using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Invoke("DestroyMyself", 1);
    }

    private void OnEnable()
    {
        Invoke("DestroyMyself", 1);
    }

    public void SetDamageTextAndColour(int DamageAmount, Color TextColour)
    {
        Text ThisText = GetComponent<Text>();
        ThisText.text = "" + DamageAmount;
        ThisText.color = TextColour;
    }
    void DestroyMyself()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
