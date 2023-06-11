using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Dict;

namespace Pachinko.Manager.Accessory
{
    public enum AccessoryActionState
    {
        NONE = 0,
        ACTION_1 = 1,
        ACTION_2 = 2,
        ACTION_3 = 3,
        ACTION_4 = 4,
        ACTION_5 = 5
    }
    public class PachinkoAccessoryManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("役物オブジェクトのリスト")] 
        [SerializeField] public GameObjectTable _accessoryObjects = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {

        }

        // 役物を動かす
        public async Task AccessoryAction(AccessoryActionState state)
        {
            switch (state)
            {
                case AccessoryActionState.ACTION_1:
                    await Action1();
                    break;
                case AccessoryActionState.ACTION_2:
                    await Action2();
                    break;
                case AccessoryActionState.ACTION_3:
                    await Action3();
                    break;
                case AccessoryActionState.ACTION_4:
                    await Action4();
                    break;
                case AccessoryActionState.ACTION_5:
                    await Action5();
                    break;
                default:
                    break;
            }
            await Task.CompletedTask;
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        protected virtual Task Action1(){
            return Task.CompletedTask;
        }
        protected virtual Task Action2(){
            return Task.CompletedTask;
        }
        protected virtual Task Action3(){
            return Task.CompletedTask;
        }
        protected virtual Task Action4(){
            return Task.CompletedTask;
        }
        protected virtual Task Action5(){
            return Task.CompletedTask;
        }

        // ---------- デバッグ用関数 ---------
    }
}

