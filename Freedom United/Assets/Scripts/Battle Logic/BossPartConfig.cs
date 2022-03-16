
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class BossPartConfig
{
    [SerializeField] private BossPartType partType;
    [SerializeField] private float baseDurability;
    [SerializeField] private bool isCore;
    [SerializeField] private bool rotatesWithBody;

    [SerializeField] private Vector2Int position;
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private float baseAttack;
    [SerializeField] private float baseDefense;
    [SerializeField] private int attackSpeed;
    [SerializeField] private int defenseSpeed;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int rotateSpeed;

    [SerializeField] private List<SetOfPositions> areasOfEffect;
    [SerializeField] private List<SetOfPositions> shapesOfAttack;
    [SerializeField] private BossDefenseType bossDefenseType;
    [SerializeField] private List<BossPartType> defendedParts;

    [SerializeField] private float normalSuccessChance;
    [SerializeField] private float criticalSuccessChance;
    [SerializeField] private float criticalFailureChance;

    public BossPartType PartType { get { return partType; } }
    public float BaseDurability { get { return baseDurability; } }
    public float BaseAttack { get { return baseAttack; } }
    public float BaseDefense { get { return baseDefense; } }
    public bool IsCore { get { return isCore; } }
    public bool RotatesWithBody { get { return rotatesWithBody; } }
    public Vector2Int InitialPosition { get { return position; } }
    public Vector2Int Dimensions { get { return dimensions; } }
    public List<SetOfPositions> AreasOfEffect { get { return areasOfEffect; } }
    public List<SetOfPositions> ShapesOfAtttack { get { return shapesOfAttack; } }
    public int AttackSpeed { get { return attackSpeed; } }
    public int DefenseSpeed { get { return defenseSpeed; } }
    public int MoveSpeed { get { return moveSpeed; } }
    public int RotateSpeed { get { return rotateSpeed; } }
    public BossDefenseType BossDefenseType { get { return bossDefenseType; } }
    public List<BossPartType> DefendedParts { get { return defendedParts; } }
    public float NormalSuccessChance { get { return normalSuccessChance; } }
    public float CriticalSuccessChance { get { return criticalFailureChance; } }
    public float CriticalFailureChance { get { return criticalFailureChance; } }

    public void FillPositions()
    {
        foreach (SetOfPositions set in areasOfEffect)
            set.FillPositions();
        foreach (SetOfPositions set in shapesOfAttack)
            set.FillPositions();
    }
}