using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToucher : MonoBehaviour
{
    internal bool TryGetItemAtCoordinates(Vector2 coordinates, float maxDistance, out Item item)
    {
        item = null;
        if (TryRaycast(coordinates, maxDistance, out RaycastHit hitInfo))
        {
            item = hitInfo.transform.GetComponent<Item>();
        }
        return item != null;
    }

    private bool TryRaycast(Vector2 coordinates, float maxDistance, out RaycastHit hitInfo)
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(coordinates.x, coordinates.y, 0));
        return Physics.Raycast(ray, out hitInfo, maxDistance);
    }
}
