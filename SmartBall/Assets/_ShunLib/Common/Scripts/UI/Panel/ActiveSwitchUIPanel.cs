namespace ShunLib.UI.Panel.ActiveSwitch
{
    public class ActiveSwitchUIPanel : CommonPanel
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private int _curindex = default;
        private string[] _activeSwitchUIKeyList = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _curindex = 0;
            _activeSwitchUIKeyList = _activeSwitchTable.GetKeyArray();

            SetActiveSwitchUI(_curindex);
        }

        // UIの切り替え(指定)
        public void SetActiveSwitchUI(int index)
        {
            if (index >= _activeSwitchUIKeyList.Length) return;
            _curindex = index;
            string key = _activeSwitchUIKeyList[_curindex];
            SetOnlyActiveSwitchUI(key);
        }

        // UIの切り替え(順番)
        public void ChangeActiveSwitchUI()
        {
            _curindex++;
            if (_curindex >= _activeSwitchUIKeyList.Length) _curindex = 0;
            string key = _activeSwitchUIKeyList[_curindex];
            SetOnlyActiveSwitchUI(key);
        }

        // 現在の表示番号を取得
        public int GetCurIndex()
        {
            return _curindex;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


