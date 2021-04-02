using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{  
    public class CheckLight : MonoBehaviour
    {
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 2.5f);
            foreach (Collider coll in hitColliders)
            {
                if (coll.gameObject.tag == "TileCor")
                {
                    Destroy(gameObject);
                }              
            }        
        }
    }
}
