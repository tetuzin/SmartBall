using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Manager.ScenarioUI;

using ShunLib.Utils.Resource;
using ShunLib.Utils.Json;
using ShunLib.Utils.Debug;

using ShunLib.Scenario.TextWindow;
using ShunLib.Scenario.CharacterView;
using ShunLib.Scenario.Model;

namespace ShunLib.Manager.Scenario
{
    public class ScenarioManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        // [SerializeField, Tooltip("UIマネージャー")] protected ScenarioUIManager _uiManager = default;
        [SerializeField, Tooltip("テキストウィンドウ")] protected ScenarioTextWindow _textWindow = default;
        [SerializeField, Tooltip("キャラクタービュー")] protected ScenarioCharacterView _characterView = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private ScenarioModel _curScenario = default;
        private int _curScenarioScene = default;
        private bool _isStart = default;
        private bool _isEnd = default;

        // ---------- Unity組込関数 ----------
        private void Start()
        {
            Initialize();
            LoadScenario("Scenario/TestScenario");
            StartScenario();
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _isStart = false;
            _isEnd = false;
            _textWindow.Initialize();
            _characterView.Initialize();
        }

        // シナリオ読み込み
        public void LoadScenario(string path)
        {
            string json = ResourceUtils.GetResourcesJson(path);
            _curScenario = JsonUtils.ConvertJsonToClass<ScenarioModel>(json);
        }

        // シーンスタート
        public void StartScenario()
        {
            if (_curScenario == null || _curScenario == default) return;
            DebugUtils.Log("シナリオ[" + _curScenario.ScenarioName + "]開始");
            _isStart = true;
            _curScenarioScene = -1;
        }

        // 次のシーン
        public void NextScenario()
        {
            if (_isStart != true || _isEnd == true) return;

            // 次のシーンへ進める
            _curScenarioScene++;

            if (_curScenarioScene >= _curScenario.DialogList.Count)
            {
                EndScenario();
                return;
            }

            DialogModel model = _curScenario.DialogList[_curScenarioScene];

            DoDialogActions(model);

            // キャラクター名の設定
            if (model.CharacterName == "" || model.CharacterName == null)
            {
                _textWindow.SetActiveNameWindow(false);
            }
            else
            {
                _textWindow.SetNameText(model.CharacterName);
                _textWindow.SetActiveNameWindow(true);
            }

            // メインテキストの設定
            _textWindow.SetMainText(model.MainText);
            _textWindow.SetActiveBaseWindow(true);
        }

        // シーンエンド
        public void EndScenario()
        {
            DebugUtils.Log("シナリオ[" + _curScenario.ScenarioName + "]終了");
            _isEnd = true;
            _textWindow.SetActiveWindow(false);
            _characterView.DeleteStandCharacter();
        }

        // ---------- Private関数 ----------

        // アクションをすべて実行
        private void DoDialogActions(DialogModel model)
        {
            for (int i = 0; i < model.Actions.Count; i++)
            {
                DialogActionModel action = model.Actions[i];
                switch (action.Type)
                {
                    case ActionType.STAND_CHARACTER:
                        DoStandCharacterAction(action);
                        break;
                    default:
                        break;
                }
            }
        }

        // キャラ立ち絵のアクションを実行
        private void DoStandCharacterAction(DialogActionModel action)
        {
            switch (action.NumParam1)
            {
                case StandCharacterConst.SHOW:
                    _characterView.SetActiveStandCharacter(action, true);
                    break;
                case StandCharacterConst.HIDE:
                    _characterView.SetActiveStandCharacter(action, false);
                    break;
                case StandCharacterConst.MOVE:
                    _characterView.MoveStandCharacter(action);
                    break;
                default:
                    break;
            }
        }

        // ---------- protected関数 ---------
    }
}

