using ShunLib.UI.Panel.ActiveSwitch;
using ShunLib.UI.Cutin.Button;
using Pachinko.ModeSelect.Manager;

namespace Pachinko.GameMode.Select.Panel
{
    public class GameModeSelectPanel : ActiveSwitchUIPanel
    {
        // ---------- 定数宣言 ----------

        private const string MODE_SELECT_BUTTON = "mode_select_button";

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // パネルの表示
        public void ShowModeSelect(ModeSelectManager manager, float showTime)
        {
            ButtonCutin button = (ButtonCutin)GetCutin(MODE_SELECT_BUTTON, () => {
                Hide();
            });
            button.SetShowTime(showTime);
            manager.PushButtonCallback = ChangeActiveSwitchUI;
            button.ShowButtonCutin();
            Show();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


