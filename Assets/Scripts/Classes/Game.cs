// This Document contains the code for the Game class.
// This class is used to hold information about the game's state.
// This class contains all the logic for retrieving the different packs in the game,
// and this class is used to retrieve different consumbales and Mentors, and enhanced cards.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Andy (flakkid): added previous consumable variable
// Zacharia Alaoui (ZachariaAlaoui): Added the functions and created the logic for the functions, I also created
//                                   the logic for obtaining the different packs in the game.
// Fredrick (bouloutef04): Added functions to obtain mentors, cardbuffs, and textbooks for the shop.

using System.Numerics;
using System.Collections;
//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using System.Xml;
using Unity.Collections;



// The Game class contains all of the information about the Game
public class Game
{

    private static Game instance;

    //Singelton for the Game class
    public static Game access()
    {
        if (instance == null)
        {
            instance = new Game();
        }
        return instance;
    }

    public GameObject cardPrefab;
    public static int ante = 1;                                                   //Ante variable holds the current ante that the player is on
    public static int roundValue = 1;                                              //RoundValue is the Round within the current Ante
    public BigInteger currentChipAmount;                                           //currentChipAmount contains the number of chips the player currently has
    private int ChipTotal;                                                         //ChipTotal is the number of chips needed to win a round
    public Voucher[] voucherHolder;                                                //VoucherHolder contains the current Ante's Vouchers
    public SpecialBlind currentSpecialBlind;                                       //CurrentSpecialBlind contains this Ante's Special Blind
    public Player thePlayer;                                                       //The Player is a refrence to the Player class 
    private int index;                                                             //This variable will hold a value that we could use to index into our deck
    public int skipCount;                                                          //This int will be used to count the number of skips the player does
    public Consumable previousConsumable = null;                                   //Stores the name of last used consumable
    public List<SpecialBlind> pastSpecialBlinds = new List<SpecialBlind>();        //PastSpecialBlinds are the used Blinds 

    public bool isEasyMode = true;                                                 //This variable is used to determine the mode that the user selected.  

    //Getter and Setter for the ante variable   
    public int GetAnte()
    {
        return ante;
    }
    public void SetAnte(int i)
    {
        ante = i;
        //  Change value in BackendHook
        BackendHook.currentAnte = i;
    }

    //Getter and Setter for the roundValue variable
    public int GetRound()
    {
        return roundValue;
    }
    public void SetRound(int i)
    {
        roundValue = i;
    }

    //This function is used to create a seed, so that we can get a random card from the deck.
    //This randomizer will also be used to retrieve a random index so we can select a random consumable/pack.
    public int randomizer(int start, int Size)
    {
        return Random.Range(start, Size);
    }

    //This will randomly draw a card from the deck after a player plays a hand or removes cards from the deck.
    public PCard randomDraw(List<PCard> deckCards)
    {
        index = randomizer(0, deckCards.Count);
        return deckCards[index];
    }

    //This function is responsible for generating a list of cards, useful for when we want to retrieve a random cards for packs.
    public List<PCard> randomPackCards(Pack pack)
    {
        int count = 0;

        //Get access to the Deck 
        Deck deck = Deck.access();

        deck.combinePiles();    //  Ensures that when getting clonedCard, include both piles

        //This list of card objects will contain the random cards that were selected from the deck
        List<PCard> cardList = new List<PCard>();

        //Retrieve random cards from the deck so that we can potentially apply card modifiers to the cards
        while (count < pack.packSize)
        {
            int index = randomizer(0, deck.deckCards.Count);
            Debug.Log("Index: " + index);
            Debug.Log("DeckCards" + deck.deckCards);
            PCard clonedCard = PCard.CloneCard(deck.deckCards[index]);

            //Call a function here to determine what the enhancements will be on the current card
            applyEnhancement(clonedCard, pack.edition);

            bool alreadyExists = cardList.Any(card => card.Equals(clonedCard));    //This checks if the new card exists already in the card list

            if (!alreadyExists)
            {
                cardList.Add(clonedCard);
                count++;
            }
            else
            {
                continue;
            }
        }

        return cardList;
    }

