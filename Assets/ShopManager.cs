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
using System.Linq;


public class ShopManager : MonoBehaviour
{
    //All UI Components Below
    [Header("All UI Components")]
    public Button nextRoundButton;     //Button to go to next round
    public Button rerollButton;        //Button to reroll the shop
    public GameObject moneyText;       //Text for Money

    //First Purchasable Card
    [Header("First Purchasable Card")]
    private Mentor mentor1;            //Mentor Card
    private Textbook textbook1;        //Textbook Card
    private CardBuff cardBuff1;        //CardBuff Card     

    //Second Purchasable Card  
    [Header("Second Purchasable Card")]
    private Mentor mentor2;            //Mentor Card
    private Textbook textbook2;        //Textbook Card
    private CardBuff cardBuff2;        //CardBuff Card
    private Voucher voucher;           //Voucher Card

    [Header("Packs and Pack Cards")]
    private Pack pack1;                //Pack 1
    private Pack pack2;                //Pack 2
    public GameObject packText;        //Text for Pack
    private PCard PackCard1;           //Card 1 in Pack
    private PCard PackCard2;           //Card 2 in Pack
    private PCard PackCard3;           //Card 3 in Pack
    private PCard PackCard4;           //Card 4 in Pack
    private PCard PackCard5;           //Card 5 in Pack
    public GameObject voucherCard;       //GameObject for Voucher Card
    public GameObject card1;             //Gameobject for Card 1
    public GameObject card2;             //Gameobject for Card 2
    [SerializeField] public GameObject pack1Card;         //GameObject for Pack 1
    [SerializeField] public GameObject pack2Card;         //GameObject for Pack 2
    [SerializeReference] public Pack pack;

    [Header("Card Buttons")]
    public Button PackCard1Button;     //Button for Pack Card 1
    public Button PackCard2Button;     //Button for Pack Card 2
    public Button PackCard3Button;     //Button for Pack Card 3
    public Button PackCard4Button;     //Button for Pack Card 4
    public Button PackCard5Button;     //Button for Pack Card 5
    [SerializeField] public GameObject buyButtonPrefab;        //Gameobject that will hold the Buy Button Prefab
    [SerializeField] public GameObject currentBuyButton;       //Gameobject for the current card being bought
    [SerializeField] public Button buyButton;                  //Button for Buy Card

    // To visually update the Mentors and Consumables Card Group when items are added
    [Header("Card Holders")]
    [SerializeField] private JokerCardHolder mentorCardHolder;
    [SerializeField] private ConsumableCardHolder consumableCardHolder;

    [Header("UI Canvas Groups")]
    public CanvasGroup shopUI;          //Canvas Group for ShopUI
    public CanvasGroup Mentor1Details;  //Details for purchasable cards (says mentor but is for anytype)
    public CanvasGroup Mentor2Details;  //Details for purchasable cards (says mentor but is for anytype)
    public CanvasGroup VoucherDetails;  //Details for Voucher
    public CanvasGroup Pack1Details;    //Details for Pack 1
    public CanvasGroup Pack2Details;    //Details for Pack 2
    public CanvasGroup RegularUI;       //Canvas Group for All UI excluding the Packs
    public CanvasGroup PackGroup;       //Canvas Group for Cards 1-3
    public CanvasGroup PackGroupPlus;   //Canvas group for Cards 4 and 5

    private int reroll = 5;             //Reroll price
    private int shopMentorsAmount = 2;  //We will include 2 of the following: Mentors, Textbooks, or CardBuffs
    int cardsSelected = 0;              //Cards currently selected in pack
    int packSelected = 0;               //Used to tell which pack was selected
    public bool freeReroll = false;

    //Game and Player Manager Scripts for accessing functions and variables
    Game inst = Game.access();
    Player playerInst = Player.access();
    Deck deck = Deck.access();

    public static ShopManager instance { get; private set; }  //ShopManager instance varaiable

