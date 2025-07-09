//Script is used to create the functionality of the shop. This includes
//buying Mentors/mentors, buying ante vouchers, buying packs, rerolling,
//going to the next round and calling functions for cards as necessary.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;

using TMPro;

public class ShopManager : MonoBehaviour
{
    //All UI Components Below
    private Button nextRoundButton;
    public Button rerollButton;
    public GameObject moneyText;
    //First Purchasable Card
    private Mentor mentor1;
    private Textbook textbook1;
    private CardBuff cardBuff1;
    public Button cardButton1;
    //Second Purchasable Card
    private Mentor mentor2;
    private Textbook textbook2;
    private CardBuff cardBuff2;
    public Button cardButton2;
    private Pack pack1;
    private Pack pack2;
    public GameObject packText;
    //Cards in Pack
    private PCard PackCard1;
    private PCard PackCard2;
    private PCard PackCard3;
    private PCard PackCard4;
    private PCard PackCard5;
    public Button PackCardButton1;
    public Button PackCardButton2;
    public Button PackCardButton3;
    public Button PackCardButton4;
    public Button PackCardButton5;
    //Vouchers
    private Voucher voucher;
    public Button voucherButton;
    public CanvasGroup shopUI;//Canvas Group for ShopUI
    public Button pack1Button;
    public Button pack2Button;
    private int reroll = 5;//Reroll price
    private int shopMentorsAmount = 2;//Amount of top cards/purchasable cards
    public CanvasGroup Mentor1Details;//Details for purchasable cards (says mentor but is for anytype)
    public CanvasGroup Mentor2Details;
    public CanvasGroup VoucherDetails;
    public CanvasGroup Pack1Details;
    public CanvasGroup Pack2Details;
    public CanvasGroup RegularUI;//Canvas Group for All UI excluding the Packs
    public CanvasGroup PackGroup;//Canvas Group for Cards 1-3
    public CanvasGroup PackGroupPlus;//Canvas group for Cards 4 and 5
    int cardsSelected = 0;//Cards currently selected in pack
    int packSelected = 0;//Used to tell which pack was selected
    SFXManager sfxManager;

    //Game and Player Manager Scripts for accessing functions and variables
    Game inst = Game.access();
    Player playerInst = Player.access();
    Deck deck = Deck.access();

    // To visually update the Mentors and Consumables Card Group when items added
    [SerializeField] private JokerCardHolder mentorCardHolder;
    [SerializeField] private ConsumableCardHolder consumableCardHolder;

    public static ShopManager instance { get; private set; }  //ShopManager instance varaiable

    //Singleton for the ShopManager
    public static ShopManager access()
    {
        return instance;
    }

    // Enforce singleton instance
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

