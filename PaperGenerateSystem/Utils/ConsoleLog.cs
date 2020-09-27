using System;
using System.Collections.Generic;
using System.Text;
using PaperGenerateSystem.Common.Resources;

namespace PaperGenerateSystem.Utils
{
    internal static class ConsoleLog
    {
        /// <summary>
        /// 二次确认函数，提示用户确认操作
        /// </summary>
        /// <param name="sTip">函数提示信息</param>
        /// <param name="enumInitStatus">初始状态状态</param>
        /// <param name="enumTargetStatus">函数返回状态</param>
        /// <returns>返回0则取消，返回-1退出，返回1重复</returns>
        public static ProcessStatus TwoFactorCheck(string sTip, ProcessStatus enumInitStatus, ProcessStatus enumTargetStatus)
        {
            Console.Clear();
            Console.WriteLine($"请问确认要{sTip}吗？" +
                              "确认输入Y，取消输入N");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Y:
                    {
                        Console.Write(keyInfo.KeyChar.ToString());
                        Console.Clear();
                        return enumTargetStatus;
                    }
                    case ConsoleKey.N:
                    {
                        Console.Write(keyInfo.KeyChar.ToString());
                        Console.Clear();
                        return enumInitStatus;
                    }
                    default:
                    {
                        Console.Write(keyInfo.KeyChar.ToString());
                        Console.WriteLine("\r\n您的输入有误，请重新输入Y/N");
                        break;
                    }
                }
            }

        }
    }
}
