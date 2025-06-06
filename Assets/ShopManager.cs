using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;
using TMPro;


//Script is used to create the functionality of the shop. This includes
//buying Mentors/mentors, buying ante vouchers, buying packs, rerolling,
//going to the next round and calling functions for cards as necessary.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class ShopManager : MonoBehaviour
{
    private Button nextRoundButton;
    private Button rerollButton;
    private TextMeshPro moneyText;
    //private string[] cardsInShop = new string[2];
    private Mentor mentor1;
    private Textbook textbook1;
    private CardBuff cardBuff1;
    public Button mentor1Button;
    private Mentor mentor2;
    private Textbook textbook2;
    private CardBuff cardBuff2;
    public Button mentor2Button;
    private Voucher voucher;
    public Button voucherButton;
    public CanvasGroup shopUI;
    private Pack pack1;
    private Pack pack2;
    public Button pack1Button;
    public Button pack2Button;
    private int reroll = 5;
    private int shopMentorsAmount = 2;

    public void Start()
    {

        mentor1Button.image.sprite = Resources.Load<Sprite>("TestImage");
    }

    //Function called when  shopUI is opened. This intiallizes the 
    //shop with any mentors, packs, vouchers, etc. This should be called once
    //an ante.
    public void newShop()
    {
        Game inst = Game.access();
        Game game = gameObject.GetComponent<Game>();
        //Use probablilties to generate the card shops
        int cardSlot = Random.Range(1, 100);

        for (int i = 1; i <= shopMentorsAmount; i++)
        {
            //If number is under 10, create a textbook
            if (cardSlot < 10)
            {
                //cardInShop[i] = "textbook" + i;
                Textbook[] textbooks = inst.randomTextbook(shopMentorsAmount);
                textbook1 = textbooks[0];
                textbook2 = textbooks[1];
                mentor1Button.image.sprite = Resources.Load<Sprite>(textbook1.name.ToString());
                mentor2Button.image.sprite = Resources.Load<Sprite>(textbook2.name.ToString());
            }
            else if (cardSlot < 20) //Create a CardBuff
            {
                //cardInShop[i] = "cardBuff" + i;
                CardBuff[] cardBuffs = new CardBuff[shopMentorsAmount];
                cardBuffs = inst.randomCardBuff(shopMentorsAmount);
                cardBuff1 = cardBuffs[0];
                cardBuff2 = cardBuffs[1];
                mentor1Button.image.sprite = Resources.Load<Sprite>(cardBuff1.name.ToString());
                mentor2Button.image.sprite = Resources.Load<Sprite>(cardBuff2.name.ToString());
            }
            else//Create a Joker card.
            {
                //cardInShop[i] = "mentor" + i;
                Mentor[] mentors = new Mentor[shopMentorsAmount];
                mentors = inst.randomMentor(shopMentorsAmount);
                mentor1 = mentors[0];
                mentor2 = mentors[1];
                mentor1Button.image.sprite = Resources.Load<Sprite>(mentor1.name.ToString());
                mentor2Button.image.sprite = Resources.Load<Sprite>(mentor2.name.ToString());
            }

        }

        //randomVoucher
        Voucher[] vouchers = new Voucher[1];
        vouchers = game.randomVoucher(1);
        voucher = vouchers[0];
        voucherButton.image.sprite = Resources.Load<Sprite>(voucher.name.ToString());

        //randomn Packs
        Pack[] packs = new Pack[2];
        packs = game.randomPack(2);
        pack1 = packs[0];
        pack2 = packs[1];
        pack1Button.image.sprite = Resources.Load<Sprite>(pack1.name.ToString());
        pack2Button.image.sprite = Resources.Load<Sprite>(pack2.name.ToString());
    }

    //Function call takes in a Mentor Card and adds the Mentor Card into the players collection.
    public void BuyMentor(Mentor mentor, Button mentorButton)
    {
        //Add Mentor to user's collection

        //Remove Mentor from Screen
        mentorButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        //money = money - mentor.price;
        //moneyText.text = "$" - money;
    }
    //Function call takes in a voucher card and adds the effects into the player's run.
    public void BuyVoucher(Voucher voucher)
    {
        // //Add voucher effect to user's run
        // voucher.applyEffect();

        //Remove voucher from screen
        voucherButton.interactable = false;
        voucherButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        // money = money - voucher.initialPrice;
        // moneyText.text = "$" - money;
    }
    //Function call takes in a pack card and opens it, calling the necessary functions.
    public void BuyPack(Pack pack)
    {
        //Open pack and allow user to choose from cards

        //Call respected function for cards (whether it be a Mentor, Planet, or Tarrot)

        //Make pack disappear

        // //Reduce money based on price and change text to display new money
        //money = money - pack.price
        //moneyText.text = "$" - money;
    }

    //Function causes the shop UI to disappear and transitions back into the regular scene.
    //Music should also change back to the regular gameplay music.
    public void NextRound()
    {
        //Reset Reroll price to 5
        reroll = 5;

        //Make shop UI disappear
        shopUI.alpha = 0;
        shopUI.blocksRaycasts = false;


        // //Possibly change shop title screen back to the Ante screen.
        // ante.alpha = 1;
        // ante.blocksRaycasts = true;
    }
    //Function causes the Mentors to reset. 
    public void Reroll()
    {
        //Refresh the Mentor slots with new random Mentors


        //Reduce money based on reroll price and change text to display new money
        //money = money - reroll
        //moneyText.text = "$" - money;
        reroll++;

    }


}
