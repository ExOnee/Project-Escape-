using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class LightSpawn : MonoBehaviour
    {
        void Start()
        {
            if (Random.value <= 0.92)
            {
                Destroy(gameObject);
            }
        }
        void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RoomMapManager.TileSize * 0.5f);
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
