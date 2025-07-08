//Script is used to handle the logic of the end of round screen.
//This includes the interest and remaining hands and increasing the 
//players moneyCount at when they cash out.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfRoundManager : MonoBehaviour
{
    public GameObject remainingHandsNumber;
    public Button cashOutButton;
    public GameObject interestNumber;
    public GameObject interestText;
    public GameObject roundRewardText;
    public GameObject moneyText;
    public GameObject scoreAtLeastAmount;
    Game inst = Game.access();
    Player player = Player.access();
    int roundReward = 0;
    static int totalCashOut = 0;
    int interest = 0;

    public GameObject mentorRewardNumber;
    public GameObject mentorRewardText;
    private int mentorReward = 0; //  For Mentors that reward extra money

    private static EndOfRoundManager instance;  //EndOfRoundManager instance varaiable

    //Singleton for the EndOfRoundManager
    public static EndOfRoundManager access()
    {
        return instance;
    }

    // Enforce singleton instance
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

        instance = this;
    }

    public void EndScreenOpened()
    {
        SetScoreAmount();
        CalculateRoundReward();
        CalculateHands();
        CalculateInterest();
        CalculateMentors();
        SetCashOut();
    }
    private void SetScoreAmount()
    {
        int ante = Game.access().GetAnte();
        int round = Game.access().roundValueTest;
        int scoreNeeded = (int)Round.access().GetTargetScore(ante, round);
        scoreAtLeastAmount.GetComponent<TMP_Text>().text = scoreNeeded.ToString();
    }
    private void CalculateRoundReward()
    {
        int round = inst.roundValueTest;
        int ante = inst.GetAnte();

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
        interest = player.moneyCount / 5;
        if (interest > Player.access().maxInterest)
        {
            interest = Player.access().maxInterest;
        }
        interestText.GetComponent<TMP_Text>().text = $"1 Interest per $5 ({Player.access().maxInterest} max)";
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

    //  Run the buffers for economy Mentors that run in PreShop (after transition to end of round)
    private void CalculateMentors()
    {
        MentorBufferManager.access().RunBufferImmediate(UseLocation.PreShop);

        if (mentorReward > 0)
        {
            mentorRewardText.SetActive(true);
            mentorRewardNumber.SetActive(true);
            mentorRewardNumber.GetComponent<TMP_Text>().text = mentorReward.ToString();
            totalCashOut += mentorReward;
        }
        else
        {
            mentorRewardText.SetActive(false);
            mentorRewardNumber.SetActive(false);
        }
    }

    //  Used for mentors to increase the mentor reward
    public void IncrementMentorReward(int reward)
    {
        mentorReward += reward;
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
        player.handCount = 4;
        player.discards = 4;
        player.chipCount = 0;
        moneyText.GetComponentInChildren<TMP_Text>().text = "$" + player.moneyCount.ToString();

        //Set everything to default/0
        totalCashOut = 0;
        roundReward = 0;
        interest = 0;

        if (Game.access().GetRound() == 3)
        {
            int ante = Game.access().GetAnte();
            Game.access().SetAnte(ante + 1);
            Game.access().SetRound(1);
        }
        else
        {
            int round = Game.access().GetRound();
            Game.access().SetRound(round + 1);
        }
        Debug.Log("Round after CashOut: " + Game.access().GetRound());



        //Transition to shopScreen
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Shop);
        TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionManager.TransitionToShopScreen();
    }
}
