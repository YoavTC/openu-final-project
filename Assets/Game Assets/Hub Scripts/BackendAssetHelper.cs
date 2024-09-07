using System.Linq;
using AYellowpaper.SerializedCollections;

public class BackendAssetHelper
{
    private static SerializedDictionary<int, TowerSettings> towerSettingsIDMap = new SerializedDictionary<int, TowerSettings>();
    
    public static TowerSettings GetTowerSettingsFromInstanceID(int instanceID)
    {
        TowerSettings matchingTower = null;
        int[] instanceIDKeys = towerSettingsIDMap.Keys.ToArray();
        for (int i = 0; i < instanceIDKeys.Length; i++)
        {
            if (instanceIDKeys[i] == instanceID)
            {
                matchingTower = towerSettingsIDMap[instanceIDKeys[i]];
            }
        }

        return matchingTower;
    }
}
