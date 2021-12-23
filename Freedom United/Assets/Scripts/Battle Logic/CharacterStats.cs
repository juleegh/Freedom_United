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

    [SerializeField] private int baseAttack;

    [SerializeField] private float normalSuccessChance;
    [SerializeField] private float criticalSuccessChance;
    [SerializeField] private float criticalFailureChance;

    [SerializeField] private int baseDefense;

    [SerializeField] private int attackSpeed;
    [SerializeField] private int defenseSpeed;
    [SerializeField] private int recklessChangeSpeed;
    [SerializeField] private int safeChangeSpeed;

    public AttackRange AttackRange { get { return attackRange; } }
    public AttackType AttackType { get { return attackType; } }

    public float BaseHealth { get { return baseHealth; } }

    public int AttackSpeed { get { return attackSpeed; } }
    public int DefenseSpeed { get { return defenseSpeed; } }
    public int RecklessChangeSpeed { get { return recklessChangeSpeed; } }
    public int SafeChangeSpeed { get { return safeChangeSpeed; } }
    public int BaseWillPower { get { return baseWillPower; } }

    public int BaseAttack { get { return baseAttack; } }
    public int BaseDefense { get { return baseDefense; } }

    public float NormalSuccessChance { get { return normalSuccessChance; } }
    public float CriticalSuccessChance { get { return criticalFailureChance; } }
    public float CriticalFailureChance { get { return criticalFailureChance; } }
}
