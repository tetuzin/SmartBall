using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Manager.CommonScene;
using ShunLib.Utils.Debug;

namespace SmartBall
{
    public class IngameManager : CommonSceneManager
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("レバー")]
        [SerializeField] protected SmartBallLever _lever = default;

        [Header("タッチパネル")]
        [SerializeField] protected TouchPanel _touchPanel = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        protected override void Initialize()
        {
            // レバーの初期化
            if (_lever != default)
            {
                _lever.Initialize();
            }
            else
            {
                DebugUtils.LogWarning("レバーオブジェクトの参照が設定されていません！");
                return;
            }

            // タッチパネルの初期化
            if (_touchPanel != default)
            {
                _touchPanel.Initialize();
                _touchPanel.MoveTappingCallback = _lever.Pull;
                _touchPanel.ReleaseTappingCallback = _lever.Release;
            }
            else
            {
                DebugUtils.LogWarning("タッチパネルオブジェクトの参照が設定されていません！");
                return;
            }
        }

        // ---------- デバッグ用関数 ---------
    }
}