        instance = this;
    }


    public void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
        NewShop();
        UpdateMoneyDisplay();
    }

    //Function called when  shopUI is opened. This intiallizes the 
    //shop with any mentors, packs, vouchers, etc. This should be called once
    //an ante.
    public void NewShop()
    {
        //Generate Mentors/Textbooks/CardBuffs
        NewCards();

        //Generate randomVoucher
        if (Game.access().GetRound() == 1)
        {
            Voucher[] vouchers = new Voucher[1];
            vouchers = inst.randomVoucher(1);
            voucher = vouchers[0];
            voucher.initialPrice = Mathf.CeilToInt(voucher.initialPrice * playerInst.discount);
            voucherButton.image.sprite = Resources.Load<Sprite>($"Vouchers/" + voucher.name.ToString());
            Debug.Log("Voucher Name: " + voucher.name.ToString());
        }

        //Generate randomn Packs
        Pack[] packs = new Pack[2];
        packs = inst.randomPacks(1);
        pack1 = packs[0];
        pack1.price = Mathf.CeilToInt(pack1.price * playerInst.discount);
        packs = inst.randomPacks(1);
        pack2 = packs[0];
        pack2.price = Mathf.CeilToInt(pack2.price * playerInst.discount);
        Debug.Log("Pack 1 Type: " + pack1.packType);
        Debug.Log("Pack 2 Type: " + pack2.packType);

        pack1Button.image.sprite = Resources.Load<Sprite>($"Pack/" + pack1.packType.ToString() + "_" + pack1.edition.ToString());
        pack2Button.image.sprite = Resources.Load<Sprite>($"Pack/" + pack2.packType.ToString() + "_" + pack2.edition.ToString());
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
            //Use probabililties to generate the card shops
            int cardSlot = Random.Range(1, 100);
            //If number is under 10, create a textbook
            if (cardSlot <= 15)
            {
                Textbook[] Textbooks = new Textbook[2];
                Textbooks = inst.randomTextbookShop(1);
                Debug.Log("Textbook Object: " + Textbooks[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    textbook1 = Textbooks[0];
                    textbook1.price = Mathf.CeilToInt(textbook1.price * playerInst.discount);
                    //cardButton1.image.sprite = Resources.Load<Sprite>(textbook1.name.ToString());
                    cardButton1.image.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook1.name.ToString());
                    Debug.Log("Textbook 1 Name: " + textbook1.name.ToString());
                }
                else
                {
                    textbook2 = Textbooks[0];
                    textbook2.price = Mathf.CeilToInt(textbook2.price * playerInst.discount);
                    Debug.Log("Textbook 2 Name: " + textbook2.name.ToString());
                    cardButton2.image.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook2.name.ToString());
                }

            }
            else if (cardSlot < 30) //Create a CardBuff
            {
                CardBuff[] cardBuffs = new CardBuff[2];
                cardBuffs = inst.randomCardBuffShop(1);
                Debug.Log("CardBuff Object: " + cardBuffs[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    cardBuff1 = cardBuffs[0];
                    cardBuff1.price = Mathf.CeilToInt(cardBuff1.price * playerInst.discount);
                    cardButton1.image.sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff1.name.ToString());
                    Debug.Log("CardBuff 1 Name: " + cardBuff1.name.ToString());
                }
                else
                {
                    cardBuff2 = cardBuffs[0];
                    cardBuff2.price = Mathf.CeilToInt(cardBuff2.price * playerInst.discount);
                    Debug.Log("CardBuff 2 Name: " + cardBuff2.name.ToString());
                    cardButton2.image.sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff2.name.ToString());
                }
            }
            else//Create a Joker card.
            {
                // --- START DEBUGGING ---
                if (inst == null)
                {
                    Debug.LogError("FATAL ERROR: The 'inst' variable is NULL!");
                    return; // Stop the function here
                }
                // --- END DEBUGGING ---

                Mentor[] mentors = new Mentor[2];
                mentors = inst.randomMentorShop(1);

                // --- START DEBUGGING ---
                if (mentors == null || mentors.Length == 0)
                {
                    Debug.LogError("ERROR: randomMentorShop() returned an empty or null array!");
                    continue; // Skip to the next loop iteration
                }
                if (mentors[0] == null)
                {
                    Debug.LogError("ERROR: The first mentor returned from the shop was NULL!");
                    continue; // Skip to the next loop iteration
                }
                // --- END DEBUGGING ---

                Debug.Log("Mentor Object: " + mentors[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    mentor1 = mentors[0];
                    mentor1.price = Mathf.CeilToInt(mentor1.price * playerInst.discount);

                    // --- START DEBUGGING ---
                    if (cardButton1 == null)
                    {
                        Debug.LogError("FATAL ERROR: cardButton1 is NULL!");
                        return;
                    }
                    if (cardButton1.image == null)
                    {
                        Debug.LogError("FATAL ERROR: cardButton1 does not have an Image component!");
                        return;
                    }
                    // --- END DEBUGGING ---

                    cardButton1.image.sprite = Resources.Load<Sprite>($"Mentor/" + mentor1.name.ToString());
                    Debug.Log("Mentor 1 Name: " + mentor1.name.ToString());
                }
                else
                {
                    // (You would add similar checks for mentor2 and cardButton2 here)
                    mentor2 = mentors[0];
                    mentor2.price = Mathf.CeilToInt(mentor2.price * playerInst.discount);
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
        if ((mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors) ||
        playerInst.moneyCount < mentor.price)
        {
            Debug.Log("Not Enough Space In Mentors or Money Insufficient");
            sfxManager.NoSFX();
            return;
        }
        //Add Mentor to user's collection
        playerInst.mentorDeck.Add(mentor);
        if (mentorCardHolder != null)
        {
            mentorCardHolder.AddMentor(mentor); //  Visually add to holder
        }

        //Remove Mentor from Screen
        mentorButton.gameObject.SetActive(false);

        int cardsPurchased = VictoryManager.access().GetCardsPurchased();
        VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

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
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(textbook); //  Visually add to holder
            }

            //Move pack disappear
            // textbookButton.interactable |= false;
            textbookButton.gameObject.SetActive(false);

            int cardsPurchased = VictoryManager.access().GetCardsPurchased();
            VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

            // //Reduce money based on price and change text to display new money
            playerInst.moneyCount = playerInst.moneyCount - textbook.price;
            moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
        }
        else
        {
            //Should make the UI shake
            sfxManager.NoSFX();
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
            sfxManager.NoSFX();
            Debug.Log("Not Enough Space In Consumables or Money Insufficient");
            return;
        }

        //Add cardBuff to consumables
        playerInst.consumables.Add(cardBuff);
        if (consumableCardHolder != null)
        {
            consumableCardHolder.AddConsumable(cardBuff); //  Visually add to holder
        }

        //Make Pack Disappear
        cardBuffButton.gameObject.SetActive(false);

        int cardsPurchased = VictoryManager.access().GetCardsPurchased();
        VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

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
        if (mentor2 != null)
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
            sfxManager.NoSFX();
            Debug.Log("Insufficient Funds");
            return;
        }
        Debug.Log("Voucher Purchased");

        //  Add voucher to Player's voucher list
        playerInst.vouchers.Add(voucher);

        //  Add voucher effect to user's run
        voucher.applyEffect();

        //Remove voucher from screen
        voucherButton.interactable = false;
        voucherButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - voucher.initialPrice;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    public void Pack1()
    {
        BuyPack(pack1, pack1Button);
        packSelected = 1;
    }
    public void Pack2()
    {
        BuyPack(pack2, pack2Button);
        packSelected = 2;
    }

    //Function call takes in a pack card and opens it, calling the necessary functions.
    private void BuyPack(Pack pack, Button packButton)
    {
        if (playerInst.moneyCount < pack.price)
        {
            sfxManager.NoSFX();
            Debug.Log("Insufficient Funds");
            return;
        }

        int packsPurchased = VictoryManager.access().GetPacksPurchased();
        VictoryManager.access().SetPacksPurchased(packsPurchased + 1);

        //Make pack disappear
        packButton.interactable |= false;
        packButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - pack.price;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();



        //Open pack and allow user to choose from cards
        OpenPacks(pack);
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
        rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";

        GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().ChangeToRoundMusic();

        //Make shop UI disappear
        StartCoroutine(FadeOut(shopUI));
        shopUI.blocksRaycasts = false;

        // //Possibly change shop title screen back to the Ante screen.
        TransitionManager transitionMan = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionMan.TransitionToRoundSelect();

        //Set a new shop in the background
        resetShopMentor();
        NewShop();
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Shop);   //  Run shop Mentors
    }
    //Function causes the Mentors to reset. 
    public void Reroll()
    {
        if (playerInst.moneyCount < reroll)
        {
            //Possibly make screen shake
            sfxManager.NoSFX();
            Debug.Log("Money Insufficient");
            return;
        }
        //Refresh the Card slots with new random Cards
        NewCards();
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Shop);   //  Run shop Mentors

        int numRerolls = VictoryManager.access().GetNumRerolls();
        VictoryManager.access().SetCardsPurchased(numRerolls + 1);

        //Reduce money based on reroll price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - reroll;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount;
        reroll++;
        rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";

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
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(mentor1.name.ToString()) + "\n" + mentor1.description.ToString() + "\n$" + mentor1.price.ToString();
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
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(mentor2.name.ToString()) + "\n" + mentor2.description.ToString() + "\n$" + mentor2.price.ToString();
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
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = textbook1.GetDescription() + "\n$" + textbook1.price.ToString();
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
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = textbook2.GetDescription() + "\n$" + textbook2.price.ToString();
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
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(cardBuff1.name.ToString()) + "\n" + cardBuff1.GetDescription() + "\n$" + cardBuff1.price.ToString();
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
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(cardBuff2.name.ToString()) + "\n" + cardBuff2.GetDescription() + "\n$" + cardBuff2.price.ToString();
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
        VoucherDetails.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(voucher.name.ToString()) + "\n" + voucher.GetDescription() + "\n$" + voucher.initialPrice.ToString();
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
        Pack1Details.GetComponentInChildren<TMP_Text>().text = pack1.packType.ToString() + "\nChoose " + pack1.selectableCards.ToString() + " of " + pack1.packSize.ToString() + " cards" + "\n" + "$" + pack1.price.ToSafeString();

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
        Pack2Details.GetComponentInChildren<TMP_Text>().text = pack2.packType.ToString() + "\nChoose " + pack2.selectableCards.ToString() + " of " + pack2.packSize.ToString() + " cards" + "\n" + "$" + pack2.price.ToSafeString();
        Pack2Details.blocksRaycasts = true;
        StartCoroutine(FadeIn(Pack2Details));
        Pack2Details.interactable = true;
    }
    public void RemovePack2Details()
    {
        Pack2Details.blocksRaycasts = false;
        Pack2Details.interactable = false;
        StartCoroutine(FadeOut(Pack2Details));
    }
    private IEnumerator FadeIn(CanvasGroup fadeInObject)//Fade the scene when the quit button is clicked
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha < 1)
        {
            fadeInObject.alpha = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 1;
    }
    private IEnumerator FadeOut(CanvasGroup fadeInObject)//Fade the scene when the quit button is clicked
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha > 0)
        {
            fadeInObject.alpha = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 0;
    }
    // Function takes in a pack, sets the pack cards in Unity to the respective cards.
    // User is then allowed to grab 1 (or 2) or skip cards before the pack closes and 
    // returns to the shop.
    private void OpenPacks(Pack pack)
    {
        //Fade Out UI
        StartCoroutine(FadeOut(RegularUI));

        //Attach the corresponding objects(cards) onto the sections needed.
        PackCard1 = pack.cardsInPack[0];
        PackCard2 = pack.cardsInPack[1];
        PackCard3 = pack.cardsInPack[2];
        packText.GetComponentInChildren<TMP_Text>().text = "" + pack.packType.ToString() + "\n\nChoose Up To " + pack.selectableCards.ToString();


        Debug.Log("PackCard1 Mentor:" + PackCard1.mentor);
        Debug.Log("PackCard1 cardBuff:" + PackCard1.cardBuff);
        Debug.Log("PackCard1 textbook:" + PackCard1.textbook);

        //Set Images For Cards
        SetPackImage(PackCard1, PackCardButton1);
        SetPackImage(PackCard2, PackCardButton2);
        SetPackImage(PackCard3, PackCardButton3);


        StartCoroutine(FadeIn(PackGroup));
        PackGroup.interactable = true;
        PackGroup.blocksRaycasts = true;
        if (pack.packSize > 4)
        {
            PackCard4 = pack.cardsInPack[3];
            PackCard5 = pack.cardsInPack[4];
            SetPackImage(PackCard4, PackCardButton4);
            SetPackImage(PackCard5, PackCardButton5);
            StartCoroutine(FadeIn(PackGroupPlus));
            PackGroupPlus.interactable = true;
            PackGroupPlus.blocksRaycasts = true;
        }
    }

    private void SetPackImage(PCard setCard, Button setButton)
    {

        if (setCard.textbook != null)
        {
            setButton.image.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + setCard.textbook.name.ToString());
            setButton.GetComponentInChildren<TMP_Text>().text = "";
        }
        else if (setCard.mentor != null)
        {
            setButton.image.sprite = Resources.Load<Sprite>($"Mentor/" + setCard.mentor.name.ToString());
            setButton.GetComponentInChildren<TMP_Text>().text = "";
        }
        else if (setCard.cardBuff != null)
        {
            setButton.image.sprite = Resources.Load<Sprite>($"CardBuff/food_" + setCard.cardBuff.name.ToString());
            setButton.GetComponentInChildren<TMP_Text>().text = "";
            Debug.Log("CardBuff Name: " + setCard.cardBuff.name.ToString());
        }
        else
        {
            //  For Regular Playing Cards set to card base and change text element of button
            setButton.image.sprite = Resources.Load<Sprite>($"PCard/card_base");
            setButton.GetComponentInChildren<TMP_Text>().text = LinguisticTermSymbol.unicodeMap[setCard.term];
            Debug.Log("Regular Card: " + setCard.term);
        }

    }
    //Function is used to check if the user has selected the amount of cards from the pack
    private void checkPackAmountSelected()
    {
        if (packSelected == 1)
        {
            if (pack1.selectableCards <= cardsSelected)//If player has selected all cards, exit
            {
                PackCardSelected();
            }
        }
        else if (packSelected == 2)
        {
            if (pack2.selectableCards <= cardsSelected)//If player has selected all cards, exit
            {
                PackCardSelected();
            }
        }
        else
        {
            PackCardSelected();
        }
    }

    //Next set of functions is to select the different Cards in a Pack
    public void PackCard1Selected()
    {
        //If Mentor Card, check if count is not max then add if not
        if (PackCard1.mentor != null)
        {
            if (PackCard1.mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors)
            {
                Debug.Log("Mentors Is Maxed. Please Sell A Mentor To Add.");
                return;
            }
            playerInst.mentorDeck.Add(PackCard1.mentor);
            if (mentorCardHolder != null)
            {
                mentorCardHolder.AddMentor(PackCard1.mentor); //  Visually add to holder
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        if (PackCard1.textbook != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard1.textbook);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard1.textbook); //  Visually add to holder
            }
            cardsSelected++;
        }
        //If Texbook Card, check if count is not max then add if not
        if (PackCard1.cardBuff != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard1.cardBuff);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard1.cardBuff); //  Visually add to holder
            }
            cardsSelected++;
        }

        //  If regular playing card, add to player's deck
        deck.AddCard(PackCard1);

        PackCardButton1.interactable = false;
        checkPackAmountSelected();

    }
    public void PackCard2Selected()
    {
        //If Mentor Card, check if count is not max then add if not
        if (PackCard2.mentor != null)
        {
            if (PackCard2.mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors)
            {
                Debug.Log("Mentors Is Maxed. Please Sell A Mentor To Add.");
                return;
            }
            playerInst.mentorDeck.Add(PackCard2.mentor);
            if (mentorCardHolder != null)
            {
                mentorCardHolder.AddMentor(PackCard2.mentor); //  Visually add to holder
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        if (PackCard2.textbook != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard2.textbook);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard2.textbook); //  Visually add to holder
            }
            cardsSelected++;
        }
        //If Texbook Card, check if count is not max then add if not
        if (PackCard2.cardBuff != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard2.cardBuff);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard2.cardBuff); //  Visually add to holder
            }
            cardsSelected++;
        }

        //  If regular playing card, add to player's deck
        deck.AddCard(PackCard2);

        PackCardButton2.interactable = false;
        checkPackAmountSelected();
    }
    public void PackCard3Selected()
    {
        //If Mentor Card, check if count is not max then add if not
        if (PackCard3.mentor != null)
        {
            if (PackCard3.mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors)
            {
                Debug.Log("Mentors Is Maxed. Please Sell A Mentor To Add.");
                return;
            }
            playerInst.mentorDeck.Add(PackCard3.mentor);
            if (mentorCardHolder != null)
            {
                mentorCardHolder.AddMentor(PackCard3.mentor); //  Visually add to holder
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        if (PackCard3.textbook != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard3.textbook);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard3.textbook); //  Visually add to holder
            }
            cardsSelected++;
        }
        //If Texbook Card, check if count is not max then add if not
        if (PackCard3.cardBuff != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard3.cardBuff);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard3.cardBuff); //  Visually add to holder
            }
            cardsSelected++;
        }

        //  If regular playing card, add to player's deck
        deck.AddCard(PackCard3);

        PackCardButton3.interactable = false;
        checkPackAmountSelected();
    }
    public void PackCard4Selected()
    {
        //If Mentor Card, check if count is not max then add if not
        if (PackCard4.mentor != null)
        {
            if (PackCard4.mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors)
            {
                Debug.Log("Mentors Is Maxed. Please Sell A Mentor To Add.");
                return;
            }
            playerInst.mentorDeck.Add(PackCard4.mentor);
            if (mentorCardHolder != null)
            {
                mentorCardHolder.AddMentor(PackCard4.mentor); //  Visually add to holder
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        if (PackCard4.textbook != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard4.textbook);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard4.textbook); //  Visually add to holder
            }
            cardsSelected++;
        }
        //If Texbook Card, check if count is not max then add if not
        if (PackCard4.cardBuff != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard4.cardBuff);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard4.cardBuff); //  Visually add to holder
            }
            cardsSelected++;
        }

        //  If regular playing card, add to player's deck
        deck.AddCard(PackCard4);

        PackCardButton4.interactable = false;
        checkPackAmountSelected();
    }
    public void PackCard5Selected()
    {
        //If Mentor Card, check if count is not max then add if not
        if (PackCard5.mentor != null)
        {
            if (PackCard5.mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors)
            {
                Debug.Log("Mentors Is Maxed. Please Sell A Mentor To Add.");
                return;
            }
            playerInst.mentorDeck.Add(PackCard5.mentor);
            if (mentorCardHolder != null)
            {
                mentorCardHolder.AddMentor(PackCard5.mentor); //  Visually add to holder
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        if (PackCard5.textbook != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard5.textbook);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard5.textbook); //  Visually add to holder
            }
            cardsSelected++;
        }
        //If Texbook Card, check if count is not max then add if not
        if (PackCard5.cardBuff != null)
        {
            if (playerInst.consumables.Count > playerInst.maxConsumables)
            {
                Debug.Log("Consumables Is Maxed. Please Sell or Use A Consumable To Add.");
                return;
            }
            playerInst.consumables.Add(PackCard5.cardBuff);
            if (consumableCardHolder != null)
            {
                consumableCardHolder.AddConsumable(PackCard5.cardBuff); //  Visually add to holder
            }
            cardsSelected++;
        }

        //  If regular playing card, add to player's deck
        deck.AddCard(PackCard5);

        PackCardButton5.interactable = false;
        checkPackAmountSelected();
    }


    public void SkipPack()
    {
        cardsSelected = 7;//Allowable cards selection amount will never be more than 7 so this works
        checkPackAmountSelected();
    }
    //Removes UI for packs once card is selected
    public void PackCardSelected()
    {
        //Set cards to null and removing blocking for PackGroup
        PackCard1 = null;
        PackCard2 = null;
        PackCard3 = null;
        PackCard4 = null;
        PackCard5 = null;
        PackGroup.interactable = false;
        PackGroup.blocksRaycasts = false;

        //Reset Cards
        //Set cards to null and removing blocking for PackGroup
        PackCardButton1.interactable = true;
        PackCardButton2.interactable = true;
        PackCardButton3.interactable = true;
        PackCardButton4.interactable = true;
        PackCardButton5.interactable = true;

        //Fade Out UI
        StartCoroutine(FadeOut(PackGroup));
        StartCoroutine(FadeOut(PackGroupPlus));

        //Bring back Regular UI
        StartCoroutine(FadeIn(RegularUI));
        RegularUI.interactable = true;
        RegularUI.blocksRaycasts = true;

    }

    public PCard GetCardFromPack(int i)
    {
        switch (i)
        {
            case 1:
                return PackCard1;
            case 2:
                return PackCard2;
            case 3:
                return PackCard3;
            case 4:
                return PackCard4;
            case 5:
                return PackCard5;
            default:
                return null;
        }
    }

    //  Used for mentors that modify the price of reroll or items
    public void mentorShopEffect(Mentor mentor)
    {
        if (mentor.name == MentorName.Extension)
        {
            if (mentor1 != null)
            {
                mentor1.price = (mentor1.price < 3 ? 0 : mentor1.price - 3);
            }
            else if (mentor2 != null)
            {
                mentor2.price = (mentor2.price < 3 ? 0 : mentor2.price - 3);
            }
        }
        else if (mentor.name == MentorName.Astronaut)
        {
            if (textbook1 != null)
            {
                textbook1.price = 0;
            }
            if (textbook2 != null)
            {
                textbook2.price = 0;
            }
            if (pack1 != null && pack1.packType == PackType.Textbook_Pack)
            {
                pack1.price = 0;
            }
            if (pack2 != null && pack2.packType == PackType.Textbook_Pack)
            {
                pack2.price = 0;
            }
        }
    }

    //  Is this used for anything now?
    public void TransitionToShop()
    {
        NewCards();
    }

    //  Reset certain mentors at end of shop
    private void resetShopMentor()
    {
        //  Check for certain mentors that need to reset after shop
        foreach (Mentor mentor in playerInst.mentorDeck)
        {
            //  Reset first mentor discount for next shop
            if (mentor.name == MentorName.Extension)
            {
                Extension extension = (Extension)mentor;
                extension.hasDiscounted = false;
            }
        }
    }

    //  Method to update shop's moneyCount text (for example after selling)
    public void UpdateMoneyDisplay()
    {
        if (moneyText != null && playerInst != null)
        {
            Debug.Log("ShopManager received update call. New money to display: " + playerInst.moneyCount);
            moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
        }
        else
        {
            Debug.LogError("ShopManager could not update UI! moneyText or Player was null!");
        }
    }

}
