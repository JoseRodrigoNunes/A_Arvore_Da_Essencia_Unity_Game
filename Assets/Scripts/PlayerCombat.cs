using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BossHealth bossHealth;
    CharacterMovement characterMovement;
    PlayerInput playerInput;
    public Animator animator;
    public Transform attackPoint;
    public Vector3 attackRange = new Vector3(0.5f, 0.5f, 1.40000005f);

    public float attackRate = 2f;
    public float nextAttackTime = 0f;
    public float attackTime = 0f;
    public LayerMask enemyLayers;

    

    
    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Attack.started += Attack;

    }

    private void Update()
    {
        //if(Time.)
    }
    // Update is called once per frame

    void Attack(InputAction.CallbackContext ctx)
    {
        
        if (Time.time >= nextAttackTime && Time.time >= characterMovement.nextDashTime)
        {
            animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapBox(attackPoint.position, attackRange, attackPoint.rotation, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                StartCoroutine("Ataque");
                //Debug.Log("Funcionando");
            }

            nextAttackTime = Time.time + .7f;
            attackTime = Time.time + 5f;
        }
        
    }
    IEnumerator Ataque()
    {
        yield return new WaitForSeconds(0.6f);
        bossHealth.TakeDamage(1);
    }
    

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
