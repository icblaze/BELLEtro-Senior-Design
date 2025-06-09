// This Document contains the code for the PackType enum
// This enum contains the names for each kind of pack that can be rolled
// Current Devs:
// Robert (momomonkeyman): made enum
// Zacharia Alaoui: Added comments to the enum 

using System.Collections;
using UnityEngine;

public enum PackType
{   
    Standard_Pack,      // This pack can include cards such as Textbook, CardBuff, Mentor, and regular deck cards with enhancements.

    CardBuff_Pack,      // This pack can only contain CardBuff cards

    Textbook_Pack,      // This pack can only contain Textbook cards

    Mentor_Pack         // This pack can only contain Mentor cards
}
