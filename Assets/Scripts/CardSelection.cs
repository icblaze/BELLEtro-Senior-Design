using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSelection : MonoBehaviour, IPointerClickHandler
{
    private DeleteCard deleteCardScript;
    private bool isSelected = false;

    void Start()
    {
        deleteCardScript = FindFirstObjectByType<DeleteCard>(); // ✅ Fix: Use FindFirstObjectByType instead
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected)
        {
            deleteCardScript.AddSelectedCard(gameObject); // ✅ Fix: Use AddSelectedCard() instead of SetSelectedCard()
            isSelected = true;
            //GetComponent<Image>().color = Color.red; // Optional: Highlight selection
        }
        else
        {
            deleteCardScript.RemoveSelectedCard(gameObject); // ✅ Fix: Use RemoveSelectedCard() instead of SetSelectedCard()
            isSelected = false;
            //GetComponent<Image>().color = Color.white; // Optional: Reset color
        }
    }
}
