//Script is used for setting up the Collections Menu and set the objects as needed.
//Current Devs: Fredrick Bouloute (bouloutef04)
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using SplitString;
using Unity.VisualScripting;

public class CollectionsManager : MonoBehaviour
{
    //Panel objects
    private static GameObject infoPanel;
    private static GameObject titlePanel;
    private static TextMeshProUGUI titleText;
    private static GameObject descriptionPanel;
    private static TextMeshProUGUI descriptionText;
    //Current objects used to set text info box on hover
    public Mentor currentMentor;
    public Textbook currentTextbook;
    public CardBuff currentCardBuff;
    public Voucher currentVoucher;
    //Make array of for each object containing all items
    private Mentor[] allMentors;
    private Textbook[] allTextbooks;
    private CardBuff[] allCardBuffs;
    private Voucher[] allVoucher;
    //Game Objects for Items
    public GameObject mentorObject;
    public GameObject textbookObject;
    public GameObject cardbuffObject;
    public GameObject voucherObject;
    //Used to track current index of items
    private static int curMentor;
    private static int curTextbook;
    private static int curCardBuff;
    public static int curVoucher;
    public GameObject mentorNumber;
    public GameObject textbookNumber;
    public GameObject cardBuffNumber;
    public GameObject voucherNumber;

