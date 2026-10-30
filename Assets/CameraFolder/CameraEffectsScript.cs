using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectsScript : MonoBehaviour
{
    public virtual CameraEffectsScript singleton { get; private set; }

    [SerializeField]
    private Material camMaterial;
    [SerializeField]
    public float blurStrength = 0f;
    [SerializeField]
    public float hueShift = 0f;


    private void Awake() {
        singleton = this;
    }

    private void Update() {
        camMaterial.SetFloat("_BlurStrength", blurStrength);
        camMaterial.SetFloat("_HueShift", hueShift);
    }
}
