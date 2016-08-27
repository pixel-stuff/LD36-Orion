using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Custom/Compose")]
public class Compose : UnityStandardAssets.ImageEffects.PostEffectsBase
{
    
    public Shader _shader = null;
    private Material _material = null;

    public Camera _cam = null;
    private RenderTexture _Addition = null;

    public Color color;

    public new void Start()
    {
        base.CheckResources();
        if (_Addition != null)
        {
            _Addition.Release();
        }
        _Addition = new RenderTexture(Screen.width, Screen.height, 0);
        _cam.targetTexture = _Addition;
    }

    public override bool CheckResources() {
        CheckSupport(false);

        _material = CheckShaderAndCreateMaterial(_shader, _material);
        if(_material != null)
        {
        }

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    public void OnDisable() {
        if (_material)
            DestroyImmediate(_material);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        
        if (CheckResources() == false) {
            Graphics.Blit(source, destination);
            return;
        }

        // setup shader params
        _material.SetVector("_Color", color);
        _material.SetTexture("_Addition", _Addition);
        
        source.filterMode = FilterMode.Bilinear;

        // downsample
       // RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

        //rt.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, destination, _material, 0);

        //Graphics.Blit(rt, destination);

        //RenderTexture.ReleaseTemporary(rt);
    }
}
