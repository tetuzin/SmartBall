using System;
using System.Collections.Generic;
using UnityEngine;

using Pachinko.Slot.Design;

namespace Pachinko.Slot.Reel
{
    public enum SlotState
    {
        STOP,           // 停止中
        ORDER_STOP,     // 停止命令中
        STAND_STOP,     // 停止待機中
        ROTATE,         // 回転中(速)
    }
    public enum SlotSpeed
    {
        NONE,       // なし
        SLOW,       // 遅い
        NORMAL,     // ふつう
        HIGH        // 速い
    }

    public class SlotReel : MonoBehaviour
    {
        // ---------- 定数宣言 ----------

        // private const float _speedNone = 0.0f;
        // private const float _speedSlow = 500.0f;
        // private const float _speedNormal = 1000.0f;
        // private const float _speedHigh = 3000.0f;
        
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("図柄Prefab")] private List<SlotDesign> _slotDesignPrefabs = default;
        [SerializeField, Tooltip("図柄配置オブジェクト")] private Transform _designParent = default;
        [SerializeField, Tooltip("キャンバスグループ")] private CanvasGroup _canvasGroup = default;
        [SerializeField, Tooltip("フェードスピード")] private float _fadeSpeed = 0.5f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        
        [Header("停止図柄インデックス")]
        [SerializeField] private int _stopDesignIndex = default;
        [Header("下へ回転させる")]
        [SerializeField] private bool _isRotateDown = default;
        [Header("リール停止時、何個前で速度を低下させるか")]
        [SerializeField] private int _slowSpdBeforeIndex = default;
        [Header("アニメフラグ")]
        [SerializeField] private bool _isAnim = true;

        public bool IsRotate
        {
            get
            {
                if (_slotState == SlotState.STOP) return false;
                else return true;
            }
        }

