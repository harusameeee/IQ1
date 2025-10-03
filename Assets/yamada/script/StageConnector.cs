using UnityEngine;

public class StageConnector : MonoBehaviour
{
    [Header("並べるステージモジュールPrefab（順番）")]
    public GameObject[] modulePrefabs; // モジュールPrefab配列

    void Start()
    {
        Transform prevEndPoint = null;

        for (int i = 0; i < modulePrefabs.Length; i++)
        {
            GameObject prefab = modulePrefabs[i];
            GameObject module = Instantiate(prefab);

            // StartPointとEndPointを探す
            Transform startPoint = module.transform.Find("StartPoint");
            Transform endPoint = module.transform.Find("EndPoint");

            if (startPoint == null || endPoint == null)
            {
                Debug.LogError($"{module.name} に StartPoint または EndPoint がありません");
                Destroy(module);
                continue;
            }

            if (i == 0)
            {
                // 最初のモジュールはこのGameObjectの位置(StartPoint基準)に配置
                Vector3 offset = module.transform.position - startPoint.position;
                module.transform.position = this.transform.position + offset;
            }
            else
            {
                // 2個目以降は「前のEndPoint」と「今回のStartPoint」を一致させる
                Vector3 offset = module.transform.position - startPoint.position;
                module.transform.position = prevEndPoint.position + offset;
            }

            // 次の連結用にEndPointを保存
            prevEndPoint = endPoint;

            // 生成物を親としてまとめたい場合
            module.transform.parent = this.transform;
        }
    }
}