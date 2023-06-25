using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageService 
{
    public bool IsRuningAnimation { get; set; }

    private readonly List<int> _recivingDamageFrom = new List<int>();

    public IEnumerator TakeDamageAnimation(SpriteRenderer spriteRenderer)
    {
        IsRuningAnimation = true;

        Color color = spriteRenderer.color;

        int howManyTimesItWillBlink = 4;
        float blinkDuration = 0.5f;

        for (int i = 0; i < howManyTimesItWillBlink; i++)
        {
            color.a = 0;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(blinkDuration / (howManyTimesItWillBlink * 2));

            color.a = 1;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(blinkDuration / (howManyTimesItWillBlink * 2));

        }
        color.a = 1;
        spriteRenderer.color = color;

        IsRuningAnimation = false;
    }

    public bool CanReceiveDamageFrom(int instance)
    {
        return !_recivingDamageFrom.Any(e => e == instance);
    }

    public IEnumerator ManageDamageEntry(int instance)
    {
        _recivingDamageFrom.Add(instance);
        yield return new WaitForSeconds(0.5f);
        _recivingDamageFrom.Remove(instance);
    }


    public float CalculateDamageEntry(float incomingDamage)
    {
        return incomingDamage;
    }
}