    //Set all necessary objects
    void Start()
    {
        infoPanel = GameObject.Find("CardInfoPanel");

        if (infoPanel != null)
        {

            titlePanel = infoPanel.transform.Find("TitlePanel")?.gameObject;
            descriptionPanel = infoPanel.transform.Find("DescriptionPanel")?.gameObject;

            //  Get Title Panel
            Image titleImage = titlePanel.GetComponent<Image>();
            titleImage.color = new Color(255, 255, 255, 1f);

            //  Get Description Panel
            Image descImage = descriptionPanel.GetComponent<Image>();
            descImage.color = new Color(255, 255, 255, 0.8f);

            titleText = titlePanel.GetComponentInChildren<TextMeshProUGUI>();
            descriptionText = descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();

            infoPanel.SetActive(false); // Ensure it's hidden at the start


        }
        else
        {
            Debug.LogError("Could not find 'CardInfoPanel' in the scene. Make sure it is named correctly and exists in your Hierarchy.");
        }


        SetMentors();
        SetTextbooks();
        SetCardBuffs();
        SetVouchers();

        SetCurrentMentor();
        SetCurrentTextbook();
        SetCurrentCardBuff();
        SetCurrentVoucher();

    }
    //Functions for creating the full arrays of mentors, textbooks, etc.
    public void SetMentors()
    {
        allMentors = new Mentor[50];
        for (int i = 1; i <= 50; i++)
        {
            allMentors[i - 1] = Mentor.MentorFactory((MentorName)i, CardEdition.Base);
        }
        currentMentor = allMentors[0];
        curMentor = 0;
    }
    public void SetTextbooks()
    {
        allTextbooks = new Textbook[12];
        for (int i = 1; i <= 12; i++)
        {
            allTextbooks[i - 1] = new Textbook((TextbookName)i);
        }
        currentTextbook = allTextbooks[0];
        curTextbook = 0;
    }
    public void SetCardBuffs()
    {
        allCardBuffs = new CardBuff[22];
        for (int i = 1; i <= 22; i++)
        {
            allCardBuffs[i - 1] = CardBuff.CardBuffFactory((CardBuffName)i);
        }
        currentCardBuff = allCardBuffs[0];
        curCardBuff = 0;
    }
    public void SetVouchers()
    {
        allVoucher = new Voucher[12];
        for (int i = 1; i <= 12; i++)
        {
            allVoucher[i - 1] = new Voucher((VoucherNames)i);
        }
        currentVoucher = allVoucher[0];
        curVoucher = 0;
    }
    //Create functions for setting each object and their images
    private void SetCurrentMentor()
    {
        mentorObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Mentor/" + currentMentor.name.ToString());
    }
    private void SetCurrentTextbook()
    {
        textbookObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Textbook/textbook_" + currentTextbook.name.ToString());
    }
    private void SetCurrentCardBuff()
    {
        cardbuffObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardBuff/food_" + currentCardBuff.name.ToString());
    }
    private void SetCurrentVoucher()
    {
        voucherObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Vouchers/" + currentVoucher.name.ToString());
    }
    //Create functions for next and back buttons
    public void NextMentor()
    {
        if (curMentor == 49)
        {
            curMentor = 0;
        }
        else
        {
            curMentor++;
        }
        mentorNumber.GetComponent<TMP_Text>().text = (curMentor + 1).ToString() + "/50";
        currentMentor = allMentors[curMentor];
        SetCurrentMentor();
    }
    public void BackMentor()
    {
        if (curMentor == 0)
        {
            curMentor = 49;
        }
        else
        {
            curMentor--;
        }
        mentorNumber.GetComponent<TMP_Text>().text = (curMentor + 1).ToString() + "/50";
        currentMentor = allMentors[curMentor];
        SetCurrentMentor();
    }
    public void Nexttextbook()
    {
        if (curTextbook == 11)
        {
            curTextbook = 0;
        }
        else
        {
            curTextbook++;
        }
        textbookNumber.GetComponent<TMP_Text>().text = (curTextbook + 1).ToString() + "/12";
        currentTextbook = allTextbooks[curTextbook];
        SetCurrentTextbook();
    }
    public void BackTextbook()
    {
        if (curTextbook == 0)
        {
            curTextbook = 11;
        }
        else
        {
            curTextbook--;
        }
        textbookNumber.GetComponent<TMP_Text>().text = (curTextbook + 1).ToString() + "/12";
        currentTextbook = allTextbooks[curTextbook];
        SetCurrentTextbook();
    }
    public void NextCardBuff()
    {
        if (curCardBuff == 21)
        {
            curCardBuff = 0;
        }
        else
        {
            curCardBuff++;
        }
        cardBuffNumber.GetComponent<TMP_Text>().text = (curCardBuff + 1).ToString() + "/22";
        currentCardBuff = allCardBuffs[curCardBuff];
        SetCurrentCardBuff();
    }
    public void BackCardBuff()
    {
        if (curCardBuff == 0)
        {
            curCardBuff = 21;
        }
        else
        {
            curCardBuff--;
        }
        cardBuffNumber.GetComponent<TMP_Text>().text = (curCardBuff + 1).ToString() + "/22";
        currentCardBuff = allCardBuffs[curCardBuff];
        SetCurrentCardBuff();
    }
    public void NextVoucher()
    {
        if (curVoucher == 11)
        {
            curVoucher = 0;
        }
        else
        {
            curVoucher++;
        }
        voucherNumber.GetComponent<TMP_Text>().text = (curVoucher + 1).ToString() + "/12";
        currentVoucher = allVoucher[curVoucher];
        SetCurrentVoucher();
    }
    public void BackVoucher()
    {
        if (curVoucher == 0)
        {
            curVoucher = 11;
        }
        else
        {
            curVoucher--;
        }
        voucherNumber.GetComponent<TMP_Text>().text = (curVoucher + 1).ToString() + "/12";
        currentVoucher = allVoucher[curVoucher];
        SetCurrentVoucher();
    }
    //Functions for setting up the info on the info panel
    public void ShowMentorDetails()
    {
        // Show the panel
        infoPanel.SetActive(true);
        titleText.text = SplitCase.Split(currentMentor.name.ToString());
        string cardDescription = currentMentor.GetDescription();
        cardDescription += CardModifier.access().EditionDesc(currentMentor.edition);
        descriptionText.text = cardDescription;

    }
    public void ShowTextbookDetails()
    {
        // Show the panel
        infoPanel.SetActive(true);
        titleText.text = SplitCase.Split(currentTextbook.name.ToString()) + " Textbook";
        descriptionText.text = currentTextbook.GetDescription();

    }
    public void ShowCardBuffDetails()
    {
        // Show the panel
        infoPanel.SetActive(true);
        titleText.text = SplitCase.Split(currentCardBuff.name.ToString());
        descriptionText.text = currentCardBuff.GetDescription();

        Debug.Log(currentCardBuff.GetDescription());

    }
    public void ShowVoucherDetails()
    {
        // Show the panel
        infoPanel.SetActive(true);
        titleText.text = SplitCase.Split(currentVoucher.name.ToString());
        descriptionText.text = currentVoucher.GetDescription();

    }
    public void PointerExit()
    {
        if (infoPanel != null)
        {
            // Hide the panel
            infoPanel.SetActive(false);
        }
    }
}
