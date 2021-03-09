using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect1 : MonoBehaviour
{
    public Material EffectMaterial;
    [Range(0, 20)]
    public int BlurIterations = 1;

    /// <summary>
    /// Used for performance improvement over a high resolution blur effect.
    /// </summary>
    [Range(0, 4)]
    public int DownRes = 0;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width >> this.DownRes;
        int height = source.height >> this.DownRes;

        RenderTexture tempTexture = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, tempTexture);

        for (int ii = 0; ii < this.BlurIterations; ii++)
        {
            RenderTexture auxTexture = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(tempTexture, auxTexture, EffectMaterial);

            RenderTexture.ReleaseTemporary(tempTexture);

            tempTexture = auxTexture;
        }

        Graphics.Blit(tempTexture, destination);
        RenderTexture.ReleaseTemporary(tempTexture);
    }
}
