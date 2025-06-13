using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    private List<CharacterBase> allCharacters = new List<CharacterBase>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Register(CharacterBase character)
    {
        if (!allCharacters.Contains(character))
        {
            allCharacters.Add(character);
        }
    }

    public void Unregister(CharacterBase character)
    {
        allCharacters.Remove(character);
    }

    public List<CharacterBase> GetAllCharacters() => allCharacters;

    public List<CharacterBase> GetEnemies(int team)
    {
       return allCharacters.Where(c => c.GetTeam() != team).ToList();
    }

    public List<CharacterBase> GetTeamCharacters(int team)
    {
        return allCharacters.Where(c => c.GetTeam() == team).ToList();
    }

    public CharacterBase GetClosestEnemy(Vector3 fromPosition, int team)
    {
        var enemies = GetEnemies(team);
        CharacterBase closest = null;
        float minSqrDist = float.MaxValue;

        foreach (var enemy in enemies)
        {
            float sqrDist = (fromPosition - enemy.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = enemy;
            }
        }

        return closest;
    }
}
