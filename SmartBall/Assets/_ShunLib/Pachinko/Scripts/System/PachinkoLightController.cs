using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Pachinko.LightController
{
    public class PachinkoLightController : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [SerializeField, Tooltip("パチンコライト")] protected List<Light> _pachiLight_List;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        int LightingIntensity;
        bool Lighting = false;
        Tween tween;
        // ---------- Unity組込関数 ----------
        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            ALL_ChargeBrightness();
        }
        // ---------- Public関数 ----------

        // ライト有効化
        internal void LightEnabled_True(int setList)
        {
            _pachiLight_List[setList].enabled = true;
        }

        // ライト無効化
        internal void LightEnabled_False(int setList)
        {
            _pachiLight_List[setList].enabled = false;
        }


        // ALLライト有効化
        internal void ALL_LightEnabled_True()
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) LightEnabled_True(i);
        }

        // ALLライト無効化
        internal void ALL_LightEnabled_False()
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) LightEnabled_False(i);
        }

        // ライトon
        internal void LightON(int setList)
        {
            _pachiLight_List[setList].intensity = 6000;
        }

        // ライトon(色変更付き)
        internal void LightON(int setList, Color lightColor)
        {
            _pachiLight_List[setList].intensity = 6000;
            ChangeColor(_pachiLight_List[setList],lightColor);
        }

        // ライトoff
        internal void LightOFF(int setList)
        {
            _pachiLight_List[setList].intensity = 0;
        }

        // ALLライトon
        internal void ALL_LightON()
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) LightON(i);
        }

        // ALLライトon(色変更付き)
        internal void ALL_LightON(Color lightColor)
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) LightON(i, lightColor);
        }

        // ALLライトoff
        internal void ALL_LightOFF()
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) LightOFF(i);

        }

        // ライト点灯(色変更付き)(明るさ変更)
        // internal async void ALL_Lighting(Color lightColor, bool kill = false)
        // {
        //     Lighting = true;
        //     ALL_ChangeColor(lightColor);
        //     LightingIntensity=0;
        //     Debug.Log("LightingIntensity" + LightingIntensity);
        //     tween = DOTween.To(() => LightingIntensity, (x) => LightingIntensity = x, 6000, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
        //     if(kill)
        //     {
        //         tween.Kill();
        //         Lighting = false;
        //     }
        // }

        //上に流れる(一つずつ)
        internal async Task UpFlow_one(Color lightColor, int delayTime)
        {
            for (var i = 0; i < _pachiLight_List.Count; i++)
            {
                LightON(i, lightColor);
                await Task.Delay(delayTime);
                LightOFF(i);
            }
        }

        //下に流れる(一つずつ)
        internal async Task DownFlow_one(Color lightColor, int delayTime)
        {
            for (var i = _pachiLight_List.Count; i > 0 ; i--)
            {
                LightON(i-1, lightColor);
                await Task.Delay(delayTime);
                LightOFF(i-1);
            }
        }

        // フラッシュ
        internal async Task flash(Color lightColor, int delayTime)
        {
            ALL_LightEnabled_True();
            ALL_LightON(lightColor);
            await Task.Delay(delayTime);
            ALL_LightOFF();
            await Task.Delay(delayTime);

        }

        // 上に流れる(溜まる)
        internal async Task UpFlow(Color lightColor, int delayTime)
        {
            for (var i = 0; i < _pachiLight_List.Count; i++)
            {
                LightON(i, lightColor);
                await Task.Delay(delayTime);
            }
        }

        // 下に流れる(溜まる)
        internal async Task DownFlow(Color lightColor, int delayTime)
        {
            for (var i = _pachiLight_List.Count; i > 0 ; i--)
            {
                LightON(i-1, lightColor);
                await Task.Delay(delayTime);
            }
        }

        // 初期化
        internal void Initialize()
        {
            ALL_LightEnabled_True();
            ALL_LightOFF();
        }

        // ---------- Private関数 ----------

        // ライト色変更
        void ChangeColor(Light _pachiLight, Color lightColor)
        {
            _pachiLight.color = lightColor;
        }

        // ALL_ライト色変更
        void ALL_ChangeColor(Color lightColor)
        {
            for (var i = 0; i < _pachiLight_List.Count; i++) ChangeColor(_pachiLight_List[i],lightColor);
        }

        // 明るさ変更
        void ChargeBrightness(int setList)
        {
            _pachiLight_List[setList].intensity = LightingIntensity;
        }

        // ALL明るさ変更
        void ALL_ChargeBrightness()
        {
            if(Lighting) for (var i = 0; i < _pachiLight_List.Count; i++) ChargeBrightness(i);
            // for (var i = 0; i < _pachiLight_List.Count; i++) ChargeBrightness(i);
        }
    }
}