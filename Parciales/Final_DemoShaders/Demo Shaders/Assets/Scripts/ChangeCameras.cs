using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChangeCameras : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera camera3;

    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        button1.onClick.AddListener(() => SwitchCamera(camera1));
        button2.onClick.AddListener(() => SwitchCamera(camera2));
        button3.onClick.AddListener(() => SwitchCamera(camera3));
    }

    void SwitchCamera(CinemachineVirtualCamera activeCamera)
    {
        camera1.gameObject.SetActive(camera1 == activeCamera);
        camera2.gameObject.SetActive(camera2 == activeCamera);
        camera3.gameObject.SetActive(camera3 == activeCamera);
    }
}