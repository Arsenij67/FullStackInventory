using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BagUIView : MonoBehaviour
{
    [SerializeField] private TMP_Text textFullness;
    [SerializeField] private RectTransform bagPanel;
    [SerializeField] private ItemUIView [] uiItems;
    public bool isOpen;
    public IEnumerator PutOrGetItemIntoBag(Vector3 endPos, Item selectedItem )
    {
         
        Rigidbody itemRigidbody = selectedItem.GetComponent<Rigidbody>();

        itemRigidbody.isKinematic = true;

        // ����� �������� (����� ���������)
        float duration = 0.5f;
        float elapsedTime = 0f;

        // ��������� ������� ��������
        Vector3 startPosition = selectedItem.transform.position;

        // ������� ������� (����� �������)
        Vector3 targetPosition = endPos;

        // ������� ����������� �������� � ������� �������
        while (elapsedTime < duration)
        {
            // ��������� �������� �������� (�� 0 �� 1)
            float t = elapsedTime / duration;

           
            selectedItem.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

         
            elapsedTime += Time.deltaTime;

         
            yield return null;
        }

      
        selectedItem.transform.position = targetPosition;

    }
    public void OpenBagPanel()
    {
        isOpen = true;
        bagPanel.gameObject.SetActive(true);
    }

    public void CloseBagPanel()
    {
        isOpen = false;
        bagPanel.gameObject.SetActive(false);
    }

    public void AddItemPanel(int indexAddedItem, Item addedItem)
    {
        uiItems[indexAddedItem].AddInfo(addedItem);
    }

    public void RemoveItemPanel(int indexAddedItem)
    {
        uiItems[indexAddedItem].RemoveInfo();

    }

}
