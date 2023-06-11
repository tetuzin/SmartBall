using UnityEngine;

using ShunLib.Dict;
using ShunLib.Btn.Common;
using ShunLib.UI.Panel;

namespace ShunLib.UI.Tab
{
    public class TabContent : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("タブ")]
        [SerializeField] protected TabContentTable _tabContentTable = default;

        [Header("初期表示パネルのボタン")]
        [SerializeField] protected CommonButton _firstShowPanelBtn = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------

        void Start()
        {
            Initialize();
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            // 全コンテンツの初期化
            InitializePanels();
            InitializeButtons();

            // 全コンテンツの描画
            HideAllPanel();
            ActiveAllButton();

            // 初期パネルの表示
            if (_firstShowPanelBtn == default)
            {
                if (_tabContentTable.GetKeyArray().Length > 0)
                {
                    CommonButton button = _tabContentTable.GetKeyArray()[0];
                    button.SetOnActive(false);
                    _tabContentTable.GetValue(button).Show();
                }
            }
            else
            {
                _firstShowPanelBtn.SetOnActive(false);
                _tabContentTable.GetValue(_firstShowPanelBtn).Show();
            }
        }

        // ---------- Private関数 ----------

        // 全パネル初期化
        private void InitializePanels()
        {
            foreach (CommonPanel panel in _tabContentTable.GetValueList())
            {
                panel.Initialize();
            }
        }

        // 全パネル非表示
        private void HideAllPanel()
        {
            foreach (CommonPanel panel in _tabContentTable.GetValueList())
            {
                panel.Hide();
            }
        }

        // 全ボタン初期化
        private void InitializeButtons()
        {
            foreach (CommonButton button in _tabContentTable.GetKeyList())
            {
                button.SetOnEvent(() => {
                    HideAllPanel();
                    ActiveAllButton();
                    _tabContentTable.GetValue(button).Show();
                    button.SetOnActive(false);
                });
            }
        }

        // 全ボタン活性化
        private void ActiveAllButton()
        {
            foreach (CommonButton button in _tabContentTable.GetKeyList())
            {
                button.SetOnActive(true);
            }
        }

        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


