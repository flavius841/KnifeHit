using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombineChildrenOfEachChild : MonoBehaviour
{
    [Header("Settings")]
    public bool hideOriginalsOnSuccess = true;

    [Header("Results")]
    public List<Sprite> createdSprites = new List<Sprite>();

    void Start()
    {
        StartCoroutine(CombineRoutine());
    }

    IEnumerator CombineRoutine()
    {
        // Wait for Unity to build the UI layout
        yield return new WaitForEndOfFrame();

        CombineAllGroups();
    }

    void CombineAllGroups()
    {
        createdSprites.Clear();

        // Loop through each Top-Level Child (Child 1, Child 2...)
        foreach (Transform topGroup in transform)
        {
            // Skip inactive objects
            if (!topGroup.gameObject.activeInHierarchy) continue;

            // Find all images in this group (including the group parent itself)
            List<Image> images = new List<Image>();
            images.AddRange(topGroup.GetComponentsInChildren<Image>());

            // Remove images that are invisible (alpha 0) or disabled
            images.RemoveAll(img => img.sprite == null || !img.enabled || img.color.a == 0);

            if (images.Count == 0) continue;

            // Create the combined sprite
            Sprite combined = MergeImagesPreservingTransparency(images, topGroup);

            if (combined != null)
            {
                combined.name = topGroup.name + "_Sprite";
                createdSprites.Add(combined);
                Debug.Log($"Success: Created transparent sprite for '{topGroup.name}'");

                // Optional: Hide the original messy objects
                if (hideOriginalsOnSuccess)
                {
                    foreach (Transform t in topGroup) t.gameObject.SetActive(false);
                    if (topGroup.GetComponent<Image>()) topGroup.GetComponent<Image>().enabled = false;

                    // Assign the new sprite to the top object if it has an Image component
                    Image parentImg = topGroup.GetComponent<Image>();
                    if (parentImg == null) parentImg = topGroup.gameObject.AddComponent<Image>();

                    parentImg.sprite = combined;
                    parentImg.enabled = true;
                    parentImg.color = Color.white; // Reset color to ensure visibility
                }
            }
        }
    }

    Sprite MergeImagesPreservingTransparency(List<Image> images, Transform rootParams)
    {
        // 1. Calculate Bounds
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        foreach (Image img in images)
        {
            Vector3[] corners = new Vector3[4];
            img.rectTransform.GetWorldCorners(corners);

            foreach (Vector3 worldC in corners)
            {
                Vector3 localC = rootParams.InverseTransformPoint(worldC);
                if (localC.x < min.x) min.x = localC.x;
                if (localC.y < min.y) min.y = localC.y;
                if (localC.x > max.x) max.x = localC.x;
                if (localC.y > max.y) max.y = localC.y;
            }
        }

        int width = Mathf.CeilToInt(max.x - min.x);
        int height = Mathf.CeilToInt(max.y - min.y);

        if (width <= 0 || height <= 0) return null;

        // 2. Create a blank Transparent Texture
        Texture2D finalTex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // IMPORTANT: Fill with clear transparency (0,0,0,0)
        Color[] clearColors = new Color[width * height];
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = Color.clear;
        finalTex.SetPixels(clearColors);

        // 3. Draw Images
        foreach (Image img in images)
        {
            Texture2D sourceTex = GetReadableTexture(img.sprite.texture); // <--- MAGIC HELPER
            if (sourceTex == null) continue;

            // Calculate where this image goes
            Vector3[] corners = new Vector3[4];
            img.rectTransform.GetWorldCorners(corners);
            Vector3 localBottomLeft = rootParams.InverseTransformPoint(corners[0]);

            int startX = Mathf.RoundToInt(localBottomLeft.x - min.x);
            int startY = Mathf.RoundToInt(localBottomLeft.y - min.y);
            int targetW = Mathf.RoundToInt(img.rectTransform.rect.width);
            int targetH = Mathf.RoundToInt(img.rectTransform.rect.height);

            // Copy pixels with resizing
            for (int y = 0; y < targetH; y++)
            {
                float v = y / (float)targetH;
                for (int x = 0; x < targetW; x++)
                {
                    float u = x / (float)targetW;

                    Color srcColor = sourceTex.GetPixelBilinear(u, v);
                    srcColor *= img.color; // Apply tint

                    int finalX = startX + x;
                    int finalY = startY + y;

                    if (finalX >= 0 && finalX < width && finalY >= 0 && finalY < height)
                    {
                        // Alpha Blending (Standard "Over" operator)
                        if (srcColor.a > 0)
                        {
                            Color bgColor = finalTex.GetPixel(finalX, finalY);

                            float outA = srcColor.a + bgColor.a * (1 - srcColor.a);
                            Color outColor = (srcColor * srcColor.a + bgColor * bgColor.a * (1 - srcColor.a)) / outA;
                            outColor.a = outA;

                            finalTex.SetPixel(finalX, finalY, outColor);
                        }
                    }
                }
            }
        }

        finalTex.Apply();
        return Sprite.Create(finalTex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }

    // This function solves the "Read/Write Enabled" error by using a temporary RenderTexture
    Texture2D GetReadableTexture(Texture2D source)
    {
        if (source == null) return null;

        // If it's already readable, just return it
        if (source.isReadable) return source;

        // If not, copy it to a temporary RenderTexture
        RenderTexture tmp = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, tmp);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmp;

        Texture2D myTex = new Texture2D(source.width, source.height);
        myTex.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmp);

        return myTex;
    }
}