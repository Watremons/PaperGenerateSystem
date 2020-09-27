using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PaperGenerateSystem.Common;
using PaperGenerateSystem.Common.Resources;
using PaperGenerateSystem.Common.Resources.DataType;
using PaperGenerateSystem.DataAccess.PaperHelper;
using PaperGenerateSystem.Utils;

namespace PaperGenerateSystem.Handle
{
    internal class WorkHandle
    {
        #region 属性

        /// <summary>
        /// 用户名
        /// </summary>
        private string m_username { get; set; }

        /// <summary>
        /// 题目等级
        /// </summary>
        private LevelType m_paperLevel { get; set; }

        /// <summary>
        /// 数据库路径
        /// </summary>
        private string m_DBPath { get; set; }

        #endregion

        #region 构造函数

        public WorkHandle(string sUsername,LevelType enumLevelType,string sDBPath)
        {
            m_username = sUsername;
            m_paperLevel = enumLevelType;
            m_DBPath = sDBPath;
        }

        #endregion

        #region 响应函数

        /// <summary>
        /// 根据结果返回不同信号
        /// </summary>
        /// <returns>返回-1表示退出，返回0表示切换，返回1表示生成</returns>
        public ProcessStatus GetResponse(string sInputOrder,out LevelType levelType)
        {
            levelType = m_paperLevel;

            #region 处理退出登录相关指令
            if (sInputOrder == "-1")
            {
                return ConsoleLog.TwoFactorCheck("退出登录",ProcessStatus.WORKING,ProcessStatus.LOGIN);
            }
            #endregion

            #region 处理切换相关指令
            if (sInputOrder.StartsWith("切换为"))
            {
                if (Encoding.Default.GetBytes(sInputOrder).Length == 10)
                {

                    if (sInputOrder.EndsWith("小学"))
                    {
                        Console.WriteLine("\r\n" + (
                                              levelType == LevelType.PRIMARY
                                                  ? "目前已经为"
                                                  : "成功切换至") +
                                          StringUtils.GetStringOfType(LevelType.PRIMARY));
                        levelType = LevelType.PRIMARY;
                    }
                    else if (sInputOrder.EndsWith("初中"))
                    {
                        Console.WriteLine("\r\n" + (
                                              levelType == LevelType.JUNIOR
                                                  ? "目前已经为"
                                                  : "成功切换至") +
                                          StringUtils.GetStringOfType(LevelType.JUNIOR));
                        levelType = LevelType.JUNIOR;
                    }
                    else if (sInputOrder.EndsWith("高中"))
                    {
                        Console.WriteLine("\r\n" + (
                                              levelType == LevelType.SENIOR
                                                  ? "目前已经为"
                                                  : "成功切换至") +
                                          StringUtils.GetStringOfType(LevelType.SENIOR));
                        levelType = LevelType.SENIOR;
                    }
                    else
                    {
                        Console.WriteLine("\r\n输入错误，目标只能为小学、初中、高中三个选项中的一个，请重新输入");
                    }
                }
                else
                {
                    Console.WriteLine("\r\n输入错误，目标只能为小学、初中、高中三个选项中的一个，请重新输入");
                }
                return ProcessStatus.WORKING;
            }
            #endregion

            #region 处理生成相关指令
            Regex regex = new Regex("^[0-9]+$");
            if (regex.IsMatch(sInputOrder))
            {
                if (int.TryParse(sInputOrder, out int numOfQuestion))
                {
                    if (numOfQuestion < 31 && (numOfQuestion > 0))
                    {
                        //生成题目
                        PaperGenerater paperGenerater = new PaperGenerater(m_DBPath);
                        if (!paperGenerater.GeneratePaper(numOfQuestion, m_username,m_paperLevel))
                        {
                            Console.WriteLine("\r\n生成题目失败，请联系管理员");
                        }
                        else
                        {
                            Console.WriteLine($"\r\n已为用户{m_username}生成试卷" +
                                              $"难度为{levelType}" +
                                              $"题目数量为{numOfQuestion}道");
                        }
                        return ProcessStatus.WORKING;
                    }

                    Console.WriteLine("\r\n输入题目数量不合法，请输入10到30的整数");
                }
                Console.WriteLine("\r\n输入非法，请输入正确的指令");
            }
            Console.WriteLine("\r\n输入非法，请输入正确的指令");
            #endregion

            return ProcessStatus.WORKING;
        }
        #endregion
    }
}