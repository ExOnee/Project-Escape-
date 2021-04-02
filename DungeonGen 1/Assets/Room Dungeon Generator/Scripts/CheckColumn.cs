using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckColumn : MonoBehaviour
    {
        void Update()
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 0.5f);
            foreach (Collider coll in hitColliders)
            {
                if (coll.gameObject.tag == "CorColumn")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}