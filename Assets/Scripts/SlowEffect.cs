using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterMovement characterMovement;

    private void Awake()
    {
        StartCoroutine("Expire");
    }

    private void Start()
    {
        float randomNumber = Random.Range(6f, 12f);
        transform.localScale = new Vector3(randomNumber, 0.1875f, randomNumber);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Deu Certo!");
            other.GetComponent<CharacterMovement>().setSpeed(4, 0.5f);
            other.GetComponent<CharacterMovement>().canDash = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("Deu Certo!");
            other.GetComponent<CharacterMovement>().setSpeed(4, 0.5f);
            other.GetComponent<CharacterMovement>().canDash = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("Deu Certo Denovo");
            other.GetComponent<CharacterMovement>().setSpeed(8, 1f);
            other.GetComponent<CharacterMovement>().canDash = true;
        }
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(15f);
        FindObjectOfType<CharacterMovement>().setSpeed(8, 1f);
        FindObjectOfType<CharacterMovement>().canDash = true;
        Destroy(gameObject);
    }

}
