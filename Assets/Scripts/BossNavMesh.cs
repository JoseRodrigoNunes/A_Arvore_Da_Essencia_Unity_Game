using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNavMesh : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private GameObject poca;
    [SerializeField] private BoxCollider HitBox;
    [SerializeField] private Animator animator;
    [SerializeField] private SphereCollider hurtBox;

    public GameObject Head;
    private changeSkullColor skullColor;
    private NavMeshAgent navMeshAgent;

    public bool canTakeDamage;
    public int stamina;
    public bool firstHit;
    //int isTiredHash;

    private void Awake()
    {
        HitBox.enabled = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        skullColor = Head.GetComponent<changeSkullColor>();
    }

    private void Start()
    {
        firstHit = false;
        stamina = 3;
        canTakeDamage = true;
        StartCoroutine("AttackLoop1");
    }

    private void Update()
    {
        //float distance = Vector3.Distance(playerLocation.position, transform.position);
        //if(distance >= navMeshAgent.stoppingDistance
        if(bossHealth.BossHP)
        {
            faceTarget(1f);
        }
        else 
        {
            faceTarget(5.0f);
        }
    }

    public void ativarHitBoxDash()
    {
        HitBox.enabled = true;
    }

    public void desativarHitBoxDash()
    {
        HitBox.enabled = false;
    }

    public void stopRoutine1()
    {
        Debug.Log("stopRoutine1()");
        StopCoroutine("Attack");
        StopCoroutine("Tired");
    }

    void faceTarget(float turnSpeed)
    {
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public IEnumerator Tired()
    {
        ativarCansado();
        WaitForSeconds wait = new WaitForSeconds(1f);
        //animator.SetBool("isTired", true);
        for (int i = 0 ; i < 4 ; i++)
        {
            yield return wait;
            Debug.Log("Segundo: "+ (i+1));
            if (firstHit)
            {
                firstHit = false;
                yield return new WaitForSeconds(1.0f);
                break;
            }
        }
        desativarCansado();
        yield return new WaitForSeconds(0.2f);
    }

    private void ativarCansado()
    {
        animator.SetBool("isTired", true);
        skullColor.skullTired();
        hurtBox.enabled = true;
        bossHealth.BossHP = true;
        Debug.Log("Cansado");
    }

    private void desativarCansado()
    {
        hurtBox.enabled = false;
        bossHealth.BossHP = false;
        skullColor.skullAttack();
        animator.SetBool("isTired", false);
        Debug.Log("Recuperou do Cansaço");
    }

    private void dash()
    {
        animator.SetTrigger("Dash");
        navMeshAgent.SetDestination(playerLocation.position);
    }

    public void CriarPoca()
    {
        Instantiate(poca, transform.position, transform.rotation);
        Instantiate(poca, pocaRandom(), transform.rotation);
        Instantiate(poca, pocaRandom(), transform.rotation);
        Instantiate(poca, pocaRandom(), transform.rotation);
    }

    private Vector3 pocaRandom()
    {
        float rng = 8.0f;
        Vector3 position = new Vector3(Random.Range(-rng, rng), 0, Random.Range(-rng, rng));
        Vector3 spawnPosition = transform.position + position;
        return spawnPosition;
    }

    public IEnumerator AttackLoop1()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            if(stamina>0)
            {
                //if (firstHit)
                //{
                //    Debug.Log("PAROU!!!");
                //    break;
                //}
                dash();
                yield return new WaitForSeconds(3.0f);
                --stamina;
            }
            else
            {
                yield return StartCoroutine(Tired());
                stamina = 3;
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerHP>())
        {
            other.GetComponent<PlayerHP>().receberDano(1);
        }
    }
}
