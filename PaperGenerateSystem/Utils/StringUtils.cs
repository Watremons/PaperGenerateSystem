using System;
using System.Collections.Generic;
using System.Text;
using PaperGenerateSystem.Common.Resources;
using PaperGenerateSystem.Common.Resources.DataType;

namespace PaperGenerateSystem.Utils
{
    internal static class StringUtils
    {
        /// <summary>
        /// 获得账户类型枚举对应账户名的字符串
        /// </summary>
        /// <param name="enumTypes">账户枚举类型</param>
        /// <returns>账户枚举类型字符串</returns>
        public static string GetStringOfType(LevelType enumTypes)
        {
            switch (enumTypes)
            {
                case LevelType.PRIMARY:
                    return "小学";
                case LevelType.JUNIOR:
                    return "初中";
                case LevelType.SENIOR:
                    return "高中";
                default:
                    return "非法";
            }
        }
    }
}
