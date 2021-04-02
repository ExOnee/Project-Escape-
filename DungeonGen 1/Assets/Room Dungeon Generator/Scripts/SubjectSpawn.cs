using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectSpawn : MonoBehaviour
{
    void Start()
    {
        if (Random.value <= 0.5)
        {
            Destroy(gameObject);
        }
    }
}
