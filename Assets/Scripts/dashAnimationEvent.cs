using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashAnimationEvent : MonoBehaviour
{
    [SerializeField] Slime1 slime;
    [SerializeField] puloTeste pulo;

    public void dash()
    {
        //slime.Pulo();
        slime.controladorDeEtapas();
    }

    public void fimDoDash()
    {
        slime.fimDoDash();
    }

    public void ContadorDeDashs()
    {
        slime.cansado();
    }

    public void descendo()
    {
        slime.descendo();
    }

    public void impacto()
    {
        slime.impacto();
    }

    public void pocoDoPulo()
    {
        slime.PocaDoPulo();
    }

    public void fimDoPulo()
    {
        slime.fimDoPulo();
    }

    public void inicioDoHitBox()
    {
        slime.inicioDaHitBox();
    }

    public void fimDoHitBox()
    {
        slime.fimDaHitbox();
    }
}
