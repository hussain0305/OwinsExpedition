using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    
    public void AddRevives(int Amount)
    {
        GameController.GameControl.RevivesAdded(Amount); //Revives += Amount;
        GameController.GameControl.SaveGameStats();
    }

    public bool SpendRevives(int Amount)
    {
        if(GameController.GameControl.GetRevives() < Amount)
        {
            return false;
        }
        else
        {
            GameController.GameControl.RevivesSpent(Amount);
            GameController.GameControl.SaveGameStats();
            return true;
        }
    }

}
