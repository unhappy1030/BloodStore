// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// [System.Serializable]
// public class PosTree
// {
//     public PosTree parent;
//     public GameObject male;
//     public GameObject female;
//     public List<PosTree> children;

//     public void setData(GameObject prefab)
//     {
//         male = prefab;
//         female = prefab;
//         for (int i = 0; i < 5; i++)
//         {

//             PosTree posTree =  new PosTree{
//                 parent = this,
//                 male = prefab,
//                 female = prefab,
//                 children = new List<PosTree>(),
//             };
//             children.Add(posTree);
//         }
//     }

//     public void SetParent()
//     {
//         PlaceParent();
//     }

//     public void SetChildren()
//     {
//         int childNum = children.Count;
//         int idx = 0;
//         for (float i = (((float)childNum * 5 / 2) - 2.5f) * -1 ; i <= (((float)childNum * 5 / 2) - 2.5f) ; i += 5)
//         {
//             PlaceChild(children[idx].male, children[idx].female, i);
//             idx++;
//         }
//     }


//     void PlaceParent(){
//         float halfX = male.GetComponent<SpriteRenderer>().bounds.extents.x;
//         float halfY = male.GetComponent<SpriteRenderer>().bounds.extents.y;
//         Vector2 centerPos = new Vector2(0,halfY);
//         Vector2 malePos = centerPos - new Vector2(halfX * 1.2f,0);
//         Vector2 femalePos = centerPos + new Vector2(halfX * 1.2f,0);
//         GameObject personInstance1 = Object.Instantiate(male, malePos, Quaternion.identity);
//         GameObject personInstance2 = Object.Instantiate(female, femalePos, Quaternion.identity);
//     }

//     void PlaceChild(GameObject male, GameObject female, float x){
//         float halfX = male.GetComponent<SpriteRenderer>().bounds.extents.x;
//         float halfY = male.GetComponent<SpriteRenderer>().bounds.extents.y;
//         Vector2 centerPos = new Vector2(x * halfX,-1 * halfY);
//         Vector2 malePos = centerPos - new Vector2(halfX * 1.2f,0);
//         Vector2 femalePos = centerPos + new Vector2(halfX * 1.2f,0);
//         GameObject personInstance1 = Object.Instantiate(male, malePos, Quaternion.identity);
//         GameObject personInstance2 = Object.Instantiate(female, femalePos, Quaternion.identity);
//     }
// }
//     // private void PlaceParent(Tilemap tilemap, int x, int y)
//     // {
//     //     Vector3Int cellPos1 = new Vector3Int(x - 1, y, 0);
//     //     Vector3Int cellPos2 = new Vector3Int(x + 1, y, 0);
//     //     tilemap.SetTile(cellPos1, ScriptableObject.CreateInstance<Tile>());
//     //     tilemap.SetTile(cellPos2, ScriptableObject.CreateInstance<Tile>());

//     //     Vector3 worldPos1 = tilemap.GetCellCenterWorld(cellPos1);
//     //     Vector3 worldPos2 = tilemap.GetCellCenterWorld(cellPos2);

//     //     float prefabHalfWidth = male.GetComponent<SpriteRenderer>().bounds.extents.x * 0.85f;
//     //     worldPos1 += new Vector3(prefabHalfWidth, 0f, 0f);
//     //     worldPos2 -= new Vector3(prefabHalfWidth, 0f, 0f);

//     //     GameObject personInstance1 = Object.Instantiate(male, worldPos1, Quaternion.identity);
//     //     GameObject personInstance2 = Object.Instantiate(female, worldPos2, Quaternion.identity);
//     // }

//     // private void PlaceChild(Tilemap tilemap, GameObject male, GameObject female, int x, int y)
//     // {
//     //     Vector3Int cellPos1 = new Vector3Int(x - 1, y, 0);
//     //     Vector3Int cellPos2 = new Vector3Int(x + 1, y, 0);
//     //     tilemap.SetTile(cellPos1, ScriptableObject.CreateInstance<Tile>());
//     //     tilemap.SetTile(cellPos2, ScriptableObject.CreateInstance<Tile>());

//     //     Vector3 worldPos1 = tilemap.GetCellCenterWorld(cellPos1);
//     //     Vector3 worldPos2 = tilemap.GetCellCenterWorld(cellPos2);

//     //     float prefabHalfWidth = male.GetComponent<SpriteRenderer>().bounds.extents.x * 0.85f;
//     //     worldPos1 += new Vector3(prefabHalfWidth, 0f, 0f);
//     //     worldPos2 -= new Vector3(prefabHalfWidth, 0f, 0f);

//     //     // 여기서 worldPos1의 y좌표를 수정
//     //     worldPos1 -= new Vector3(0f, prefabHalfWidth * 2, 0f);
//     //     worldPos2 -= new Vector3(0f, prefabHalfWidth * 2, 0f);

//     //     GameObject personInstance1 = Object.Instantiate(male, worldPos1, Quaternion.identity);
//     //     GameObject personInstance2 = Object.Instantiate(female, worldPos2, Quaternion.identity);
//     // }
//     // void PlacePrefabOnTilemap(Tilemap tilemap, GameObject male, GameObject female, float x, float y)
//     // {
//     //     // 타일맵 좌표에 프리팹 배치
//     //     Vector3Int cellPos1 = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0);
//     //     Vector3Int cellPos2 = new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0);
//     //     tilemap.SetTile(cellPos1, ScriptableObject.CreateInstance<Tile>());
//     //     tilemap.SetTile(cellPos2, ScriptableObject.CreateInstance<Tile>());

//     //     // 소수점 단위로 좌표 이동
//     //     Vector3 worldPos1 = tilemap.GetCellCenterWorld(cellPos1);
//     //     Vector3 worldPos2 = tilemap.GetCellCenterWorld(cellPos1);
//     //     worldPos1 += new Vector3(x - Mathf.Floor(x), y - Mathf.Floor(y), 0);
//     //     worldPos2 += new Vector3(x - Mathf.Floor(x), y - Mathf.Floor(y), 0);

//     //     // 월드 좌표로 변환
//     //     GameObject personInstance1 = Object.Instantiate(male, worldPos1, Quaternion.identity);
//     //     GameObject personInstance2 = Object.Instantiate(female, worldPos2, Quaternion.identity);
//     // }
// // }