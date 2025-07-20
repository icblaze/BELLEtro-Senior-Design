using UnityEngine;

public class Studious : Tag
{
    private string hand = "";
    public Studious() : base(TagNames.Studious)
    {
        SetHandName();
        description = "Upgrades " + hand + " by three levels";
        tagName = "Studious";
    }

    private void SetHandName()
    {
        int index = randomizer(0, 12);
        switch (index)
        {
            case 0:
                hand = "High Card";
                break;
            case 1:
                hand = "Pair";
                break;
            case 2:
                hand = "Two Pair";
                break;
            case 3:
                hand = "Three Of A Kind";
                break;
            case 4:
                hand = "Straight";
                break;
            case 5:
                hand = "Flush";
                break;
            case 6:
                hand = "Full House";
                break;
            case 7:
                hand = "Four Of A Kind";
                break;
            case 8:
                hand = "Straight Flush";
                break;
            case 9:
                hand = "Five Of A Kind";
                break;
            case 10:
                hand = "Flush House";
                break;
            case 11:
                hand = "Flush Five";
                break;
            default:
                break;
        }

    }

    public override void applyTag()
    {
        switch (hand)
        {
            case "High Card":
                Player.access().handTable[TextbookName.HighCard].IncreaseLevel();
                Player.access().handTable[TextbookName.HighCard].IncreaseLevel();
                Player.access().handTable[TextbookName.HighCard].IncreaseLevel();
                break;
            case "Pair":
                Player.access().handTable[TextbookName.Pair].IncreaseLevel();
                Player.access().handTable[TextbookName.Pair].IncreaseLevel();
                Player.access().handTable[TextbookName.Pair].IncreaseLevel();
                break;
            case "Two Pair":
                Player.access().handTable[TextbookName.TwoPair].IncreaseLevel();
                Player.access().handTable[TextbookName.TwoPair].IncreaseLevel();
                Player.access().handTable[TextbookName.TwoPair].IncreaseLevel();
                break;
            case "Three Of A Kind":
                Player.access().handTable[TextbookName.ThreeKind].IncreaseLevel();
                Player.access().handTable[TextbookName.ThreeKind].IncreaseLevel();
                Player.access().handTable[TextbookName.ThreeKind].IncreaseLevel();
                break;
            case "Straight":
                Player.access().handTable[TextbookName.Straight].IncreaseLevel();
                Player.access().handTable[TextbookName.Straight].IncreaseLevel();
                Player.access().handTable[TextbookName.Straight].IncreaseLevel();
                break;
            case "Flush":
                Player.access().handTable[TextbookName.Flush].IncreaseLevel();
                Player.access().handTable[TextbookName.Flush].IncreaseLevel();
                Player.access().handTable[TextbookName.Flush].IncreaseLevel();
                break;
            case "Full House":
                Player.access().handTable[TextbookName.FullHouse].IncreaseLevel();
                Player.access().handTable[TextbookName.FullHouse].IncreaseLevel();
                Player.access().handTable[TextbookName.FullHouse].IncreaseLevel();
                break;
            case "Four Of A Kind":
                Player.access().handTable[TextbookName.FourKind].IncreaseLevel();
                Player.access().handTable[TextbookName.FourKind].IncreaseLevel();
                Player.access().handTable[TextbookName.FourKind].IncreaseLevel();
                break;
            case "Straight Flush":
                Player.access().handTable[TextbookName.StraightFlush].IncreaseLevel();
                Player.access().handTable[TextbookName.StraightFlush].IncreaseLevel();
                Player.access().handTable[TextbookName.StraightFlush].IncreaseLevel();
                break;
            case "Five Of A Kind":
                Player.access().handTable[TextbookName.FiveKind].IncreaseLevel();
                Player.access().handTable[TextbookName.FiveKind].IncreaseLevel();
                Player.access().handTable[TextbookName.FiveKind].IncreaseLevel();
                break;
            case "Flush House":
                Player.access().handTable[TextbookName.FlushHouse].IncreaseLevel();
                Player.access().handTable[TextbookName.FlushHouse].IncreaseLevel();
                Player.access().handTable[TextbookName.FlushHouse].IncreaseLevel();
                break;
            case "Flush Five":
                Player.access().handTable[TextbookName.FlushFive].IncreaseLevel();
                Player.access().handTable[TextbookName.FlushFive].IncreaseLevel();
                Player.access().handTable[TextbookName.FlushFive].IncreaseLevel();
                break;
            default:
                return;
        }
    }
    private int randomizer(int start, int Size)
    {
        return Random.Range(start, Size);
    }
}
