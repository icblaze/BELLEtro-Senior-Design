// This Document contains the code for the Textbook class
// This class contains information about a Textbook which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and varuables
// Andy (flakkid): applying hand table logic, singleton change

using System.Collections;
using UnityEngine;
using System.Linq;

// Textbooks will enhance the base chips and mult for a kind of hand
public class Textbook : Consumable
{
    public TextbookName name;
    Player player = Player.access();

    //  Placeholder default constructor (High Card)
    public Textbook()
    {
        name = TextbookName.HighCard;
        price = 3;
        sellValue = 1;
        isInstant = true;
        type = ConsumableType.Textbook;
        isDisabled = false;
        isNegative = false;
        description = GetDescription();
    }

    //  Construct Textbook consumable with name of hand
    public Textbook(TextbookName name)
    {
        this.name = name;
        price = 3; 
        sellValue = 1; 
        isInstant = true; 
        type = ConsumableType.Textbook;
        isDisabled = false;
        isNegative = false;
        description = GetDescription();
    }

    //  Increases appropiate hand based on textbook name
    public void applyTextbook()
    {
        bool hasAnnotated = player.vouchers.Any(voucher => voucher.name == VoucherNames.AnnotatedEdition);
        player.handTable[name].IncreaseLevel();
        if(hasAnnotated)
        {
            player.handTable[name].IncreaseLevel(); //  Level up again
        }

        //  Set previous consumable to used Textbook
        Game.access().previousConsumable = new Textbook(name);
    }

    //  Return description of textbook including it's current level
    public string GetDescription()
    {
        bool hasAnnotated = player.vouchers.Any(voucher => voucher.name == VoucherNames.AnnotatedEdition);

        string handName = name.ToString();
        int level = player.handTable[name].level;
        int incrementMult = player.handTable[name].incrementMult;
        int incrementChips = player.handTable[name].incrementChips;

        if(!hasAnnotated)
        {
            return "(lvl. " + level + ") Level up " + handName + " +" + incrementMult + " Mult and" + "+" + incrementChips + " Chips";
        }
        else
        {
            return "(lvl. " + level + ") Level up " + handName + " +" + (incrementMult * 2) + " Mult and" + "+" + (incrementChips * 2) + " Chips (Annotated)";
        }
    }
}
