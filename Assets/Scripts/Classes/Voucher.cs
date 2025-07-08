// This Document contains the code for the Voucher class.
// This class is used to hold information on a Voucher that is either 
// purchaseable (where the price is used) or owned by a player.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui: Added variables for the base class
// Van: Designed functionality

using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

// The Vouchers will permanently enhance the player with many kinds of Buffs
public class Voucher
{
    public VoucherNames name;
    public int initialPrice = 10; // Vouchers have a base price of 10

    // Constructor for Voucher
    public Voucher(VoucherNames voucherName)
    {
        this.name = voucherName;
    }

    // Apply the effect of the voucher to the player
    public void applyEffect()
    {
        switch (name)
        {
            case VoucherNames.ExtraCredit:
                // Grants bonus points on correct play or combo
                break;

            case VoucherNames.StudyGroup:
                // Draw +1 card each round
                break;

            case VoucherNames.AnnotatedEdition:
                // Makes Textbooks more effective (e.g., increase power or reduce cost)
                break;

            case VoucherNames.OfficeHours:
                // Reduces shop prices
                break;

            case VoucherNames.FluentStart:
                // Start the run with an extra buff or card
                break;

            case VoucherNames.LectureBoost:
                // Increase multiplier on specific hand types like Full House or Straight
                break;

            case VoucherNames.TenureTrack:
                // Gain a growing passive bonus each round (e.g., +1% score/round)
                break;

            case VoucherNames.BrainstormBonus:
                // Buffs from cards or mentors stack better or last longer
                break;

            default:
                Debug.LogWarning("Voucher effect not implemented: " + name);
                break;
        }
    }
}