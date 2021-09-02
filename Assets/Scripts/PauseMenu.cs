using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    PlayerInput playerInput;
    bool setup;

    // Start is called before the first frame update
    void Awake()
    {
        setup = true;
        playerInput = new PlayerInput();
        playerInput.CharacterControls.PauseMenu.started += pause;
    }

    void pause(InputAction.CallbackContext ctx)
    {
        Debug.Log("Teste");
        
        if (setup)
        {
            gameOverScreen.setup();    
        }
        if (setup == false)
        {
            gameOverScreen.setupFalse();
        }
        if (setup){
            setup = false;
        }
        else
        {
            setup = true;
        }
    }
    // Update is called once per frame
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
