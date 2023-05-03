
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLayoutSpawner : MonoBehaviour
{
    public StagePieceList stageLayouts;
    public GameObject startPiece;
    // Start is called before the first frame update
    void Start()
    {
        CreateLayout();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateLayout()
    {
        LevelBuilder lastLevelPiece, currentLevelPiece;
        GameObject levelObject = null;
        //start
        lastLevelPiece = transform.GetChild(0).gameObject.GetComponent<LevelBuilder>();

        

        
        for (int i = 0; i < 4; i++)
        {

            switch (i)
            {
                case 0:
                    levelObject = Instantiate(stageLayouts.part1Layouts[(int)(Random.value * stageLayouts.part1Layouts.Count)], transform.position, Quaternion.identity);
                    break;
                case 1:
                    levelObject = Instantiate(stageLayouts.part2Layouts[(int)(Random.value * stageLayouts.part2Layouts.Count)], transform.position, Quaternion.identity);
                    break;
                case 2:
                    levelObject = Instantiate(stageLayouts.part3Layouts[(int)(Random.value * stageLayouts.part3Layouts.Count)], transform.position, Quaternion.identity);
                    break;
                case 3:
                    levelObject = Instantiate(stageLayouts.part4Layouts[(int)(Random.value * stageLayouts.part4Layouts.Count)], transform.position, Quaternion.identity);
                    break;
            }

            SceneManager.MoveGameObjectToScene(levelObject, gameObject.scene);
            currentLevelPiece = levelObject.GetComponent<LevelBuilder>();
            //place the piece in the correct position
            currentLevelPiece.transform.position = lastLevelPiece.GetLevelEnd() + Vector3.forward * 2;



            lastLevelPiece = currentLevelPiece;
        }


        //end
        levelObject = Instantiate(stageLayouts.endPiece, transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(levelObject, gameObject.scene);
        currentLevelPiece = levelObject.GetComponent<LevelBuilder>();
        //place the piece in the correct position
        currentLevelPiece.transform.position = lastLevelPiece.GetLevelEnd() + Vector3.forward * 2;
    }
}
