using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace ShunLib.UI.Cutin
{
    public enum ShowAnimState
    {
        NONE,
        TOP_SLIDE,
        BOTTOM_SLIDE,
        RIGHT_SLIDE,
        LEFT_SLIDE,
        SCALE,
        FADE,
        FADE_TOP_SLIDE,
        FADE_BOTTOM_SLIDE,
        FADE_RIGHT_SLIDE,
        FADE_LEFT_SLIDE,
        FADE_SCALE,
        BOTTOM_RIGHT_SLIDE,
    }
    public enum HideAnimState
    {
        NONE,
        TOP_SLIDE,
        BOTTOM_SLIDE,
        RIGHT_SLIDE,
        LEFT_SLIDE,
        SCALE,
        FADE,
        FADE_TOP_SLIDE,
        FADE_BOTTOM_SLIDE,
        FADE_RIGHT_SLIDE,
        FADE_LEFT_SLIDE,
        FADE_SCALE,
        TOP_LEFT_SLIDE,
    }
    public class BaseCutin : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("キャンバスグループ")]
        [SerializeField] protected CanvasGroup _canvasGroup = default;

        [Header("座標")]
        [SerializeField] protected RectTransform _pos = default;

        [Header("カットイン表示時間")]
        [SerializeField] protected float _showTime = 3f;

        [Header("表示アニメーション")]
        [SerializeField] protected ShowAnimState _showAnim = ShowAnimState.NONE;
        [Header("非表示アニメーション")]
        [SerializeField] protected HideAnimState _hideAnim = HideAnimState.NONE;
        [Header("アニメーション速度")]
        [SerializeField] protected float _animTime = 0.25f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public RectTransform Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        protected bool _isShow = default;
        protected Action _callback = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {
            _isShow = false;
            SetActive(false);
        }

        // カットイン表示
        public virtual async Task Show(Action callback = null)
        {
            if (!_isShow)
            {
                _isShow = true;
                ShowAnimation(callback);
                await Task.Delay((int)(_showTime * 1000));
                Hide();
            }
        }

        // カットイン非表示
        public virtual void Hide()
        {
            if (_isShow)
            {
                _isShow = false;
                HideAnimation(_callback);
            }
        }

        // 表示・非表示
        public virtual void SetActive(bool b)
        {
            _canvasGroup.alpha = b ? 1 : 0;
            _canvasGroup.interactable = b;
            _canvasGroup.blocksRaycasts = b;
        }

        // カットイン表示時間の設定
        public void SetShowTime(float showTime)
        {
            _showTime = showTime;
        }

        // カットイン後のコールバック設定
        public virtual void SetCallback(Action callback = null)
        {
            _callback = callback;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 表示アニメーション再生
        protected virtual void ShowAnimation(Action callback = null)
        {
            switch (_showAnim)
            {
                // スケール０から拡大表示
                case ShowAnimState.SCALE:
                    _canvasGroup.transform.localScale = Vector3.zero;
                    SetActive(true);
                    _canvasGroup.transform.DOScale(Vector3.one, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 上側からスライドして表示
                case ShowAnimState.TOP_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        0f, this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                    );
                    SetActive(true);
                    _canvasGroup.transform.DOLocalMoveY(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 下側からスライドして表示
                case ShowAnimState.BOTTOM_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        0f, -this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                    );
                    SetActive(true);
                    _canvasGroup.transform.DOLocalMoveY(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 右側からスライドして表示
                case ShowAnimState.RIGHT_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f
                    );
                    SetActive(true);
                    _canvasGroup.transform.DOLocalMoveX(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 左側からスライドして表示
                case ShowAnimState.LEFT_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f
                    );
                    SetActive(true);
                    _canvasGroup.transform.DOLocalMoveX(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // フェードして表示
                case ShowAnimState.FADE:
                    _canvasGroup.DOFade(1f, _animTime).OnComplete(() => {
                        SetActive(true);
                        callback?.Invoke();
                    });
                    break;

                // スケール０から拡大表示(フェード)
                case ShowAnimState.FADE_SCALE:
                    _canvasGroup.transform.localScale = Vector3.zero;
                    SetActive(true);
                    _canvasGroup.DOFade(1f, _animTime / 2);
                    _canvasGroup.transform.DOScale(Vector3.one, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 上側からスライドして表示(フェード)
                case ShowAnimState.FADE_TOP_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        0f, this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                    );
                    SetActive(true);
                    _canvasGroup.DOFade(1f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveY(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 下側からスライドして表示(フェード)
                case ShowAnimState.FADE_BOTTOM_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        0f, -this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                    );
                    SetActive(true);
                    _canvasGroup.DOFade(1f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveY(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 右側からスライドして表示(フェード)
                case ShowAnimState.FADE_RIGHT_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f
                    );
                    SetActive(true);
                    _canvasGroup.DOFade(1f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveX(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 左側からスライドして表示(フェード)
                case ShowAnimState.FADE_LEFT_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.x, 0f, 0f
                    );
                    SetActive(true);
                    _canvasGroup.DOFade(1f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveX(0f, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // 左下からスライドして表示
                case ShowAnimState.BOTTOM_RIGHT_SLIDE:
                    _canvasGroup.transform.localPosition = new Vector3(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.x,
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                    );
                    SetActive(true);
                    _canvasGroup.transform.DOLocalMove(Vector3.zero, _animTime).OnComplete(() => {
                        callback?.Invoke();
                    });
                    break;

                // アニメーション無しで表示
                default:
                    SetActive(true);
                    callback?.Invoke();
                    break;
            }
        }

        // 非表示アニメーション再生
        protected virtual void HideAnimation(Action callback = null)
        {
            switch (_hideAnim)
            {
                // スケール１から縮小非表示
                case HideAnimState.SCALE:
                    _canvasGroup.transform.localScale = Vector3.one;
                    _canvasGroup.transform.DOScale(Vector3.zero, _animTime).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 上側へスライドして非表示
                case HideAnimState.TOP_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.transform.DOLocalMoveY(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.y, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 下側へスライドして非表示
                case HideAnimState.BOTTOM_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.transform.DOLocalMoveY(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.y, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 右側へスライドして非表示
                case HideAnimState.RIGHT_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.transform.DOLocalMoveX(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.x, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 左側へスライドして非表示
                case HideAnimState.LEFT_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.transform.DOLocalMoveX(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.x, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // フェードして非表示
                case HideAnimState.FADE:
                    _canvasGroup.DOFade(0f, _animTime).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // スケール１から縮小非表示(フェード)
                case HideAnimState.FADE_SCALE:
                    _canvasGroup.transform.localScale = Vector3.one;
                    _canvasGroup.DOFade(0f, _animTime / 2);
                    _canvasGroup.transform.DOScale(Vector3.zero, _animTime).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 上側へスライドして非表示(フェード)
                case HideAnimState.FADE_TOP_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.DOFade(0f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveY(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.y, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 下側へスライドして非表示(フェード)
                case HideAnimState.FADE_BOTTOM_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.DOFade(0f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveY(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.y, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 右側へスライドして非表示(フェード)
                case HideAnimState.FADE_RIGHT_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.DOFade(0f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveX(
                        this.gameObject.GetComponent<RectTransform>().sizeDelta.x, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 左側へスライドして非表示(フェード)
                case HideAnimState.FADE_LEFT_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.DOFade(0f, _animTime / 2);
                    _canvasGroup.transform.DOLocalMoveX(
                        -this.gameObject.GetComponent<RectTransform>().sizeDelta.x, _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // 右上へスライドして非表示
                case HideAnimState.TOP_LEFT_SLIDE:
                    _canvasGroup.transform.localPosition = Vector3.zero;
                    _canvasGroup.transform.DOLocalMove(
                        new Vector3(
                            -this.gameObject.GetComponent<RectTransform>().sizeDelta.x,
                            this.gameObject.GetComponent<RectTransform>().sizeDelta.y, 0f
                        ), _animTime
                    ).OnComplete(() => {
                        SetActive(false);
                        callback?.Invoke();
                    });
                    break;

                // アニメーション無しで表示
                default:
                    SetActive(false);
                    callback?.Invoke();
                    break;
            }
        }
    }
}


