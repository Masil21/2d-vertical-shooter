using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public SpriteRenderer[] backgrounds;
    public float scrollSpeed = 0.1f;

    private float height;
    private Vector3 startCenterPos;

    void Start()
    {
        if (backgrounds == null || backgrounds.Length < 3)
        {
            Debug.LogWarning("backgrounds 배열에 SpriteRenderer 3개를 넣어주세요.");
            return;
        }

        startCenterPos = backgrounds[1].transform.position;
        height = backgrounds[1].bounds.size.y;

        backgrounds[0].transform.position = new Vector3(
            startCenterPos.x,
            startCenterPos.y + height,
            startCenterPos.z
        );

        backgrounds[1].transform.position = new Vector3(
            startCenterPos.x,
            startCenterPos.y,
            startCenterPos.z
        );

        backgrounds[2].transform.position = new Vector3(
            startCenterPos.x,
            startCenterPos.y - height,
            startCenterPos.z
        );
    }

    void Update()
    {
        if (backgrounds == null || backgrounds.Length < 3)
        {
            return;
        }

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        SpriteRenderer bottomBg = backgrounds[0];
        SpriteRenderer topBg = backgrounds[0];

        for (int i = 1; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].transform.position.y < bottomBg.transform.position.y)
            {
                bottomBg = backgrounds[i];
            }

            if (backgrounds[i].transform.position.y > topBg.transform.position.y)
            {
                topBg = backgrounds[i];
            }
        }

        float recycleLineY = startCenterPos.y - (height * 1.5f);

        if (bottomBg.transform.position.y <= recycleLineY)
        {
            bottomBg.transform.position = new Vector3(
                startCenterPos.x,
                topBg.transform.position.y + height,
                startCenterPos.z
            );
        }
    }
}