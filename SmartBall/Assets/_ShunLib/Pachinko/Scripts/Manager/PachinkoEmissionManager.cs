using UnityEngine;
using System.Threading.Tasks;

namespace Pachinko.Manager.Emission
{
    public enum AccessoryEmissionState
    {
        NONE,                   //
        OFF,                    // エミッションOFF
        FLASH,                  // フラッシュ
        RED_FLASH,              // 赤フラッシュ
        RAINDOW_FLASH,          // 虹フラッシュ
        RIGHT_FLOW,             // 右から左に
        LEFT_FLOW,              // 左から右に
        RIGHT_FLOW_ONE,         // 右から左に(一つずつ)
        LEFT_FLOW_ONE           // 左から右に(一つずつ)
    }
    public class PachinkoEmissionManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        public virtual Task AccessoryEmissionStatus(
            AccessoryEmissionState accessoryEmissionState = default,
            int delayTime = default,
            Color setColor = default
            )
        {
            return Task.CompletedTask;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}

