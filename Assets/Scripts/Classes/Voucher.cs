// This Document contains the code for the Voucher class.
// This class is used to hold information on a Voucher that is either 
// purchaseable (where the price is used) or owned by a player.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui: Added variables for the base class
// Van: Designed functionality

using System.Collections;
using System;
using System.Reflection.Emit;
using UnityEngine;

// The Vouchers will permanently enhance the player with many kinds of Buffs
public class Voucher
{
    public VoucherNames name;
    public int initialPrice = 10; // Vouchers have a base price of 10
    public string description;

    // Constructor for Voucher
    public Voucher(VoucherNames voucherName)
    {
        this.name = voucherName;
        description = SetVoucherDescription();
    }

    private string SetVoucherDescription()
    {
        switch (name)
        {
            case VoucherNames.ExtraCredit:
                return "Gain +1 Discard Per Round";
            case VoucherNames.StudyGroup:
                return "+1 Hand Size";
            case VoucherNames.PopQuiz:
                return "Give a random Card Buff after Special Blind is selected";
            case VoucherNames.AnnotatedEdition:
                return "Textbooks will level hand by two levels";
            case VoucherNames.OfficeHours:
                return "New Shop Items are 25% off";
            case VoucherNames.FluentStart:
                return "Gain +1 Hand";
            case VoucherNames.LectureBoost:
                return "Level up every hand once";
            case VoucherNames.TenureTrack:
                return "Raises interest cap to $10";
            case VoucherNames.SpeedReading:
                return "Skipping blinds grant $3";
            case VoucherNames.RerollPass:
                return "Shop Reroll costs $2 less";
            case VoucherNames.BrainstormBonus:
                return "+1 Consumable Slot";
            case VoucherNames.SyntaxSurge:
                return "+5 Chips added to every card in deck";
        }
        return "Nothing?";
    }

    public string GetDescription()
    {
        return description;
    }

    // Apply the effect of the voucher to the player
    public void applyEffect()
    {
        switch (name)
        {
            case VoucherNames.ExtraCredit:
                // Gain +1 Discard Per Round
                Player.access().maxDiscards++;
                GameObject.FindFirstObjectByType<DeleteCard>().discardsLeft.text = Player.access().maxDiscards.ToString();
                break;

            case VoucherNames.StudyGroup:
                // +1 Hand Size
                Player.access().handSize++;
                break;

            case VoucherNames.PopQuiz:
                //  Give a random Card Buff after Special Blind is selected
                //  Logic handled in BlindSelectManager.cs
                break;

            case VoucherNames.AnnotatedEdition:
                // Textbooks will level hand by two levels
                //  Textbook.cs will handle this logic
                break;

            case VoucherNames.OfficeHours:
                // New Shop Items are 25% off
                Player.access().discount = 0.75f;
                break;

            case VoucherNames.FluentStart:
                // Gain +1 Hand Per Round
                Player.access().maxHandCount++;
                GameObject.FindFirstObjectByType<PlayHand>().handsLeft.text = Player.access().maxHandCount.ToString();
                break;

            case VoucherNames.LectureBoost:
                // Level up every hand
                foreach (TextbookName name in Enum.GetValues(typeof(TextbookName)))
                {
                    Player.access().handTable[name].IncreaseLevel();
                }
                break;

            case VoucherNames.TenureTrack:
                // Raise interest cap to $10
                Player.access().maxInterest = 10;
                break;

            case VoucherNames.SpeedReading:
                // Skipping blinds grant $3
                //  Logic handled in BlindSelectManager.cs
                break;

            case VoucherNames.BrainstormBonus:
                // +1 Consumable Slot
                Player.access().maxConsumables++;
                break;

            case VoucherNames.RerollPass:
                // Shop Reroll costs $2 less
                // Handled in ShopManager.cs
                break;

            case VoucherNames.SyntaxSurge:
                // +5 Chips added to every card in deck
                Deck.access().combinePiles();
                for (int i = 0; i < Deck.access().deckCards.Count; i++)
                {
                    Deck.access().deckCards[i].chips += 5;
                }
                break;

            default:
                Debug.LogWarning("Voucher effect not implemented: " + name);
                break;
        }
    }
}