//Script is used to close out the RunInfo Manager.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using UnityEngine;
using TMPro;

public class RunInfoManager : MonoBehaviour
{
    public CanvasGroup runInfo;
    private GameObject highCardBlueScoreText;
    private GameObject highCardRedScoreText;
    private GameObject highCardLevelText;
    private GameObject highCardTimesPlayedText;

    private GameObject pairBlueScoreText;
    private GameObject pairRedScoreText;
    private GameObject pairLevelText;
    private GameObject pairTimesPlayedText;

    private GameObject twoPairBlueScoreText;
    private GameObject twoPairRedScoreText;
    private GameObject twoPairLevelText;
    private GameObject twoPairTimesPlayedText;

    private GameObject threeKindBlueScoreText;
    private GameObject threeKindRedScoreText;
    private GameObject threeKindLevelText;
    private GameObject threeKindTimesPlayedText;

    private GameObject straightBlueScoreText;
    private GameObject straightRedScoreText;
    private GameObject straightLevelText;
    private GameObject straightTimesPlayedText;

    private GameObject flushBlueScoreText;
    private GameObject flushRedScoreText;
    private GameObject flushLevelText;
    private GameObject flushTimesPlayedText;

    private GameObject fullHouseBlueScoreText;
    private GameObject fullHouseRedScoreText;
    private GameObject fullHouseLevelText;
    private GameObject fullHouseTimesPlayedText;

    private GameObject fourKindBlueScoreText;
    private GameObject fourKindRedScoreText;
    private GameObject fourKindLevelText;
    private GameObject fourKindTimesPlayedText;

    private GameObject straightFlushBlueScoreText;
    private GameObject straightFlushRedScoreText;
    private GameObject straightFlushLevelText;
    private GameObject straightFlushTimesPlayedText;

    private GameObject fiveKindBlueScoreText;
    private GameObject fiveKindRedScoreText;
    private GameObject fiveKindLevelText;
    private GameObject fiveKindTimesPlayedText;

    private GameObject flushHouseBlueScoreText;
    private GameObject flushHouseRedScoreText;
    private GameObject flushHouseLevelText;
    private GameObject flushHouseTimesPlayedText;

    private GameObject flushFiveBlueScoreText;
    private GameObject flushFiveRedScoreText;
    private GameObject flushFiveLevelText;
    private GameObject flushFiveTimesPlayedText;

    //Function called to go back to regular gameplay. Make run info menu
    //disappear
    public void BackButton()
    {
        runInfo.alpha = 0;
        runInfo.blocksRaycasts = false;
    }

    public void OpenRunInfo()
    {
        SetRunInfoText();
    }

    public void Start()
    {
        highCardLevelText = GameObject.Find("High Card Level Text");
        highCardBlueScoreText = GameObject.Find("High Card Base Score Text");
        highCardRedScoreText = GameObject.Find("High Card Mult Score Text");
        highCardTimesPlayedText = GameObject.Find("High Card Played Number Text");

        pairLevelText = GameObject.Find("Pair Level Text");
        pairBlueScoreText = GameObject.Find("Pair Base Score Text");
        pairRedScoreText = GameObject.Find("Pair Mult Score Text");
        pairTimesPlayedText = GameObject.Find("Pair Played Number Text");

        twoPairLevelText = GameObject.Find("Two Pair Level Text");
        twoPairBlueScoreText = GameObject.Find("Two Pair Base Score Text");
        twoPairRedScoreText = GameObject.Find("Two Pair Mult Score Text");
        twoPairTimesPlayedText = GameObject.Find("Two Pair Played Number Text");

        threeKindLevelText = GameObject.Find("Three Of A Kind Level Text");
        threeKindBlueScoreText = GameObject.Find("Three Of A Kind Base Score Text");
        threeKindRedScoreText = GameObject.Find("Three Of A Kind Mult Score Text");
        threeKindTimesPlayedText = GameObject.Find("Three Of A Kind Played Number Text");

        straightLevelText = GameObject.Find("Straight Level Text");
        straightTimesPlayedText = GameObject.Find("Straight Played Number Text");
        straightBlueScoreText = GameObject.Find("Straight Base Score Text");
        straightRedScoreText = GameObject.Find("Straight Mult Score Text");

        flushLevelText = GameObject.Find("Flush Level Text");
        flushBlueScoreText = GameObject.Find("Flush Base Score Text");
        flushRedScoreText = GameObject.Find("Flush Mult Score Text");
        flushTimesPlayedText = GameObject.Find("Flush Played Number Text");

        fullHouseLevelText = GameObject.Find("Full House Level Text");
        fullHouseBlueScoreText = GameObject.Find("Full House Base Score Text");
        fullHouseRedScoreText = GameObject.Find("Full House Mult Score Text");
        fullHouseTimesPlayedText = GameObject.Find("Full House Played Number Text");

        fourKindLevelText = GameObject.Find("Four Of A Kind Level Text"); ;
        fourKindBlueScoreText = GameObject.Find("Four Of A Kind Base Score Text");
        fourKindRedScoreText = GameObject.Find("Four Of A Kind Mult Score Text");
        fourKindTimesPlayedText = GameObject.Find("Four Of A Kind Played Number Text");

        straightFlushLevelText = GameObject.Find("Straight Flush Level Text");
        straightFlushBlueScoreText = GameObject.Find("Straight Flush Base Score Text");
        straightFlushRedScoreText = GameObject.Find("Straight Flush Mult Score Text");
        straightFlushTimesPlayedText = GameObject.Find("Straight Flush Played Number Text");

        fiveKindLevelText = GameObject.Find("Five Of A Kind Level Text");
        fiveKindBlueScoreText = GameObject.Find("Five Of A Kind Base Score Text");
        fiveKindRedScoreText = GameObject.Find("Five Of A Kind Mult Score Text");
        fiveKindTimesPlayedText = GameObject.Find("Five Of A Kind Played Number Text");

        flushHouseLevelText = GameObject.Find("Flush House Level Text");
        flushHouseBlueScoreText = GameObject.Find("Flush House Base Score Text");
        flushHouseRedScoreText = GameObject.Find("Flush House Mult Score Text");
        flushHouseTimesPlayedText = GameObject.Find("Flush House Played Number Text");

        flushFiveLevelText = GameObject.Find("Flush Five Level Text");
        flushFiveBlueScoreText = GameObject.Find("Flush Five Base Score Text");
        flushFiveRedScoreText = GameObject.Find("Flush Five Mult Score Text");
        flushFiveTimesPlayedText = GameObject.Find("Flush Five Played Number Text");

        //Should reset the text since all values will be 0
        SetRunInfoText();
    }

