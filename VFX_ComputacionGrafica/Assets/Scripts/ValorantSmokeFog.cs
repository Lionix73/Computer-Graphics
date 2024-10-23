using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ValorantSmokeFog : VolumeComponent, IPostProcessComponent
{
    public ColorParameter fogColor = new ColorParameter(new Color(0.1f, 0.25f, 1f));
    public FloatParameter fogStart = new FloatParameter(5f);
    public FloatParameter fogSmoothness = new FloatParameter(5f);

    public bool IsActive() => fogColor.value.a > 0.05f;
    public bool IsTileCompatible() => false;
}
