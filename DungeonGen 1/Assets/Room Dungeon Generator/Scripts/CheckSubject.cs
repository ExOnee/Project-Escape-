using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckSubject : MonoBehaviour
    {
        void Start()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 2f);
            foreach (Collider coll in hitColliders)
            {
                if (coll.gameObject.tag == "Sub")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
