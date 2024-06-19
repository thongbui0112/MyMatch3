using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour {
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cbmcp;

    public float shakeIntensity;
    public float shakeTime;
    private float timer;
    private bool shakeCam;

    public bool ShakeCam { get => shakeCam; set => shakeCam = value; }

    private void Awake() {
        this.cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        this.cbmcp = this.cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }
    private void Start() {
        StopShake();
        this.shakeCam = false;
    }
    void Update() {
        if(this.shakeCam) {
            ShakeCamera();
            this.shakeCam =false;
        }
        if(this.timer > 0)
            this.timer -= Time.deltaTime;
        if(this.timer < 0) {
            StopShake();
        }
    }

    public void ShakeCamera() {
        this.cbmcp.m_AmplitudeGain = this.shakeIntensity;
        this.timer = this.shakeTime;
    }

    public void StopShake() {
        this.cbmcp.m_AmplitudeGain = 0;
        this.timer = 0f;
    }
}
