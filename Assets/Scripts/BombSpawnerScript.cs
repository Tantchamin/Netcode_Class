using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BombSpawnerScript : NetworkBehaviour
{
    public GameObject bombPrefab;
    private List<GameObject> spawnedBomb = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBombServerRpc();
        }

    }

    [ServerRpc]
    void SpawnBombServerRpc()
    {
        Vector3 spawnPos = transform.position + (transform.forward * -1.5f) + (transform.up * 0.8f);
        Quaternion spawnRot = transform.rotation;
        GameObject bomb = Instantiate(bombPrefab, spawnPos, spawnRot);
        spawnedBomb.Add(bomb);
        bomb.GetComponent<BombScript>().bombSpawner = this;
        bomb.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc (RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjectId)
    {
        GameObject toDestroy = FindBombFromNetworkId(networkObjectId);
        if (toDestroy == null) return;
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBomb.Remove(toDestroy);
        Destroy(toDestroy);
    }

    private GameObject FindBombFromNetworkId(ulong networkObjectId)
    {
        foreach(GameObject bomb in spawnedBomb)
        {
            ulong bombId = bomb.GetComponent<NetworkObject>().NetworkObjectId;
            if(bombId == networkObjectId) { return bomb; }
        }
        return null;
    }
}
