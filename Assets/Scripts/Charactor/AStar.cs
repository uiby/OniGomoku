using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {
  int gridLevel;
  Mass[,] massArrays;
  Vector2 goalPos = Vector2.zero; //正規化されたゴール位置
  Vector2 startPos = Vector2.zero; //正規化されたスタート位置
  Vector2 realGoalPos = Vector2.zero; //タップされた位置

  //gridLevel = 格子数
  public void Initialize(int _gridLevel) {
    gridLevel = _gridLevel;
    massArrays = new Mass[gridLevel, gridLevel];
  }

  public void MakeMassArrays(int width, int height) {
    var xRange = (float)width/gridLevel;
    var yRange = (float)height/gridLevel;
    var yPos = yRange/2f;
    var xPos = xRange/2f;
    for (int y = 0; y < gridLevel; y++) {
      xPos = xRange/2f;
      for (int x = 0; x < gridLevel; x++) {
        var center = new Vector3(xPos - width/2f, 0, yPos - height/2f);
        if (StageCollision.HasBlock(center, new Vector3(xRange/2f, 0.5f, yRange/2f))) {
          var block = StageCollision.GetBlock(center);
          if (block.GetComponent<BlockProvider>().Decided()) {
            massArrays[y, x] = new Mass();
          } else { //まだ決まってない場合==歩ける
            massArrays[y, x] = new Mass(center, xRange, yRange, new Vector2(x, y));
          }
        } else {
          massArrays[y, x] = new Mass();
        }
        xPos += xRange;
      }
      yPos += yRange;
    }
  }

  public List<Vector2> MakePath(Vector2 tapPos) {
    Reset();
    var pos = transform.position;
    MarkStartTile(new Vector2(pos.x, pos.z));
    SearchGoalBlock(tapPos);
    DecideCostToGoal();
    ShowDebug(0);
    Search();
    ShowDebug(1);
    ShowDebug(2);
    ShowPath();

    return GetPath();
  }

  void Reset() {
    for (int y = 0; y < gridLevel; y++) {
      for (int x = 0; x < gridLevel; x++) {
        if (!massArrays[y, x].isMass) continue;
        massArrays[y, x].Reset();
      }
    }
  }

  void MarkStartTile(Vector2 charaPos) {
    var pos = Vector2.zero;
    var minDistance = 999999f;
    for (int y = 0; y < gridLevel; y++) {
      for (int x = 0; x < gridLevel; x++) {
        if (!massArrays[y, x].isMass) continue;
        var distance = (charaPos - massArrays[y, x].ToVector2()).sqrMagnitude;
        if (distance < minDistance) {
          minDistance = distance;
          pos.Set(x, y);
        }
      }
    }

    massArrays[(int)pos.y, (int)pos.x].SetMassType(MassType.START);
    startPos.Set(pos.x, pos.y);
  }

  void SearchGoalBlock(Vector2 tapPos) {
    for (int y = 0; y < gridLevel; y++) {
      for (int x = 0; x < gridLevel; x++) {
        if (!massArrays[y, x].isMass) continue;
        if ((massArrays[y, x].pos.x - massArrays[y, x].width/2f < tapPos.x && tapPos.x < massArrays[y, x].pos.x + massArrays[y, x].width/2f) && 
            (massArrays[y, x].pos.y - massArrays[y, x].height/2f < tapPos.y && tapPos.y < massArrays[y, x].pos.y + massArrays[y, x].height/2f)) {
          massArrays[y, x].SetMassType(MassType.GOAL);
          goalPos.Set(x, y);
          realGoalPos = tapPos;
          return; //finish
        }
      }
    }
  }

  void DecideCostToGoal() {
    for (int y = 0; y < gridLevel; y++) {
      for (int x = 0; x < gridLevel; x++) {
        if (!massArrays[y, x].isMass) continue;
        if (massArrays[y, x].massType != MassType.CHECK_POINT) continue;        
        var cost = Mathf.RoundToInt((goalPos - new Vector2(x, y)).magnitude);
        massArrays[y, x].Initialize(cost);
      }
    }
  }

  void Search() {
    var y = (int)startPos.y;
    var x = (int)startPos.x;
    var searchCost = 0;

    //massArrays[y, x].Search(startPos); //スタート地点を探索済みにする (親:スタート地点)

    while (true) {
      //周り(上下左右)のマスを発見する
      searchCost++;
      var pos = new Vector2(x, y+1);
      if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].canSearch()) {
        massArrays[(int)pos.y, (int)pos.x].Discover(searchCost, new Vector2(x, y));
      } else if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].massType == MassType.GOAL) {
        break;
      }
      pos = new Vector2(x, y-1);
      if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].canSearch()) {
        massArrays[(int)pos.y, (int)pos.x].Discover(searchCost, new Vector2(x, y));
      } else if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].massType == MassType.GOAL) {
        break;
      }
      pos = new Vector2(x-1, y);
      if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].canSearch()) {
        massArrays[(int)pos.y, (int)pos.x].Discover(searchCost, new Vector2(x, y));
      } else if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].massType == MassType.GOAL) {
        break;
      }
      pos = new Vector2(x+1, y);
      if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].canSearch()) {
        massArrays[(int)pos.y, (int)pos.x].Discover(searchCost, new Vector2(x, y));
      } else if (InField(pos) && massArrays[(int)pos.y, (int)pos.x].massType == MassType.GOAL) {
        break;
      }

      if (searchCost > 10000) {
        Debug.Log("error: don't finish search");
        break;
      }

      //探索する = 次の基点を探す
      var nextSearchPos = GetTileOfMinimumTotalCost();
      //Debug.Log("next:"+nextSearchPos);
      massArrays[(int)nextSearchPos.y, (int)nextSearchPos.x].Search();
      x = (int)nextSearchPos.x;
      y = (int)nextSearchPos.y;
    }

    massArrays[(int)goalPos.y, (int)goalPos.x].Search(massArrays[y, x].address);
  }

  List<Vector2> GetPath() {
    List<Vector2> checkPointList = new List<Vector2>();
    var pos = goalPos;
    var counter = 0;
    checkPointList.Add(realGoalPos);
    //checkPointList.Add(massArrays[(int)pos.y, (int)pos.x].pos); //ゴール地点
    while (massArrays[(int)pos.y, (int)pos.x].massType != MassType.START) {
      checkPointList.Add(massArrays[(int)pos.y, (int)pos.x].ToVector2());
      pos = massArrays[(int)pos.y, (int)pos.x].parnentPos;
      if (counter++ > 10000)
        break;
    }

    checkPointList.Reverse();//スタート順にソート

    return checkPointList;
  }

  void ShowDebug(int type) {
    var str = "";
    if (type == 0) str += "推定コスト\n";
    else if (type == 1) str += "実コスト\n";
    else if (type == 2) str += "合計コスト\n";
    for (int y = gridLevel - 1; y >= 0 ; y--) {
      for (int x = 0; x < gridLevel; x++) {
        if (type == 0) 
          str += massArrays[y, x].costToGoal.ToString("00") +" ";
        else if (type == 1)
          str += massArrays[y, x].costFromStart.ToString("00") +" ";
        else if (type == 2)
          str += massArrays[y, x].totalCost.ToString("00") +" ";
      }
      str +="\n";
    }

    Debug.Log(str);
  }

  void ShowPath() {
    var str = "最短経路\n";

    var pos = goalPos;
    var counter = 0;
    while (massArrays[(int)pos.y, (int)pos.x].massType != MassType.START) {
      str += massArrays[(int)pos.y, (int)pos.x].address +" -> ";
      pos = massArrays[(int)pos.y, (int)pos.x].parnentPos;
      if (counter++ > 10000)
        break;
    }

    Debug.Log(str);
  }

  bool InField(Vector2 pos) {
    return (0 <= (int)pos.x && (int)pos.x < gridLevel) && (0 <= (int)pos.y && (int)pos.y < gridLevel);
  }

  Vector2 GetTileOfMinimumTotalCost() {
    var pos = Vector2.zero;
    var minTotalCost = 99999;
    var minHeuristicCost = 99999;
    for (int y = 0; y < gridLevel; y++) {
      for (int x = 0; x < gridLevel; x++) {
        if (!massArrays[y, x].isMass) continue;
        if (massArrays[y, x].searched) continue;
        if (!massArrays[y, x].discovered) continue;
        if (massArrays[y, x].massType != MassType.CHECK_POINT) continue;

        var totalCost = massArrays[y, x].totalCost;
        var heuristicCost = massArrays[y, x].costFromStart;

        if (totalCost > minTotalCost) continue;
        if (totalCost == minTotalCost) {
          if (heuristicCost >= minHeuristicCost) continue;
        }

        pos.Set(x, y);
        minTotalCost = totalCost;
        minHeuristicCost = heuristicCost;
      }
    }

    return pos;
  }
}

