using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHP : MonoBehaviour
{
    PlayerInput playerInput;

    public HPDisplay hp;
    public displayBossHP bossHP;
    public int maxHp = 3;
    public int currentHP;
    public bool isDead = false;
    public GameOverScreen gameOverScreen;
    private float InvinsibleAmt;
    public BossNavMesh bossNavMesh;

    public float cooldownTime = 1f;
    private float nextTime = 0f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        
        playerInput.CharacterControls.testarHP.started += testarInput;
    }

        void Start()
    {
        currentHP = maxHp;
    }

    void testarInput(InputAction.CallbackContext context)
    {
        receberDano(1);
    }

    public void receberDano(int dano)
    {
        
        if (currentHP > 0 && Time.time > nextTime)
        {
            currentHP -= dano;
            hp.SetHealth(currentHP.ToString());
            Debug.Log("Pedeu HP");
            nextTime = Time.time + cooldownTime;
            if (currentHP == 0)
            {
                isDead = true;
                gameOverScreen.setup();
                hp.SetNull();
                bossHP.SetNull();
            }
        }
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
