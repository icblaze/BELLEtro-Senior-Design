// This Document contains the code for the LibraryCard mentor
// Effect is to give $1 at end of round per unique Textbook used
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class LibraryCard : Mentor
{
    Player player = Player.access();
    int uniqueCount = 0;

    //  Mentor name and basePrice are preset
    public LibraryCard(CardEdition edition) : base(MentorName.LibraryCard, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.PostBlind };
        description = "Give <color=#BB8525FF>$1</color> at end of round per unique Textbook used";
    }

    //  Change description basd on uniqueCount
    public override string GetDescription()
    {
        calculateUnique();
        description = "Give <color=#BB8525FF>$1</color> at end of round per unique Textbook used " + $"(<color=#BB8525FF>${uniqueCount}</color>)";
        return description;
    }


    //  Give $1 at end of round per unique Textbook used
    public override void UseMentor()
    {
        calculateUnique();

        //  Add to player's moneyCount
        EndOfRoundManager.access().IncrementMentorReward(uniqueCount);

    }

    //  For calculating unique used textbook
    private void calculateUnique()
    {
        uniqueCount = 0;

        //  Iterate through player's handTable
        for (int tbook = 0; tbook < player.handTable.Count; tbook++)
        {
            //  If a hand's level is above 1, then textbook has been used for it
            if (player.handTable[(TextbookName)tbook].level > 1)
            {
                uniqueCount++;
            }
        }
    }
}
