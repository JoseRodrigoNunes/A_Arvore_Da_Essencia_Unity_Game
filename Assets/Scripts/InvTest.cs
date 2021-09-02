using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvTest : MonoBehaviour
{
    // Start is called before the first frame update
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHP>())
        {
            other.GetComponent<PlayerHP>().receberDano(1);
        }
    }
}
