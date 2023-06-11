using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace ShunLib.UI.Cutin.Text
{
    public class TextCutin : BaseCutin
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("表示テキストのリスト")]
        [SerializeField] private List<TextMeshProUGUI> _textList = default;

        [Header("テキスト表示間隔")]
        [SerializeField] protected float _showTextTime = 0f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            foreach (TextMeshProUGUI text in _textList)
            {
                text.gameObject.SetActive(false);
            }
        }

        // カットイン表示
        public override async Task Show(Action callback = null)
        {
            await base.Show(() => {
                ShowTextList();
                callback?.Invoke();
            });
        }

        // テキストの設定
        public void SetTextList(List<string> textList)
        {
            for (int i = 0; i < textList.Count; i++)
            {
                _textList[i].text = textList[i];
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // テキストリストを前から順番に表示
        protected virtual async void ShowTextList()
        {
            for (int i = 0; i < _textList.Count; i++)
            {
                await Task.Delay((int)_showTextTime * 1000);
                ShowText(_textList[i]);
            }
        }

        // テキストを表示
        protected virtual void ShowText(TextMeshProUGUI text)
        {
            text.gameObject.SetActive(true);
        }
    }
}

