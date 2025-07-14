using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;

using TMPro;
using System.Linq;


public class BlindSceneManager : MonoBehaviour
{
    public CanvasGroup Tag1Details;
    public CanvasGroup Tag2Details;
    public CanvasGroup SmallBlindCover;
    public CanvasGroup BigBlindCover;
    public CanvasGroup SpecialBlindCover;
    public GameObject BlindInfoBox;
    public GameObject SpecialBlindChips;
    public GameObject BigBlindChips;
    public GameObject SmallBlindChips;
    public GameObject SpecialBlindName;
    public GameObject SpecialBlindDesc;
    public GameObject SpecialBlindToken;
    public Button Skip1Button;
    public Button Skip2Button;
    public Button SmallBlindButton;
    public Button BigBlindButton;
    public Button SpecialBlindButton;
    public GameObject HandNumber;
    public GameObject DiscardNumber;
    public GameObject MoneyNumber;
    public GameObject AnteNumber;
    public GameObject RoundNumber;

    private Tag tag1;
    private Tag tag2;
    private SpecialBlind special;

    private Game gameInst = Game.access();
    private Player playerInst = Player.access();
    private FadeScript fadeInst = FadeScript.access();
    private Round roundInst = Round.access();
    


    // This function is called whenever the blind scene is brought up and initializes the variables for the scene
    public void NewBlind()
    {
        Tag[] newTags = gameInst.randomTag(2);
        tag1 = newTags[0];
        tag2 = newTags[1];

        //ginst.SetChipTotal(600);

        if (gameInst.currentSpecialBlind == null)
        {
            gameInst.currentSpecialBlind = gameInst.randomSpecialBlind();
        }

        special = gameInst.currentSpecialBlind;
        
        SetBlindScreenInfo();
       
        // Insert code for changing the tag sprites to ones that match the generated tags & code for special blind
    }

    public void SetBlindScreenInfo()
    {
        SpecialBlindToken.GetComponent<Image>().sprite = Resources.Load<Sprite>($"BlindTokens/" + special.blindType.ToString());

        SpecialBlindName.GetComponentInChildren<TMP_Text>().text = special.nameText;

        SpecialBlindDesc.GetComponentInChildren<TMP_Text>().text = special.description;

        SmallBlindChips.GetComponentInChildren<TMP_Text>().text = "" + roundInst.baseAnteChips[gameInst.GetAnte()];

        BigBlindChips.GetComponentInChildren<TMP_Text>().text = "" + roundInst.baseAnteChips[gameInst.GetAnte()] * 1.5;

        SpecialBlindChips.GetComponentInChildren<TMP_Text>().text = "" + roundInst.GetTargetScore(gameInst.GetAnte(), 3);

        SpecialBlindChips.GetComponentInChildren<TMP_Text>().text = "" + roundInst.baseAnteChips[gameInst.GetAnte()] * special.chipMultiplier;

        HandNumber.GetComponentInChildren<TMP_Text>().text = "" + playerInst.maxHandCount;

        DiscardNumber.GetComponentInChildren<TMP_Text>().text = "" + playerInst.maxDiscards;

        MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + playerInst.moneyCount;

        AnteNumber.GetComponentInChildren<TMP_Text>().text = "" + gameInst.GetAnte();

        RoundNumber.GetComponentInChildren<TMP_Text>().text = "" + gameInst.GetRound();

        setBlindCover();
    }

