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
    [SerializeField] private ParticleSystem electricPS;
    [SerializeField] private float initialDelayPowerUp = 0.4f;
    [SerializeField] private float durationPowerUp = 1.2f;

    [Header("Jump Attack Effect")]
    [SerializeField] private GameObject FireballShockWaveVFX;
    [SerializeField] private VisualEffect LightningTrailVFX;
    [SerializeField] private ParticleSystem jumpShockWavePS;
    [SerializeField] private ParticleSystem smashGroundPS;
    [SerializeField] private float smashGroundDelay;

    [Header("Ground Slash Effect")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem invocationPS;
    [SerializeField] private ParticleSystem preInvocationPS;
    [SerializeField] private Transform invocationPoint;
    [SerializeField] private float preInvocationDelay = 0.3f;
    [SerializeField] private float invocationDelay = 0.6f;

    private Vector3 destination;
    private GroundSlash groundSlash;


    
    private bool poweringUp = false;
    private bool jumpAttacking = false;
    private bool groundSlashing = false;
    private Transform playerTransform;


    void Start()
    {
        destination = player.transform.forward * 10;
    }

    void Update()
    {
        smashGroundPS.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        invocationPS.transform.position = invocationPoint.position;
        preInvocationPS.transform.position = invocationPoint.position;

        if (animator != null){
            if (Input.GetKeyDown(KeyCode.Alpha1) && !poweringUp)
            {
                animator.SetTrigger("PowerUp");
                powerUpEffect.Play();
                StartCoroutine(ChangeLayerWithDelay(electricVFX, initialDelayPowerUp, durationPowerUp));
                StartCoroutine(ActivateParticleSystemWithDelay(electricPS, initialDelayPowerUp));

                poweringUp = true;
                StartCoroutine(ResetBool(poweringUp, 3f));

                StartCoroutine(ResetPos(3f));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !jumpAttacking)
            {
                animator.SetTrigger("JumpAttack");
                LightningTrailVFX.Play();
                StartCoroutine(ActivateParticleSystemWithDelay(jumpShockWavePS, 0.6f));
                StartCoroutine(ActivateWithDelay(FireballShockWaveVFX, smashGroundDelay, 0.7f));
                StartCoroutine(ActivateParticleSystemWithDelay(smashGroundPS, smashGroundDelay));

                jumpAttacking = true;
                StartCoroutine(ResetBool(jumpAttacking, 4f));


                StartCoroutine(ResetPos(4f));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !groundSlashing)
            {
                animator.SetTrigger("GroundSlash");
                Invoke("InstantiateGroundSlash", 1.2f);
                StartCoroutine(ActivateParticleSystemWithDelay(preInvocationPS, preInvocationDelay));
                StartCoroutine(ActivateParticleSystemWithDelay(invocationPS, invocationDelay));

                groundSlashing = true;
                StartCoroutine(ResetBool(groundSlashing, 3.5f));

                StartCoroutine(ResetPos(3.5f));
            }
        }
    }

    private IEnumerator ResetBool (bool boolToReset, float delay = 0.1f){
        yield return new WaitForSeconds(delay);
        
        if (boolToReset == poweringUp){
            poweringUp = false;
        }
        else if (boolToReset == jumpAttacking){
            jumpAttacking = false;
        }
        else if (boolToReset == groundSlashing){
            groundSlashing = false;
        }
    }

    private IEnumerator ActivateParticleSystemWithDelay(ParticleSystem particleSystem, float delay)
    {
        yield return new WaitForSeconds(delay);
        particleSystem.Play();
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

    private IEnumerator ResetPos(float animDuration){
        yield return new WaitForSeconds(animDuration);
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    private void InstantiateGroundSlash(){
        var projectileObk = Instantiate(projectile, firePoint.position, Quaternion.identity);

        groundSlash = projectileObk.GetComponent<GroundSlash>();
        RotateToDestination(projectileObk, destination, true);
        projectileObk.GetComponent<Rigidbody>().velocity = player.transform.forward * groundSlash.Speed;
    }

    private void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
    {
        Vector3 direction = destination - obj.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = rotation;

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
