using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Pachinko.HoldView.Icon;
using Pachinko.Dict;
using Pachinko.Const;
using Pachinko.Model;

namespace Pachinko.HoldView
{
    public class HoldViewManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        private readonly Vector3 CUR_HOLD_SCALE = new Vector3(1.5f, 1.5f, 1.5f);
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("保留アイコンオブジェクト")] 
        [SerializeField] private HoldIconTable _holdIconObjects = default;

        [Header("保留アニメーション")] 
        [SerializeField] private bool _isHoldAnim = true;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public int HoldCount
        {
            get 
            {
                int count = 0;
                if (_holdIcons != null)
                {
                    foreach (HoldIcon holdIcon in _holdIcons)
                    {
                        if (holdIcon != null) count++;
                    }
                }
                return count;
            }
        }

        public HoldIcon CurHoldIcon
        {
            get { return _curHoldIcon; }
        }

        public bool IsPause { get; set; }

        // 保留追加時コールバック
        public Action<HoldModel> AddHoldCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 最大保留数
        private int _maxHoldCount = default;
        // 変動中保留
        private HoldIcon _curHoldIcon = default;

        // 保留値リスト
        private HoldIcon[] _holdIcons = default;

        // 次に追加するインデックス番号
        private int _nextAddIndex = default;

        // 保留アニメーション反転フラグ
        private bool _isAnimReverse = default;

        // 保留アニメーション回転座標
        private Quaternion _holdRotate = default;

        // 変動中保留表示ポイント
        private GameObject _showIconPoint = default;

        // 保留表示ポイント
        private List<GameObject> _showIconPoints = default;

        // ---------- Unity組込関数 ----------

        void FixedUpdate()
        {
            if (_isHoldAnim)
            {
                Vector3 rotate = Vector3.zero;
                rotate.z = _isAnimReverse ? 1f : -1f;
                if (_curHoldIcon != null)
                {
                    _curHoldIcon.gameObject.transform.localRotation = _holdRotate;
                    _curHoldIcon.gameObject.transform.Rotate(rotate);
                }
                for (int i = 0; i < HoldCount; i++)
                {
                    _holdIcons[i].gameObject.transform.localRotation = _holdRotate;
                    _holdIcons[i].gameObject.transform.Rotate(rotate);
                }
                if (_curHoldIcon != null)
                {
                    _holdRotate = _curHoldIcon.gameObject.transform.localRotation;
                }
                else if (HoldCount > 0)
                {
                    _holdRotate = _holdIcons[0].gameObject.transform.localRotation;
                }
                if (_holdRotate.z >= 0.15f) _isAnimReverse = !_isAnimReverse;
                else if (_holdRotate.z <= -0.15f) _isAnimReverse = !_isAnimReverse;
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(int maxHoldCount)
        {
            // 描画系パラメータの初期化
            _maxHoldCount = maxHoldCount;
            _holdIcons = new HoldIcon[_maxHoldCount];
            _nextAddIndex = 0;
            IsPause = true;
            _curHoldIcon = null;
            _holdRotate = Quaternion.identity;
        }

        // 各パラメータの設定
        public void SetParameter(HoldIconTable holdIconObjects = null, bool isHoldAnim = true)
        {
            _holdIconObjects = holdIconObjects;
            _isHoldAnim = isHoldAnim;
        }

        // 保留オブジェクトテーブルを返す
        public HoldIconTable GetHoldIconObjects()
        {
            return _holdIconObjects;
        }

        // 保留を追加
        public void AddHold(HoldModel hold)
        {
            if (IsPause) return;
            if (HoldCount >= _maxHoldCount) return;

            // 保留の生成
            _holdIcons[_nextAddIndex] = CreateHoldIcon(hold);
            _nextAddIndex++;

            AddHoldCallback?.Invoke(hold);
        }

        // 保留を先頭から一つ取り出す
        public HoldModel PopHold()
        {
            // 変動中保留のクリア
            RemoveCurHoldIcon();
            
            // 保留アイコンの移動を描画
            _curHoldIcon = _holdIcons[0];
            _holdIcons[0] = null;
            _curHoldIcon.gameObject.transform.SetParent(_showIconPoint.transform);
            HoldMoveAnim(_curHoldIcon, Vector3.zero, true);
            for (int i = 1; i < _holdIcons.Length; i++)
            {
                if (_holdIcons[i] == null) continue;
                
                _holdIcons[i].gameObject.transform.SetParent(_showIconPoints[i - 1].transform);
                HoldMoveAnim(_holdIcons[i], Vector3.zero);
                _holdIcons[i - 1] = _holdIcons[i];
                _holdIcons[i] = null;
            }
            _nextAddIndex--;
            return _curHoldIcon.Model;
        }

        // 変動中保留の削除
        public void RemoveCurHoldIcon()
        {
            // 保留アイコンの描画
            if (_curHoldIcon != null)
            {
                HoldRemoveAnim(_curHoldIcon);
                _curHoldIcon = null;
            }
        }

        // 保留リストのクリア
        public void RemoveHoldIconList()
        {
            for (int i = 0; i < _holdIcons.Length; i++)
            {
                if (_holdIcons[i] == null) continue;
                Destroy(_holdIcons[i].gameObject);
            }
            _holdIcons = new HoldIcon[_maxHoldCount];
            _nextAddIndex = 0;
        }

        // 保留を初期化
        public void InitializeHold()
        {
            // 変動中保留のクリア
            RemoveCurHoldIcon();

            // 保留リストのクリア
            RemoveHoldIconList();
        }

        // 保留リストを描画
        public void CreateHoldIcons(List<HoldModel> holdList)
        {
            InitializeHold();
            for (int i = 0; i < holdList.Count; i++)
            {
                _holdIcons[_nextAddIndex] = CreateHoldIcon(holdList[i]);
                _nextAddIndex++;
            }
        }

        // 保留アイコンの昇格を描画
        public Transform ChanceUpHold(
            bool isChanceUpCurHold, int chanceUpHoldIndex, HoldIconState nextIconState = HoldIconState.NORMAL
        )
        {
            if (IsPause) return null;

            // 昇格保留の検索
            HoldIcon curIcon = null;
            if (isChanceUpCurHold)
            {
                curIcon = _curHoldIcon;
            }
            else
            {
                curIcon = _holdIcons[chanceUpHoldIndex];
            }
            if (curIcon == null)
            {
                Debug.Log("昇格する保留が見つかりませんでした");
                return null;
            }

            // 昇格先保留の選定
            HoldIcon iconObject = _holdIconObjects.GetValue(nextIconState);

            // 保留アイコンの差し替え
            HoldIcon chanceUpIcon = CreateHoldIconObject(iconObject, curIcon.transform.parent);
            Transform parent = curIcon.transform.parent;
            if (curIcon == _curHoldIcon)
            {
                chanceUpIcon.transform.localScale = CUR_HOLD_SCALE;
                _curHoldIcon = chanceUpIcon;
            }
            else
            {
                _holdIcons[chanceUpHoldIndex] = chanceUpIcon;
            }
            chanceUpIcon.Value = curIcon.Value;
            Destroy(curIcon.gameObject);
            return parent;
        }

        // 保留の昇格先オブジェクトを返す
        public HoldIconState GetChanceUpHoldState(HoldModel hold)
        {
            HoldIconState nextState = HoldIconState.NORMAL;
            HoldIconState curState = (HoldIconState)hold.HoldId;
            int stateIndex = PachinkoConst.chanceUpHoldArray.IndexOf(curState);
            nextState = PachinkoConst.chanceUpHoldArray[stateIndex + 1];
            return nextState;
        }

        // 保留表示位置の設定
        public void SetHoldIconPoint(GameObject iconPoint, List<GameObject> iconPoints)
        {
            _showIconPoint = iconPoint;
            _showIconPoints = iconPoints;
        }

        // ---------- Private関数 ----------

        // 保留アイコンを作成
        private HoldIcon CreateHoldIcon(HoldModel model)
        {
            // 保留アイコンの種類抽選
            HoldIconState iconState = (HoldIconState)model.HoldId;

            // 保留アイコンの生成
            HoldIcon icon = CreateHoldIconObject(
                _holdIconObjects.GetValue(iconState), _showIconPoints[_nextAddIndex].transform
            );
            icon.Value = model.Value;
            icon.Model = model;
            icon.IsRemain = false;
            return icon;
        }

        // 保留が持つ演出状態を生成
        private ReachDirectionState CreateReachDirectionState(ReachDirectionModel direction)
        {
            if (direction == null || direction == default)
            {
                return ReachDirectionState.NONE;
            }
            else
            {
                return direction.GetRandomState();
            }
        }

        // 保留アイコンオブジェクトの生成
        private HoldIcon CreateHoldIconObject(HoldIcon iconObj, Transform parent)
        {
            HoldIcon icon = Instantiate(iconObj, Vector3.zero, Quaternion.identity);
            icon.transform.SetParent(parent);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = this.gameObject.transform.localScale;
            HoldShowAnim(icon);
            return icon;
        }

        // 保留出現アニメーション
        private void HoldShowAnim(HoldIcon icon)
        {
            if (_isHoldAnim)
            {
                icon.CanvasGroup.DOFade(1f, 0.5f);
            }
            else
            {
                icon.CanvasGroup.alpha = 1f;
            }
        }

        // 保留移動アニメーション
        private void HoldMoveAnim(HoldIcon icon, Vector3 pos, bool isCurHold = false)
        {
            if (_isHoldAnim)
            {
                icon.gameObject.transform.DOLocalMove(pos, 0.5f);
                if (isCurHold) icon.gameObject.transform.DOScale(CUR_HOLD_SCALE, 0.5f);
            }
            else
            {
                icon.gameObject.transform.localPosition = pos;
                if (isCurHold) icon.gameObject.transform.localScale = CUR_HOLD_SCALE;
            }
        }

        // 保留消滅アニメーション
        private void HoldRemoveAnim(HoldIcon icon)
        {
            if (_isHoldAnim)
            {
                icon.CanvasGroup.DOFade(0f, 0.5f).OnComplete(() => {
                    Destroy(icon.gameObject);
                });
            }
            else
            {
                Destroy(icon.gameObject);
            }
        }

        // 保留が半分以上たまっているか
        private bool CheckHoldHalfCount()
        {
            return _maxHoldCount / 2 < HoldCount;
        }

        // ---------- protected関数 ---------
    }
}


