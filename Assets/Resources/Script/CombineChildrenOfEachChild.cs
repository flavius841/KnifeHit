using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombineUIImages : MonoBehaviour
{
    // This list will hold your final combined sprites
    public List<Sprite> resultingSprites = new List<Sprite>();

    void Start()
    {
        MergeTopLevelChildren();
    }

    public void MergeTopLevelChildren()
    {
        resultingSprites.Clear();

        // 1. Loop through every direct child of the object this script is attached to
        foreach (Transform topLevelChild in transform)
        {
            // Get the Main Image and all its nested children images
            List<Image> imagesToMerge = new List<Image>();

            // Add all images found inside this top level child (including itself)
            imagesToMerge.AddRange(topLevelChild.GetComponentsInChildren<Image>());

            // If this child has no images at all, skip it
            if (imagesToMerge.Count == 0) continue;

            // 2. Combine them into one sprite
            Sprite newSprite = CombineListToSprite(imagesToMerge);

            if (newSprite != null)
            {
                newSprite.name = topLevelChild.name + "_Combined";
                resultingSprites.Add(newSprite);

                // Optional: Hide the original children and show only the new one?
                // For now, we just log it.
                Debug.Log($"Created combined sprite for: {topLevelChild.name}");
            }
        }
    }

    private Sprite CombineListToSprite(List<Image> images)
    {
        // --- Step A: Calculate the Size of the new texture ---
        // We need to find the Bottom-Left and Top-Right corners of the WHOLE group in World Space
        Vector2 minPos = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxPos = new Vector2(float.MinValue, float.MinValue);

        foreach (Image img in images)
        {
            if (img.sprite == null) continue;

            // Get the 4 corners of the UI element in World Space
            Vector3[] corners = new Vector3[4];
            img.rectTransform.GetWorldCorners(corners);

            // corners[0] is bottom-left, corners[2] is top-right
            foreach (Vector3 corner in corners)
            {
                if (corner.x < minPos.x) minPos.x = corner.x;
                if (corner.y < minPos.y) minPos.y = corner.y;
                if (corner.x > maxPos.x) maxPos.x = corner.x;
                if (corner.y > maxPos.y) maxPos.y = corner.y;
            }
        }

        // Calculate width and height required
        int width = Mathf.CeilToInt(maxPos.x - minPos.x);
        int height = Mathf.CeilToInt(maxPos.y - minPos.y);

        if (width <= 0 || height <= 0) return null;

        // --- Step B: Create the Texture ---
        Texture2D finalTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // Fill with transparent pixels first
        Color[] clearColors = new Color[width * height];
        finalTexture.SetPixels(clearColors);

        // --- Step C: Draw each image onto the texture ---
        foreach (Image img in images)
        {
            if (img.sprite == null) continue;

            // Important: We must check if the sprite texture is readable
            if (!img.sprite.texture.isReadable)
            {
                Debug.LogError($"Texture '{img.sprite.texture.name}' is not Readable. Go to Import Settings -> Advanced -> Check 'Read/Write Enabled'");
                continue;
            }

            Texture2D sourceTex = img.sprite.texture;

            // Calculate where this specific image starts relative to the Group's bottom-left (minPos)
            Vector3[] corners = new Vector3[4];
            img.rectTransform.GetWorldCorners(corners);
            Vector3 imageBottomLeft = corners[0]; // World space bottom-left of this image

            int startX = Mathf.RoundToInt(imageBottomLeft.x - minPos.x);
            int startY = Mathf.RoundToInt(imageBottomLeft.y - minPos.y);

            // Get original pixels
            Color[] srcPixels = sourceTex.GetPixels();

            // Loop through pixels to mix them (Alpha Blending)
            for (int y = 0; y < sourceTex.height; y++)
            {
                for (int x = 0; x < sourceTex.width; x++)
                {
                    int finalX = startX + x;
                    int finalY = startY + y;

                    // Safety check to ensure we don't write outside the texture bounds
                    if (finalX >= 0 && finalX < width && finalY >= 0 && finalY < height)
                    {
                        Color newColor = srcPixels[y * sourceTex.width + x];

                        // Only draw if the new pixel has some visibility
                        if (newColor.a > 0)
                        {
                            Color oldColor = finalTexture.GetPixel(finalX, finalY);
                            // Simple Alpha Blending: Lerp based on new alpha
                            Color blendedColor = Color.Lerp(oldColor, newColor, newColor.a);
                            blendedColor.a = Mathf.Max(oldColor.a, newColor.a); // Keep max alpha

                            finalTexture.SetPixel(finalX, finalY, blendedColor);
                        }
                    }
                }
            }
        }

        finalTexture.Apply();

        // --- Step D: Create Sprite ---
        return Sprite.Create(finalTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }
}