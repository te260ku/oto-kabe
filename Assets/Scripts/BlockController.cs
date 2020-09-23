using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class InputJson
{
    public string name;
    public int maxBlock;
    public int BPM;
    public int offset;
    public Note[] notes;
}

[System.Serializable]
public class Note
{
    public int LPB;
    public int num;
    public int block;
    public int type;
    public int notes;
}

public class BlockController : MonoBehaviour
{
    private Block[] blocks = new Block[9];
            public float span = 3f;
    private float currentTime = 0f;
    private string dataPath;

    public struct NotesData {
        public float startTime;
        public int type;
    }

    public NotesData[] notesData;
    private int notesCount;
    private int previousBlockNum = -1;
    private int nextBlockNum;
    private List<int> blockNums = new List<int>();
    

    
    void Start()
    {
        GameObject[] blockObjects = new GameObject[9];
        int count = 0;
        foreach (Transform child in this.transform){
            blocks[count] = child.gameObject.GetComponent<Block>();
            count++;
        }

        for (int i = 0; i < 9; i++) {
            blockNums.Add(i);
        }
 
        notesData = new NotesData[100];
        LoadNotes();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SetNextBlock();
        }  

        currentTime += Time.deltaTime;
        
        if(currentTime > notesData[notesCount].startTime){
            SetNextBlock();
            notesCount++;
        }
        
    }

    private void SetNextBlock() {

        nextBlockNum = Random.Range(0, 9);
        previousBlockNum = nextBlockNum;

        if (!(blockNums.Count > 0)) {
            for (int i = 0; i < 9; i++) {
            blockNums.Add(i);
            }
        }

        if (blockNums.Count > 0) {
 
            int index = Random.Range(0, blockNums.Count);
            nextBlockNum = blockNums[index];
            blockNums.RemoveAt(index);
        } 

        Block target = blocks[nextBlockNum];
        target.ActivateBlock();
        

    }

    private void LoadNotes() {

        var info = new FileInfo(Application.dataPath + "/" + "bgm1.json");
        var reader = new StreamReader (info.OpenRead ());
        var json = reader.ReadToEnd ();


        InputJson inputJson = JsonUtility.FromJson<InputJson>(json);

        float barToTime = 60f/(inputJson.BPM*inputJson.notes[0].LPB);
        

        for (int i=0; i<inputJson.notes.Length; i++) {

            notesData[i].startTime = inputJson.notes[i].num*barToTime;
        }
    }
}
