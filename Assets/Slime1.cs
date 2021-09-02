using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime1 : MonoBehaviour
{
    [SerializeField] Collider hitBoxDash;
    [SerializeField] Collider hitBoxPulo;
    [SerializeField] Collider hurtBox;
    [SerializeField] Transform Player;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;// para saber a duração de um clip no animator basta dividir a duração pela velocidade (length/speed)
    [SerializeField] GameObject poca;
    // Start is called before the first frame update
    private bool encarando;
    private int ContadorDedashs;
    public bool perdeuHP;

    public float jumpDuration = 1f;
    public float jumpDistance = 6;
    private bool jumping = false;
    private float jumpStartVelocityY;
    private bool pulando = false;
    private int hpDoBoss = 3;
    private int Ataque = 0;

    void Start()
    {
        encarando = true;
        ContadorDedashs = 0;
        perdeuHP = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ataque = RandomAtaque();
        if (encarando)
        {
            faceTarget(5f);
        }
        if(perdeuHP)
        {
            perdeuhp();
        }
    }

    void perdeuhp()
    {
        hpDoBoss -= 1;
        hurtBox.enabled = false;
        animator.SetBool("Cansado", false);
        animator.SetTrigger("Preparando");
        StopCoroutine("fimDoCansaco");
        perdeuHP = false;
    }

    public void dash()
    {
        animator.SetTrigger("Dash");
        navMeshAgent.SetDestination(Player.position);
    }

    void faceTarget(float turnSpeed)
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void CriarPoca(float range)
    {
        Instantiate(poca, transform.position, transform.rotation);
        Instantiate(poca, pocaRandom(range), transform.rotation);
        Instantiate(poca, pocaRandom(range), transform.rotation);
        Instantiate(poca, pocaRandom(range), transform.rotation);
    }

    private Vector3 pocaRandom(float range)
    {
        Vector3 position = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        Vector3 spawnPosition = transform.position + position;
        return spawnPosition;
    }

    //implementar um switch case que receber a quantidade de hp como parametro para executar qual a ação que o boss vai executar no momento
    public void controladorDeEtapas()
    {
        if(hpDoBoss == 3)
        {
            inicioDoDash();
        }
        if(hpDoBoss == 2)
        {
            navMeshAgent.isStopped = true;
            Pulo();
        }
        if(hpDoBoss == 1)
        {
            if(Ataque == 1)
            {
                navMeshAgent.isStopped = true;
                Pulo();
            }
            if(Ataque == 2)
            {
                navMeshAgent.isStopped = false;
                inicioDoDash();
            }
        }
    }
    private int RandomAtaque()
    {
        return Random.Range(1, 3);
    }

    public void inicioDoDash()
    {
        dash();
        hitBoxDash.enabled = true;
        encarando = false;
    }

    public void fimDoDash()
    {
        CriarPoca(8f);
        cansado();
        hitBoxDash.enabled = false;
        encarando = true;
    }

    public void incioDoPulo()
    {
        Pulo();
    }

    public void fimDoPulo()
    {
        cansado();
    }
    public void inicioDaHitBox()
    {
        hitBoxPulo.enabled = true;
        encarando = false;
    }

    public void fimDaHitbox()
    {
        cansado();
        hitBoxPulo.enabled = false;
        encarando = true;
    }

    public void cansado()
    {
        Debug.Log("ContradorDeDash:"+ContadorDedashs);
        ++ContadorDedashs;
        if (ContadorDedashs > 3)
        {
            animator.SetBool("Cansado",true);
            //parei aqui
            hurtBox.enabled = true;
            StartCoroutine("fimDoCansaco");
            Debug.Log("Teste");
            ContadorDedashs = 0;
        }
    }

    private IEnumerator fimDoCansaco()
    {
        //ativar HurtBox
        yield return new WaitForSeconds(3f);
        if (perdeuHP == false)
        {
            animator.SetBool("Cansado", false);
            hurtBox.enabled = false;
            animator.SetTrigger("Preparando");
        }
    }

    public void Pulo()
    {
        animator.SetTrigger("Pulando");
        StartCoroutine(Jump(Player.position));
        pulando = true;
    }

    private IEnumerator Jump(Vector3 direction)
    {
        jumping = true;
        Vector3 startPoint = transform.position;
        Vector3 targetPoint = direction;
        float time = 0;
        float jumpProgress = 0;
        float velocityY = jumpStartVelocityY;
        float height = startPoint.y;

        while (jumping)
        {
            jumpProgress = time / jumpDuration;

            if (jumpProgress > 1)
            {
                jumping = false;
                jumpProgress = 1;
            }

            Vector3 currentPos = Vector3.Lerp(startPoint, targetPoint, jumpProgress);
            currentPos.y = height + 12f;
            transform.position = currentPos;

            //Wait until next frame.
            yield return null;

            height += velocityY * Time.deltaTime;
            velocityY += Time.deltaTime * Physics.gravity.y;
            time += Time.deltaTime;
        }
        transform.position = targetPoint;
        yield break;
    }

    public void descendo()
    {
        animator.SetTrigger("Descendo");
    }

    public void impacto()
    {
        animator.SetTrigger("Impacto");
    }

    public void PocaDoPulo()
    {
        CriarPoca(12f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerHP>())
        {
            other.GetComponent<PlayerHP>().receberDano(1);
        }
    }
}
