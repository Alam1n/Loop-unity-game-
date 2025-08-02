using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    public void AttachToPlayer(Transform holdPoint)
    {
        if (holdPoint == null) return;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.isKinematic = true;
        }

        if (TryGetComponent<Collider2D>(out var col))
        {
            col.enabled = false;
        }

        Debug.Log("Item picked up!");
    }
    

}
