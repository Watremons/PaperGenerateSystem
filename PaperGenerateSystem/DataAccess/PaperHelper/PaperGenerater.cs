using PaperGenerateSystem.Common;
using System;
using System.Collections.Generic;
using System.Text;
using AuctionBot.Code.SqliteTool;
using SqlSugar;
using PaperGenerateSystem.Common.Resources.DataType;
using PaperGenerateSystem.Utils;
using System.Linq;

namespace PaperGenerateSystem.DataAccess.PaperHelper
{
    internal class PaperGenerater
    {
        #region 属性
        /// <summary>
        /// 题库文件路径
        /// </summary>
        private string m_dbPath { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sDBPath">数据库路径</param>
        public PaperGenerater(string sDBPath)
        {
            m_dbPath = sDBPath;
        }
        #endregion

        /// <summary>
        /// 生成题卷
        /// </summary>
        /// <param name="iQuestionNum">题目数量</param>
        /// <param name="sUsername">用户名</param>
        /// <param name="enumLevelType">题目难度</param>
        /// <returns>是否成功生成题卷</returns>
        public bool GeneratePaper(int iQuestionNum, string sUsername,LevelType enumLevelType)
        {
            List<Question> listOfQuestions = new List<Question>();
            for (int i = 0; i < iQuestionNum; i++)
            {
                //问题生成
                Question question = GenerateQuestion(sUsername, enumLevelType);
                if (question.m_questionLevel == LevelType.ILLEGAL)
                {
                    return false;
                }

                Console.WriteLine(question.m_stem);

                using SqlSugarClient dbClient = SugarUtils.CreateSqlSugarClient(m_dbPath);
                dbClient.Insertable<Question>(question).ExecuteCommand();
            }

            //问题写入文件存到用户名目录中
            return true;
        }

        /// <summary>
        /// 生成单道题目
        /// </summary>
        /// <param name="sUsername">用户名</param>
        /// <param name="enumLevelType">题目难度</param>
        /// <returns>生成的单道题目</returns>
        public Question GenerateQuestion(string sUsername, LevelType enumLevelType)
        {
            do
            {
                #region 操作符操作数随机生成逻辑
                Random random = new Random((int)DateTime.Now.Ticks);
                int operandNum = enumLevelType == LevelType.PRIMARY ? random.Next(2, 6) : random.Next(1, 6);
                int operatorNum = operandNum - 1;

                List<CalUnit> operands = GeneraterUtils.GenerateRandomOperand(operandNum);
                List<CalUnit> operators = GeneraterUtils.GenerateRandomOperator(operatorNum);

                #endregion

                Question infixExpression = new Question();
                if (operandNum == 1)
                {
                    if (enumLevelType == LevelType.JUNIOR)
                    {
                        int index = random.Next(0, 2);
                        operands.First().m_unitContent += GeneraterUtils.g_operatorJunior[index];
                    }

                    if (enumLevelType == LevelType.SENIOR)
                    {
                        int index = random.Next(0, 3);
                        operands.First().m_unitContent = GeneraterUtils.g_operatorSenior[index] + operands.First().m_unitContent;
                    }

                    infixExpression.m_stem = operands.First().m_unitContent;

                }
                else
                {
                    List<CalUnit> ReversePolish = GeneraterUtils.GenerateReversePolish(operators, operands);

                    if (enumLevelType == LevelType.PRIMARY)
                    {
                        infixExpression = GeneraterUtils.GenerateInfixExpressionPrimary(ReversePolish);
                    }
                    else if (enumLevelType == LevelType.JUNIOR)
                    {
                        infixExpression = GeneraterUtils.GenerateInfixExpressionJunior(ReversePolish);
                    }
                    else if (enumLevelType == LevelType.SENIOR)
                    {
                        infixExpression = GeneraterUtils.GenerateInfixExpressionSenior(ReversePolish);
                    }
                    else
                    {
                        Console.WriteLine("生成了非法题目，退出生成");
                        infixExpression.m_questionLevel = LevelType.ILLEGAL;
                        return infixExpression;
                    }
                }



                infixExpression.m_owner = sUsername;
                infixExpression.m_questionLevel = enumLevelType;

                #region 问题查询插入逻辑
                using SqlSugarClient dbClient = SugarUtils.CreateSqlSugarClient(m_dbPath);

                if (!dbClient.Queryable<Question>().Where(questionInDB =>
                        questionInDB.m_owner.Equals(infixExpression.m_owner) && (questionInDB.m_stem.Equals(infixExpression.m_stem)))
                    .Any())
                {
                    return infixExpression;
                }
                #endregion
                
            } while (true);
        }
    }
}
