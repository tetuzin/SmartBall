using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Pachinko.FinalBattle.ChargeIcon
{
    public enum ShowIconAnimState
    {
        NONE,
        CUTIN_AND_TOP_SLIDE,
    }
    public enum ShowChanceUpAnimState
    {
        NONE,
        FLASH,
    }
    public class FinalBattleChargeIcon : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("アイコン画像")] 
        [SerializeField] private Image _iconImage = default;

        [Header("フレーム画像")] 
        [SerializeField] private Image _frameImage = default;

        [Header("キャンバスグループ")] 
        [SerializeField] private CanvasGroup _canvasGroup = default;

        [Header("昇格オブジェクト")] 
        [SerializeField] private CanvasGroup _chanceUpObject = default;

        [Header("フラッシュ用画像")] 
        [SerializeField] private Image _flashImage = default;

        [Header("アイコン表示アニメーション")] 
        [SerializeField] private ShowIconAnimState _showIconAnimState = default;

        [Header("昇格表示アニメーション")] 
        [SerializeField] private ShowChanceUpAnimState _showChanceUpAnimState = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        private float _localPosX = default;
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _chanceUpObject.alpha = 0f;
            _chanceUpObject.interactable = false;
            _chanceUpObject.blocksRaycasts = false;
            _flashImage.gameObject.SetActive(false);
            _localPosX = this.gameObject.transform.localPosition.x;
        }

        // 活性状態の取得
        public bool GetActive()
        {
            return _canvasGroup.alpha != 0f;
        }

        // アイコンの表示・非表示
        public void SetActive(bool b)
        {
            _canvasGroup.alpha = b ? 1f : 0f;
            _canvasGroup.interactable = b;
            _canvasGroup.blocksRaycasts = b;
        }

        // アイコン表示アニメーション
        public void ShowIconAnim()
        {
            switch (_showIconAnimState)
            {
                case ShowIconAnimState.CUTIN_AND_TOP_SLIDE:
                    RectTransform iconRect = _canvasGroup.gameObject.GetComponent<RectTransform>();
                    iconRect.anchoredPosition = new Vector2(0f, 1080f);
                    SetActive(true);
                    iconRect.DOLocalMoveY(0f, 0.05f);
                    break;
                default:
                    SetActive(true);
                    break;
            }
        }

        // アイコン画像の設定
        public void SetIconImage(Sprite sprite)
        {
            _iconImage.sprite = sprite;
            // _iconImage.SetNativeSize();
            _iconImage.maskable = true;
        }

        // フレーム画像の色設定
        public void SetFrameColor(Color color)
        {
            _frameImage.color = color;
        }

        // アイコンの点滅
        public async Task FlashIcon()
        {
            for (int i = 0; i < 5; i++)
            {
                _flashImage.gameObject.SetActive(true);
                await Task.Delay(100);
                _flashImage.gameObject.SetActive(false);
                await Task.Delay(100);
            }
            _flashImage.gameObject.SetActive(false);
            await Task.CompletedTask;
        }

        // 昇格アニメーションの再生
        public async void ShowChanceUpAnim()
        {
            switch (_showChanceUpAnimState)
            {
                case ShowChanceUpAnimState.FLASH:
                    for (int i = 0; i < 5; i++)
                    {
                        _chanceUpObject.alpha = 1f;
                        await Task.Delay(50);
                        _chanceUpObject.alpha = 0f;
                        await Task.Delay(50);
                    }
                    break;
                default:
                    _chanceUpObject.alpha = 1f;
                    await Task.Delay(500);
                    _chanceUpObject.alpha = 0f;
                    break;
            }
        }

        // 初期位置を返す
        public float GetDefaultPosX()
        {
            return _localPosX;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}

