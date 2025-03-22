using Zenject;
using UnityEngine;
 

public class DraggableItem : WorldToucher
{
    private static Item ? currItem;
    private float startPosZ = 0;

    private void OnMouseDown()
    {
        // Используем Input.mousePosition для получения координат мыши
        if (TryGetItemAtCoordinates(Input.mousePosition, 20f, out currItem))
        {
            startPosZ = transform.position.z;
        }
    }
    public void LetGoItem()
    {
        currItem = null;

    }

    public void GetItem(Item item)
    {
        currItem = item;
    }

    private void OnMouseDrag()
    {
        if (currItem != null && !currItem.added) // Проверяем, что currItem не равен null
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, startPosZ));
            currItem.transform.position = mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (currItem)
        {
            Rigidbody rb = currItem.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;

        }
    }

}