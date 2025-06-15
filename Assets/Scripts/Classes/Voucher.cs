// This Document contains the code for the Voucher class.
// This class is used to hold information on a Voucher that is eiher 
// purchaseable (where the price is used) or owned by a player.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui: Added variables for the base class

using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

// The Vouchers will permanently enhance the player with many kinds of Buffs
public class Voucher
{
    public VoucherNames name;
    public int initialPrice = 10; //Vouchers have a base price of 10

    //Constructor for Voucher
    public Voucher(VoucherNames voucherName)
    {
        this.name = voucherName;
    }    


    public void applyEffect(Player player)
    {

    }
}
