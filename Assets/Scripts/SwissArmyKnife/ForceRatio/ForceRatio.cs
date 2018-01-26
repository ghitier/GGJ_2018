using UnityEngine;
using System.Collections;

// HOW-TO:
// * Put this behaviour on the camera of each scene
// * ScreenSpace - Camera only !
// * The camera must be named "Main Camera" to match with fullscreen WebGL Template
public class ForceRatio : MonoBehaviour {

    // Based on http://gamedesigntheory.blogspot.fr/2010/09/controlling-aspect-ratio-in-unity.html --
    private Vector2 ratioMin = new Vector2(4.0f, 3.0f);
    private Vector2 ratioMax = new Vector2(16.0f, 9.0f);

    void Start () {
        UpdateViewport();
    }

    public void UpdateViewport()
    {
        // Determine the target and the game window's current aspects ratios --
        float targetAspectMin = ratioMin.x / ratioMin.y;
        float targetAspectMax = ratioMax.x / ratioMax.y;

        // Reverse if Min > Max
        if(targetAspectMin > targetAspectMax)
        {
            float temp = targetAspectMin;
            targetAspectMin = targetAspectMax;
            targetAspectMax = temp;
        }

        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Current viewport height should be scaled by this amount
        float scaleHeightMin = windowAspect / targetAspectMin;
        float scaleHeightMax = windowAspect / targetAspectMax;

        // Obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();
        if (!camera)
        {
            Debug.Log("ForceRatio: Missing camera in " + name);
            return;
        }
        
        // if scaled height is less than current height, add letterbox
        if (scaleHeightMin < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeightMin;
            rect.x = 0;
            rect.y = (1.0f - scaleHeightMin) / 2.0f;

            camera.rect = rect;
        }
        else if(scaleHeightMax > 1.0f) // add pillarbox
        {
            float scalewidth = 1.0f / scaleHeightMax;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
