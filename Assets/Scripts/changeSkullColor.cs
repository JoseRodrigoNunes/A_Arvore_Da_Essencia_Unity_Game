using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeSkullColor : MonoBehaviour
{
    [SerializeField] private Material redSkull;
    [SerializeField] private Material WhiteSkull;

    // Start is called before the first frame update
    
    public void skullTired()
    {
        GetComponent<Renderer>().material = redSkull;
    }
    public void skullAttack()
    {
        GetComponent<Renderer>().material = WhiteSkull;
    }

}
