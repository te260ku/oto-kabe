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
    // private GameObject[,] blocks = new GameObject[3,3];
    private Block[] blocks = new Block[9];
            public float span = 3f;
    private float currentTime = 0f;
    private string dataPath;

    [System.Serializable]
    public struct NotesData
    {
        // "LPB":4,"num":586,"block":0,"type":1,"notes":[]
        public int LPB;
        public int num;
        public int block;
        public int type;
        public int notes;
        public void Dump() {
        Debug.Log("time : " + type);
    }
    }

    
    void Start()
    {
        GameObject[] blockObjects = new GameObject[9];
        int count = 0;
        foreach (Transform child in this.transform){
            blocks[count] = child.gameObject.GetComponent<Block>();
            // blocks[count] = blockObjects[count].GetComponent<Block>();
            
            count++;
        }


        LoadNotes();

        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SetNextBlock();
        }  

        currentTime += Time.deltaTime;
        if(currentTime > span){
            SetNextBlock();
            currentTime = 0f;
        }
        
    }

    private void SetNextBlock() {
        int nextBlockNum = Random.Range(0, 9);
        Block target = blocks[nextBlockNum];
        target.ActivateBlock();
        

    }

    private void LoadNotes() {

        var info = new FileInfo(Application.dataPath + "/" + "bgm1.json");
        var reader = new StreamReader (info.OpenRead ());
        var json = reader.ReadToEnd ();


        InputJson inputJson = JsonUtility.FromJson<InputJson>(json);
        Debug.Log(inputJson.notes[0].num);  // 111
        Debug.Log(inputJson.notes[1].num);  // 444
    }
}
