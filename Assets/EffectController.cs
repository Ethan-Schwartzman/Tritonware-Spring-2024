using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static Color damageColor = Color.red;

    public static IEnumerator DamageEffect(SpriteRenderer sr)
    {
        if (sr.color != damageColor)
        {
            Color originalColor = sr.color;
            sr.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            if (sr != null) sr.color = originalColor;
        }
    }
}
