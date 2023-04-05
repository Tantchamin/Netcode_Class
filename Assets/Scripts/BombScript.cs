using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BombScript : NetworkBehaviour
{
    public BombSpawnerScript bombSpawner;
    public GameObject bombEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if(collision.gameObject.tag == "Player")
        {
            ulong networkObkjectId = GetComponent<NetworkObject>().NetworkObjectId;
            SpawnBombEffect();
            bombSpawner.DestroyServerRpc(networkObkjectId);
        }
    }

    private void SpawnBombEffect()
    {
        GameObject bombeffect = Instantiate(bombEffectPrefab, transform.position, Quaternion.identity);
        bombeffect.GetComponent<NetworkObject>().Spawn();
    }

    ParticleSystem ps;
   
}
