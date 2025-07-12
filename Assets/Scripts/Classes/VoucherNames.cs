// This Document contains the code for the VoucherNames Enum.
// This Enum contains the names for each of the Vouchers 
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

public enum VoucherNames
{
    None,
    ExtraCredit,        // Gain +1 Discard Per Round
    StudyGroup,         // +1 Hand Size
    PopQuiz,            // Give a random Card Buff after Special Blind is selected
    AnnotatedEdition,   // Textbooks will level hand by two levels
    OfficeHours,        // New Shop Items are 25% off
    FluentStart,        // Gain +1 Hand
    LectureBoost,       // Level up every hand once
    TenureTrack,        // Raises interest cap to $10
    SpeedReading,       // Skipping blinds grant $3
    BrainstormBonus,    // +1 Consumable Slot
    RerollPass,         // Shop Reroll costs $2 less
    SyntaxSurge         // +5 Chips added to every card in deck
}
