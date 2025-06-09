//This document contains the code for the PackEdition enum
//This enum is responsible for giving a certain edition for each pack
//Current Devs:
//Zacharia Alaoui : made the enum class, and added comments for each enum


using System.ComponentModel.Design;

public enum PackEdition
{
    None,
    Normal_Pack,       // Normal packs allow players to select 1 out of 3 playing cards to add to their deck

    Jumbo_Pack,        // Jumbo packs allow players to select 1 out of 5 playing cards to add to their deck

    Mega_Pack          // Mega packs allow players to select 2 out of 5 playing cards to add to their deck
}