
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using SplitString;

public class Card : MonoBehaviour, IDragHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Hover Information")]
    [Tooltip("This is the specific information for this card. Set this from DeckManager.")]
    [TextArea(3, 5)]
    public string cardDescription = "Default card information.";

    private static GameObject infoPanel;
    private static GameObject titlePanel;
    private static TextMeshProUGUI titleText;
    private static GameObject descriptionPanel;
    private static TextMeshProUGUI descriptionText;
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private VisualCardsHandler visualHandler;
    private Vector3 offset;

    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;

    [Header("Selection")]
    public bool selected;
    public bool isPlayed = false;
    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;
    public static void ResetStaticPanel()
    {
        infoPanel = null;
        titlePanel = null;
        titleText = null;
        descriptionPanel = null;
        descriptionText = null;
    }

    [Header("Visual")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public PCardVisual cardVisual;

    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    [HideInInspector] public bool wasDragged;

    [Header("Events")]
    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<Card> PointerDownEvent;
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card, bool> SelectEvent;
    [HideInInspector] public UnityEvent<Card> PointerClickEvent;

    [Header("Card Type Name")]
    public CardType cardType;

    //The following fields sets the Cards info
    public PCard pcard;
    [Header("PCard Object")]
    [SerializeField] private int cardID;
    [SerializeField] private string termName = "";
    [SerializeField] private string termSymbol = "";
    [SerializeField] private string suitName = "";
    [SerializeField] private string enhancementName = "";
    [SerializeField] private string sealName = "";
    [SerializeField] private string editionName = "";
    [SerializeField] private bool disabled = false;

    public Consumable consumable;
    [Header("Consumable Object")]
    public ConsumableType consumableType;
    [SerializeField] public string consumableName = "";
    [SerializeField] private int consumableSellValue = 0;
    [SerializeField] private bool consumableDisabled = false;

    public Mentor mentor;
    [Header("Mentor Object")]
    [SerializeField] private string mentorName = "";
    [SerializeField] private string mentorEditionName = "";
    [SerializeField] private int mentorSellValue = 0;
    [SerializeField] private bool mentorDisabled = false;

    private DeleteCard deleteCardScript;

    void Start()
    {
        deleteCardScript = FindFirstObjectByType<DeleteCard>();

        //  Should only be called once
        if (infoPanel == null)
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
        }
    }

    void Update()
    {
        ClampPosition();

        //This handles changing the transform values when the card is being dragged
        if (isDragging)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    void ClampPosition()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        wasDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }
    }

    //This shows the card details in the info panel when the card is being hovered or clicked by the mouse
    //This also plays the voice sfx for the card
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
        if (infoPanel != null && descriptionText != null)
        {
            //  Update specific description for card type
            if (cardType == CardType.Card)
            {
                //  Hide based on PlayerPrefs HIDE_TERM 
                if (PlayerPrefs.GetInt("HIDE_TERM", 0) == 1)
                { 
                    string symbol;
                    LinguisticTermSymbol.unicodeMap.TryGetValue(pcard.term, out symbol);
                    titleText.text = $"/{symbol}/";
                }
                else
                {
                    titleText.text = SplitCase.Split(pcard.term.ToString());
                }
                cardDescription = pcard.ToString();
            }
            else if (cardType == CardType.Mentor)
            {
                titleText.text = SplitCase.Split(mentor.name.ToString());
                cardDescription = mentor.GetDescription();
                cardDescription += CardModifier.access().EditionDesc(mentor.edition);
            }
            else
            {
                if (consumableType == ConsumableType.CardBuff)
                {
                    CardBuff cardBuff = (CardBuff)consumable;
                    titleText.text = SplitCase.Split(cardBuff.name.ToString());
                    cardDescription = cardBuff.GetDescription();
                }
                else
                {
                    Textbook tbook = (Textbook)consumable;
                    titleText.text = SplitCase.Split(tbook.name.ToString()) + " Textbook";
                    cardDescription = tbook.GetDescription();
                }
            }

            // Update the text with this card's specific description
            descriptionText.text = cardDescription;

            // Show the panel
            infoPanel.SetActive(true);
        }
    }

    //This hides the text that describes the card when the card is not being hovered anymore
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
        if (infoPanel != null)
        {
            // Hide the panel
            infoPanel.SetActive(false);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;
    }

    //This moves the card up when selected
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        pointerUpTime = Time.time;

        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

        if (pointerUpTime - pointerDownTime > .2f)
            return;

        if (wasDragged)
            return;

        selected = !selected;
        SelectEvent.Invoke(this, selected);

        //  Call select cards here instead of separate CardSelection script
        if (selected)
        {
            deleteCardScript.AddSelectedCard(gameObject);
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
            SFXManager sfxManager = GameObject.FindFirstObjectByType<SFXManager>().GetComponent<SFXManager>();
            sfxManager.CardSelected();

            //  Check if Playing Card
            if (cardType == CardType.Card)
            {
                sfxManager.VoiceSFX(Resources.Load<AudioClip>("Voice/" + pcard.term.ToString()));
            }
        }
        else
        {
            deleteCardScript.RemoveSelectedCard(gameObject);
            transform.localPosition = Vector3.zero;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card was clicked! Name: " + gameObject.name); // <-- ADD THIS LINE
        PointerClickEvent.Invoke(this);
    }

    //This moves the card back to its original position
    public void Deselect()
    {
        //if (selected)
        //{
        //    selected = false;
        //    if (selected)
        //        transform.localPosition += (cardVisual.transform.up * 50);
        //    else
        //        transform.localPosition = Vector3.zero;
        //}
    }


    public int SiblingAmount()
    {
        Transform slot = transform.parent;
        if (slot == null || !slot.CompareTag("Slot"))
        {
            Debug.LogWarning("SiblingAmount: Invalid Slot structure.");
            return 1;
        }

        Transform container = slot.parent;
        if (container == null)
        {
            Debug.LogWarning("SiblingAmount: Slot has no container parent.");
            return 1;
        }

        int siblingSlots = 0;
        foreach (Transform child in container)
        {
            if (child.CompareTag("Slot"))
                siblingSlots++;
        }

        return Mathf.Max(siblingSlots, 1); // Always return at least 1
    }




    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        // First, check if parent exists
        if (transform.parent == null)
        {
            Debug.LogWarning("Card has no parent transform!");
            return 0;
        }

        // Check if parent has "Slot" tag
        if (!transform.parent.CompareTag("Slot"))
        {
            Debug.LogWarning("Card's parent does not have 'Slot' tag!");
            return 0;
        }

        // Check if parent's parent exists
        if (transform.parent.parent == null)
        {
            Debug.LogWarning("Card's parent has no parent transform!");
            return 0;
        }

        // Get the total child count of the parent's parent
        int totalChildren = transform.parent.parent.childCount;

        // Prevent division by zero
        if (totalChildren <= 1)
        {
            return 0;
        }

        // Use the safe version of Remap
        return ExtensionMethods.Remap(
            (float)transform.parent.GetSiblingIndex(),
            0,
            (float)(totalChildren - 1),
            0,
            1
        );
    }

    private void OnDestroy()
    {
        if (cardVisual != null)
            Destroy(cardVisual.gameObject);
    }

    //  Instantiate card visual and sets up card info panel
    private void InstantiateCardVisual()
    {
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        if (!instantiateVisual)
            return;

        //  Instantiate at correct corresponding visual handler depending on type
        if (cardType == CardType.Card)
        {
            visualHandler = GameObject.FindWithTag("HandVisual").GetComponent<VisualCardsHandler>();
        }
        else if (cardType == CardType.Consumable)
        {
            visualHandler = GameObject.FindWithTag("ConsumableVisual").GetComponent<VisualCardsHandler>();
        }
        else if (cardType == CardType.Mentor)
        {
            visualHandler = GameObject.FindWithTag("MentorVisual").GetComponent<VisualCardsHandler>();
        }

        cardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<PCardVisual>();
        cardVisual.Initialize(this);
    }

    //  Hide the description box
    public void HideDescription()
    {
        infoPanel.SetActive(false);
    }

    //  Assigns the PCard object to Card and assigns attributes from PCard
    public void AssignPCard(PCard pcard)
    {
        if (pcard == null)
        {
            return;
        }
        this.pcard = pcard;

        //  Specifiy this is of PCard type
        cardType = CardType.Card;

        //  Instantiate card visual before you can access components of it
        if (cardVisual == null)
        {
            InstantiateCardVisual();
        }

        //  Get object to modify appearance of PCard 
        AppearancePCard appearance = cardVisual.GetComponentInChildren<AppearancePCard>();
        ShaderCodePCard shaderAppearance = cardVisual.GetComponentInChildren<ShaderCodePCard>();

        //  Assign Horizontal Card Holder for Special Blinds
        HorizontalCardHolder cardHolder = FindFirstObjectByType<HorizontalCardHolder>();

        //  Assign Card ID
        cardID = pcard.cardID;

        //  Update Card Term Name
        termName = pcard.term.ToString();

        //  Update Card Symbol if different
        string newSymbol;
        LinguisticTermSymbol.unicodeMap.TryGetValue(pcard.term, out newSymbol);
        if (termSymbol != newSymbol)
        {
            termSymbol = newSymbol;
            appearance.UpdateTerm(newSymbol);
        }

        //  Update Suit if different
        string newSuit = pcard.suit.ToString();
        if (suitName != newSuit)
        {
            suitName = newSuit;
            appearance.UpdateSuit(newSuit, cardHolder.hideSuitFlag);
        }

        //  Update Enhancement if different
        string newEnhancement = pcard.enhancement.ToString();
        if (enhancementName != newEnhancement)
        {
            enhancementName = newEnhancement;
            appearance.UpdateEnhancement(newEnhancement);
        }

        //  Update Seal if different
        string newSeal = pcard.seal.ToString();
        if (sealName != newSeal)
        {
            sealName = newSeal;
            appearance.UpdateSeal(newSeal);
        }

        //  Update Edition if different
        string newEdition = pcard.edition.ToString();
        if (editionName != newEdition)
        {
            editionName = newEdition;
            shaderAppearance.UpdateEdition(pcard.edition);
        }

        //  Check for special blind that disable suits
        if (cardHolder.disabledSuitFlag)
        {
            //  If it's the disabled suit or is wild card, then set isDisabled to true
            if (pcard.suit == cardHolder.disabledSuitName || pcard.enhancement == CardEnhancement.WildCard)
            {
                pcard.isDisabled = true;
            }
        }
        else
        {
            pcard.isDisabled = false;
        }

        //  Disable card
        if (disabled != pcard.isDisabled)
        {
            disabled = !disabled;
            appearance.UpdateDisabled(disabled);
        }

        //  Description shows these attributes (for now just term and suit)
        cardDescription = pcard.ToString();
    }

    //  Assigns Consumable object to Card and assigns its attributes
    public void AssignConsumable(Consumable consumable)
    {
        if (consumable == null)
        {
            return;
        }
        this.consumable = consumable;

        //  Specifiy this is of PCard type
        cardType = CardType.Consumable;

        //  Instantiate card visual before you can access components of it
        if (cardVisual == null)
        {
            InstantiateCardVisual();
        }

        //  Assign consumable type
        consumableType = consumable.type;

        //  Assign name and description based on consumableType
        if (consumableType == ConsumableType.CardBuff)
        {
            CardBuff cardBuff = (CardBuff)consumable;
            consumableName = cardBuff.name.ToString();
            cardDescription = cardBuff.GetDescription();

        }
        else
        {
            Textbook textbook = (Textbook)consumable;
            consumableName = textbook.name.ToString();
            cardDescription = textbook.GetDescription();
        }

        //  Get object to modify appearance of Consumable (shares PCard Logic)
        AppearancePCard appearance = cardVisual.GetComponentInChildren<AppearancePCard>();
        ShaderCodePCard shaderAppearance = cardVisual.GetComponentInChildren<ShaderCodePCard>();

        //  Fetches sprite for card
        appearance.UpdateConsumable(consumableType, consumableName);
        shaderAppearance.UpdateEdition(CardEdition.Base);

        //  Assign other fields
        consumableSellValue = consumable.sellValue;
        consumableDisabled = consumable.isDisabled;

        //  Reformat the consumable name
        consumableName = SplitCase.Split(consumableName);
    }

    //  Assigns Mentor object to Card and assigns its attributes
    public void AssignMentor(Mentor mentor)
    {
        if (mentor == null)
        {
            return;
        }
        this.mentor = mentor;

        //  Specifiy this is of PCard type
        cardType = CardType.Mentor;

        //  Instantiate card visual before you can access components of it
        if (cardVisual == null)
        {
            InstantiateCardVisual();
        }

        //  Assign mentor name
        mentorName = mentor.name.ToString();

        //  Assign mentor edition
        mentorEditionName = mentor.edition.ToString();

        //  Get object to modify appearance of Mentor (shares PCard Logic)
        AppearancePCard appearance = cardVisual.GetComponentInChildren<AppearancePCard>();
        ShaderCodePCard shaderAppearance = cardVisual.GetComponentInChildren<ShaderCodePCard>();

        //  Fetches sprite/appearance for card
        appearance.UpdateMentor(mentorName);
        shaderAppearance.UpdateEdition(mentor.edition);

        //  Reformat the mentor name
        mentorName = SplitCase.Split(mentorName);

        //  Assign other fields
        mentorSellValue = mentor.sellValue;
        mentorDisabled = mentor.isDisabled;

        //  Assign description using GetDescription method of Mentor
        cardDescription = mentorName + ": " + mentor.GetDescription();
    }
}
