using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject levelObject;
        //start
        lastLevelPiece = transform.GetChild(0).gameObject.GetComponent<LevelBuilder>();

        


        for (int i = 0; i < 3; i++)
        {
            levelObject = Instantiate(stageLayouts.stageLayouts[0], transform.position, Quaternion.identity);
            currentLevelPiece = levelObject.GetComponent<LevelBuilder>();
            //place the piece in the correct position
            currentLevelPiece.transform.position = lastLevelPiece.GetLevelEnd() + Vector3.forward * 2;



            lastLevelPiece = currentLevelPiece;
        }


        //end
        levelObject = Instantiate(stageLayouts.endPiece, transform.position, Quaternion.identity);
        currentLevelPiece = levelObject.GetComponent<LevelBuilder>();
        //place the piece in the correct position
        currentLevelPiece.transform.position = lastLevelPiece.GetLevelEnd() + Vector3.forward * 2;
    }
}