    private void SetRunInfoText()
    {
        highCardLevelText.GetComponent<TMP_Text>().text = "lvl. " + Player.access().handTable[TextbookName.HighCard].level.ToString();
        highCardBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.HighCard].GetCurrChips().ToString();
        highCardRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.HighCard].GetCurrMult().ToString();
        highCardTimesPlayedText.GetComponent<TMP_Text>().text = "#" + Player.access().handTable[TextbookName.HighCard].GetTimesPlayed().ToString();

        pairLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Pair].level.ToString();
        pairBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Pair].GetCurrChips().ToString();
        pairRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Pair].GetCurrMult().ToString();
        pairTimesPlayedText.GetComponent<TMP_Text>().text = "#" + Player.access().handTable[TextbookName.Pair].GetTimesPlayed().ToString();

        twoPairLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.TwoPair].level.ToString();
        twoPairBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.TwoPair].GetCurrChips().ToString();
        twoPairRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.TwoPair].GetCurrMult().ToString();
        twoPairTimesPlayedText.GetComponent<TMP_Text>().text = "#" + Player.access().handTable[TextbookName.TwoPair].GetTimesPlayed().ToString();

        threeKindLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.ThreeKind].level.ToString();
        threeKindBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.ThreeKind].GetCurrChips().ToString();
        threeKindRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.ThreeKind].GetCurrMult().ToString();
        threeKindTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.ThreeKind].GetTimesPlayed().ToString();

        straightLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Straight].level.ToString();
        straightBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Straight].GetCurrChips().ToString();
        straightRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Straight].GetCurrMult().ToString();
        straightTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.Straight].GetTimesPlayed().ToString();

        flushLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Flush].level.ToString();
        flushBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Flush].GetCurrChips().ToString();
        flushRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.Flush].GetCurrMult().ToString();
        flushTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.Flush].GetTimesPlayed().ToString();

        fullHouseLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FullHouse].level.ToString();
        fullHouseBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FullHouse].GetCurrChips().ToString();
        fullHouseRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FullHouse].GetCurrMult().ToString();
        fullHouseTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.FullHouse].GetTimesPlayed().ToString();

        fourKindLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FourKind].level.ToString();
        fourKindBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FourKind].GetCurrChips().ToString();
        fourKindRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FourKind].GetCurrMult().ToString();
        fourKindTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.FourKind].GetTimesPlayed().ToString();

        straightFlushLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.StraightFlush].level.ToString();
        straightFlushBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.StraightFlush].GetCurrChips().ToString();
        straightFlushRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.StraightFlush].GetCurrMult().ToString();
        straightFlushTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.StraightFlush].GetTimesPlayed().ToString();

        fiveKindLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FiveKind].level.ToString();
        fiveKindBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FiveKind].GetCurrChips().ToString();
        fiveKindRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FiveKind].GetCurrMult().ToString();
        fiveKindTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.FiveKind].GetTimesPlayed().ToString();

        flushHouseLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushHouse].level.ToString();
        flushHouseBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushHouse].GetCurrChips().ToString();
        flushHouseRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushHouse].GetCurrMult().ToString();
        flushHouseTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.FlushHouse].GetTimesPlayed().ToString();

        flushFiveLevelText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushFive].level.ToString();
        flushFiveBlueScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushFive].GetCurrChips().ToString();
        flushFiveRedScoreText.GetComponent<TMP_Text>().text = Player.access().handTable[TextbookName.FlushFive].GetCurrMult().ToString();
        flushFiveTimesPlayedText.GetComponent<TMP_Text>().text = "#" +  Player.access().handTable[TextbookName.FlushFive].GetTimesPlayed().ToString();
    }

}
