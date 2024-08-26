using System;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using NaughtyAttributes;

public class LootLockerManager : MonoBehaviour
{
    void Start()
    {
        StartGuestSession();
    }

    private void StartGuestSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Guest session started");
            }
            else
            {
                Debug.Log("Failed to start guest session");
            }
        });
    }

    [Button]
    private void GetPlayerInventory()
    {
        LootLockerSDKManager.GetInventory(response =>
        {
            foreach (LootLockerInventory item in response.inventory)
            {
                Debug.Log("--------------------");
                Debug.Log("Item: " + item);
                Debug.Log("Item Instance ID: " + item.instance_id);
                Debug.Log("Item Asset Name: " + item.asset.name);
                Debug.Log("Item Asset ID: " + item.asset.id);
                foreach (var VARIABLE in item.asset.storage)
                {
                    Debug.Log(VARIABLE.id + " = " + VARIABLE.key +":" + VARIABLE.value);
                }
                Debug.Log("--------------------");
            }
        });
    }

    [Button]
    private void GetAssetStorage()
    {
        LootLockerSDKManager.GetAllKeyValuePairsToAnInstance(assetInstanceID, response =>
        {
            foreach (var VARIABLE in response.storage)
            {
                Debug.Log(VARIABLE.id + " = " + VARIABLE.key +":" + VARIABLE.value);
            }
        });
    }

    [SerializeField] private int assetInstanceID;
    [SerializeField] private float damage;
    [Button]
    private void SetDamageData()
    {
        LootLockerSDKManager.UpdateKeyValuePairForAssetInstances(assetInstanceID, "damage", damage.ToString(), response =>
        {
            Debug.Log("response.text: " + response.text);
            if (response.success)
            {
                Debug.Log(String.Format("Successfully updated asset {0}'s damage value to {1}", itemID, damage));
            }
            else
            {
                Debug.Log("Error updating asset.");
                Debug.Log(response.errorData.message);
            }
        });
    }

    [SerializeField] private int itemID;
    [Button]
    private void GrantItemToPlayer()
    {
        LootLockerSDKManager.GrantAssetToPlayerInventory(itemID, response =>
        {
            Debug.Log("Granted asset " + response.asset_id + " to player!");
        });
    }

    [Button]
    private void GetAllItems()
    {
        LootLockerSDKManager.GetAssetListWithCount(-1, response =>
        {
            foreach (LootLockerCommonAsset asset in response.assets)
            {
                Debug.Log(asset.name);
            }
        });
    }
}