using UnityEngine;
using TMPro;

namespace ShunLib.UI.Input
{
    public class CommonInputField : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("入力フォーム")]
        [SerializeField] protected TMP_InputField _inputField = default;
        [Header("入力テキスト")]
        [SerializeField] protected TextMeshProUGUI _inputText = default;
        [Header("未入力時テキスト")]
        [SerializeField] protected TextMeshProUGUI _emptyText = default;
        [Header("未入力時に表示する文言")]
        [SerializeField] protected string _emptyStr = "Enter Text...";
        [Header("パスワード入力用")]
        [SerializeField] protected bool _isPassword = false;
        [Header("入力文字数")]
        [SerializeField] protected int _inputMaxCount = 0;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _inputField.characterLimit = _inputMaxCount;
            // _inputField.wasCanceled = true;
            _emptyText.text = _emptyStr;
        }

        // 入力テキスト削除
        public void CleanText()
        {
            _inputField.text = default;
        }

        // 入力テキスト取得
        public string GetInputText()
        {
            if (_inputField.text == null || _inputField.text == default || _inputField.text == "")
            {
                return "";
            }
            return _inputField.text;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}

