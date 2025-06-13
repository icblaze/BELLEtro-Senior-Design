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
    public CanvasGroup VoucherDetails;
    public CanvasGroup Pack1Details;
    public CanvasGroup Pack2Details;
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
        voucherButton.image.sprite = Resources.Load<Sprite>($"Voucher/voucher_" + voucher.name.ToString());

        //Generate randomn Packs
        Pack[] packs = new Pack[2];
        packs = inst.randomPacks(2);
        pack1 = packs[0];
        pack2 = packs[1];
        pack1Button.image.sprite = Resources.Load<Sprite>($"Pack/pack_" + pack1.price.ToString());
        pack2Button.image.sprite = Resources.Load<Sprite>($"Pack/pack_" + pack2.price.ToString());
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



        for (int i = 1; i <= shopMentorsAmount; i++)
        {
            //Use probablilties to generate the card shops
            int cardSlot = Random.Range(1, 100);
            //If number is under 10, create a textbook
            if (cardSlot <= 15)
            {
                Textbook[] Textbooks = new Textbook[2];
                Textbooks = inst.randomTextbookShop();
                Debug.Log("Textbook Object: " + Textbooks[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    textbook1 = Textbooks[0];
                    //cardButton1.image.sprite = Resources.Load<Sprite>(textbook1.name.ToString());
                    cardButton1.image.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook1.name.ToString());
                    Debug.Log("Textbook 1 Name: " + textbook1.name.ToString());
                }
                else
                {
                    textbook2 = Textbooks[0];
                    Debug.Log("Textbook 2 Name: " + textbook2.name.ToString());
                    cardButton2.image.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook2.name.ToString());
                }

            }
            else if (cardSlot < 30) //Create a CardBuff
            {
                CardBuff[] CardBuffs = new CardBuff[2];
                CardBuffs = inst.randomCardBuffShop();
                Debug.Log("CardBuff Object: " + CardBuffs[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    cardBuff1 = CardBuffs[0];
                    cardButton1.image.sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff1.name.ToString());
                    Debug.Log("CardBuff 1 Name: " + cardBuff1.name.ToString());
                }
                else
                {
                    cardBuff2 = CardBuffs[0];
                    Debug.Log("CardBuff 2 Name: " + cardBuff2.name.ToString());
                    cardButton2.image.sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff2.name.ToString());
                }
            }
            else//Create a Joker card.
            {
                Mentor[] mentors = new Mentor[2];
                mentors = inst.randomMentorShop();
                Debug.Log("Mentor Object: " + mentors[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    mentor1 = mentors[0];
                    cardButton1.image.sprite = Resources.Load<Sprite>($"Mentor/" + mentor1.name.ToString());
                    Debug.Log("Mentor 1 Name: " + mentor1.name.ToString());
                }
                else
                {
                    mentor2 = mentors[0];
                    Debug.Log("Mentor 2 Name: " + mentor2.name.ToString());
                    cardButton2.image.sprite = Resources.Load<Sprite>($"Mentor/" + mentor2.name.ToString());
                }
            }

        }
    }

    //Function call takes in a Mentor Card and adds the Mentor Card into the players collection.
    private void BuyMentor(Mentor mentor, Button mentorButton)
    {
        //If Joker threshold is hit, do not purchase.
        if (playerInst.mentorDeck.Count >= playerInst.maxMentors ||
        playerInst.moneyCount < mentor.price)
        {
            Debug.Log("Not Enough Space In Mentors or Money Insufficient");
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
        //If consumables is maxed or not enough money
        if (playerInst.consumables.Count < playerInst.maxConsumables
        && playerInst.moneyCount >= textbook.price)
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
            Debug.Log("Not Enough Space In Consumables or Money Insufficient");
            return;
        }
    }
    private void BuyCardBuff(CardBuff cardBuff, Button cardBuffButton)
    {
        if (playerInst.consumables.Count >= playerInst.maxConsumables
        || playerInst.moneyCount < cardBuff.price)
        {
            //Should make the UI shake
            Debug.Log("Not Enough Space In Consumables or Money Insufficient");
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
        else if (cardBuff2 != null)
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
        if (playerInst.moneyCount < voucher.initialPrice)
        {
            Debug.Log("Insufficient Funds");
            return;
        }
        Debug.Log("Voucher Purchased");
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
        if (playerInst.moneyCount < pack.price)
        {
            Debug.Log("Insufficient Funds");
            return;
        }
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
        if (playerInst.moneyCount < reroll)
        {
            //Possibly make screen shake
            Debug.Log("Money Insufficient");
            return;
        }
        //Refresh the Card slots with new random Cards
        NewCards();

        //Reduce money based on reroll price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - reroll;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount;
        reroll++;

    }

    //Card Details Hovering Functions
    public void ShowCard1Detail()
    {
        if (mentor1 != null)
        {
            ShowMentor1Details();
        }
        else if (cardBuff1 != null)
        {
            ShowCardBuff1Details();
        }
        else
        {
            ShowTextbook1Details();
        }
    }
    public void ShowCard2Detail()
    {
        if (mentor2 != null)
        {
            ShowMentor2Details();
        }
        else if (cardBuff2 != null)
        {
            ShowCardBuff2Details();
        }
        else
        {
            ShowTextbook2Details();
        }
    }

    public void RemoveCard1Detail()
    {
        if (mentor1 != null)
        {
            RemoveMentor1Details();
        }
        else if (cardBuff1 != null)
        {
            RemoveCardBuff1Details();
        }
        else
        {
            RemoveTextbook1Details();
        }
    }
    public void RemoveCard2Detail()
    {
        if (mentor2 != null)
        {
            RemoveMentor2Details();
        }
        else if (cardBuff2 != null)
        {
            RemoveCardBuff2Details();
        }
        else
        {
            RemoveTextbook2Details();
        }
    }
    //Function is used to show the details of what the Joker does
    private void ShowMentor1Details()
    {
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = mentor1.name.ToString();
        Mentor1Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveMentor1Details()
    {
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowMentor2Details()
    {
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = mentor2.name.ToString();
        Mentor2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    private void RemoveMentor2Details()
    {
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }

    private void ShowTextbook1Details()
    {
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = textbook1.name.ToString();
        Mentor1Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveTextbook1Details()
    {
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowTextbook2Details()
    {
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = textbook2.name.ToString();
        Mentor2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    private void RemoveTextbook2Details()
    {
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }

    private void ShowCardBuff1Details()
    {
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = cardBuff1.name.ToString();
        Mentor1Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveCardBuff1Details()
    {
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowCardBuff2Details()
    {
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = cardBuff2.name.ToString();
        Mentor2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    private void RemoveCardBuff2Details()
    {
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }
    public void ShowVoucherDetails()
    {
        //VoucherDetails.GetComponentInChildren<TMP_Text>().text = voucher.name.ToString();
        VoucherDetails.blocksRaycasts = true;
        VoucherDetails.interactable = true;
        StartCoroutine(FadeIn(VoucherDetails));
    }
    public void RemoveVoucherDetails()
    {
        VoucherDetails.blocksRaycasts = false;
        VoucherDetails.interactable = false;
        StartCoroutine(FadeOut(VoucherDetails));
    }
    public void ShowPack1Details()
    {
        //Pack1Details.GetComponentInChildren<TMP_Text>().text = pack1.price.ToString();

        Pack1Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Pack1Details));
        Pack1Details.interactable = true;
    }
    public void RemovePack1Details()
    {
        Pack1Details.blocksRaycasts = false;
        Pack1Details.interactable = false;
        StartCoroutine(FadeOut(Pack1Details));
    }
    public void ShowPack2Details()
    {
        Pack2Details.GetComponentInChildren<TMP_Text>().text = pack2.price.ToString();
        Pack2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Pack2Details));
        Pack2Details.interactable = true;
    }
    public void RemovePack2Details()
    {//
        Pack2Details.blocksRaycasts = false;
        Pack2Details.interactable = false;
        StartCoroutine(FadeOut(Pack2Details));
    }
    private IEnumerator FadeIn(CanvasGroup fadeInObject)//Fade the scene when the quit button is clikced
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha < 1)
        {
            //float opacity = fadeInObject.alpha - .05f;
            //Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            //fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 1;
    }
    private IEnumerator FadeOut(CanvasGroup fadeInObject)//Fade the scene when the quit button is clikced
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha > 0)
        {
            //float opacity = fadeInObject.alpha - .05f;
            //Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            //fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 0;
    }
    //
    // private void OpenPacks(Pack pack)
    // {
    //     //Take in Pack

    //     //Attach the corresponding objects onto the sections needed.
    //     if (pack[i].packType == PackType.Standard_Pack)
    //         {
    //             pack[i].cardsInPack = randomPackCards(pack[i]);
    //         }
    //         else if (pack[i].packType == PackType.CardBuff_Pack)
    //         {
    //             pack[i].cardsInPack = randomCardBuff(pack[i]);
    //         }
    //         else if (pack[i].packType == PackType.Textbook_Pack)
    //         {
    //             pack[i].cardsInPack = randomTextbook(pack[i]);
    //         }
    //         else if (pack[i].packType == PackType.Mentor_Pack)
    //         {
    //             pack[i].cardsInPack = randomMentor(pack[i]);
    //         }
    //     //Allow User to choose the specific card(s) and add them where needed

    // }
}
