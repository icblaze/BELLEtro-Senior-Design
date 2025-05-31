// This Document contains the code for the Voucher class.
// This class is used to hold information on a Voucher that is eiher 
// purchaseable (where the price is used) or owned by a player.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

// The Vouchers will permanently enhance the player with many kinds of Buffs
public class Voucher
{
    public VoucherNames name;
    public int initialPrice;

    public void applyEffect(Player player)
    {

    }
}
