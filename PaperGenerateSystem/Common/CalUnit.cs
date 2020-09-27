using PaperGenerateSystem.Common.Resources.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperGenerateSystem.Common
{
    internal class CalUnit
    {
        #region 属性
        /// <summary>
        /// 运算单元的内容
        /// </summary>
        public string m_unitContent{ get; set; }
        /// <summary>
        /// 运算单元的优先级
        /// </summary>
        public UnitPriority m_unitPriority{ get; set; }
        #endregion

        #region 构造函数
        public CalUnit(string sUnitContent, UnitPriority enumUnitPriority)
        {
            m_unitContent = sUnitContent;
            m_unitPriority = enumUnitPriority;
        }
        #endregion

    }
}
