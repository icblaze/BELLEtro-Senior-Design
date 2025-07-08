using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;

using TMPro;


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


    private Game ginst = Game.access();
    private Player pinst = Player.access();
    private FadeScript ainst = FadeScript.access();

    // This function is called whenever the blind scene is brought up and initializes the variables for the scene
    public void NewBlind()
    {
        Tag[] newTags = ginst.randomTag(2);
        tag1 = newTags[0];
        tag2 = newTags[1];

        //ginst.SetChipTotal(600);

        if (ginst.currentSpecialBlind == null)
        {
            ginst.currentSpecialBlind = ginst.randomSpecialBlind();
        }

        ginst.roundValue = 1;

        special = ginst.currentSpecialBlind;

        SpecialBlindToken.GetComponent<Image>().sprite = Resources.Load<Sprite>($"BlindTokens/" + special.blindType.ToString());

        SpecialBlindName.GetComponentInChildren<TMP_Text>().text = special.nameText;

        SpecialBlindDesc.GetComponentInChildren<TMP_Text>().text = special.description;

        SmallBlindChips.GetComponentInChildren<TMP_Text>().text = "" + ginst.GetChipTotal();

        BigBlindChips.GetComponentInChildren<TMP_Text>().text = "" + ginst.GetChipTotal() * 1.5;

        SpecialBlindChips.GetComponentInChildren<TMP_Text>().text = "" + ginst.GetChipTotal() * special.chipMultiplier;
        
        HandNumber.GetComponentInChildren<TMP_Text>().text = "" + pinst.handCount;

        DiscardNumber.GetComponentInChildren<TMP_Text>().text = "" + pinst.discards;

        MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + pinst.moneyCount;

        AnteNumber.GetComponentInChildren<TMP_Text>().text = "" + ginst.ante;

        RoundNumber.GetComponentInChildren<TMP_Text>().text = "" + ginst.roundValue;

        setBlindCover();

        // Insert code for changing the tag sprites to ones that match the generated tags & code for special blind
    }

    private void setBlindCover()
    {
        SmallBlindCover.blocksRaycasts = true;
        SmallBlindCover.interactable = true;
        BigBlindCover.blocksRaycasts = true;
        BigBlindCover.interactable = true;
        SpecialBlindCover.blocksRaycasts = true;
        SpecialBlindCover.interactable = true;

        if (ginst.roundValue == 1)
        {
            SmallBlindCover.blocksRaycasts = false;
            SmallBlindCover.interactable = false;
            StartCoroutine(ainst.FadeOut(SmallBlindCover));
  
        }
        else if (ginst.roundValue == 2)
        {
            BigBlindCover.blocksRaycasts = false;
            BigBlindCover.interactable = false;
            StartCoroutine(ainst.FadeOut(BigBlindCover));
        }
        else
        {
            SpecialBlindCover.blocksRaycasts = false;
            SpecialBlindCover.interactable = false;
            StartCoroutine(ainst.FadeOut(SpecialBlindCover));
        }
    }


    public void useSmallBlindButton()
    {
        
    }

    public void useBigBlindButton()
    {

    }

    public void useSpecialBlindButton()
    {

    }

    public void UseSkipButton1()
    {
        ginst.roundValue = 2;

        applyTag(tag1);

        setBlindCover();

        StartCoroutine(ainst.FadeIn(SmallBlindCover));
    }

    public void UseSkipButton2()
    {
        ginst.roundValue = 3;

        applyTag(tag2);

        setBlindCover();

        StartCoroutine(ainst.FadeIn(BigBlindCover));
    }

    private void applyTag(Tag tag)
    {
        tag.applyTag();

        MoneyNumber.GetComponentInChildren<TMP_Text>().text = "$ " + pinst.moneyCount;
    }

    public void showSkip1Details()
    {
        Tag1Details.GetComponentInChildren<TMP_Text>().text = tag1.tagName + "\n" + tag1.description;
        Tag1Details.blocksRaycasts = true;
        StartCoroutine(ainst.FadeIn(Tag1Details));
        Tag1Details.interactable = true;
    }

    public void showSkip2Details()
    {
        Tag2Details.GetComponentInChildren<TMP_Text>().text = tag2.tagName + "\n" + tag2.description;
        Tag2Details.blocksRaycasts = true;
        StartCoroutine(ainst.FadeIn(Tag2Details));
        Tag2Details.interactable = true;

    }

    public void hideSkip1Details()
    {
        Tag1Details.blocksRaycasts = false;
        Tag1Details.interactable = false;
        StartCoroutine(ainst.FadeOut(Tag1Details));

    }

    public void hideSkip2Details()
    {
        Tag2Details.blocksRaycasts = false;
        Tag2Details.interactable = false;
        StartCoroutine(ainst.FadeOut(Tag2Details));

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewBlind();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