        public int DesignCount
        {
            get
            {
                return _designCount;
            }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 回転速度
        private SlotSpeed _rotateSpeed = SlotSpeed.NONE;
        // 図柄数
        private int _designCount = default;
        // 図柄リスト
        private SlotDesign[] _slotDesignList = default;
        // 図柄サイズリスト
        private RectTransform[] _designTransList = default;
        // 図柄座標リスト
        private Vector2[] _designPosList = default;
        // 一番上の座標
        private float _topPos = default;
        // 中央の座標
        private float _centerPos = default;
        // 一番下の座標
        private float _bottomPos = default;
        // 回転状態
        private SlotState _slotState = default;
        // 回転開始アニメーションフラグ
        private bool _isStartAnim = default;
        // 回転開始アニメーション経過時間
        private float _startAnimeTime = default;
        // ポーズフラグ
        private bool _isPause = default;
        // 高速消化フラグ
        private bool _isFast = default;
        // 停止時のコールバック
        private Action _stopCallback = default;

        private float _speedNone = 0.0f;
        private float _speedSlow = 1.5f;
        private float _speedNormal = 3.0f;
        private float _speedHigh = 10.0f;

        // ---------- Unity組込関数 ----------

        void FixedUpdate()
        {
            // ポーズ中なら動作しない
            if (!_isPause)
            {
                // 回転開始アニメーション用ロジック
                if (_isStartAnim)
                {
                    _startAnimeTime += Time.deltaTime;
                    
                    if (_startAnimeTime >= 0.4f)
                        _rotateSpeed = SlotSpeed.SLOW;

                    if (_startAnimeTime >= 0.5f)
                    {
                        _isStartAnim = false;
                        _rotateSpeed = SlotSpeed.HIGH;
                        _isRotateDown = !_isRotateDown;
                        SetReelAlpha(0.2f);
                    }
                }
                
                // リールの状態が停止以外のとき
                if (_slotState != SlotState.STOP)
                {
                    for (int i = 0; i < _designTransList.Length; i++)
                    {
                        // 図柄を回転
                        if (_isRotateDown)
                            _designTransList[i].anchoredPosition += Vector2.down * GetRotateSpeed() * Time.deltaTime;
                        else
                            _designTransList[i].anchoredPosition -= Vector2.down * GetRotateSpeed() * Time.deltaTime;

                        // 図柄の座標がリールの範囲外に出たら、図柄を移動させる
                        if (_designTransList[i].anchoredPosition.y < _bottomPos)
                        {
                            _designTransList[i].anchoredPosition =
                                new Vector2(_designTransList[i].anchoredPosition.x, _topPos);
                        }
                        else if (_designTransList[i].anchoredPosition.y > _topPos)
                        {
                            _designTransList[i].anchoredPosition =
                                new Vector2(_designTransList[i].anchoredPosition.x, _bottomPos);
                        }
                    }
                }
                
                // リールが停止命令をかけている時
                if (_slotState == SlotState.ORDER_STOP)
                {
                    _rotateSpeed = SlotSpeed.SLOW;

                    // 速度低下インデックス
                    int index = _stopDesignIndex;
                    if (_isRotateDown)
                    {
                        index += _isFast ? 1 : _slowSpdBeforeIndex;
                        if (index >= _designCount) index -= _designCount;
                    }
                    else
                    {
                        index -= _isFast ? 1 : _slowSpdBeforeIndex;
                        if (index < 0) index += _designCount;
                    }
                    UpdateDesignPositions(index);

                    _slotState = SlotState.STAND_STOP;
                    SetReelAlpha(1.0f);
                }
                
                // リールが停止状態の時
                else if (_slotState == SlotState.STAND_STOP)
                {
                    // if ((int)_designTransList[_stopDesignIndex].anchoredPosition.y == (int)_centerPos)
                    if (
                        (int)_designTransList[_stopDesignIndex].anchoredPosition.y >= (int)_centerPos - 10f &&
                        (int)_designTransList[_stopDesignIndex].anchoredPosition.y <= (int)_centerPos + 10f
                    )
                    {
                        if (_slotState == SlotState.STOP) return;
                        _slotState = SlotState.STOP;
                        PlayAnimations(SlotAnimState.Stop);
                        SetDesignAlpha(1.0f, 0.0f);
                        _stopCallback?.Invoke();

                        // HACK 時々図柄にズレが生じるので、止まる度に整形
                        UpdateDesignPositions(_stopDesignIndex);
                    }
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            if (_slotDesignPrefabs == default) return;

            _stopDesignIndex = 0;
            _slotState = SlotState.STOP;
            _isPause = false;
            _isStartAnim = false;
            _startAnimeTime = 0.0f;
            _designCount = _slotDesignPrefabs.Count;

            // 図柄生成
            CreateSlotDesigns();

            // 回転速度の設定
            SetRotateSpeed();

            // 各座標の設定
            SetPositionParameters();

            // 図柄の配置整形
            SortSlotDesigns();
        }

        // 図柄の初期化
        public void InitializeSlotDesigns(int stopDesignIndex = 0)
        {
            _stopDesignIndex = stopDesignIndex;
            UpdateDesignPositions(_stopDesignIndex);
            SetDesignAlpha(1.0f, 0.0f);
        }

        // 回転
        public void StartRotate()
        {
            PlayAnimations(SlotAnimState.Rotate);
            SetDesignAlpha(1.0f, 1.0f);
            
            if (_isAnim) 
            {
                _startAnimeTime = 0.0f;
                _isStartAnim = true;
                _isRotateDown = !_isRotateDown;
                _rotateSpeed = SlotSpeed.NORMAL;
            }
            else
            {
                _rotateSpeed = SlotSpeed.HIGH;
            }
            _slotState = SlotState.ROTATE;
        }
        
        // 図柄指定の回転
        public void StartRotate(int stopIndex)
        {
            _stopDesignIndex = stopIndex;
            StartRotate();
        }

        // 停止図柄の設定
        public void SetStopDesign(int stopIndex)
        {
            _stopDesignIndex = stopIndex;
        }
        
        // 図柄の停止指示
        public void Stop(int stopIndex, Action stopCallback = null, bool fast = false)
        {
            if (_slotState == SlotState.ROTATE)
            {
                _stopDesignIndex = stopIndex;
                _slotState = SlotState.ORDER_STOP;
                _isFast = fast;
            }
            _stopCallback = stopCallback;
        }

        // 図柄の停止(即時)
        public void RotateStop(int stopIndex)
        {
            _stopDesignIndex = stopIndex;
            _slotState = SlotState.STOP;
            UpdateDesignPositions(_stopDesignIndex);
        }

        // 停止中図柄の取得
        public int GetStopDesign()
        {
            return _stopDesignIndex;
        }
        
        // リールの透明度設定
        public void SetReelAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        // 図柄の透明度設定
        public void SetDesignAlpha(float centerAlpha, float otherAlpha)
        {
            for (int i = 0; i < _slotDesignList.Length; i++)
            {
                if (_fadeSpeed < -1)
                {
                    if (i == _stopDesignIndex) _slotDesignList[i].SetAlpha(centerAlpha);
                    else _slotDesignList[i].SetAlpha(otherAlpha);
                }
                else
                {
                    if (i == _stopDesignIndex) _slotDesignList[i].Fade(centerAlpha, _fadeSpeed);
                    else _slotDesignList[i].Fade(otherAlpha, _fadeSpeed);
                }
            }
        }

        // リールの速度設定
        public void SetRotateSpeed(SlotSpeed speed)
        {
            _rotateSpeed = speed;
        }

        // 指定図柄が中央にくるように全図柄の座標を更新
        public void UpdateDesignPositions(int designIndex)
        {
            for (int i = 0; i < _designCount; i++)
            {
                int index = designIndex + i - (_designCount / 2);
                if (index >= _designCount)
                    index -= _designCount;
                else if (index < 0)
                    index += _designCount;

                _designTransList[index].anchoredPosition = _designPosList[i];
            }
        }

        // 図柄取得
        public SlotDesign GetSlotDesign(int index)
        {
            if (_slotDesignPrefabs.Count <= index || index < 0) return null;
            return _slotDesignPrefabs[index];
        }

        // ---------- Private関数 ----------

        // 全図柄生成
        private void CreateSlotDesigns()
        {
            _slotDesignList = new SlotDesign[_designCount];
            _designTransList = new RectTransform[_designCount];
            _designPosList = new Vector2[_designCount];

            for (int i = 0; i < _designCount; i++)
            {
                SlotDesign slotDesign = CreateSlotDesign(_slotDesignPrefabs[i]);
                _slotDesignList[i] = slotDesign;
                _designTransList[i] = slotDesign.gameObject.GetComponent<RectTransform>();
            }
        }

        // 回転速度の設定
        private void SetRotateSpeed()
        {
            if (_designTransList == default || _designTransList.Length == 0) return;

            float height = _designTransList[0].sizeDelta.y;
            _speedSlow *= height;
            _speedNormal *= height;
            _speedHigh *= height;
        }

        // 図柄生成
        private SlotDesign CreateSlotDesign(SlotDesign slotDesignObj)
        {
            SlotDesign slotDesign = Instantiate(slotDesignObj);
            slotDesign.transform.SetParent(_designParent);
            slotDesign.transform.localPosition = Vector3.zero;
            slotDesign.transform.localRotation = Quaternion.identity;
            slotDesign.transform.localScale = this.gameObject.transform.localScale;
            return slotDesign;
        }

        // 図柄の配置整形
        private void SortSlotDesigns()
        {
            float height = _topPos;
            for (int i = 0; i < _designTransList.Length; i++)
            {
                Vector2 pos = new Vector2(0.0f, height);
                _designTransList[i].anchoredPosition = pos;
                _designPosList[i] = pos;
                height -= _designTransList[i].sizeDelta.y;
            }
        }

        // 各座標の設定
        private void SetPositionParameters()
        {
            _centerPos = 0.0f;
            int topCnt = _designCount / 2;
            for (int i = 0; i < topCnt; i++)
            {
                _topPos += _designTransList[i].sizeDelta.y;
            }
            for (int i = topCnt; i < _designCount; i++)
            {
                _bottomPos -= _designTransList[i].sizeDelta.y;
            }
        }
        
        // 回転速度取得
        private float GetRotateSpeed()
        {
            switch (_rotateSpeed)
            {
                case SlotSpeed.NONE:
                    return _speedNone;
                case SlotSpeed.SLOW:
                    return _speedSlow;
                case SlotSpeed.NORMAL:
                    return _speedNormal;
                case SlotSpeed.HIGH:
                    return _speedHigh;
                default:
                    return _speedNone;
            }
        }

        // 図柄のアニメーションを再生
        private void PlayAnimations(SlotAnimState animState)
        {
            for (int i = 0; i < _slotDesignList.Length; i++)
            {
                _slotDesignList[i].PlayAnimation(animState);
            }
        }

        // ---------- protected関数 ---------
    }
}


