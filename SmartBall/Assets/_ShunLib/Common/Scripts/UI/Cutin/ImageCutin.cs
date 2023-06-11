using UnityEngine;
using UnityEngine.UI;

namespace ShunLib.UI.Cutin.Img
{
    public class ImageCutin : BaseCutin
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("カットイン画像")]
        [SerializeField] private Image _cutinImage = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 画像の設定
        public void SetCutinImage(Sprite sprite)
        {
            _cutinImage.sprite = sprite;
            _cutinImage.SetNativeSize();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