    //This function modifies a deck card , and applies any modifiers to it so that it can be added to a pack for use.
    public PCard applyEnhancement(PCard newCard, PackEdition packEdition)
    {
        //Set the temporary dictionaries with the ones in the CardModifier class.
        //This allows us to enhance the dictionaries below based on packtype, this allows rarer cards to show up in higher ranked packs.
        var adjustedEditions = CardModifier.access().editionRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var adjustedEnhancements = CardModifier.access().enhancementRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var adjustedSeals = CardModifier.access().sealRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (packEdition == PackEdition.Jumbo_Pack)
        {
            //This updates all the keys in the dictionary that are less than 60.
            //This piece of code doubles the Key in the dictionary, which gives 
            //rarer cards a higher chance to be selected

            adjustedEditions = adjustedEditions.ToDictionary(
                kvp => (kvp.Key < 60) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedEnhancements = adjustedEnhancements.ToDictionary(
                kvp => (kvp.Key < 40) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedSeals = adjustedSeals.ToDictionary(
                kvp => (kvp.Key < 50) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

        }
        else if (packEdition == PackEdition.Mega_Pack)
        {
            adjustedEditions = adjustedEditions.ToDictionary(
                kvp => (kvp.Key < 60) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedEnhancements = adjustedEnhancements.ToDictionary(
                kvp => (kvp.Key < 40) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedSeals = adjustedSeals.ToDictionary(
                kvp => (kvp.Key < 50) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );
        }

        newCard.seal = CardModifier.access().GetWeightedModifier(adjustedSeals);
        newCard.enhancement = CardModifier.access().GetWeightedModifier(adjustedEnhancements);
        newCard.edition = CardModifier.access().GetWeightedModifier(adjustedEditions);


        return newCard;
    }

    //This will generate a random voucher for the shop.
    public Voucher[] randomVoucher(int voucherCount)
    {
        Voucher[] vouchers = new Voucher[voucherCount];
        int count = 0;

        while (count < voucherCount)
        {
            index = randomizer(1, System.Enum.GetValues(typeof(VoucherNames)).Length);                                            // Select a random number that is in the range of the VoucherNames enum 
            Voucher voucherCard = new Voucher((VoucherNames)index);                                                            // This constructs a new Voucher with the name of a random voucher from the VoucherNames enum  

            bool alreadyExists = System.Array.Exists(vouchers, voucher => voucher != null && voucher.name == voucherCard.name); //This checks if the Voucher already exists in out vouchers array
            bool playerHas = Player.access().vouchers.Contains(voucherCard);    //  Check if Player already has the voucher

            if (!alreadyExists && !playerHas)
            {
                vouchers[count] = voucherCard;
                count++;
            }
            else
            {
                continue;
            }

        }

        return vouchers;
    }

    //This will generate a random textbook card for packs.
    public List<PCard> randomTextbook(Pack pack)
    {
        List<PCard> textBookCards = new List<PCard>();
        int count = 0;
        bool alreadyExists;

        while (count < pack.packSize)
        {
            int index = randomizer(1, System.Enum.GetValues(typeof(TextbookName)).Length);

            PCard textBookCard = new PCard();
            textBookCard.textbook = new Textbook((TextbookName)index);

            //Checks if the textbook card already exists in the textBookCards list
            alreadyExists = textBookCards.Any(textBook => textBook.textbook.name == textBookCard.textbook.name);

            if (!alreadyExists)
            {
                textBookCards.Add(textBookCard);
                count++;
            }
            else
            {
                continue;
            }
        }

        return textBookCards;
    }

    //This will generate a random textbook card for the shop.
    public Textbook[] randomTextbookShop(int num)
    {
        Textbook[] textbooksList = new Textbook[num];
        int count = 0;

        while (count < num)
        {
            index = randomizer(1, System.Enum.GetValues(typeof(TextbookName)).Length);    // Select a random number that is in the range of the TextbookName enum 
            Textbook textBookCard = new Textbook((TextbookName)index);                 // Created a new textbook card  

            bool alreadyExists = System.Array.Exists(textbooksList, textBook => textBook != null && textBook.name == textBookCard.name);

            //This checks if the textBook already exists within the array.
            //This statement helps prevent producing duplicate textbook cards.
            if (!alreadyExists)
            {
                textbooksList[count] = textBookCard;
                count++;
            }
            else
            {
                continue;
            }

        }

        return textbooksList;
    }

    //This function is responsible for retrieving random card buffs for the shop.
    public CardBuff[] randomCardBuffShop(int numCardBuffs)
    {
        CardBuff[] cardBuffCards = new CardBuff[numCardBuffs];
        int count = 0;

        while (count < numCardBuffs)
        {
            index = randomizer(1, System.Enum.GetValues(typeof(CardBuffName)).Length);    // Select a random number that is in the range of the cardBuffName enum 
            CardBuff cardBuffCard = CardBuff.CardBuffFactory((CardBuffName)index);     // Create a new cardbuff card with the card buff factory

            bool alreadyExists = System.Array.Exists(cardBuffCards, cardBuff => cardBuff != null && cardBuff.name == cardBuffCard.name);    //Check to see if the cardbuff already exists within the cardBuffCards array

            if (!alreadyExists)
            {
                cardBuffCards[count] = cardBuffCard;
                count++;
            }
            else
            {
                continue;
            }
        }


        return cardBuffCards;
    }
    //This function is responsible for retrieving random Mentors for packs.
    public List<PCard> randomMentor(Pack pack)
    {
        List<PCard> mentorCards = new List<PCard>();
        int count = 0;
        bool alreadyExists;

        while (count < pack.packSize)
        {
            int mentorNameIndex = randomizer(1, System.Enum.GetValues(typeof(MentorName)).Length);    
            CardEdition mentorEdition = CardModifier.access().GetWeightedModifier(CardModifier.access().editionRates);

            PCard newMentorCard = new PCard();

            newMentorCard.mentor = Mentor.MentorFactory((MentorName)mentorNameIndex, mentorEdition);

            //Checks if the cardBuff already exists in the cardBuffCards list
            alreadyExists = mentorCards.Any(mentorCard => mentorCard.mentor.name == newMentorCard.mentor.name);

            if (!alreadyExists)
            {
                mentorCards.Add(newMentorCard);
                count++;
            }
            else
            {
                continue;
            }

        }

        return mentorCards;
    }

    //This function is responsible for retrieving random Mentors for the shop.
    public Mentor[] randomMentorShop(int numMentors)
    {
        Mentor[] mentorCards = new Mentor[numMentors];
        int count = 0;

        while (count < numMentors)
        {
            int mentorNameIndex = randomizer(1, System.Enum.GetValues(typeof(MentorName)).Length);
            CardEdition mentorEdition = CardModifier.access().GetWeightedModifier(CardModifier.access().editionRates); // Select a random edition (weighted)

            Mentor mentorCard = Mentor.MentorFactory((MentorName)mentorNameIndex, mentorEdition);     // Create a Mentor with the MentorFactory

            bool alreadyExists = System.Array.Exists(mentorCards, mentor => mentor != null && mentor.name == mentorCard.name);    //Check to see if the mentor already exists within the mentorCards array

            if (!alreadyExists)
            {
                mentorCards[count] = mentorCard;
                count++;
            }
            else
            {
                continue;
            }
        }

        return mentorCards;
    }

    //This function is responsible for retrieving random card buffs for packs.
    public List<PCard> randomCardBuff(Pack pack)
    {
        List<PCard> cardBuffCards = new List<PCard>();    //This list contains card objects

        int count = 0;
        while (count < pack.packSize)
        {
            int index = randomizer(1, System.Enum.GetValues(typeof(CardBuffName)).Length);

            PCard newCardBuff = new PCard();

            newCardBuff.cardBuff = CardBuff.CardBuffFactory((CardBuffName)index);  //Create a new CardBuff and assign it to our new PCard instance

            //Checks if the cardBuff already exists in the cardBuffCards list
            bool alreadyExists = cardBuffCards.Any(cardBuffCard => cardBuffCard.cardBuff.name == newCardBuff.cardBuff.name);

            if (!alreadyExists)
            {
                cardBuffCards.Add(newCardBuff);
                count++;
            }
            else
            {
                continue;
            }
        }

        return cardBuffCards;
    }

    //This function is responsible for retrieving random packs for the shop.
    public Pack[] randomPacks(int packCount)
    {
        Pack[] pack = new Pack[packCount];

        for (int i = 0; i < packCount; i++)
        {
            pack[i] = new Pack(); //Instantiate a new pack object

            PackEdition selectedEdition = (PackEdition)Random.Range(1, System.Enum.GetValues(typeof(PackEdition)).Length); // Get a PackEdition value that corresponds to a packEdition in the enum list
            PackType selectedType = (PackType)Random.Range(0, System.Enum.GetValues(typeof(PackType)).Length);             // Get the PackType value that corresponds to a PackType in the enum list

            //The following code fills all the variables for the pack
            pack[i].packType = selectedType;
            pack[i].edition = selectedEdition;
            pack[i].price = pack[i].getPackPrice(selectedEdition);
            pack[i].selectableCards = pack[i].getSelectableCount(selectedEdition);
            pack[i].packSize = pack[i].getPackSize(selectedEdition);

            if (pack[i].packType == PackType.Standard_Pack)
            {
                pack[i].cardsInPack = randomPackCards(pack[i]);
            }
            else if (pack[i].packType == PackType.CardBuff_Pack)
            {
                pack[i].cardsInPack = randomCardBuff(pack[i]);
            }
            else if (pack[i].packType == PackType.Textbook_Pack)
            {
                pack[i].cardsInPack = randomTextbook(pack[i]);
            }
            else if (pack[i].packType == PackType.Mentor_Pack)
            {
                pack[i].cardsInPack = randomMentor(pack[i]);
            }
        }

        return pack;
    }

    //This function is responsible for retrieving random special blinds for each Ante
    public SpecialBlind randomSpecialBlind()
    {
        int count = 0;
        bool alreadyExists = false;

        SpecialBlind blind = null;
        while (count < 1)
        {
            index = randomizer(0, 8);
            blind = SpecialBlind.BlindFromNumber(index);
            
            if (blind != null && blind.minimumAnte > ante)//If special blind can't spawn yet, find different blind
            {
                continue;
            }
            alreadyExists = pastSpecialBlinds.Any(currentBlind => currentBlind.blindType == blind.blindType);

            if (!alreadyExists)
            {
                pastSpecialBlinds.Add(blind);          //This line adds the blind to the list that contains the past special blinds in the game.
                count++;
            }
            else
            {
                continue;
            }

        }

        return blind;
    }

    public Tag[] randomTag(int tagCount)
    {
        Tag[] tagList = new Tag[tagCount];

        Tag tag = null;
        for (int i = 0; i < tagCount; ++i)
        {
            index = randomizer(0, 5);
            tag = Tag.TagFromIndex(index);
            tagList[i] = tag;
        }

        return tagList;
    }

    public int GetChipTotal()
    {
        return ChipTotal;
    }

    public int SetChipTotal(int chipTotal)
    {
        ChipTotal = chipTotal;
        return ChipTotal;
    }

    public void ResetGame()//Resets the variables held in Game.cs
    {
        ante = 1;
        roundValue = 1;
        currentChipAmount = 0;
        ChipTotal = 0;
        voucherHolder = null;
        currentSpecialBlind = null;
        //index = 0;
        skipCount = 0;
        previousConsumable = null;
        pastSpecialBlinds = new List<SpecialBlind>();
        isEasyMode = true;
    }

    public void Start()
    {
        BlindSceneManager blindSceneManager = GameObject.FindFirstObjectByType<BlindSceneManager>().GetComponent<BlindSceneManager>();
        blindSceneManager.NewBlind();
    }


}
