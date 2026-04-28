using System.Collections;
using System.IO;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    // 스폰 포인트 0~4: 화면 상단 좌→우 순서
    private static readonly float[] spawnX = { -2.0f, -1.0f, 0f, 1.0f, 2.0f };

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        TextAsset csv = Resources.Load<TextAsset>("StageData");
        if (csv == null)
        {
            Debug.LogError("Resources/StageData.csv 를 찾을 수 없습니다.");
            yield break;
        }

        using (StringReader reader = new StringReader(csv.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] tokens = line.Split(',');
                if (tokens.Length < 3) continue;

                float delay = float.Parse(tokens[0]);
                string type  = tokens[1].Trim().ToUpper();
                int    point = int.Parse(tokens[2].Trim());

                yield return new WaitForSeconds(delay);

                SpawnEnemy(type, point);
            }
        }
    }

    void SpawnEnemy(string type, int point)
    {
        int prefabIndex = TypeToIndex(type);
        GameObject enemyGo = ObjectPoolManager.Instance.GetEnemy(prefabIndex);
        if (enemyGo == null) return;

        float x = (point >= 0 && point < spawnX.Length) ? spawnX[point] : 0f;
        enemyGo.transform.position = new Vector3(x, 4.5f, 0);
        enemyGo.SetActive(true);
    }

    // CSV 타입 문자 → enemyPrefabs 배열 인덱스
    // Inspector에서 enemyPrefabs 배열을 [0]=소형, [1]=중형, [2]=대형 순으로 할당
    int TypeToIndex(string type)
    {
        switch (type)
        {
            case "S": return 0; // 소형
            case "A": return 1; // 중형
            case "B": return 2; // 대형
            default:  return 0;
        }
    }
}
