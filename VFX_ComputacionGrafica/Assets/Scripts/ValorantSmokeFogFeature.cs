using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ValorantSmokeFogFeature : ScriptableRendererFeature
{
    [SerializeField] private RenderPassEvent passEvent;
    [SerializeField] private Material smokeFogMaterial;

    ValorantSmokeFogPass pass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

    public override void Create()
    {
        pass = new ValorantSmokeFogPass(smokeFogMaterial);
        pass.renderPassEvent = passEvent;
    }
}
