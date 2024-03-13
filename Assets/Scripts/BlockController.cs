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
    public NotesParam[] notes;
}

[System.Serializable]
public class NotesParam
{
    public int LPB;
    public int num;
    public int block;
    public int type;
    public int notes;
}

public class BlockController : MonoBehaviour
{
    [SerializeField] MainController mainController;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject gridParentObj;
    [SerializeField] int gridWidth = 3;
    [SerializeField] int gridHeight = 3;
    [SerializeField] AudioSource audiosourceSE;
    [SerializeField] AudioClip onHitSound;
    [SerializeField] string jsonFileName;
    [SerializeField] GameObject gridCornerMarkerObj;
    [SerializeField] BoneFollower middleBaseBone;
    [SerializeField] BoneFollower middleTipBone;
    [SerializeField] BoneFollower indexTipBone;
    [SerializeField] BoneFollower ringTipBone;
    [SerializeField] GameObject handRObj;
    [SerializeField] GameObject debugAxis;
    [SerializeField] float notesStartOffset;
    public class Note {
        public float startTime;
        public int blockID;
        public int type;
    }
    List<Note> notes = new List<Note>();
    List<Block> blocks = new List<Block>();
    float elapsedTime;
    int currentNoteNum;
    int currentActiveBlockID;
    int totalGridCount;
    
    
    void Start()
    {
        totalGridCount = gridHeight * gridWidth;
        GenerateGrid();
        LoadNotes();
    }


    void GenerateGrid() 
    {
        var blockScale = blockPrefab.GetComponent<Block>().BodyObjScale;

        // グリッドの中心からのオフセットを計算
        Vector3 offset = new Vector3(
            (gridWidth - 1) * blockScale.x / 2f,
            0f,
            (gridHeight - 1) * blockScale.z / 2f
        );
        
        // グリッドを作成
        int blockCount = 0;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // 座標を指定してブロックを作成
                Vector3 spawnPosition = new Vector3(x * blockScale.x, 0f, z * blockScale.z) - offset;
                GameObject blockObj = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
                blockObj.transform.SetParent(gridParentObj.transform);

                // ブロックの初期設定
                var b = blockObj.GetComponent<Block>();
                b.Initialize(blockCount);
                blocks.Add(b);

                blockCount ++;
            }
        }
    }

    

    public void AdjustGridPosition() {
        var blockScale = blockPrefab.GetComponent<Block>().BodyObjScale;
        float multiply = 4f;
        Vector3 positionOffset = new Vector3(0f, -blockScale.y/2 * multiply, 0f);
        gridParentObj.transform.localPosition = handRObj.transform.position;
        foreach (Transform block in gridParentObj.transform)
        {
            block.transform.localPosition = new Vector3(block.transform.localPosition.x, positionOffset.y, block.transform.localPosition.z);
        }

        Vector3 indexTipBonePosition = indexTipBone.gameObject.transform.position;
        Vector3 middleTipBonePosition = middleTipBone.gameObject.transform.position;
        Vector3 ringTipBonePosition = ringTipBone.gameObject.transform.position;

        Vector3 v1 = middleTipBonePosition - indexTipBonePosition;
        Vector3 v2 = ringTipBonePosition - indexTipBonePosition;

        Vector3 normal = Vector3.Cross(v1, v2).normalized;

        gridParentObj.transform.rotation = Quaternion.FromToRotation(gridParentObj.transform.up, normal) * gridParentObj.transform.rotation;
    }

    public void AdjustGridScale() {
        var blockScale = blockPrefab.GetComponent<Block>().BodyObjScale;
        var distance = gridCornerMarkerObj.transform.position - middleBaseBone.gameObject.transform.position;
        gridParentObj.transform.localScale = new Vector3(distance.x*2/(blockScale.x*3), gridParentObj.transform.localScale.y, distance.z*2/(blockScale.z*3));
    }


    public void OnHit(int id) {
        // アクティブでないブロックに衝突した場合は無視
        if (blocks[id].state != Block.STATE.ACTIVE) return;

        blocks[id].OnHitHand();
        // 現在アクティブなブロックに衝突した場合
        if (id == currentActiveBlockID) {
            OnHitCorrectBlock();
        }
    }

    
    void OnHitCorrectBlock() {
        audiosourceSE.PlayOneShot(onHitSound);
        mainController.AddScore();
    }

    
    void Update()
    {
        // プレイ中のみ実行する処理
        if (mainController.GameState == MainController.GAME_STATE.PLAY) {
            elapsedTime += Time.deltaTime;
        
            if (elapsedTime > notes[currentNoteNum].startTime) {
                SetNextBlock();
                currentNoteNum ++;
            }
        }

        // デバッグ用
        // ---
        if (Input.GetKeyDown(KeyCode.T)) {
            AdjustGridPosition();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            AdjustGridScale();
        }

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                OnHit(i);
            }
        }
        // ---
    }


    void SetNextBlock() {
        // すべてのブロックを非アクティブにする
        foreach (var block in blocks)
        {
            block.DeactivateBlock();
        }

        if (currentNoteNum < notes.Count-1) {
            // 次のノーツまでの時間
            float duration = notes[currentNoteNum+1].startTime - notes[currentNoteNum].startTime;
            // 次のノーツを準備状態にする
            blocks[notes[currentNoteNum+1].blockID].ReadyBlock(duration);
        }

        currentActiveBlockID = notes[currentNoteNum].blockID;
    
        // 現在のノーツをアクティブにする
        blocks[currentActiveBlockID].ActivateBlock();
    }


    void LoadNotes() {
        var info = new FileInfo(Application.dataPath + "/Notes/" + jsonFileName + ".json");
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();
        InputJson inputJson = JsonUtility.FromJson<InputJson>(json);

        int notesCount = inputJson.notes.Length;
        float barToTime = 60f / (inputJson.BPM * inputJson.notes[0].LPB);

        // アクティブにするブロックの番号を作成する
        var activeBlockIDs = GenerateNonConsecutiveList(notesCount);

        for (int i=0; i<notesCount; i++) {
            Note note = new Note
            {
                startTime = inputJson.notes[i].num * barToTime + notesStartOffset, 
                blockID = activeBlockIDs[i]
            };
            notes.Add(note);
        }
    }


    List<int> GenerateNonConsecutiveList(int size)
    {
        List<int> nonConsecutiveList = new List<int>();
        // 前の数字を保持する変数を初期化
        int previousNumber = -1; 

        for (int i = 0; i < size; i++)
        {
            // ランダムな整数を生成
            int randomNumber = Random.Range(0, totalGridCount); 
            // 前の数字と異なる場合のみリストに追加
            if (randomNumber != previousNumber)
            {
                nonConsecutiveList.Add(randomNumber);
                previousNumber = randomNumber;
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
