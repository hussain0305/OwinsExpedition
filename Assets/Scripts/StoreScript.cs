using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    public Text UnifiedText;
    public GameObject RITText;

    private int SliderRevivesValue = 0;
    private int CurrentSliderValue;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRITText();
    }

    private void OnEnable()
    {
        UpdateRITText();
    }

    public void SliderValueChanged(float SliderValue)
    {
        CurrentSliderValue = (int)SliderValue;
        SliderRevivesValue = GetNumberOfRevives(SliderValue);
        UpdateUnifiedText(SliderRevivesValue, SliderValue);
    }

    public void UpdateUnifiedText(int RevivesValue, float MoneyValue)
    {
        UnifiedText.text = "Get<size=60><color=green> " + RevivesValue +"</color></size> Revives \n for <size=60><color=green>$" + (MoneyValue - 1) + ".99</color></size>";
    }

    public void PressedPurchaseButton()
    {
        IAPManager.PurchaseManager.BuyRevives(CurrentSliderValue);
    }

    public void TestAdd20()
    {
        GameController.GameControl.RevivesAdded(20);
        UpdateRITText();
    }

    int GetNumberOfRevives(float SliderValue)
    {
        int FirstComp = (int)SliderValue * 10;
        int SecondComp = (int)((SliderValue - 1) * (SliderValue - 1));
        SecondComp = SecondComp - (SecondComp % 5);

        return (FirstComp + SecondComp);
    }

    public void UpdateRITText()
    {
        if (RITText)
            RITText.GetComponent<Text>().text = "" + GameController.GameControl.GetRevives();
    }
}
