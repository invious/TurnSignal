using System;
using UnityEngine;
using Valve.VR;
using OVRLay;

[RequireComponent(typeof(Overlay_Unity))]
public class Overlay_CameraTexture : MonoBehaviour
{
    [Space(10)]
    public Camera targetCamera;
    public int antialiasing = 8;
    public FilterMode filterMode = FilterMode.Trilinear;
    [Space(10)]
    public int textureWidthOverride = 0;
    public int textureHeightOverride = 0;

    private RenderTexture targetTex;

    private Overlay_Unity u_overlay;
    private OVRLay.OVRLay overlay;

    void Start()
    {
        u_overlay = GetComponent<Overlay_Unity>();
        overlay = u_overlay.overlay;

        int width = (textureWidthOverride > 0) ? textureWidthOverride : targetCamera.pixelWidth;
        int height = (textureHeightOverride > 0) ? textureHeightOverride : targetCamera.pixelHeight;

        targetTex = new RenderTexture(width, height, 24);

        targetTex.antiAliasing = antialiasing;
        targetTex.filterMode = filterMode;

        targetCamera.targetTexture = targetTex;
        targetCamera.enabled = false;
    }

    void Update()
    {
        if (overlay.Created)
        {
            if (overlay.TextureType != ETextureType.DirectX)
                overlay.TextureType = ETextureType.DirectX;

            if (!overlay.TextureBounds.Equals(Overlay_Unity.TextureBounds))
                overlay.TextureBounds = Overlay_Unity.TextureBounds;

            targetCamera.Render();

            overlay.Texture = targetTex;
        }
    }
}