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
    public float span = 3f;
    private float elapsedTime = 0f;

    public class NotesData {
        public float startTime;
        public int blockID;
        public int type;
    }

    List<NotesData> notesData = new List<NotesData>();
    private int currentNoteNum;
    private int previousActiveBlockID;
    private int currentActiveBlockID;
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
    [SerializeField] string bgmFileName;
    [SerializeField] GameObject gridCornerMarkerObj;
    [SerializeField] BoneFollower middleBaseBone;
    [SerializeField] BoneFollower middleTipBone;
    [SerializeField] GameObject debugAxis;

    
    void Start()
    {
        totalGridCount = gridHeight * gridWidth;

        GenerateGrid();
        LoadNotes();
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
        if (id == currentActiveBlockID) {
            audiosourceSE.PlayOneShot(onHitSound);
            score ++;
            scoreText.text = score.ToString();
        }
    }

    void AdjustGridPosition() {
        gridParentObj.transform.position = middleBaseBone.gameObject.transform.position;
    }

    void AdjustGridScale() {
        // var centerToCornerDistance = Vector3.Distance(middleBaseBone.transform.position, gridCornerMarkerObj.transform.position);
        // var originalDistance = 0.423f;
        // var scaleMultiply = centerToCornerDistance / originalDistance;
        // gridParentObj.transform.localScale = new Vector3(
        //     gridParentObj.transform.localScale.x * scaleMultiply, 
        //     gridParentObj.transform.localScale.y, 
        //     gridParentObj.transform.localScale.z * scaleMultiply
        // );

        // // Vector3 direction = (gridCornerMarkerObj.transform.position - middleBaseBone.transform.position).normalized;
        // // gridParentObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        // cubeのサイズを設定する
        // float distance = Vector3.Distance(middleBaseBone.transform.position, gridCornerMarkerObj.transform.position);
        // gridParentObj.transform.localScale = new Vector3(distance / 2f * 5f, gridParentObj.transform.localScale.y, distance / 2f * 5f);
    }

    void AlignGridWithTwoPoints() {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            AdjustGridPosition();
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            AlignGridWithTwoPoints();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            AdjustGridScale();
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
        
            if (elapsedTime > notesData[currentNoteNum].startTime) {
                SetNextBlock();
                currentNoteNum ++;
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

        // すべてのブロックを非アクティブにする
        foreach (var block in blocks)
        {
            block.DeactivateBlock();
        }

        if (currentNoteNum < notesData.Count-1) {
            // 次のノーツまでの時間
            float duration = notesData[currentNoteNum+1].startTime - notesData[currentNoteNum].startTime;
            // 次のノーツを準備状態にする
            blocks[notesData[currentNoteNum+1].blockID].ReadyBlock(duration);
        }

        currentActiveBlockID = notesData[currentNoteNum].blockID;
        

        
        // 現在のノーツをアクティブにする
        blocks[currentActiveBlockID].ActivateBlock();
        

    }

    private void LoadNotes() {


        var info = new FileInfo(Application.dataPath + "/Notes/" + bgmFileName + ".json");
        var reader = new StreamReader (info.OpenRead ());
        var json = reader.ReadToEnd ();


        InputJson inputJson = JsonUtility.FromJson<InputJson>(json);

        int notesCount = inputJson.notes.Length;

        float barToTime = 60f/(inputJson.BPM*inputJson.notes[0].LPB);



        var blockIDs = GenerateNonConsecutiveList(notesCount);

        for (int i=0; i<notesCount; i++) {
            

            NotesData note = new NotesData
            {
                startTime = inputJson.notes[i].num * barToTime, 
                blockID = blockIDs[i]
            };
            notesData.Add(note);
        }

    }


    List<int> GenerateNonConsecutiveList(int size)
    {
        List<int> nonConsecutiveList = new List<int>();
        int previousNumber = -1; // 前の数字を保持する変数を初期化

        for (int i = 0; i < size; i++)
        {
            int randomNumber = Random.Range(0, totalGridCount); // 0から9までのランダムな整数を生成
            // 前の数字と異なる場合のみリストに追加
            if (randomNumber != previousNumber)
            {
                nonConsecutiveList.Add(randomNumber);
                previousNumber = randomNumber; // 前の数字を更新
            }
            else
            {
                // 同じ数字が生成された場合、もう一度乱数を生成する
                i--;
            }
        }

        return nonConsecutiveList;
    }



}
