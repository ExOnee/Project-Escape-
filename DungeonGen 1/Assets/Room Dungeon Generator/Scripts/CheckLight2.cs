using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class CheckLight2 : MonoBehaviour
    {
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.localPosition + new Vector3(0.2f, 0, 0.2f) * RoomMapManager.TileSize, RoomMapManager.TileSize);
            foreach (Collider coll in hitColliders)
            {
                if (coll.gameObject.tag == "thing")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
