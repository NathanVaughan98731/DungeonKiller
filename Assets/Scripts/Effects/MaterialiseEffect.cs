using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialiseEffect : MonoBehaviour
{
    // Materialise effect coroutine
    public IEnumerator MaterialiseRoutine(Shader materialiseShader, Color materialiseColor, float materialiseTime, SpriteRenderer[] spriteRendererArray, Material normalMaterial)
    {
        Material materialiseMaterial = new Material(materialiseShader);

        materialiseMaterial.SetColor("_EmissionColor", materialiseColor);

        // Set the materialise material in sprite renderers
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = materialiseMaterial;
        }

        float dissolveAmount = 0f;

        // Materialise enemy
        while (dissolveAmount < 1f) {
            dissolveAmount += Time.deltaTime / materialiseTime;

            materialiseMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        }

        // Set standard material in sprite renderers
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = normalMaterial;
        }
    }
}
