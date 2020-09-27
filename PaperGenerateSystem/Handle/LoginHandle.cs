using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuctionBot.Code.SqliteTool;
using PaperGenerateSystem.Common;
using PaperGenerateSystem.Common.Resources;
using PaperGenerateSystem.Common.Resources.DataType;
using PaperGenerateSystem.Utils;
using SqlSugar;

namespace PaperGenerateSystem.Handle
{
    internal class LoginHandle
    {
        #region 属性
        /// <summary>
        /// 待检测账户
        /// </summary>
        private string m_username { get; set; }
        /// <summary>
        /// 待检测密码
        /// </summary>
        private string m_password { get; set; }

        /// <summary>
        /// 数据库路径
        /// </summary>
        private string m_dbPath { get; set; }
        #endregion

        #region 构造函数

        public LoginHandle(string sUsername, string sPassword, string sDBPath)
        {
            m_username = sUsername;
            m_password = sPassword;
            m_dbPath = sDBPath;
        }
        #endregion

        #region 响应函数
        /// <summary>
        /// 解析并响应登录阶段命令
        /// </summary>
        /// <returns>返回进程状态</returns>
        public ProcessStatus GetResponse(out LevelType enumReturnType,out string sUsername)
        {
            if (m_username.Equals("-1"))
            {
                enumReturnType = LevelType.ILLEGAL;
                sUsername = "ExitSystem";
                return ConsoleLog.TwoFactorCheck("退出系统", ProcessStatus.LOGIN, ProcessStatus.EXIT); ;
            }
            try
            {
                using SqlSugarClient dbClient = SugarUtils.CreateSqlSugarClient(m_dbPath);

                var resultListAccounts = dbClient.Queryable<Account>().Where(account =>
                    account.m_username.Equals(m_username) && account.m_password.Equals(m_password));
                if (resultListAccounts.Any() && resultListAccounts.Count() <= 1)
                {
                    var resultAccount = resultListAccounts.First();

                    Console.WriteLine("\r\n登录成功！" +
                                      $"\r\n当前用户名为：{resultAccount.m_username}" +
                                      $"\r\n当前选择为{StringUtils.GetStringOfType(resultAccount.m_type)}题目");

                    enumReturnType = resultAccount.m_type;
                    sUsername = resultAccount.m_username;
                    return ProcessStatus.WORKING;
                }
                else
                {
                    Console.WriteLine("\r\n用户名不存在或密码错误，请输入正确的用户名、密码");

                    enumReturnType = LevelType.ILLEGAL;
                    sUsername = "WrongAccount";

                    return ProcessStatus.LOGIN;
                }
            }
            catch
            {
                Console.WriteLine("\r\n查询数据库账号出现问题");
                throw;
            }
        }
        #endregion
    }
}
