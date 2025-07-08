// This Document contains the code for the VoucherNames Enum.
// This Enum contains the names for each of the Vouchers 
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

public enum VoucherNames
{
    None,
    ExtraCredit,        // Grants bonus points on correct play or combo
    StudyGroup,         // Draw +1 card each round
    //PopQuiz,            // Grants a random bonus effect at the start of each round
    AnnotatedEdition,   // Makes Textbooks more effective (e.g., increase power or reduce cost)
    OfficeHours,        // Reduces shop prices
    FluentStart,        // Start the run with an extra buff or card
    LectureBoost,       // Increase multiplier on specific hand types like Full House or Straight
    TenureTrack,        // Gain a growing passive bonus each round (e.g., +1% score/round)
    //SpeedReading,       // Speeds up draw/animation or allows playing more cards
    BrainstormBonus,    // Buffs from cards or mentors stack better or last longer
    //RerollPass,         // Grants 1 free reroll per shop phase or card draw phase
    //SyntaxSurge         // Big score bonus when forming advanced hands (Full House, Straight Flush, etc.)
}
