using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private AttackRange attackRange;
    [SerializeField] private AttackType attackType;

    [SerializeField] private float baseHealth;
    [SerializeField] private int baseWillPower;
    [SerializeField] private int baseShieldDurability;

    [SerializeField] private int baseAttack;

    [SerializeField] private float normalSuccessChance;
    [SerializeField] private float criticalSuccessChance;
    [SerializeField] private float criticalFailureChance;

    [SerializeField] private int attackSpeed;
    [SerializeField] private int defenseSpeed;
    [SerializeField] private int recklessChangeSpeed;
    [SerializeField] private int safeChangeSpeed;

    [SerializeField] private int sadWPDelta;
    [SerializeField] private int deathWPDelta;
    [SerializeField] private int happyWPDelta;

    public AttackRange AttackRange { get { return attackRange; } }
    public AttackType AttackType { get { return attackType; } }

    public float BaseHealth { get { return baseHealth; } }

    public int AttackSpeed { get { return attackSpeed; } }
    public int DefenseSpeed { get { return defenseSpeed; } }
    public int RecklessChangeSpeed { get { return recklessChangeSpeed; } }
    public int SafeChangeSpeed { get { return safeChangeSpeed; } }
    public int BaseWillPower { get { return baseWillPower; } }
    public int BaseShieldDurability { get { return baseShieldDurability; } }

    public int BaseAttack { get { return baseAttack; } }

    public float NormalSuccessChance { get { return normalSuccessChance; } }
    public float CriticalSuccessChance { get { return criticalFailureChance; } }
    public float CriticalFailureChance { get { return criticalFailureChance; } }

    public int SadWPDelta { get { return -sadWPDelta; } }
    public int DeathWPDelta { get { return -deathWPDelta; } }
    public int HappyWPDelta { get { return happyWPDelta; } }
}
