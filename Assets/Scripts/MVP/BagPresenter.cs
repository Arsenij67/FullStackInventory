using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;
 

public class BagPresenter : MonoBehaviour
{
    [Inject] private List<Item> items;
    [Inject] private BagUIView bagUI;

    [SerializeField] private Transform[] bagFreePlaces = new Transform[4];
    private int indexFreeSection = 0;

    private Transform placeItemSpawn;
    public const float MaxWeight = 30f; // Максимальный вес сумки
    public float freeWeight = MaxWeight;
    private string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    private int itemIndexDelete;

    public bool UiIsOpen => bagUI.isOpen;
    private void OnEnable()
    {
        DraggableItem[] dItems = FindObjectsOfType<DraggableItem>();
       dItems.ToList().ForEach(item => onItemAdded.AddListener(item.LetGoItem));
       dItems.ToList().ForEach(item => onItemRemoved.AddListener(item.GetItem));
       
    }

    private void OnDisable()
    {
        DraggableItem[] dItems = FindObjectsOfType<DraggableItem>();
        dItems.ToList().ForEach(item => onItemAdded.RemoveListener(item.LetGoItem));
        dItems.ToList().ForEach(item => onItemRemoved.RemoveListener(item.GetItem));

    }


    // события
    private UnityEvent onItemAdded = new UnityEvent();
   private UnityEvent<Item> onItemRemoved = new UnityEvent<Item>();
    public void AddItem(Item item)
    {
        if (CanAddItem(item))
        {
            StartCoroutine(bagUI.PutOrGetItemIntoBag(bagFreePlaces[indexFreeSection].position, item));
            indexFreeSection++;
            AddItemToBag(item);
            SendToServer(item.id, "add");

            bagUI.AddItemPanel(items.IndexOf(item),item);
            onItemAdded.Invoke();
        }
    }

    public void ChangeItemDelete(int index)
    {
        itemIndexDelete = index;
    }

    public void RemoveItem()
    {
    
        if (IsBagEmpty() || items.Count()-1<itemIndexDelete) return;

        Item activeItem = items[itemIndexDelete];
        StartCoroutine(bagUI.PutOrGetItemIntoBag(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1)), activeItem));
        activeItem.added = false;
        indexFreeSection--;
        RemoveItemFromBag(activeItem);
        SendToServer(activeItem.id, "removed");
        bagUI.RemoveItemPanel(itemIndexDelete);
        onItemRemoved.Invoke(activeItem);
    }

    private bool CanAddItem(Item item)
    {
        return freeWeight - item.Weight >= 0;
    }

    private void AddItemToBag(Item item)
    {
        freeWeight -= item.Weight;
        items.Add(item);
    }

    private bool IsBagEmpty()
    {
        Debug.Log(items.Count());
        return !items.Any();
    }

    private void RemoveItemFromBag(Item item)
    {
        freeWeight += item.Weight;
        items.Remove(item);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item selectedItem = collision.transform.GetComponent<Item>();
        if (selectedItem && !selectedItem.added)
        {
            AddItem(selectedItem);
            selectedItem.added = true;
            
        }
    }

    public void OpenBag()
    {
        bagUI.OpenBagPanel();
    }

    public void CloseBag()
    {
        bagUI.CloseBagPanel();
    }


    // Метод для отправки POST-запроса на сервер
    private void SendToServer(string itemId, string eventType)
    {
        StartCoroutine(SendPostRequest(itemId, eventType));
    }

    // Корутина для отправки POST-запроса
    private IEnumerator SendPostRequest(string itemId, string eventType)
    {
        // Создаем JSON-тело запроса
        string jsonData = $"{{\"item_id\": \"{itemId}\", \"event\": \"{eventType}\"}}";

        // Создаем UnityWebRequest для отправки POST-запроса
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Ошибка: {request.error}");
            }
            else
            {
                Debug.Log($"Ответ: {request.downloadHandler.text}");
            }
        }
    }


}