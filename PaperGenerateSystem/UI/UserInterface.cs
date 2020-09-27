using PaperGenerateSystem.Common.Resources.DataType;
using System;
using System.Collections.Generic;
using System.Text;
using PaperGenerateSystem.Handle;
using PaperGenerateSystem.Common.Resources;
using PaperGenerateSystem.Utils;
using PaperGenerateSystem.DataAccess.DatabaseHelper;

namespace PaperGenerateSystem.UI
{
    class UserInterface
    {
        #region 属性
        /// <summary>
        /// 系统状态
        /// </summary>
        private ProcessStatus m_processStatus { get; set; }

        /// <summary>
        /// 应用程序的当前工作目录
        /// </summary>
        private string m_localPath { get; set; }

        /// <summary>
        /// 应用程序的当前数据库目录
        /// </summary>
        private string m_dbPath { get; set; }

        /// <summary>
        /// 当前登录账户
        /// </summary>
        private string m_username { get; set; }

        /// <summary>
        /// 当前出题类型
        /// </summary>
        private LevelType m_paperType { get; set; }
        #endregion

        #region 构造函数

        public UserInterface()
        {
            m_processStatus = ProcessStatus.INIT;
            m_localPath = System.IO.Directory.GetCurrentDirectory();
            m_dbPath = m_localPath.Replace('\\','/') + @"/database/paper.db";
            DatabaseHelper.Init(m_dbPath);
        }
        #endregion

        #region 阶段函数
        /// <summary>
        /// 开始运行系统，循环选择状态函数
        /// </summary>
        public void Start()
        {
            Console.WriteLine("\r\n欢迎来到考卷生成系统");
            m_processStatus = ProcessStatus.LOGIN;
            while (true)
            {
                if (m_processStatus == ProcessStatus.LOGIN)
                {
                    Login();
                }
                else if (m_processStatus == ProcessStatus.WORKING)
                {
                    Work();
                }
                else if (m_processStatus == ProcessStatus.EXIT)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You meet some unexpected error, please call the monitor.");
                    break;
                }
            }
        }

        /// <summary>
        /// 进入登录状态，将指令移交给LoginHandler
        /// </summary>
        private void Login()
        {
            //初始化账户与出题类型
            m_username = null;
            m_paperType = LevelType.ILLEGAL;

            Console.WriteLine("\r\n请输入您的用户名，在输入任意数量空格后开始输入密码" +
                              "\r\n输入-1退出系统");
            #region 控制台输入账户与密码

            string usernameInput = "";
            string passwordInput = "";
            
            bool passwordIsStart = false;

            //设计输入，将密码显示为*
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.Write("\r\n");
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    passwordIsStart = true;
                    Console.Write(" ");
                    continue;
                }

                if (!passwordIsStart)
                {
                    if (keyInfo.Key != ConsoleKey.Backspace)
                    {
                        usernameInput += keyInfo.KeyChar.ToString();
                        Console.Write(keyInfo.KeyChar.ToString());
                    }
                    else
                    {
                        usernameInput = usernameInput.Substring(0,usernameInput.Length - 1 > 0 ? usernameInput.Length - 1 : 0);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    if (keyInfo.Key != ConsoleKey.Backspace)
                    {
                        passwordInput += keyInfo.KeyChar.ToString();
                        Console.Write('*');
                    }
                    else
                    {
                        usernameInput = usernameInput.Substring(0, usernameInput.Length - 1 > 0 ? usernameInput.Length - 1 : 0);
                        Console.Write("\b");
                    }
                }
            }
            #endregion

            //将账户与密码移交LoginHandler
            //由于无数据库，适合直接在内存进行检测，考虑模块化要求，仍移交到LoginHandler处理
            LoginHandle loginHandler = new LoginHandle(usernameInput, passwordInput, m_dbPath);

            m_processStatus = loginHandler.GetResponse(out LevelType returnType,out string returnUsername);
            m_username = returnUsername;
            m_paperType = returnType;
        }

        /// <summary>
        /// 进入工作状态，将指令移交给WorkHandle
        /// </summary>
        private void Work()
        {
            #region 检测账户合法性
            if (m_paperType == LevelType.ILLEGAL)
            {
                Console.WriteLine("Your account is illegal, please call the monitor.");
                m_processStatus = ProcessStatus.EXIT;
            }
            #endregion

            Console.WriteLine($"\r\n准备生成{StringUtils.GetStringOfType(m_paperType)}数学题目，您可以选择输入");
            Console.WriteLine($"输入10-30的数字，可生成对应数量的{StringUtils.GetStringOfType(m_paperType)}数学题目" +
                              $"\r\n输入”切换为XX“可切换题目类型为XX对应题目，可选项为小学，初中，高中" +
                              $"\r\n输入-1将退出当前用户，重新登录" +
                              $"\r\n请输入：");
            string inputOrder = Console.ReadLine();

            WorkHandle workHandle = new WorkHandle(m_username, m_paperType, m_dbPath);
            m_processStatus = workHandle.GetResponse(inputOrder, out LevelType levelType);
            m_paperType = levelType;
        }
        #endregion
    }
}
