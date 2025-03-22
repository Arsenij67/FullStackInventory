using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using TMPro;

public class ItemUIView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    private Color passiveColor = Color.white;
    private Color activeColor = Color.green;
    private Image imageView;
    [SerializeField] private TMP_Text textName;
    [Inject] private BagPresenter bagPresenter;
    private void Awake()
    {
        imageView = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        imageView.color = activeColor;
        bagPresenter.ChangeItemDelete(index);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        imageView.color = passiveColor;

    }

    public void AddInfo(Item addedItem)
    {
        textName.text = addedItem.name;
    }

    public void RemoveInfo()
    {
        textName.text = "";
    }
    private void OnDisable()
    {
        imageView.color = passiveColor;
    }
}
