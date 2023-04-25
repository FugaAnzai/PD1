using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
    int[,] map;
    GameObject[,] field;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject wallPrefab;

    Vector2Int GetPlayerIndex()
    {

        for (int y = 0; y < field.GetLength(0); y++)
        {

            for(int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null && field[y,x].tag == "Player")
                {
                    Vector2Int resultTrue = new(x, y);
                    Debug.Log(resultTrue.ToString());
                    return resultTrue;
                }
            }
        }

        Vector2Int resultFalse = new(-1, -1);

        return resultFalse;
    }

    bool MoveObject(string tag,Vector2Int moveFrom, Vector2Int moveTo)
    {
        if(field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            Debug.Log("Failed");
            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            Vector2Int direction = moveTo - moveFrom;

            bool success = MoveObject("Box", moveTo, moveTo + direction);

            if(!success) { return false; }

        }

        //オブジェクトの座標を変更
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        //現在のプレイヤーオブジェクトを移動先に代入
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        //移動前のオブジェクトにnullを代入
        field[moveFrom.y, moveFrom.x] = null;
        return true;

    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]{
            { 3,3,3,3,3,3,3,3,3,3 },
            { 3,0,0,0,0,0,0,0,0,3 },
            { 3,0,0,0,0,0,0,2,0,3 },
            { 3,0,0,0,1,0,0,0,0,3 },
            { 3,0,2,0,0,0,0,0,0,3 },
            { 3,0,0,0,0,0,0,0,0,3 },
            { 3,3,3,3,3,3,3,3,3,3 }
        };

        field = new GameObject[map.GetLength(0),map.GetLength(1)];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //1だった場合、プレイヤーをInit
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                    playerPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity);
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    boxPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity);
                }

                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                    wallPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity);
                    field[y,x].transform.GetComponent<MeshRenderer>().material.color = Color.red;
                }

            }
        }

       Debug.Log("Start");

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.RightArrow)) {

            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int move = new(1, 0);

            MoveObject("Player", playerIndex, playerIndex + move);
            

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            Vector2Int move = new(1, 0);

            MoveObject("Player",playerIndex, playerIndex - move);

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();
            Vector2Int move = new(0, 1);

            MoveObject("Player", playerIndex, playerIndex - move);


        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            Vector2Int move = new(0, 1);

            MoveObject("Player", playerIndex, playerIndex + move);

        }

    }
}
