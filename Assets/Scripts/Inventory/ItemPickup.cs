using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))] //pickup Radius collider
public class ItemPickup : MonoBehaviour
{
    public float pickUpRadius;
    public ItemData itemData;

    private SphereCollider thisCollider;

    private void Awake()
    {
        thisCollider = GetComponent<SphereCollider>();
        thisCollider.isTrigger= true; //set Trigger Pickup
        thisCollider.radius = pickUpRadius; //set collider radius
    }


}
