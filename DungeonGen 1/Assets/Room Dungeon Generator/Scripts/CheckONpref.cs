using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckONpref : MonoBehaviour
    {
        void Start()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 1f);
            foreach (Collider coll in hitColliders)
            {
                if (coll.gameObject.tag == "thing" || coll.gameObject.tag == "TileCor" || coll.gameObject.tag == "Wall")
                {
                    Destroy(gameObject);
                }
            }
        }

    }
}
