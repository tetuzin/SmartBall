using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Particle
{
    public class CommonParticle : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("パーティクル")]
        [SerializeField] protected List<ParticleSystem> _particleList = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 再生
        public void Show()
        {
            foreach (ParticleSystem particle in _particleList)
            {
                particle.Play();
            }
        }

        // 一時停止
        public void Pause()
        {
            foreach (ParticleSystem particle in _particleList)
            {
                particle.Pause();
            }
        }

        // 停止
        public void Hide()
        {
            foreach (ParticleSystem particle in _particleList)
            {
                particle.Stop();
            }
        }

        // 色の設定
        public void SetColor(Color color)
        {
            foreach (ParticleSystem particle in _particleList)
            {
                var par = particle.main;
                par.startColor = color;
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}