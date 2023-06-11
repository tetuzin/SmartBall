using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Utils.Debug;
using ShunLib.Utils.Resource;
using ShunLib.Utils.Find;
using ShunLib.Scenario.Model;

namespace ShunLib.Scenario.CharacterView
{
    public class ScenarioCharacterView : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("キャラ表示ポイントのリスト")] protected List<Transform> _charaPointList = default;
        [SerializeField, Tooltip("キャラ立ち絵のプレハブ")] protected StandCharacter _standCharacterPrefab = default;
        [SerializeField, Tooltip("キャラ立ち絵の親オブジェクト")] protected GameObject _parentObj = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Dictionary<string, StandCharacter> _charaDict = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            DeleteStandCharacter();
        }

        // キャラ立ち絵の削除
        public void DeleteStandCharacter()
        {
            _charaDict = new Dictionary<string, StandCharacter>();
            GameObject[] array = FindUtils.GetChildObjects(_parentObj);
            foreach (GameObject obj in array)
            {
                Destroy(obj);
            }
        }

        // キャラ立ち絵の生成
        public StandCharacter CreateStandCharacter(DialogActionModel action)
        {
            string charaId = action.StrParam1;
            Sprite sprite = ResourceUtils.GetResourcesSprite(action.FilePath);
            int pointIndex = action.NumParam2;
            return CreateStandCharacter(charaId, sprite, pointIndex);
        }

        // キャラ立ち絵の生成
        public StandCharacter CreateStandCharacter(string charaId, Sprite sprite, int pointIndex)
        {
            StandCharacter chara = Instantiate(
                _standCharacterPrefab,
                GetCharacterShowPoint(pointIndex).position,
                Quaternion.identity,
                _parentObj.transform
            );
            chara.Initialize();
            chara.SetCharacterImage(sprite);
            chara.SetActive(false, false);
            _charaDict.Add(charaId, chara);
            return chara;
        }

        // キャラ立ち絵の表示・非表示
        public void SetActiveStandCharacter(DialogActionModel action, bool isShow)
        {
            string charaId = action.StrParam1;
            if (_charaDict.ContainsKey(charaId))
            {
                _charaDict[charaId].SetActive(isShow);
            }
            else
            {
                CreateStandCharacter(action);
                _charaDict[charaId].SetActive(isShow);
            }
        }

        // キャラ立ち絵の移動
        public void MoveStandCharacter(DialogActionModel action)
        {
            string charaId = action.StrParam1;
            if (_charaDict.ContainsKey(charaId))
            {
                _charaDict[charaId].MovePosition(
                    GetCharacterShowPoint(action.NumParam2)
                );
            }
            else
            {
                CreateStandCharacter(action);
            }
        }

        // キャラ表示ポイントを返す
        public Transform GetCharacterShowPoint(int index)
        {
            if (index < 0 || index >= _charaPointList.Count) 
            {
                DebugUtils.LogWarning("指定のキャラクター表示ポイントは存在しません");
                return null;
            }
            return _charaPointList[index];
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}