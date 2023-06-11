using UnityEngine;

using ShunLib.Dict;
using ShunLib.Particle;

namespace ShunLib.Manager.Particle
{
    public class ParticleManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("演出表示親オブジェクト")]
        [SerializeField] protected Transform _particleParent = default;
        
        [Header("パーティクル")]
        [SerializeField] protected CommonParticleTable _particleTable = new CommonParticleTable();
        
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 表示済みパーティクルリスト
        protected CommonParticleTable _showParticleTable = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _showParticleTable = new CommonParticleTable();
        }

        // 各パラメータの設定
        public void SetParameter(Transform particleParent = null, CommonParticleTable particleTable = null)
        {
            _particleParent = particleParent;
            _particleTable = particleTable;
        }
        
        // パーティクルの生成
        public CommonParticle CreateParticle(string key, Transform parent = null)
        {
            if (_showParticleTable.GetKeyList().Contains(key)) 
            {
                if (parent != null)
                {
                    _showParticleTable.GetValue(key).gameObject.transform.SetParent(parent);
                    _showParticleTable.GetValue(key).gameObject.transform.localPosition = Vector3.zero;
                }
                return _showParticleTable.GetValue(key);
            }
            Transform parentPos = this.gameObject.transform;
            if (parent != null)
            {
                parentPos = parent;
            }
            else if (_particleParent != default)
            {
                parentPos = _particleParent;
            }
            CommonParticle particle = Instantiate(_particleTable.GetValue(key), parentPos);
            _showParticleTable.SetValue(key, particle);
            particle.Hide();
            return particle;
        }

        // パーティクルの表示
        public void ShowParticle(string key, Transform parent = null)
        {
            Transform parentPos = this.gameObject.transform;
            if (parent != null)
            {
                parentPos = parent;
            }
            else if (_particleParent != default)
            {
                parentPos = _particleParent;
            }
            if (_showParticleTable.GetKeyList().Contains(key)) 
            {
                _showParticleTable.GetValue(key).gameObject.transform.SetParent(parentPos);
                _showParticleTable.GetValue(key).gameObject.transform.localPosition = Vector3.zero;
                _showParticleTable.GetValue(key).Show();
            }
            else
            {
                CommonParticle particle = CreateParticle(key, parentPos);
                particle.Show();
            }
        }

        // パーティクルの非表示（全て）
        public void HideAllParticle()
        {
            if (_showParticleTable == default) return;
            foreach (string key in _showParticleTable.GetKeyList())
            {
                HideParticle(key);
            }
        }

        // パーティクルの非表示
        public void HideParticle(string key)
        {
            if (_showParticleTable.GetKeyList().Contains(key)) 
            {
                _showParticleTable.GetValue(key).Hide();
            }
        }

        // パーティクルの削除
        public void RemoveParticle(string key)
        {
            if (!_showParticleTable.GetKeyList().Contains(key)) return;
            Destroy(_showParticleTable.GetValue(key).gameObject);
            _showParticleTable.RemovePair(key);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}

