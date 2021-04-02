using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class SpawnShield : MonoBehaviour
    {
        void Start()
        {
            if (Random.value <= 0.97)
            {
                Destroy(gameObject);
            }
        }

    }
}