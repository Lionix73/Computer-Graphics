using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VFXControllerUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> vfxList;
    [SerializeField] private TextMeshProUGUI vfxNameText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private AnimatorController animatorController;

    private int currentVFXIndex = 0;

    void Start()
    {
        UpdateVFXDisplay();
        nextButton.onClick.AddListener(NextVFX);
        previousButton.onClick.AddListener(PreviousVFX);
        playButton.onClick.AddListener(PlayVFX);
        stopButton.onClick.AddListener(StopVFX);
    }

    void UpdateVFXDisplay()
    {
        for (int i = 0; i < vfxList.Count; i++)
        {
            vfxList[i].gameObject.SetActive(i == currentVFXIndex);
        }
        vfxNameText.text = vfxList[currentVFXIndex].name;
    }

    void NextVFX()
    {
        currentVFXIndex = (currentVFXIndex + 1) % vfxList.Count;
        UpdateVFXDisplay();
    }

    void PreviousVFX()
    {
        currentVFXIndex = (currentVFXIndex - 1 + vfxList.Count) % vfxList.Count;
        UpdateVFXDisplay();
    }

    void PlayVFX()
    {
        if (Time.timeScale == 0){
            Time.timeScale = 1;
        }
        else{
            if (vfxList[currentVFXIndex].name == "PoweringUp")
            {
                animatorController.PowerUp();
            }
            else if (vfxList[currentVFXIndex].name == "JumpAttacking")
            {
                animatorController.JumpAttack();
            }
            else if (vfxList[currentVFXIndex].name == "GroundSlashing")
            {
                animatorController.GroundSlash();
            }
        }  
    }

    void StopVFX()
    {
        Time.timeScale = 0;
    }
}