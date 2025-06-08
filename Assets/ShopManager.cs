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
    //UI Components
    private Button nextRoundButton;
    private Button rerollButton;
    public GameObject moneyText;
    //private string[] cardsInShop = new string[2];
    private Mentor mentor1;
    private Textbook textbook1;
    private CardBuff cardBuff1;
    public Button cardButton1;
    private Mentor mentor2;
    private Textbook textbook2;
    private CardBuff cardBuff2;
    public Button cardButton2;
    private Voucher voucher;
    public Button voucherButton;
    public CanvasGroup shopUI;
    private Pack pack1;
    private Pack pack2;
    public Button pack1Button;
    public Button pack2Button;
    private int reroll = 5;
    private int shopMentorsAmount = 2;
    public CanvasGroup Mentor1Details;
        public CanvasGroup Mentor2Details;
    //Game and Player Manager Scripts for accessing functions and variables
    Game inst = Game.access();
    Player playerInst = Player.access();

    public void Start()
    {
        //cardButton1.image.sprite = Resources.Load<Sprite>("TestImage");
        NewShop();
    }

    //Function called when  shopUI is opened. This intiallizes the 
    //shop with any mentors, packs, vouchers, etc. This should be called once
    //an ante.
    public void NewShop()
    {
        //Generate Mentors/Textbooks/CardBuffs
        NewCards();

        //Generate randomVoucher
        Voucher[] vouchers = new Voucher[1];
        vouchers = inst.randomVoucher(1);
        voucher = vouchers[0];
        voucherButton.image.sprite = Resources.Load<Sprite>(voucher.name.ToString());

        //Generate randomn Packs
        Pack[] packs = new Pack[2];
        packs = inst.randomPack(2);
        pack1 = packs[0];
        //pack2 = packs[1];
        pack1Button.image.sprite = Resources.Load<Sprite>(pack1.name.ToString());
        //pack2Button.image.sprite = Resources.Load<Sprite>(pack2.name.ToString());
    }

    private void NewCards()
    {
        //Reset all cards
        cardButton1.gameObject.SetActive(true);
        cardButton2.gameObject.SetActive(true);
        mentor1 = null;
        mentor2 = null;
        textbook1 = null;
        textbook2 = null;
        cardBuff1 = null;
        cardBuff2 = null;

        //Use probablilties to generate the card shops
        int cardSlot = Random.Range(1, 100);

        for (int i = 1; i <= shopMentorsAmount; i++)
        {
            //If number is under 10, create a textbook
            if (cardSlot < 10)
            {
                //cardInShop[i] = "textbook" + i;
                Textbook[] textbooks = inst.randomTextbook(shopMentorsAmount);
                Debug.Log(textbooks);
                textbook1 = textbooks[0];
                //textbook2 = textbooks[1];
                cardButton1.image.sprite = Resources.Load<Sprite>(textbook1.name.ToString());
                //cardButton2.image.sprite = Resources.Load<Sprite>(/TextBook/textbook_" + textbook2.name.ToString());
            }
            else if (cardSlot < 20) //Create a CardBuff
            {
                //cardInShop[i] = "cardBuff" + i;
                CardBuff[] cardBuffs = new CardBuff[shopMentorsAmount];
                cardBuffs = inst.randomCardBuff(shopMentorsAmount);
                Debug.Log(cardBuffs);
                cardBuff1 = cardBuffs[0];
                //cardBuff2 = cardBuffs[1];
                cardButton1.image.sprite = Resources.Load<Sprite>(cardBuff1.name.ToString());
                //cardButton2.image.sprite = Resources.Load<Sprite>("/CardBuff/" + cardBuff2.name.ToString());
            }
            else//Create a Joker card.
            {
                //cardInShop[i] = "mentor" + i;
                Mentor[] mentors = new Mentor[2];
                mentors = inst.randomMentor(2);
                Debug.Log("Mentor Object: " + mentors[0]);
                mentor1 = mentors[0];
                Debug.Log("Mentor 1 Name: " + mentor1.name.ToString());
                //mentor2 = mentors[1];
                cardButton1.image.sprite = Resources.Load<Sprite>(mentor1.name.ToString());
                //cardButton2.image.sprite = Resources.Load<Sprite>(mentor2.name.ToString());
            }

        }
    }

    //Function call takes in a Mentor Card and adds the Mentor Card into the players collection.
    private void BuyMentor(Mentor mentor, Button mentorButton)
    {
        //If Joker threshold is hit, do not purchase.
        if (playerInst.mentorDeck.Count >= playerInst.maxMentors)
        {
            Debug.Log("Not Enough Space In Mentors");
            return;
        }
        //Add Mentor to user's collection
        playerInst.mentorDeck.Add(mentor);

        //Remove Mentor from Screen
        mentorButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - mentor.price;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    private void BuyTextbook(Textbook textbook, Button textbookButton)
    {
        if (playerInst.consumables.Count < playerInst.maxConsumables)
        {
            //Open pack and allow user to choose from cards
            playerInst.consumables.Add(textbook);

            //Move pack disappear
            // textbookButton.interactable |= false;
            textbookButton.gameObject.SetActive(false);

            // //Reduce money based on price and change text to display new money
            playerInst.moneyCount = playerInst.moneyCount - textbook.price;
            moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
        }
        else
        {
            //Should make the UI shake
            Debug.Log("Not Enough Space In Consumables");
        }
    }
    private void BuyCardBuff(CardBuff cardBuff, Button cardBuffButton)
    {
        if (playerInst.consumables.Count >= playerInst.maxConsumables)
        {
            Debug.Log("Not Enough Space In Consumables");
            return;
        }

        //Add cardBuff to consumables
        playerInst.consumables.Add(cardBuff);

        //Make Pack Disappear
        cardBuffButton.gameObject.SetActive(false);

        //Reduce Money and Update
        playerInst.moneyCount = playerInst.moneyCount - cardBuff.price;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();

    }

    //Function is used to call the BuyMentors function for Card 1
    public void CardButton1()
    {
        //Should use if statements to determine what the card is
        //and call the respective function
        if (mentor1 != null)
        {
            BuyMentor(mentor1, cardButton1);
        }
        else if (cardBuff1 != null)
        {
            BuyCardBuff(cardBuff1, cardButton1);
        }
        else
        {
            BuyTextbook(textbook1, cardButton1);
        }

    }
    //Function is used to call the BuyMentors function for Card 2
    public void CardButton2()
    {
        //Should use if statements to determine what the card is
        //and call the respective function
        if (mentor1 != null)
        {
            BuyMentor(mentor2, cardButton2);
        }
        else if (cardBuff1 != null)
        {
            BuyCardBuff(cardBuff2, cardButton2);
        }
        else
        {
            BuyTextbook(textbook2, cardButton2);
        }
    }
    //Function call takes in a voucher card and adds the effects into the player's run.
    public void BuyVoucher()
    {
        // //Add voucher effect to user's run
        voucher.applyEffect(playerInst);

        //Remove voucher from screen
        voucherButton.interactable = false;
        voucherButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - voucher.initialPrice;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    //Function call takes in a pack card and opens it, calling the necessary functions.
    private void BuyPack(Pack pack, Button packButton)
    {
        //Open pack and allow user to choose from cards

        //Call respected function for cards (whether it be a Mentor, Planet, or Tarrot)

        //Make pack disappear
        packButton.interactable |= false;
        packButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - pack.price;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    public void BuyPack1()
    {
        BuyPack(pack1, pack1Button);
    }
    public void BuyPack2()
    {
        BuyPack(pack2, pack2Button);
    }

    //Function causes the shop UI to disappear and transitions back into the regular scene.
    //Music should also change back to the regular gameplay music.
    public void NextRound()
    {
        //Reset Reroll price to 5
        reroll = 5;

        //Make shop UI disappear
        StartCoroutine(FadeOut(shopUI));
        shopUI.blocksRaycasts = false;

        // //Possibly change shop title screen back to the Ante screen.
        //StartCoroutine(FadeIn(ante));
        // ante.alpha = 1;
        // ante.blocksRaycasts = true;
    }
    //Function causes the Mentors to reset. 
    public void Reroll()
    {
        //Refresh the Card slots with new random Cards
        NewCards();

        //Reduce money based on reroll price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - reroll;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount;
        reroll++;

    }


    //Function is used to show the details of what the Joker does
    public void ShowMentor1Details()
    {
        //Mentor1Detailsetails.GetComponentInChildren<TMP_Text>().enabled = true;
        Mentor1Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    public void RemoveMentor1Details()
    {
        //Mentor1Detailsetails.GetComponentInChildren<TMP_Text>().enabled = false;
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    public void ShowMentor2Details()
    {
        //Mentor1Detailsetails.GetComponentInChildren<TMP_Text>().enabled = true;
        Mentor2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    public void RemoveMentor2Details()
    {
        //Mentor1Detailsetails.GetComponentInChildren<TMP_Text>().enabled = false;
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }
    private IEnumerator FadeIn(CanvasGroup fadeInObject)//Fade the scene when the quit button is clikced
    {
        while (fadeInObject.alpha < 1)
        {
            float opacity = fadeInObject.alpha + .05f;
            Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 1;
    }
    private IEnumerator FadeOut(CanvasGroup fadeInObject)//Fade the scene when the quit button is clikced
    {
        while (fadeInObject.alpha > 0)
        {
            float opacity = fadeInObject.alpha - .05f;
            Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 0;
    }
}
