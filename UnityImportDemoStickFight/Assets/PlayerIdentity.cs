using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    private static List<PlayerIdentity> players = new List<PlayerIdentity>();
    public int doesntmatter = 0;

    void Start()
    {
        players.Add(this);
        doesntmatter = 1;
    }

    private void OnDestroy()
    {
        players.Remove(this);
    }

    public static List<PlayerIdentity> GetPlayers() => players;
}
