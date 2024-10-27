using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;

    [Header("PowerUp Effect")]
    [SerializeField] private VisualEffect powerUpEffect;
    [SerializeField] private GameObject electricVFX;
    [SerializeField] private float initialDelay = 0.4f;
    [SerializeField] private float duration = 1.2f;

    private bool poweringUp = false;
    private bool jumpAttacking = false;


    void Start()
    {
        
    }

    void Update()
    {
        if (animator != null){
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.SetTrigger("PowerUp");
                powerUpEffect.Play();
                StartCoroutine(ChangeLayerWithDelay(electricVFX, initialDelay, duration));

                poweringUp = true;
                StartCoroutine(ResetBool(poweringUp, 0.5f));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.SetTrigger("JumpAttack");

                jumpAttacking = true;
            }
        }
    }

    private IEnumerator ResetBool (bool boolToReset, float delay = 0.1f){
        yield return new WaitForSeconds(delay);
        poweringUp = !poweringUp;
    }

    private IEnumerator ActivateWithDelay(GameObject targetObject, float initialDelay, float duration)
    {
        yield return new WaitForSeconds(initialDelay);
        targetObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        targetObject.SetActive(false);
    }

    private IEnumerator ChangeLayerWithDelay(GameObject targetObject, float initialDelay, float duration)
    {
        yield return new WaitForSeconds(initialDelay);
        ChangeLayerRecursively(targetObject, LayerMask.NameToLayer("Default"));
        yield return new WaitForSeconds(duration);
        ChangeLayerRecursively(targetObject, LayerMask.NameToLayer("Invisible"));
    }

    private void ChangeLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, newLayer);
        }
    }

    private void ResetPos(){
        player.transform.position = Vector3.zero;
    }
}
