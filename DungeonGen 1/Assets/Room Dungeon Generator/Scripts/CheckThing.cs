using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckThing : MonoBehaviour
    {

        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 1.5f);
            foreach (Collider coll in hitColliders)
            {
                if (coll.tag == "TileCor")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}