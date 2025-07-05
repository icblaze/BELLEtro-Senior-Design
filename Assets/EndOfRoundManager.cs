using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Script is used to handle the logic of the end of round screen.
//This includes the interest and remaining hands and increasing the 
//players moneyCount at when they cash out.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class EndOfRoundManager : MonoBehaviour
{
    public GameObject remainingHandsNumber;
    public Button cashOutButton;
    public GameObject interestNumber;
    public GameObject roundRewardText;
    public GameObject moneyText;
    public GameObject scoreAtLeastAmount;
    Game inst = Game.access();
    Player player = Player.access();
    int roundReward = 0;
    static int totalCashOut = 0;
    int interest = 0;

    private static EndOfRoundManager instance;  //EndOfRoundManager instance varaiable

    //Singleton for the EndOfRoundManager
    public static EndOfRoundManager access()
    {
        if (instance == null)
        {
            instance = new EndOfRoundManager();
        }

        return instance;
    }

    public void EndScreenOpened()
    {
        SetScoreAmount();
        CalculateRoundReward();
        CalculateHands();
        CalculateInterest();
        SetCashOut();
    }
    private void SetScoreAmount()
    {
        int ante = Game.access().anteValue;
        int round = Game.access().roundValueTest;
        int scoreNeeded = (int)Round.access().GetTargetScore(ante, round);
        scoreAtLeastAmount.GetComponent<TMP_Text>().text = scoreNeeded.ToString();
    }
    private void CalculateRoundReward()
    {
        int round = inst.roundValueTest;
        int ante = inst.anteValue;

        switch (round)
        {
            case 1:
                roundReward = 3;
                break;
            case 2:
                roundReward = 4;
                break;
            case 3:
                if (ante == 8)
                {
                    roundReward = 8;
                    break;
                }
                roundReward = 5;
                break;
            default:
                roundReward = 3;
                break;
        }

        roundRewardText.GetComponent<TMP_Text>().text = "Reward: $" + roundReward.ToString();
        totalCashOut += roundReward;
    }

    private void CalculateInterest()
    {
        interest = player.moneyCount/5;
        if (interest > 5)
        {
            interest = 5;
        }
        interestNumber.GetComponent<TMP_Text>().text = interest.ToString();
        totalCashOut += interest;
    }

    private void CalculateHands()
    {
        int hands = player.handCount;
        Debug.Log("Hands: " + hands);
        remainingHandsNumber.GetComponent<TMP_Text>().text = hands.ToString();
        totalCashOut += hands;
    }
    private void SetCashOut()
    {
        cashOutButton.GetComponentInChildren<TMP_Text>().text = "Cash Out: $" + totalCashOut.ToString();
    }

    public void CashOut()
    {
        Debug.Log("Total CashOut: $" + totalCashOut);
        //Add cash out to player's cash and change text
        player.moneyCount += totalCashOut;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + player.moneyCount.ToString();

        //Set everything to default/0
        totalCashOut = 0;
        roundReward = 0;
        interest = 0;

        //Transition to shopScreen
        TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionManager.TransitionToShopScreen();
    }
}
