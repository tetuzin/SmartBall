using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ShunLib.Popup;
using ShunLib.Btn.Common;
using ShunLib.Strix.Manager.Master;

namespace ShunLib.Strix.Popup.Connect
{
    public class ConnectPopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("テキストオブジェクト")]
        [SerializeField] protected TextMeshProUGUI _mainText;
        [Header("接続中文言")]
        [SerializeField] protected string _connectStartStr;
        [Header("接続成功時文言")]
        [SerializeField] protected string _connectStr;
        [Header("接続失敗時文言")]
        [SerializeField] protected string _connectFailedStr;
        [Header("キャンセルボタン")]
        [SerializeField] protected CommonButton _cancelButton = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isEnd = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------

        // メイン文言の設定
        private void SetMainText(string text)
        {
            if (_mainText == default) return;

            _mainText.text = text;
        }

        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            _isEnd = false;

            // Strixマスタサーバへの接続
            StrixMasterManager.Instance.Initialize();
            StrixMasterManager.Instance.SetOnConnectStartCallbackEvent(() => {
                SetMainText(_connectStartStr);
            });
            
            StrixMasterManager.Instance.SetOnConnectCallbackEvent((args) => {
                _isEnd = true;
                SetMainText(_connectStr);
            });

            StrixMasterManager.Instance.SetOnConnectFailedCallbackEvent((args) => {
                _isEnd = true;
                SetMainText(_connectFailedStr);
            });

            StrixMasterManager.Instance.Connect();
        }

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {
            if (_cancelButton != default)
            {
                _cancelButton.SetOnEvent(() => {
                    if (!_isEnd) StrixMasterManager.Instance.DisConnect();
                    Close();
                });
            }
        }
    }
}


