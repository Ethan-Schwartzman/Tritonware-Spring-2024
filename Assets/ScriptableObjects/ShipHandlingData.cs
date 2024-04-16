using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipHandlingData", menuName = "ScriptableObjects/ShipHandlingData")]
public class ShipHandlingData: ScriptableObject
{
    [SerializeField] public float torqueMultiplier = 2000; // * how fast you rotate
    [SerializeField] public float initialMaxTorqueMultiplier = 2;
    [SerializeField] public float initialTorqueBoostAngSpeedThreshold = 80;
    [SerializeField] public float maxAngSpeed = 250; // * the speed limit of rotation
    [SerializeField] public float angularDamping = 500; // * how quickly rotation stops when not actively applying torque
    [SerializeField] public float angularDampingVelocityThreshold = 120;
    [SerializeField] public float holdRotateMultiplier = 2;
    [SerializeField] public float initialRotatePower = 0.2f;
    [SerializeField] public float liftMultiplier = 100; // * how much lift (force that corrects your velocity to align to your facing direction) applies
    [SerializeField] public float topSpeed = 30; // * top speed through thrust only
    [SerializeField] public float excessSpeedDrag = 100f; // * how quickly you slow down if you go past the top speed
    [SerializeField] public float aoaDrag = 50; // * how much you slow down when you turn
}
