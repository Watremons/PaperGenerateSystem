using System;
using System.Collections.Generic;
using System.Text;

namespace PaperGenerateSystem.Common.Resources
{
    internal enum ProcessStatus
    {
        /// <summary>
        /// 进程退出
        /// </summary>
        EXIT = -1,

        /// <summary>
        /// 进程处于初始化状态
        /// </summary>
        INIT = 0,

        /// <summary>
        /// 进程处于登录状态
        /// </summary>
        LOGIN = 1,
        
        /// <summary>
        /// 进程处于工作状态
        /// </summary>
        WORKING = 2
    }
}
