using System;

namespace ShunLib.Utils.Date
{
    public class DateTimeUtils
    {
        // 現在時刻を取得
        public static DateTime GetNow()
        {
            return DateTime.Now;
        }

        // DateTimeを文字列に変換
        // 文字列変換フォーマットメモ
        // 年/月/日(yyyy/MM/dd)
        // 日時(HH:mm:ss)
        public static string ConvertString(DateTime date, string format)
        {
            return date.ToString(format);
        }

        // 文字列をDateTimeに変換
        public static DateTime ConvertDateTime(string date)
        {
            DateTime dateTime;
            if (DateTime.TryParse(date, out dateTime))
            {
                return dateTime;
            }
            else
            {
                UnityEngine.Debug.LogWarning("文字列[" + date + "]をDateTime型に変換できません");
                return new DateTime();
            }
        }

        // DateTimeの比較
        // -1 なら前の日時, 0 なら同じ, 1 なら先の日時
        public static int CompareDateTime(DateTime value, DateTime compareValue)
        {
            return value.CompareTo(compareValue);
        }
    }
}