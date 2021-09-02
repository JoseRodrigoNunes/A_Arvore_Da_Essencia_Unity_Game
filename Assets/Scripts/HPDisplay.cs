using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    public Text Health;
    // Start is called before the first frame update
    
    public void SetHealth(string hp)
    {
        Health.text = hp;
    }

    public void SetNull()
    {
        Health.text = null;
    }
}

