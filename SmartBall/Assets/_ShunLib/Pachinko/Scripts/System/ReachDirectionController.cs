using System;
using UnityEngine;

using ShunLib.Utils.Random;

using Pachinko.Model;
using Pachinko.Dict;
using Pachinko.Const;

namespace Pachinko.Controller.ReachDirection
{
    public class ReachDirectionController : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private ReachDirectionTable _reachDirectionTable = default;
        private float _hitTotal = default;
        private int _reachValue = default;
        private int _appearanceTotal = default;
        private int _hitAppearanceTotal = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(ReachDirectionTable reachDirectionTable)
        {
            _reachDirectionTable = reachDirectionTable;
            SetHitTotal();
            SetAppearanceTotal();
        }

        // リーチ演出を表示する値を返す
        public int GetReachValue()
        {
            return _reachValue;
        }

        // リーチ演出をランダムに返す
        public ReachDirectionModel GetReachDirection(bool isHit)
        {
            System.Random r = new System.Random();
            int total = 0;
            if (isHit)
            {
                int value = r.Next(0, _hitAppearanceTotal);
                
                foreach (var kvp in _reachDirectionTable.GetTable())
                {
                    total += (int)(kvp.Value.AppearanceRate * kvp.Value.HitRate);
                    if (value < total) return kvp.Value;
                }
            }
            else
            {
                int value = r.Next(0, _appearanceTotal);
                foreach (var kvp in _reachDirectionTable.GetTable())
                {
                    total += kvp.Value.AppearanceRate;
                    if (value < total) return kvp.Value;
                }
            }
            return null;
        }

        // リーチ名からリーチ演出を返す
        public ReachDirectionModel GetReachDirectionByName(string reachName)
        {
            foreach (ReachDirectionModel reach in _reachDirectionTable.GetValueArray())
            {
                if (reach.Name == reachName) return reach;
            }
            return null;
        }

        // リーチ演出から疑似連数をランダムに抽選する
        public ReachDirectionState GetReachDirectionState(ReachDirectionModel reachModel)
        {
            int randomValue = RandomUtils.GetRandomValue(reachModel.StateList.Count);
            return reachModel.StateList[randomValue];
        }

        // ---------- Private関数 ----------

        // 当たる確率の合計値の設定
        private void SetHitTotal()
        {
            float total = 0;
            foreach (var kvp in _reachDirectionTable.GetTable())
            {
                _hitTotal += kvp.Value.HitRate;
                total += 100;
            }
            _reachValue = (int)Math.Ceiling(total / _hitTotal);
        }

        // 出現確率の合計値の設定
        private void SetAppearanceTotal()
        {
            _appearanceTotal = 0;
            _hitAppearanceTotal = 0;
            foreach (var kvp in _reachDirectionTable.GetTable())
            {
                _appearanceTotal += kvp.Value.AppearanceRate;
                _hitAppearanceTotal += (int)(kvp.Value.AppearanceRate * kvp.Value.HitRate);
            }
        }

        // ---------- protected関数 ---------
    }
}