    public void setBlindCover()
    {
        SmallBlindCover.blocksRaycasts = true;
        SmallBlindCover.interactable = true;
        BigBlindCover.blocksRaycasts = true;
        BigBlindCover.interactable = true;
        SpecialBlindCover.blocksRaycasts = true;
        SpecialBlindCover.interactable = true;
        Debug.Log("Round is currently: " + gameInst.GetRound());
        
        SmallBlindCover.alpha = 1;
        BigBlindCover.alpha = 1;
        SpecialBlindCover.alpha = 1;

        if (gameInst.GetRound() == 1)
        {
            SmallBlindCover.blocksRaycasts = false;
            SmallBlindCover.interactable = false;
            StartCoroutine(fadeInst.FadeOut(SmallBlindCover));

        }
        else if (gameInst.GetRound() == 2)
        {
            BigBlindCover.blocksRaycasts = false;
            BigBlindCover.interactable = false;
            StartCoroutine(fadeInst.FadeOut(BigBlindCover));
        }
        else
        {
            SpecialBlindCover.blocksRaycasts = false;
            SpecialBlindCover.interactable = false;
            StartCoroutine(fadeInst.FadeOut(SpecialBlindCover));
        }
    }


    public void useSmallBlindButton()
    {
        gameInst.SetRound(1);
        setBlindCover();
        TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionManager.TransitionToRoundScreen();

    }

    public void useBigBlindButton()
    {
        gameInst.SetRound(2);
        setBlindCover();
        TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionManager.TransitionToRoundScreen();
    }

    public void useSpecialBlindButton()
    {
        gameInst.SetRound(3);
        setBlindCover();
        special.applySpecialBlinds();
        //Either this script or round script will then apply special blind
        TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
        transitionManager.TransitionToRoundScreen();

        //  If player has Pop Quiz voucher, generate random Card Buff when Special Blind selected
        if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.PopQuiz))
        {
            ConsumableCardHolder consumableHolder = FindFirstObjectByType<ConsumableCardHolder>();
            if (consumableHolder != null && (playerInst.consumables.Count < playerInst.maxConsumables))
            {
                consumableHolder.AddConsumable(gameInst.randomCardBuffShop(1)[0]);
            }
        }
    }

    public void UseSkipButton1()
    {
        gameInst.SetRound(2);

        applyTag(tag1);
        ++gameInst.skipCount;

        setBlindCover();

        StartCoroutine(fadeInst.FadeIn(SmallBlindCover));

        //  Give additional $3 if player has Speed Reading voucher
        if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.SpeedReading))
        {
            playerInst.moneyCount += 3;
            MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + playerInst.moneyCount;
        }
    }

    public void UseSkipButton2()
    {
        gameInst.SetRound(3);

        applyTag(tag2);
        ++gameInst.skipCount;

        setBlindCover();

        StartCoroutine(fadeInst.FadeIn(BigBlindCover));

        //  Give additional $3 if player has Speed Reading voucher
        if (playerInst.vouchers.Any(voucher => voucher.name == VoucherNames.SpeedReading))
        {
            playerInst.moneyCount += 3;
            MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + playerInst.moneyCount;
        }
    }

    private void applyTag(Tag tag)
    {
        tag.applyTag();

        MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + playerInst.moneyCount;
    }

    public void showSkip1Details()
    {
        Tag1Details.GetComponentInChildren<TMP_Text>().text = tag1.tagName + "\n" + tag1.description;
        Tag1Details.blocksRaycasts = true;
        StartCoroutine(fadeInst.FadeIn(Tag1Details));
        Tag1Details.interactable = true;
    }

    public void showSkip2Details()
    {
        Tag2Details.GetComponentInChildren<TMP_Text>().text = tag2.tagName + "\n" + tag2.description;
        Tag2Details.blocksRaycasts = true;
        StartCoroutine(fadeInst.FadeIn(Tag2Details));
        Tag2Details.interactable = true;

    }

    public void hideSkip1Details()
    {
        Tag1Details.blocksRaycasts = false;
        Tag1Details.interactable = false;
        StartCoroutine(fadeInst.FadeOut(Tag1Details));

    }

    public void hideSkip2Details()
    {
        Tag2Details.blocksRaycasts = false;
        Tag2Details.interactable = false;
        StartCoroutine(fadeInst.FadeOut(Tag2Details));

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewBlind();

        //  Hide the card info initially
        Image panelImage = GameObject.Find("CardInfoPanel").GetComponent<Image>();
        panelImage.color = new Color(255, 255, 255, 0);
    }
}
