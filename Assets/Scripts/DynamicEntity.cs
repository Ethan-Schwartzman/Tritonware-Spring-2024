using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicEntity : MonoBehaviour, IDynamicEntity
{
    public abstract Vector2 GetVelocity();
    public abstract Vector2 GetFacingDirection();
}
