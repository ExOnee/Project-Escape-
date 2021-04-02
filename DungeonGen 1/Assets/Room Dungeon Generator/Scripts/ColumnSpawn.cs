using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class ColumnSpawn : MonoBehaviour
    {
        void Start()
        {
            if (Random.value <= 0.7)
            {
                Destroy(gameObject);
            }
        }
    }
}
