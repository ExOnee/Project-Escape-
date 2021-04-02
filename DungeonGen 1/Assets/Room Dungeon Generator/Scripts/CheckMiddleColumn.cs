using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckMiddleColumn : MonoBehaviour
    {
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize / 2);
            foreach (Collider coll in hitColliders)
            {
                if (coll.tag == "thing")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