public class Mass {
  public Vector3 pos {get; private set;}
  public float width;
  public float height;
  public bool isMass {get; private set;}
  public MassType massType {get; private set;}
  public int costToGoal {get; private set;} //推定コスト
  public int costFromStart {get; private set;} //実コスト
  public int totalCost {get; private set;} //= costToGoal + costFromStart
  public bool discovered {get; private set;} //発見したかどうか
  public bool searched {get; private set;} //探索したかどうか
  public Vector2 address; //ポインタ
  public Vector2 parnentPos {get; private set;} //親ポインタ

  public Mass(Vector3 _pos, float _width, float _height, Vector2 _address) {
    isMass = true;
    pos = _pos;
    width = _width;
    height = _height;
    address = _address;
  }

  public Mass() {
    isMass = false;
  }

  public void Reset() {
    SetMassType(MassType.CHECK_POINT);
    costToGoal = 0;
    costFromStart = 0;
    totalCost = 0;
    searched = false;
    discovered = false;
    parnentPos = Vector2.zero;
  }

  public void Initialize(int initCostToGoal) {
    costToGoal = initCostToGoal;
    costFromStart = 0;
    totalCost = 0;
    searched = false;
    discovered = false;
    parnentPos = Vector2.zero;
  }

  public void Discover(int _costToFromStart, Vector2 parnent) {
    parnentPos = parnent;
    costFromStart = _costToFromStart;
    totalCost = costToGoal + costFromStart;
    discovered = true;
  }

  public void Search() {
    searched = true;
  }
  public void Search(Vector2 parnent) {
    parnentPos = parnent;
    searched = true;
  }

  public bool canSearch() {
    return isMass == true && discovered == false && massType == MassType.CHECK_POINT;
  }

  public void SetMassType(MassType type) {
    massType = type;
  }

  public Vector2 ToVector2() {
    return new Vector2(pos.x, pos.z);
  }
}

public enum MassType {
  START,
  CHECK_POINT,
  GOAL,
}