    SFXManager sfxManager;

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

        moneyText = GameObject.Find("Money Text");
    }


    public void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
        NewShop();
        UpdateMoneyDisplay();
    }

    //Function called when  shopUI is opened. This intiallizes the 
    //shop with any mentors, packs, vouchers, etc. This should be called once per ante
    public void NewShop()
    {
        //Generate Mentors/Textbooks/CardBuffs
        NewCards();
        cardsSelected = 0;//Cards currently selected in pack
        packSelected = 0;//Used to tell which pack was selected

        //Reset Reroll price to 5 (or $3 with voucher)
        if (freeReroll == true)
        {
            reroll = 0;
        }
        else if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.RerollPass))
        {
            reroll = 3;
        }
        else
        {
            reroll = 5;
        }
        rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";

        //Generate randomVoucher
        if (Game.access().GetRound() == 1)
        {
            Voucher[] vouchers = new Voucher[1];
            vouchers = inst.randomVoucher(1);
            voucher = vouchers[0];
            voucher.initialPrice = Mathf.CeilToInt(voucher.initialPrice * playerInst.discount);
            voucherCard.SetActive(true);
            voucherCard.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Vouchers/" + voucher.name.ToString());
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

        pack1Card.SetActive(true);
        pack2Card.SetActive(true);
        pack1Card.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Pack/" + pack1.packType.ToString() + "_" + pack1.edition.ToString());
        pack2Card.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Pack/" + pack2.packType.ToString() + "_" + pack2.edition.ToString());

        //Reset all Info text boxes for the different packs/cards in the shop
        VoucherDetails.alpha = 0;
        Pack1Details.alpha = 0;
        Pack2Details.alpha = 0;
        Mentor1Details.alpha = 0;
        Mentor2Details.alpha = 0;
    }

    //This function determines which cards will be placed in the shop.
    //This function also adds the image of the chosen cards to the shop.
    private void NewCards()
    {
        card1.SetActive(true);

        card2.SetActive(true);

        mentor1 = null;
        mentor2 = null;
        textbook1 = null;
        textbook2 = null;
        cardBuff1 = null;
        cardBuff2 = null;



        for (int i = 0; i <= shopMentorsAmount; i++)
        {
            //Use probabililties to generate the card shops
            int cardSlot = Random.Range(1, 100);
            //If number is 15 or under, create a textbook
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
                    card1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook1.name.ToString());
                    Debug.Log("Textbook 1 Name: " + textbook1.name.ToString());
                }
                else
                {
                    textbook2 = Textbooks[0];
                    textbook2.price = Mathf.CeilToInt(textbook2.price * playerInst.discount);
                    Debug.Log("Textbook 2 Name: " + textbook2.name.ToString());
                    card2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Textbook/textbook_" + textbook2.name.ToString());
                }

            }
            else if (cardSlot < 30 && cardSlot > 15) //Create a CardBuff
            {
                CardBuff[] cardBuffs = new CardBuff[2];
                cardBuffs = inst.randomCardBuffShop(1);
                Debug.Log("CardBuff Object: " + cardBuffs[0]);
                if (mentor1 == null && cardBuff1 == null && textbook1 == null)
                {
                    cardBuff1 = cardBuffs[0];
                    cardBuff1.price = Mathf.CeilToInt(cardBuff1.price * playerInst.discount);
                    card1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff1.name.ToString());
                    Debug.Log("CardBuff 1 Name: " + cardBuff1.name.ToString());
                }
                else
                {
                    cardBuff2 = cardBuffs[0];
                    cardBuff2.price = Mathf.CeilToInt(cardBuff2.price * playerInst.discount);
                    card2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardBuff/food_" + cardBuff2.name.ToString());
                    Debug.Log("CardBuff 2 Name: " + cardBuff2.name.ToString());
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

                    // --- START DEBUGGING ---
                    if (card1 == null)
                    {
                        Debug.LogError("FATAL ERROR: card1 is NULL!");
                        return;
                    }
                    if (card1.GetComponent<Image>() == null)
                    {
                        Debug.LogError("FATAL ERROR: card1 does not have an Image component!");
                        return;
                    }
                    // --- END DEBUGGING ---

                    card1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Mentor/" + mentor1.name.ToString());
                    Debug.Log("Mentor 1 Name: " + mentor1.name.ToString());
                }
                else
                {
                    if (mentor2 == null && cardBuff2 == null && textbook2 == null)
                    {
                        mentor2 = mentors[0];
                        mentor2.price = Mathf.CeilToInt(mentor2.price * playerInst.discount);

                        // (You would add similar checks for mentor2 and cardButton2 here)
                        if (card2 == null)
                        {
                            Debug.LogError("FATAL ERROR: card2 is NULL!");
                            return;
                        }
                        if (card2.GetComponent<Image>() == null)
                        {
                            Debug.LogError("FATAL ERROR: card2 does not have an Image component!");
                            return;
                        }

                        card2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Mentor/" + mentor2.name.ToString());
                        Debug.Log("Mentor 2 Name: " + mentor2.name.ToString());
                    }
                    else
                    {
                        Debug.Log("Mentor 2 already exists!.");
                    }
                }
            }

        }
    }

    //Function call takes in a Mentor Card and adds the Mentor Card into the players collection.
    private void BuyMentor(Mentor mentor, Button mentorButton, string mentorName)
    {
        buyButton.interactable = false; //Disable the button to prevent multiple purchases

        //If Joker threshold is hit, do not purchase.
        if ((mentor.edition != CardEdition.Negative && playerInst.mentorDeck.Count >= playerInst.maxMentors) ||
        playerInst.moneyCount < mentor.price)
        {
            Debug.Log("Not Enough Space In Mentors or Money Insufficient");
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            sfxManager.NoSFX();
            return;
        }

        //Add Mentor to user's collection
        playerInst.mentorDeck.Add(mentor);
        if (mentorCardHolder != null)
        {
            mentorCardHolder.AddMentor(mentor); //  Visually add to holder
        }

        // Check if newly bought mentor activate on Shop
        mentorShopEffect(mentor);

        //Remove Mentor from Screen
        GameObject.Find(mentorName).SetActive(false);
        mentorButton.gameObject.SetActive(false);

        int cardsPurchased = VictoryManager.access().GetCardsPurchased();
        VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - mentor.price;
        RemoveMentor1Details();
        RemoveMentor2Details();
        sfxManager.MoneyUsed();
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    private void BuyTextbook(Textbook textbook, Button textbookButton, string textbookName)
    {
        textbookButton.interactable = false; //Disable the button to prevent multiple purchases
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
            GameObject.Find(textbookName).SetActive(false);
            textbookButton.gameObject.SetActive(false);

            //Increment cards purchased
            int cardsPurchased = VictoryManager.access().GetCardsPurchased();
            VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

            // //Reduce money based on price and change text to display new money
            playerInst.moneyCount = playerInst.moneyCount - textbook.price;
            RemoveTextbook1Details();
            RemoveTextbook2Details();
            sfxManager.MoneyUsed();
            moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
        }
        else
        {
            //Should make the UI shake
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            sfxManager.NoSFX();
            Debug.Log("Not Enough Space In Consumables or Money Insufficient");
            return;
        }
    }
    private void BuyCardBuff(CardBuff cardBuff, Button cardBuffButton, string cardBuffName)
    {
        cardBuffButton.interactable = false; //Disable the button to prevent multiple purchases
        if (playerInst.consumables.Count >= playerInst.maxConsumables
        || playerInst.moneyCount < cardBuff.price)
        {
            //Should make the UI shake
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
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

        //Make cardBuff Disappear
        GameObject.Find(cardBuffName).SetActive(false);
        cardBuffButton.gameObject.SetActive(false);

        int cardsPurchased = VictoryManager.access().GetCardsPurchased();
        VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

        //Reduce Money and Update
        playerInst.moneyCount = playerInst.moneyCount - cardBuff.price;
        RemoveCardBuff1Details();
        RemoveCardBuff2Details();
        sfxManager.MoneyUsed();
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();

    }

    //Function call takes in a voucher card and adds the effects into the player's run.
    public void BuyVoucher(Voucher voucher, Button voucherButton)
    {
        voucherButton.interactable = false; //Disable the button to prevent multiple purchases
        if (playerInst.moneyCount < voucher.initialPrice)
        {
            sfxManager.NoSFX();
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            Debug.Log("Insufficient Funds");
            return;
        }
        Debug.Log("Voucher Purchased");

        //  Add voucher to Player's voucher list
        playerInst.vouchers.Add(voucher);

        //  Add voucher effect to user's run
        voucher.applyEffect();

        //  If "Reroll Pass" was bought
        if (voucher.name == VoucherNames.RerollPass)
        {
            reroll -= 2;
            rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";
        }

        //Remove voucher from screen
        voucherCard.SetActive(false);
        voucherButton.gameObject.SetActive(false);

        int cardsPurchased = VictoryManager.access().GetCardsPurchased();
        VictoryManager.access().SetCardsPurchased(cardsPurchased + 1);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - voucher.initialPrice;
        RemoveVoucherDetails();
        sfxManager.MoneyUsed();
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();
    }
    public void Pack1()
    {
        buyButton.interactable = false;
        BuyPack(pack1, buyButton, "Pack1");
        packSelected = 1;
    }
    public void Pack2()
    {
        buyButton.interactable = false;
        BuyPack(pack2, buyButton, "Pack2");
        packSelected = 2;
    }

    //Function call takes in a pack card and opens it, calling the necessary functions.
    private void BuyPack(Pack pack, Button packButton, string packName)
    {
        if (playerInst.moneyCount < pack.price)
        {
            sfxManager.NoSFX();
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            Debug.Log("Insufficient Funds");
            return;
        }

        int packsPurchased = VictoryManager.access().GetPacksPurchased();
        VictoryManager.access().SetPacksPurchased(packsPurchased + 1);

        //Make pack disappear
        packButton.interactable = false;
        GameObject.Find(packName).SetActive(false);
        packButton.gameObject.SetActive(false);

        // //Reduce money based on price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - pack.price;
        sfxManager.MoneyUsed();
        RemovePack1Details();
        RemovePack2Details();
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount.ToString();

        //Open pack and allow user to choose from cards
        OpenPacks(pack);
    }
    public void BuyPack1()
    {
        BuyPack(pack1, buyButton, "Pack1");
        packSelected = 1;
    }
    public void BuyPack2()
    {
        BuyPack(pack2, buyButton, "Pack2");
        packSelected = 2;
    }

    //Function causes the shop UI to disappear and transitions back into the regular scene.
    //Music should also change back to the regular gameplay music.
    public void NextRound()
    {
        //Reset Reroll price to 5 (or $3 with voucher)
        if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.RerollPass))
        {
            reroll = 3;
        }
        else
        {
            reroll = 5;
        }
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
            //Possibly make screen shake, or output a message saying not enough money
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            sfxManager.NoSFX();
            Debug.Log("Money Insufficient");
            return;
        }

        int numRerolls = VictoryManager.access().GetNumRerolls();
        VictoryManager.access().SetCardsPurchased(numRerolls + 1);

        //Reduce money based on reroll price and change text to display new money
        playerInst.moneyCount = playerInst.moneyCount - reroll;
        sfxManager.MoneyUsed();
        if (freeReroll == true)
        {
            freeReroll = false;
            if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.RerollPass))
            {
                reroll = 3;
            }
            else
            {
                reroll = 5;
            }
        }
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + playerInst.moneyCount;
        reroll++;
        rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";

        //Refresh the Card slots with new random Cards
        NewCards();
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Shop);   //  Run shop Mentors
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

    public void ActivateBuyButton(string cardName)
    {
        if (currentBuyButton != null)
        {
            Destroy(currentBuyButton);
        }

        RectTransform rectTransform = GameObject.Find(cardName).GetComponent<RectTransform>();
        currentBuyButton = Instantiate(buyButtonPrefab, rectTransform);
        currentBuyButton.transform.localPosition = new Vector3(0, -75, 0);

        buyButton = currentBuyButton.GetComponent<Button>();

        //Adds a listener to the buy button
        buyButton.onClick.AddListener(() =>
        {
            if (cardName == "Mentor1")
            {
                if (mentor1 != null)
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyMentor(mentor1, buyButton, cardName);
                }
                else if (cardBuff1 != null)
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyCardBuff(cardBuff1, buyButton, cardName);
                }
                else
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyTextbook(textbook1, buyButton, cardName);
                }
            }
            else if (cardName == "Mentor2")
            {
                if (mentor2 != null)
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyMentor(mentor2, buyButton, cardName);
                }
                else if (cardBuff2 != null)
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyCardBuff(cardBuff2, buyButton, cardName);
                }
                else
                {
                    buyButton.GetComponent<CanvasGroup>().alpha = 0;
                    BuyTextbook(textbook2, buyButton, cardName);
                }
            }
            else if (cardName == "Voucher")
            {
                buyButton.GetComponent<CanvasGroup>().alpha = 0;
                BuyVoucher(voucher, buyButton);
            }
            else if (cardName == "Pack1")
            {
                buyButton.GetComponent<CanvasGroup>().alpha = 0;
                BuyPack1();
            }
            else if (cardName == "Pack2")
            {
                buyButton.GetComponent<CanvasGroup>().alpha = 0;
                BuyPack2();
            }
        });
    }
    //This deactivates the Buy button if it exists.
    public void DeactivateBuyButton()
    {
        // Deactivate the buy button if it exists
        if (currentBuyButton != null)
        {

            Destroy(currentBuyButton);
        }

        currentBuyButton = null;
    }

    //Function is used to show the details of what the Joker does
    private void ShowMentor1Details()
    {
        ActivateBuyButton("Mentor1");
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(mentor1.name.ToString()) + "\n" + mentor1.description.ToString() + "\n$" + mentor1.price.ToString();
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveMentor1Details()
    {
        DeactivateBuyButton();
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowMentor2Details()
    {
        ActivateBuyButton("Mentor2");
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(mentor2.name.ToString()) + "\n" + mentor2.description.ToString() + "\n$" + mentor2.price.ToString();
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    private void RemoveMentor2Details()
    {
        DeactivateBuyButton();
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }

    private void ShowTextbook1Details()
    {
        ActivateBuyButton("Mentor1");
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = textbook1.GetDescription() + "\n$" + textbook1.price.ToString();
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveTextbook1Details()
    {
        DeactivateBuyButton();
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowTextbook2Details()
    {
        ActivateBuyButton("Mentor2");
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = textbook2.GetDescription() + "\n$" + textbook2.price.ToString();
        StartCoroutine(FadeIn(Mentor2Details));
        Mentor2Details.interactable = true;
    }
    private void RemoveTextbook2Details()
    {
        DeactivateBuyButton();
        Mentor2Details.blocksRaycasts = false;
        Mentor2Details.interactable = false;
        StartCoroutine(FadeOut(Mentor2Details));
    }

    private void ShowCardBuff1Details()
    {
        ActivateBuyButton("Mentor1");
        Mentor1Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(cardBuff1.name.ToString()) + "\n" + cardBuff1.GetDescription() + "\n$" + cardBuff1.price.ToString();
        StartCoroutine(FadeIn(Mentor1Details));
        Mentor1Details.interactable = true;
    }
    private void RemoveCardBuff1Details()
    {
        DeactivateBuyButton();
        Mentor1Details.blocksRaycasts = false;
        Mentor1Details.interactable = false;
        StartCoroutine(FadeOut(Mentor1Details));
    }
    private void ShowCardBuff2Details()
    {
        ActivateBuyButton("Mentor2");
        Mentor2Details.GetComponentInChildren<TMP_Text>().text = SplitString.SplitCase.Split(cardBuff2.name.ToString()) + "\n" + cardBuff2.GetDescription() + "\n$" + cardBuff2.price.ToString();
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
        ActivateBuyButton("Voucher");
        VoucherDetails.GetComponentInChildren<TMP_Text>().text = voucher.name.ToString() + "\n" + voucher.description + "\n$" + voucher.initialPrice.ToString();
        StartCoroutine(FadeIn(VoucherDetails));
        VoucherDetails.interactable = true;
    }
    public void RemoveVoucherDetails()
    {
        DeactivateBuyButton();
        VoucherDetails.blocksRaycasts = false;
        VoucherDetails.interactable = false;
        StartCoroutine(FadeOut(VoucherDetails));
    }
    public void ShowPack1Details()
    {
        ActivateBuyButton("Pack1");

        Pack1Details.GetComponentInChildren<TMP_Text>().text = pack1.packType.ToString() + "\nChoose " + pack1.selectableCards.ToString() + " of " + pack1.packSize.ToString() + " cards" + "\n" + "$" + pack1.price.ToSafeString();
        StartCoroutine(FadeIn(Pack1Details));
        Pack1Details.interactable = true;
    }
    public void RemovePack1Details()
    {
        DeactivateBuyButton();
        Pack1Details.blocksRaycasts = false;
        Pack1Details.interactable = false;
        StartCoroutine(FadeOut(Pack1Details));
    }
    public void ShowPack2Details()
    {
        ActivateBuyButton("Pack2");
        Pack2Details.GetComponentInChildren<TMP_Text>().text = pack2.packType.ToString() + "\nChoose " + pack2.selectableCards.ToString() + " of " + pack2.packSize.ToString() + " cards" + "\n" + "$" + pack2.price.ToSafeString();
        StartCoroutine(FadeIn(Pack2Details));
        Pack2Details.interactable = true;
    }
    public void RemovePack2Details()
    {
        DeactivateBuyButton();
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
        StartCoroutine(FadeOut(shopUI));

        //Attach the corresponding objects(cards) onto the sections needed.
        PackCard1 = pack.cardsInPack[0];
        PackCard2 = pack.cardsInPack[1];
        PackCard3 = pack.cardsInPack[2];

        packText.GetComponentInChildren<TMP_Text>().text = "" + pack.packType.ToString() + "\n\nChoose Up To " + pack.selectableCards.ToString();


        Debug.Log("PackCard1 Mentor:" + PackCard1.mentor);
        Debug.Log("PackCard1 cardBuff:" + PackCard1.cardBuff);
        Debug.Log("PackCard1 textbook:" + PackCard1.textbook);

        PackCard1Button = GameObject.Find("Pack Card 1").GetComponent<Button>();
        PackCard2Button = GameObject.Find("Pack Card 2").GetComponent<Button>();
        PackCard3Button = GameObject.Find("Pack Card 3").GetComponent<Button>();

        //Set Images For Cards
        SetPackImage(PackCard1, PackCard1Button);
        SetPackImage(PackCard2, PackCard2Button);
        SetPackImage(PackCard3, PackCard3Button);


        StartCoroutine(FadeIn(PackGroup));
        PackGroup.interactable = true;
        PackGroup.blocksRaycasts = true;
        if (pack.packSize > 4)
        {
            PackCard4 = pack.cardsInPack[3];
            PackCard5 = pack.cardsInPack[4];
            PackCard4Button = GameObject.Find("Pack Card 4").GetComponent<Button>();
            PackCard5Button = GameObject.Find("Pack Card 5").GetComponent<Button>();
            SetPackImage(PackCard4, PackCard4Button);
            SetPackImage(PackCard5, PackCard5Button);
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
            if (pack1.getSelectableCount(pack1.edition) <= cardsSelected)//If player has selected all cards, exit
            {
                PackCardSelected();
            }
        }
        else if (packSelected == 2)
        {
            if (pack2.getSelectableCount(pack2.edition) <= cardsSelected)//If player has selected all cards, exit
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
                mentorShopEffect(PackCard1.mentor);
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        else if (PackCard1.textbook != null)
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
        else if (PackCard1.cardBuff != null)
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
        else
        {
            deck.AddCard(PackCard1);
            cardsSelected++;
        }

        PackCard1Button.interactable = false;
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
                mentorShopEffect(PackCard2.mentor);
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        else if (PackCard2.textbook != null)
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
        else if (PackCard2.cardBuff != null)
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
        else
        {
            deck.AddCard(PackCard2);
            cardsSelected++;
        }

        PackCard2Button.interactable = false;
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
                mentorShopEffect(PackCard3.mentor);
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        else if (PackCard3.textbook != null)
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
        else if (PackCard3.cardBuff != null)
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
        else
        {
            deck.AddCard(PackCard3);
            cardsSelected++;
        }

        PackCard3Button.interactable = false;
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
                mentorShopEffect(PackCard4.mentor);
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        else if (PackCard4.textbook != null)
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
        else if (PackCard4.cardBuff != null)
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
        else
        {
            deck.AddCard(PackCard4);
            cardsSelected++;
        }

        PackCard4Button.interactable = false;
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
                mentorShopEffect(PackCard5.mentor);
            }
            Debug.Log("Mentor Count: " + playerInst.mentorDeck.Count);
            cardsSelected++;
        }
        //If CardBuff Card, check if count is not max then add if not
        else if (PackCard5.textbook != null)
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
        else if (PackCard5.cardBuff != null)
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
        else
        {
            deck.AddCard(PackCard5);
            cardsSelected++;
        }

        PackCard5Button.interactable = false;
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
        cardsSelected = 0;
        PackGroup.interactable = false;
        PackGroup.blocksRaycasts = false;

        //Reset Cards
        //Set cards to null and removing blocking for PackGroup
        PackCard1Button.interactable = true;
        PackCard2Button.interactable = true;
        PackCard3Button.interactable = true;
        PackCard4Button.interactable = true;
        PackCard5Button.interactable = true;

        //Fade Out UI
        StartCoroutine(FadeOut(PackGroup));
        StartCoroutine(FadeOut(PackGroupPlus));

        //Bring back Regular UI
        StartCoroutine(FadeIn(RegularUI));
        StartCoroutine(FadeIn(shopUI));
        RegularUI.interactable = true;
        RegularUI.blocksRaycasts = true;

    }

    //This returns the card from the pack based on the index provided.
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
        else if (mentor.name == MentorName.Revisionist)
        {
            reroll = 0;
            rerollButton.GetComponentInChildren<TMP_Text>().text = $"Reroll\n${reroll}";
        }
    }

    //  Is this used for anything now?
    public void TransitionToShop()
    {
        NewCards();
    }

    //  Reset certain mentors at end of shop
    public void resetShopMentor()
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
            else if (mentor.name == MentorName.Revisionist)
            {
                Revisionist revisionist = (Revisionist)mentor;
                revisionist.hasRerolled = false;
            }
        }
    }

    //  Method to update shop's moneyCount text (for example after selling)
    public void UpdateMoneyDisplay()
    {
        if (moneyText == null)
        {
            Debug.LogError("ShopManager could not update UI! moneyText is null!");
        }

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
