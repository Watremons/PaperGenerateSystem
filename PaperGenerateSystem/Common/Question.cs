using PaperGenerateSystem.Common.Resources.DataType;
using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace PaperGenerateSystem.Common
{
    /// <summary>
    /// 题目
    /// </summary>
    [SugarTable("questions")]
    internal class Question
    {
        #region 属性
        /// <summary>
        /// 题目拥有者
        /// </summary>
        [SugarColumn(ColumnName = "owner", ColumnDataType = "VARCHAR")]
        public string m_owner { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        [SugarColumn(ColumnName = "stem", ColumnDataType = "VARCHAR", IsPrimaryKey = true)]
        public string m_stem { get; set; }

        /// <summary>
        /// 题目答案
        /// </summary>
        [SugarColumn(ColumnName = "answer", ColumnDataType = "VARCHAR", IsNullable = true)]
        public string m_answer { get; set; }

        /// <summary>
        /// 题目等级
        /// </summary>
        [SugarColumn(ColumnName = "level", ColumnDataType = "INTEGER")]
        public LevelType m_questionLevel { get; set; }
        #endregion

    }
}
