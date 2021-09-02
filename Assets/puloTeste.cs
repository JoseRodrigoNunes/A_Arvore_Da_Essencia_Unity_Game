using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puloTeste : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Animator animator;
    public float jumpDuration = 1f;
    public float jumpDistance = 6;

    private bool jumping = false;
    private float jumpStartVelocityY;
    // Start is called before the first frame update

    public void Pulo()
    {
        StartCoroutine(Jump(player.position));
        animator.SetTrigger("Pulo");
    }

    private IEnumerator Jump(Vector3 direction)
    {
        Debug.Log("Teste do pulo");
        jumping = true;
        Vector3 startPoint = transform.position;
        Vector3 targetPoint = /*startPoint +*/ direction;
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
            currentPos.y = height+12f;
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
}
