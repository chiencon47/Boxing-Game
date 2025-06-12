using UnityEngine;

public class SpawnCharacterController : MonoBehaviour
{
    public GameObject characterPrefab;




    public void Spawn1v1(int level)
    {
        SpawnPlayer();
        SpawnTeam(1, team2SpawnPoints, characterPrefab, 2, level);
    }

    public void Spawn1vN(int level)
    {
        SpawnPlayer();
        SpawnTeam(level, team2SpawnPoints, characterPrefab, 2);
    }

    public void SpawnNvN(int level)
    {
        SpawnPlayer();
        SpawnTeam(level - 1, team1SpawnPoints, characterPrefab, 1);
        SpawnTeam(level, team2SpawnPoints, characterPrefab, 2, level);
    }

    private void SpawnTeam(int count, Transform[] spawnPoints, GameObject prefab, int team, int characterLevel = 1)
    {
        for (int i = 0; i < count && i < spawnPoints.Length; i++)
        {
            var bot = Instantiate(prefab, spawnPoints[i].position, spawnPoints[i].rotation).AddComponent<BotCharacter>();
            bot.SetTeam(team);
            bot.SetLevel(characterLevel);
        }
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(characterPrefab).AddComponent<PlayerCharacter>();
        player.SetTeam(1);

    }
}
