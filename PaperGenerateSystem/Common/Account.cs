using PaperGenerateSystem.Common.Resources.DataType;
using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace PaperGenerateSystem.Common
{
    /// <summary>
    /// 账户表定义
    /// </summary>
    [SugarTable("accounts")]
    internal class Account
    {
        #region 属性
        /// <summary>
        /// 账户类型
        /// </summary>
        [SugarColumn(ColumnName = "type", ColumnDataType = "INTEGER")]
        public LevelType m_type { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        [SugarColumn(ColumnName = "username", ColumnDataType = "VARCHAR",IsPrimaryKey = true)]
        public string m_username { get; set; }

        /// <summary>
        /// 账户密码
        /// </summary>
        [SugarColumn(ColumnName = "password", ColumnDataType = "VARCHAR")]
        public string m_password { get; set; }

        /// <summary>
        /// 账户备注信息
        /// </summary>
        [SugarColumn(ColumnName = "info", ColumnDataType = "VARCHAR")]
        public string m_info { get; set; }
        #endregion
    }
}
