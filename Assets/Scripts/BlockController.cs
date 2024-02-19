using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Pool;
using TMPro;
using UnityEditorInternal;

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
    private Block[] blocks_old = new Block[9];
            public float span = 3f;
    private float elapsedTime = 0f;
    private string dataPath;

    public struct NotesData {
        public float startTime;
        public int type;
    }

    public NotesData[] notesData;
    private int currentBlockNum;
    private int previousActiveBlockNum = 0;
    private int currentActiveBlockNum;
    private List<int> blockNums = new List<int>();

    ObjectPool<GameObject> pool;
    [SerializeField] GameObject blockPrefab;
    List<Block> blocks = new List<Block>();
    [SerializeField] GameObject gridParentObj;
    [SerializeField] int gridWidth = 3;
    [SerializeField] int gridHeight = 3;
    [SerializeField] float spacing = 1f;
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] AudioClip bgm;
    [SerializeField] float musicVolume = 0.02f;
    int totalGridCount;
    bool isPlaying;
    int score;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] AudioSource audiosourceSE;
    [SerializeField] AudioClip onHitSound;
    

    
    void Start()
    {
        // GameObject[] blockObjects = new GameObject[9];
        // int count = 0;
        // foreach (Transform child in this.transform){
        //     blocks_old[count] = child.gameObject.GetComponent<Block>();
        //     count++;
        // }

        // for (int i = 0; i < 9; i++) {
        //     blockNums.Add(i);
        // }
 
        notesData = new NotesData[100];
        LoadNotes();

        // InitializeBlocks();

        GenerateGrid();

        totalGridCount = gridHeight * gridWidth;
    }



    void GenerateGrid() 
    {
        var cubeSize = blockPrefab.transform.localScale;
        // グリッドの中心からのオフセットを計算
        Vector3 offset = new Vector3(
            (gridWidth - 1) * cubeSize.x / 2f,
            0f,
            (gridHeight - 1) * cubeSize.z / 2f
        );
        int blockCount = 0;

        // グリッドを作成
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Cubeの位置を計算
                Vector3 spawnPosition = new Vector3(x * cubeSize.x, 0f, z * cubeSize.z) - offset;

                // Cubeを作成して配置
                GameObject blockObj = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
                blockObj.transform.localScale = cubeSize;
                blockObj.transform.SetParent(gridParentObj.transform);
                var b = blockObj.GetComponent<Block>();
                b.Initialize(blockCount);
                
                blocks.Add(b);

                blockCount ++;
            }
        }
    }

    public void OnHit(int id) {
        if (blocks[id].state == Block.STATE.IDLE) return;
        blocks[id].OnHitHand();
        if (id == currentActiveBlockNum) {
            audiosourceSE.PlayOneShot(onHitSound);
            score ++;
            scoreText.text = score.ToString();
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartGame();
        }  

        // デバッグ用
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                OnHit(i);
            }
        }


        if (isPlaying) {
            elapsedTime += Time.deltaTime;
        
            if (elapsedTime > notesData[currentBlockNum].startTime) {
                SetNextBlock();
                currentBlockNum ++;
            }
        }
    }

    void StartGame() {
        audioSourceMusic.clip = bgm;
        audioSourceMusic.volume = musicVolume;
        audioSourceMusic.Play();
        isPlaying = true;
    }

    private void SetNextBlock() {

        blocks[previousActiveBlockNum].DeactivateBlock();

        currentActiveBlockNum = Random.Range(0, totalGridCount);
        previousActiveBlockNum = currentActiveBlockNum;

        
        
        blocks[currentActiveBlockNum].ActivateBlock();
        

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
