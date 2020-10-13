using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Lets keep this here for a future, but randomizing levels in this game is not something useful...
public class RandomizeLevelBlocks : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    private float minX = 1f;
    private float maxX = 15f;
    private float minY = 5f;
    private float maxY = 10f;

    Dictionary<float, Dictionary<float, bool>> blockMatrix = new Dictionary<float, Dictionary<float, bool>>();

    private float blockSize = 1f;
    //can fit 14*2 in X and 5*2 in Y

    public void CreateNewLevel()
    {
        var blocksPerX = (maxX - minX) / blockSize + 1;
        var blocksPerY = (maxY - minY) / blockSize + 1;

        var maxBlockNumber = blocksPerX * blocksPerY;

        var levelObject = FindObjectOfType<Level>();

        int currentLevel = levelObject.CurrentLevel;
        int maxLevel = levelObject.MaxLevel;

        var screenPercentInBlocks = (float)(currentLevel / maxLevel) * 100;
        var blocksOnLevel = screenPercentInBlocks;

        int count = 0;
        if (levelObject != null && blockPrefab != null)
        {
            int minBlocks = levelObject.CurrentLevel;
            for (float ii = minX; ii <= maxX; ii += blockSize)
            {
                for (float jj = minY; jj <= maxY; jj += blockSize)
                {
                    var newBlock = Instantiate(blockPrefab, new Vector3(ii, jj, 0.0f), blockPrefab.transform.rotation);
                    if (jj == 8f)
                    {
                        newBlock.GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else
                    {
                        newBlock.tag = "GameBlock";
                        var blockClass = newBlock.gameObject.GetComponent<Block>();
                        blockClass.blockSprites = new List<Sprite>() { blockClass.blockSprites.First() };
                    }
                    count++;
                }
            }
        }

        Debug.Log(count);
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
