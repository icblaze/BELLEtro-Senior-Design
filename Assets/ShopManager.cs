using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;
using TMPro;
//Script is used to create the functionality of the shop. This includes
//buying jokers/mentors, buying ante vouchers, buying packs, rerolling,
//going to the next round and calling functions for cards as necessary.
public class ShopManager : MonoBehaviour
{
    private Button nextRoundButton;
    private Button rerollButton;
    private TextMeshPro moneyText;
    //private Joker joker1;
    //private Joker joker2;
    //private Voucher voucher;
    //private Pack pack;
    //private Pack pack2;

    //Function call takes in a Joker Card and adds the Joker Card into the players collection.
    public void BuyJoker(/*Joker Card Here*/)
    {
        //Add Joker to user's collection

        //Remove Joker from Screen

        //money = money - joker price
        //moneyText.text = "$" - money;
    }
    //Function call takes in a voucher card and adds the effects into the player's run.
    public void BuyVoucher(/*Voucher voucher*/)
    {
        //Add voucher effect to user's run

        //Remove voucher from screen

        //money = money - voucher price
        //moneyText.text = "$" - money;
    }
    //Function call takes in a pack card and opens it, calling the necessary functions.
    public void BuyPack(/**/)
    {
        //Open pack and allow user to choose from cards

        //Call respected function for cards (whether it be a Joker, Planet, or Tarrot)
        
        //Make pack disappear

        //money = money - pack price
        //moneyText.text = "$" - money;
    }

    //Function causes the shop UI to disappear and transitions back into the regular scene.
    //Music should also change back to the regular gameplay music.
    public void NextRound()
    {
        //Make shop UI disappear

        //Possibly change shop title screen back to the Ante screen.
    }
    //Function causes the Mentors/Jokers to reset. 
    public void Reroll()
    {
        //Refresh the joker slots with new jokers

        //money = money - reroll price
        //moneyText.text = "$" - money;
    }

  
}
