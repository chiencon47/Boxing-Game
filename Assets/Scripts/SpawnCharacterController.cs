using UnityEngine;

public class SpawnCharacterController : MonoBehaviour
{
    public GameObject characterPrefab;

    public void Spawn1v1(int level)
    {
        SpawnPlayer();
        SpawnTeam(1, 2, level);
    }

    public void Spawn1vN(int level)
    {
        SpawnPlayer();
        SpawnTeam(level, 2);
    }

    public void SpawnNvN(int level)
    {
        SpawnPlayer();
        SpawnTeam(level - 1, 1);
        SpawnTeam(level, level);
    }

    private void SpawnTeam(int count, int team, int characterLevel = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var bot = Instantiate(characterPrefab).AddComponent<BotCharacter>();
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
