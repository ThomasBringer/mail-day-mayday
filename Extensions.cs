using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class Extensions
{
    public static Collider2D FindClosestCollider2D(this Collider2D[] colliders, Vector3 pos)
    {
        Collider2D closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector3.Distance(pos, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        return closestCollider;
    }

    public static Vector2 Round(this Vector2 v) => new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    public static Vector2 SnapToSemi(this Vector2 v) => (v + .5f * Vector2.one).Round() - .5f * Vector2.one;

    public static bool Check(this Vector2 v, LayerMask layer) => Physics2D.OverlapCircle(v, .1f, layer);
    public static Collider2D CheckGet(this Vector2 v, LayerMask layer) => Physics2D.OverlapCircle(v, .1f, layer);

    public static Vector2 SnapToCardinal(this Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return Mathf.Sign(v.x) * Vector2.right;
        else
            return Mathf.Sign(v.y) * Vector2.up;
    }

    // public static void FadeAllRenderersInChildren(this GameObject go, MonoBehaviour m, bool show = false, float duration = 1)
    // {
    //     Renderer[] childRenderers = go.GetComponentsInChildren<Renderer>();

    //     if (childRenderers.Length == 0) return;

    //     float targetAlpha = show ? 1 : 0;

    //     foreach (Renderer childRenderer in childRenderers)
    //     {
    //         m.StartCoroutine(FadeRenderer(childRenderer, targetAlpha, duration));
    //     }
    // }

    // static IEnumerator FadeRenderer(Renderer renderer, float targetAlpha, float duration)
    // {
    //     Color originalColor = renderer.material.color;
    //     float originalAlpha = originalColor.a;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < duration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsedTime / duration);
    //         float alpha = Mathf.Lerp(originalAlpha, targetAlpha, t);
    //         Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    //         renderer.material.color = newColor;
    //         yield return null;
    //     }

    //     Color finalColor = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    //     renderer.material.color = finalColor;
    // }


    // Method to fade components in or out based on 'show' boolean input
    public static void FadeComponents(this GameObject go, MonoBehaviour m, bool show, float fadeDuration = 1)
    {
        List<Component> childComponents = new List<Component>();
        // Get all child components of type Renderer or TextMeshProUGUI
        go.GetComponentsInChildren(childComponents);

        // Set starting and ending alpha values based on 'show' input
        float startAlpha = show ? 0.0f : 1.0f;
        float endAlpha = show ? 1.0f : 0.0f;

        // Loop through all child components and fade them
        foreach (Component component in childComponents)
        {
            if (component is Renderer)
            {
                Renderer renderer = component as Renderer;
                m.StartCoroutine(FadeRenderer(renderer, startAlpha, endAlpha));
            }
            else if (component is TextMeshProUGUI)
            {
                TextMeshProUGUI textMeshPro = component as TextMeshProUGUI;
                m.StartCoroutine(FadeTextMeshPro(textMeshPro, startAlpha, endAlpha, fadeDuration));
            }
        }
    }

    // Coroutine to fade a Renderer component
    static IEnumerator FadeRenderer(Renderer renderer, float startAlpha, float endAlpha, float fadeDuration = 1)
    {
        // Get the material(s) for the Renderer
        Material[] materials = renderer.materials;

        // Loop through all materials and fade their alpha values
        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = startAlpha;
            materials[i].color = color;
        }

        // Calculate the amount to change the alpha value per frame
        float deltaAlpha = (endAlpha - startAlpha) / fadeDuration * Time.deltaTime;

        // Loop until the alpha value reaches the target value
        while (Mathf.Abs(renderer.materials[0].color.a - endAlpha) > 0.01f)
        {
            // Update the alpha value for each material
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a += deltaAlpha;
                materials[i].color = color;
            }

            // Wait for the next frame
            yield return null;
        }
    }

    // Coroutine to fade a TextMeshProUGUI component
    static IEnumerator FadeTextMeshPro(TextMeshProUGUI textMeshPro, float startAlpha, float endAlpha, float fadeDuration = 1)
    {
        // Get the color of the TextMeshProUGUI
        Color color = textMeshPro.color;
        color.a = startAlpha;
        textMeshPro.color = color;

        // Calculate the amount to change the alpha value per frame
        float deltaAlpha = (endAlpha - startAlpha) / fadeDuration * Time.deltaTime;

        // Loop until the alpha value reaches the target value
        while (Mathf.Abs(textMeshPro.color.a - endAlpha) > 0.01f)
        {
            // Update the alpha value
            color = textMeshPro.color;
            color.a += deltaAlpha;
            textMeshPro.color = color;

            // Wait for the next frame
            yield return null;
        }
    }
}
