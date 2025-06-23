
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{   
    [Header("Hover Information")]
    [Tooltip("This is the specific information for this card. Set this from DeckManager.")]
    [TextArea(3, 5)]
    public string cardDescription = "Default card information.";

    private static GameObject infoPanel;
    private static TextMeshProUGUI infoText;
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
    private bool loggedMissingParent = false;


    [Header("Visual")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;

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

    
    public PCard pcard;
    [Header("PCard Object")]
    public string termName;
    public string termSymbol;
    public string suitName;
    public string enhancementName;
    public string sealName;
    public string editionName;


    void Start()
    {
         if (infoPanel == null)
        {
            infoPanel = GameObject.Find("CardInfoPanel");
            if (infoPanel != null)
            {
                infoText = infoPanel.GetComponentInChildren<TextMeshProUGUI>();
                infoPanel.SetActive(false); // Ensure it's hidden at the start
            }
            else
            {
                Debug.LogError("Could not find 'CardInfoPanel' in the scene. Make sure it is named correctly and exists in your Hierarchy.");
            }
        }
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        if (!instantiateVisual)
            return;

        visualHandler = FindObjectOfType<VisualCardsHandler>();
        cardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
    }

    void Update()
    {
        ClampPosition();

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
        if (infoPanel != null && infoText != null)
        {
            // Update the text with this card's specific description
            infoText.text = cardDescription;
            // Show the panel
            infoPanel.SetActive(true);
            
        }
    }

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

        if (selected)
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
        else
            transform.localPosition = Vector3.zero;
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (selected)
                transform.localPosition += (cardVisual.transform.up * 50);
            else
                transform.localPosition = Vector3.zero;
        }
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
        if(cardVisual != null)
        Destroy(cardVisual.gameObject);
    }

    //  Assigns the PCard object to Card and assigns attributes from PCard
    public void AssignPCard(PCard pcard)
    {
        this.pcard = pcard;
        termName = pcard.term.ToString();
        LinguisticTermSymbol.unicodeMap.TryGetValue(pcard.term, out termSymbol);
        suitName = pcard.suit.ToString();
        enhancementName = pcard.enhancement.ToString();
        sealName = pcard.seal.ToString();
        editionName = pcard.edition.ToString();

        //  Description shows these attributes (for now just term and suit)
        cardDescription = pcard.ToString(); 
    }
}
