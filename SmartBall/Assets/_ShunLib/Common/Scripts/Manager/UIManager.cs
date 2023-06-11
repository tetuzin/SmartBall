using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

using ShunLib.Dict;
using ShunLib.Popup;
using ShunLib.Btn.Common;
using ShunLib.Toggle.Common;
using ShunLib.Utils.Popup;
using ShunLib.UI;
using ShunLib.UI.Cutin;
using ShunLib.UI.Panel;
using ShunLib.UI.Input;

namespace ShunLib.Manager.UI
{
    public class UIManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("キャンバス")]
        //  Unityの2020以降でしか"CommonDictionary"が使えない 2022/11/01 西
        // [SerializeField] protected CommonDictionary<string, Canvas> _canvasList = new CommonDictionary<string, Canvas>();
        [SerializeField] protected CanvasTable _canvasList = new CanvasTable();

        [Header("テキスト")]
        // [SerializeField] protected CommonDictionary<string, TextMeshProUGUI> _textList = new CommonDictionary<string, TextMeshProUGUI>();
        [SerializeField] protected TextMeshProUGUITable _textList = new TextMeshProUGUITable();

        [Header("ボタン")]
        // [SerializeField] protected CommonDictionary<string, CommonButton> _buttonList = new CommonDictionary<string, CommonButton>();
        [SerializeField] protected CommonButtonTable _buttonList = new CommonButtonTable();

        [Header("トグル")]
        // [SerializeField] protected CommonDictionary<string, CommonToggle> _toggleList = new CommonDictionary<string, CommonToggle>();
        [SerializeField] protected CommonToggleTable _toggleList = new CommonToggleTable();

        [Header("画像")]
        // [SerializeField] protected CommonDictionary<string, Image> _imageList = new CommonDictionary<string, Image>();
        [SerializeField] protected ImageTable _imageList = new ImageTable();

        [Header("パネル")]
        [SerializeField] protected CommonPanelTable _panelList = new CommonPanelTable();

        [Header("入力フォーム")]
        [SerializeField] protected CommonInputFieldTable _inputFieldList = new CommonInputFieldTable();

        [Header("キャンバスグループ")]
        // [SerializeField] protected CommonDictionary<string, CanvasGroup> _canvasGroupList = new CommonDictionary<string, CanvasGroup>();
        [SerializeField] protected CanvasGroupTable _canvasGroupList = new CanvasGroupTable();

        [Header("ポップアップ")]
        [SerializeField, Tooltip("ポップアップ生成時の親オブジェクト")] protected GameObject _popupParent = default;
        [SerializeField] protected BasePopupTable _PopupList = new BasePopupTable();

        [Header("カットイン")]
        [SerializeField] protected CutinTable _cutinTable = new CutinTable();

        [Header("活性切り替えUI")]
        [SerializeField] protected ActiveSwitchUITable _activeSwitchTable = new ActiveSwitchUITable();

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        // 初期化
        public virtual void Initialize()
        {
            InitPopup();
            InitButtons();
            InitCanvasGroup();
            InitToggles();
            InitCutin();
            InitPanel();
            InitInputField();
        }

        // テキストの表示・非表示
        public void SetTextActive(string key, bool isActive)
        {
            if (!_textList.IsValue(key)) return;
            _textList.GetValue(key).gameObject.SetActive(isActive);
        }
        
        // テキストに文字列の設定
        public void SetText(string key, string text)
        {
            if (!_textList.IsValue(key)) return;
            _textList.GetValue(key).text = text;
        }

        // テキストに文字列を追加
        public void AddText(string key, string text)
        {
            if (!_textList.IsValue(key)) return;
            _textList.GetValue(key).text += text;
        }
        
        // テキストの色の設定
        public void SetTextColor(string key, Color color)
        {
            if (!_textList.IsValue(key)) return;
            _textList.GetValue(key).color = color;
        }
        
        // ボタンの表示・非表示
        public void SetButtonActive(string key, bool isActive)
        {
            if (!_buttonList.IsValue(key)) return;
            _buttonList.GetValue(key).gameObject.SetActive(isActive);
        }
        
        // ボタンのイベント設定
        public void SetButtonEvent(string key, UnityAction action)
        {
            if (!_buttonList.IsValue(key)) return;
            _buttonList.GetValue(key).SetOnEvent(() =>
            {
                action();
            });
        }

        // トグルの表示・非表示
        public void SetToggleActive(string key, bool isActive)
        {
            if (!_toggleList.IsValue(key)) return;
            _toggleList.GetValue(key).gameObject.SetActive(isActive);
        }

        // トグルの値設定
        public void SetToggleValue(string key, bool value)
        {
            if (!_toggleList.IsValue(key)) return;
            _toggleList.GetValue(key).SetValue(value);
        }

        // トグルのイベント設定
        public void SetToggleAction(string key, Action onAction = null, Action offAction = null)
        {
            if (!_toggleList.IsValue(key)) return;
            _toggleList.GetValue(key).SetIsOnAction(onAction);
            _toggleList.GetValue(key).SetIsOffAction(offAction);
        }

        // 画像の取得
        public Image GetImage(string key)
        {
            if (!_imageList.IsValue(key)) return null;
            return _imageList.GetValue(key);
        }
        
        // 画像の表示・非表示
        public void SetImageActive(string key, bool isActive)
        {
            if (!_imageList.IsValue(key)) return;
            _imageList.GetValue(key).gameObject.SetActive(isActive);
        }
        
        // 画像色の変更
        public void SetImageColor(string key, Color color)
        {
            if (!_imageList.IsValue(key)) return;
            _imageList.GetValue(key).color = color;
        }

        // パネルの表示・非表示
        public void SetPanelActive(string key, bool isActive)
        {
            if (!_panelList.IsValue(key)) return;
            if (isActive)
            {
                _panelList.GetValue(key).Show();
            }
            else
            {
                _panelList.GetValue(key).Hide();
            }
        }
        
        // キャンバスグループの表示・非表示
        public void SetCanvasGroupActive(string key, bool isActive)
        {
            if (!_canvasGroupList.IsValue(key)) return;
            _canvasGroupList.GetValue(key).alpha = isActive ? 1 : 0;
            _canvasGroupList.GetValue(key).interactable = isActive;
            _canvasGroupList.GetValue(key).blocksRaycasts = isActive;
        }

        // キャンバスグループのフェード
        public void FadeCanvasGroup(string key, bool isActive, float speed, Action callback = null)
        {
            if (!_canvasGroupList.IsValue(key)) return;
            _canvasGroupList.GetValue(key).DOFade(isActive ? 1f : 0f, speed).OnComplete(() => {
                _canvasGroupList.GetValue(key).alpha = isActive ? 1 : 0;
                _canvasGroupList.GetValue(key).interactable = isActive;
                _canvasGroupList.GetValue(key).blocksRaycasts = isActive;
                callback?.Invoke();
            });
        }
        
        // キャンバスグループのアルファ値設定
        public void SetCanvasGroupAlpha(string key, float alpha)
        {
            if (!_canvasGroupList.IsValue(key)) return;
            if (alpha > 1.0f)
            {
                _canvasGroupList.GetValue(key).alpha = 1.0f;
            }
            else if (alpha < 0)
            {
                _canvasGroupList.GetValue(key).alpha = 0.0f;
            }
            else
            {
                _canvasGroupList.GetValue(key).alpha = alpha;
            }
        }

        // ポップアップ生成親オブジェクトの設定
        public void SetPopupParent(GameObject parent)
        {
            _popupParent = parent;
        }

        // ポップアップ生成親オブジェクトを返す
        public GameObject GetPopupParent()
        {
            return _popupParent;
        }

        // ポップアップの表示
        public void ShowPopup(string key, bool isModal = true)
        {
            if (!_PopupList.IsValue(key)) return;
            _PopupList.GetValue(key).Open(isModal);
        }
        
        // ポップアップの設定
        public void SetPopup(string key, Dictionary<string, Action> actions)
        {
            if (!_PopupList.IsValue(key)) return;
            _PopupList.GetValue(key).InitPopup(actions);
        }
        
        // ポップアップの生成と表示
        public void CreateOpenPopup(
            string key,
            Dictionary<string, Action> actions = null,
            Action<BasePopup> popupAction = null,
            bool isModal = true
        )
        {
            if (!_PopupList.IsValue(key)) return;
            if (_popupParent == default)
            {
                Debug.LogWarning("ポップアップを生成するための親オブジェクトが設定されていません！");
                return;
            }
            BasePopup popup = PopupUtils.OpenPopup(
                _popupParent,
                _PopupList.GetValue(key).gameObject,
                actions
            );
            if (popup != null) 
            {
                popupAction?.Invoke(popup);
                popup.Open(isModal);   
            }
        }

        // カットインの表示
        public async Task ShowCutin(string key, Action action = null, float showTime = -1f)
        {
            if (!_cutinTable.IsValue(key))
            {
                action?.Invoke();
                return;
            }
            BaseCutin obj = Instantiate(
                _cutinTable.GetValue(key),
                _popupParent.transform
            );
            if (showTime != -1f) obj.SetShowTime(showTime);
            obj.Position.anchoredPosition = _cutinTable.GetValue(key).Position.anchoredPosition;
            obj.Initialize();
            obj.SetCallback(() => {
                action?.Invoke();
                Destroy(obj.gameObject);
            });
            await obj.Show();
        }

        // カットインの取得
        public BaseCutin GetCutin(string key, Action action = null, float showTime = -1f)
        {
            if (!_cutinTable.IsValue(key))
            {
                action?.Invoke();
                return null;
            }
            BaseCutin obj = Instantiate(
                _cutinTable.GetValue(key),
                _popupParent.transform
            );
            if (showTime != -1f) obj.SetShowTime(showTime);
            obj.Position.anchoredPosition = _cutinTable.GetValue(key).Position.anchoredPosition;
            obj.Initialize();
            obj.SetCallback(() => {
                action?.Invoke();
                Destroy(obj.gameObject);
            });
            return obj;
        }

        // 活性切り替えUIの活性状態設定
        public void SetActiveSwitchUI(string key, bool b)
        {
            if (!_activeSwitchTable.IsValue(key)) return;
            _activeSwitchTable.GetValue(key).SetActiveGrayout(!b);
        }

        // 活性切り替えUIの活性状態設定
        public void SetOnlyActiveSwitchUI(string key)
        {
            if (!_activeSwitchTable.IsValue(key)) return;
            foreach (ActiveSwitchUI obj in _activeSwitchTable.GetValueList())
            {
                obj.SetActiveGrayout(true);
            }
            _activeSwitchTable.GetValue(key).SetActiveGrayout(false);
        }
        
        // ---------- Private関数 ----------

        // ポップアップの初期化
        private void InitPopup()
        {
            PopupUtils.InitOpenPopupList();
        }

        // 全ボタンの初期化
        private void InitButtons()
        {
            foreach (CommonButton button in _buttonList.GetValueArray())
            {
                button.RemoveOnEvent();
            }
        }

        // 全キャンバスグループの初期化
        private void InitCanvasGroup()
        {
            foreach (CanvasGroup cg in _canvasGroupList.GetValueArray())
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        // 全トグルの初期化
        private void InitToggles()
        {
            foreach (CommonToggle toggle in _toggleList.GetValueArray())
            {
                toggle.Initialize();
            }
        }

        // カットインの初期化
        private void InitCutin()
        {
            foreach (BaseCutin obj in _cutinTable.GetValueArray())
            {
                obj.Initialize();
            }
        }

        // パネルの初期化
        private void InitPanel()
        {
            foreach (CommonPanel panel in _panelList.GetValueArray())
            {
                panel.Initialize();
            }
        }

        // 入力フォームの初期化
        private void InitInputField()
        {
            foreach (CommonInputField inputField in _inputFieldList.GetValueArray())
            {
                inputField.Initialize();
            }
        }

        // ---------- protected関数 ---------
    }
}