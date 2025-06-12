using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public int level = 1;

    public float baseMoveSpeed = 1.5f;
    public int baseHP = 100;
    public int baseDamage = 10;

    [HideInInspector] public int currentHP;

    private const float percentPerLevel = 0.1f; 

    public int MaxHP => Mathf.RoundToInt(baseHP + baseHP * percentPerLevel * (level - 1));
    public int AttackDamage => Mathf.RoundToInt(baseDamage + baseDamage * percentPerLevel * (level - 1));
    public float MoveSpeed => baseMoveSpeed;

    public void InitStats()
    {
        currentHP = MaxHP;
    }

    public bool IsDead => currentHP <= 0;

    public void ResetHP()
    {
        currentHP = MaxHP;
    }
}
