using PaperGenerateSystem.Common;
using PaperGenerateSystem.Common.Resources.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperGenerateSystem.Utils
{
    internal static class GeneraterUtils
    {
        public static readonly string[] g_operatorPrimary = new string[]
        {
            "+", "-", "*", "/"
        };

        public static readonly string[] g_operatorJunior = new string[]
        {
            "^2","^0.5"
        };

        public static readonly string[] g_operatorSenior = new string[]
        {
            "sin","cos","tan"
        };

        #region 生成操作符和操作数
        /// <summary>
        /// 根据输入的操作数数量生成合法操作数
        /// </summary>
        /// <param name="iOperandNum">需要的操作数数量</param>
        /// <returns>包装好的操作数List</returns>
        public static List<CalUnit> GenerateRandomOperand(int iOperandNum)
        {
            List<CalUnit> returnOperandList = new List<CalUnit>();
            Random random = new Random((int) DateTime.Now.Ticks);
            for (int i = 0; i < iOperandNum; i++)
            {
                int operand = random.Next(1, 101);
                CalUnit operandUnit = new CalUnit(operand.ToString(), UnitPriority.NUM);
                returnOperandList.Add(operandUnit);
            }

            return returnOperandList;
        }

        /// <summary>
        /// 根据输入的操作符数量生成合法的二目操作符
        /// </summary>
        /// <param name="iOperatorNum">需要的操作符数量</param>
        /// <returns>包装好的操作符List</returns>
        public static List<CalUnit> GenerateRandomOperator(int iOperatorNum)
        {
            List<CalUnit> returnOperatorList = new List<CalUnit>();
            Random random = new Random((int) DateTime.Now.Ticks);
            for (int i = 0; i < iOperatorNum; i++)
            {

                int index = random.Next(0, 4);
                string operatorStr = g_operatorPrimary[index];
                UnitPriority unitPriority = UnitPriority.NUM;
                switch (index)
                {
                    case 0:
                    {
                        unitPriority = UnitPriority.PLUS;
                        break;
                    }
                    case 1:
                    {
                        unitPriority = UnitPriority.SUB;
                        break;
                    }
                    case 2:
                    {
                        unitPriority = UnitPriority.MUL;
                        break;
                    }
                    case 3:
                    {
                        unitPriority = UnitPriority.DIV;
                        break;
                    }
                    default:
                    {
                        unitPriority = UnitPriority.NUM;
                        break;
                    }
                }

                CalUnit operatorUnit = new CalUnit(operatorStr, unitPriority);
                returnOperatorList.Add(operatorUnit);
            }

            return returnOperatorList;
        }
        #endregion

        /// <summary>
        /// 根据操作数和操作符生成逆波兰表达式
        /// </summary>
        /// <param name="listOperators">操作符list</param>
        /// <param name="listOperands">操作数list</param>
        /// <returns>逆波兰表达式list</returns>
        public static List<CalUnit> GenerateReversePolish(List<CalUnit> listOperators, List<CalUnit> listOperands)
        {
            List<CalUnit> reversePolish = new List<CalUnit>();
            Random random = new Random((int) DateTime.Now.Ticks);
            int operandInStackNum = 0;
            while (true)
            {
                if (operandInStackNum < 2)
                {
                    reversePolish.Add(new CalUnit(listOperands.First().m_unitContent,
                        listOperands.First().m_unitPriority));
                    listOperands.RemoveAt(0);
                    operandInStackNum++;
                }
                else
                {
                    int typeRandom = random.Next(0, 2);
                    if (typeRandom == 0 || !listOperands.Any())
                    {
                        reversePolish.Add(new CalUnit(listOperators.First().m_unitContent,
                            listOperators.First().m_unitPriority));
                        listOperators.RemoveAt(0);
                        operandInStackNum--;
                        if (!listOperators.Any())
                        {
                            break;
                        }
                    }
                    else
                    {
                        reversePolish.Add(new CalUnit(listOperands.First().m_unitContent,
                            listOperands.First().m_unitPriority));
                        listOperands.RemoveAt(0);
                        operandInStackNum++;
                    }
                }
            }

            return reversePolish;
        }

        #region 小学题目生成
        /// <summary>
        /// 根据逆波兰表达式list生成小学等级中缀表达式
        /// </summary>
        /// <param name="listReversePolish">逆波兰表达式List</param>
        /// <returns>中缀表达式List</returns>
        public static Question GenerateInfixExpressionPrimary(List<CalUnit> listReversePolish)
        {
            Stack<CalUnit> unitStack = new Stack<CalUnit>();

            foreach (var midUnit in listReversePolish)
            {
                if (midUnit.m_unitPriority == UnitPriority.NUM)
                {
                    unitStack.Push(midUnit);
                }
                else
                {
                    CalUnit rightUnit = unitStack.Peek();
                    unitStack.Pop();
                    CalUnit leftUnit = unitStack.Peek();
                    unitStack.Pop();

                    if ((int)midUnit.m_unitPriority - (int) leftUnit.m_unitPriority > 1)
                    {
                        leftUnit.m_unitContent = "(" + leftUnit.m_unitContent + ")";
                    }

                    if (((int) midUnit.m_unitPriority % 2 == 0 &&
                         (int) midUnit.m_unitPriority - (int) rightUnit.m_unitPriority >= 0)
                        || ((int) midUnit.m_unitPriority % 2 != 0 &&
                            (int) midUnit.m_unitPriority - (int) rightUnit.m_unitPriority > 0))
                    {
                        rightUnit.m_unitContent = "(" + rightUnit.m_unitContent + ")";
                    }

                    unitStack.Push(
                        new CalUnit(
                            leftUnit.m_unitContent + midUnit.m_unitContent + rightUnit.m_unitContent,
                            midUnit.m_unitPriority));
                }
            }

            return new Question()
            {
                m_owner = null,
                m_answer = null,
                m_questionLevel = LevelType.ILLEGAL,
                m_stem = unitStack.Peek().m_unitContent
            };
        }
        #endregion

        #region 初中题目生成
        /// <summary>
        /// 根据逆波兰表达式list生成初中等级中缀表达式
        /// </summary>
        /// <param name="listReversePolish">逆波兰表达式List</param>
        /// <returns>中缀表达式List</returns>
        public static Question GenerateInfixExpressionJunior(List<CalUnit> listReversePolish)
        {

            Stack<CalUnit> unitStack = new Stack<CalUnit>();
            Random random = new Random((int) DateTime.Now.Ticks);
            bool makeFlag = false;
            foreach (var midUnit in listReversePolish)
            {
                if (midUnit.m_unitPriority == UnitPriority.NUM)
                {
                    unitStack.Push(midUnit);
                }
                else
                {
                    CalUnit rightUnit = unitStack.Peek();
                    unitStack.Pop();
                    CalUnit leftUnit = unitStack.Peek();
                    unitStack.Pop();



                    if (leftUnit.m_unitPriority == UnitPriority.NUM)
                    {
                        makeFlag = makeFlag || AddJuniorOp(leftUnit, random);
                    }

                    if (rightUnit.m_unitPriority == UnitPriority.NUM)
                    {
                        makeFlag = makeFlag || AddJuniorOp(rightUnit, random);
                    }

                    if ((int)midUnit.m_unitPriority - (int)leftUnit.m_unitPriority > 1)
                    {
                        leftUnit.m_unitContent = "(" + leftUnit.m_unitContent + ")";
                        makeFlag = makeFlag || AddJuniorOp(leftUnit, random);
                    }

                    if (((int)midUnit.m_unitPriority % 2 == 0 &&
                         (int)midUnit.m_unitPriority - (int)rightUnit.m_unitPriority >= 0)
                        || ((int)midUnit.m_unitPriority % 2 != 0 &&
                            (int)midUnit.m_unitPriority - (int)rightUnit.m_unitPriority > 0))
                    {
                        rightUnit.m_unitContent = "(" + rightUnit.m_unitContent + ")";
                        makeFlag = makeFlag || AddJuniorOp(rightUnit, random);
                    }

                    unitStack.Push(
                        new CalUnit(
                            leftUnit.m_unitContent + midUnit.m_unitContent + rightUnit.m_unitContent,
                            midUnit.m_unitPriority));
                }
            }

            if (!makeFlag)
            {
                unitStack.Peek().m_unitContent =
                    "(" + unitStack.Peek().m_unitContent + ")";
                int index = random.Next(0, 2);
                unitStack.Peek().m_unitContent += g_operatorJunior[index];
            }

            return new Question()
            {
                m_owner = null,
                m_answer = null,
                m_questionLevel = LevelType.ILLEGAL,
                m_stem = unitStack.Peek().m_unitContent
            };
        }

        /// <summary>
        /// 判断并添加初中等级的运算符
        /// </summary>
        /// <param name="calUnit">需要被添加的Unit</param>
        /// <param name="random">随机类变量</param>
        /// <returns>是否添加运算符</returns>
        public static bool AddJuniorOp(CalUnit calUnit,Random random)
        {
            int randomJudge = random.Next(0, 2);
            if (randomJudge == 1)
            {
                int index = random.Next(0, 2);
                calUnit.m_unitContent += g_operatorJunior[index];
                return true;
            }

            return false;
        }
        #endregion

        #region 高中题目生成
        /// <summary>
        /// 根据逆波兰表达式list生成高中等级中缀表达式
        /// </summary>
        /// <param name="listReversePolish">逆波兰表达式List</param>
        /// <returns>中缀表达式List</returns>
        public static Question GenerateInfixExpressionSenior(List<CalUnit> listReversePolish)
        {

            Stack<CalUnit> unitStack = new Stack<CalUnit>();
            Random random = new Random((int)DateTime.Now.Ticks);
            bool makeFlag = false;
            foreach (var midUnit in listReversePolish)
            {
                if (midUnit.m_unitPriority == UnitPriority.NUM)
                {
                    unitStack.Push(midUnit);
                }
                else
                {
                    CalUnit rightUnit = unitStack.Peek();
                    unitStack.Pop();
                    CalUnit leftUnit = unitStack.Peek();
                    unitStack.Pop();

                    if (leftUnit.m_unitPriority == UnitPriority.NUM)
                    {
                        makeFlag = makeFlag || AddSeniorOp(leftUnit, random);
                    }

                    if (rightUnit.m_unitPriority == UnitPriority.NUM)
                    {
                        makeFlag = makeFlag || AddSeniorOp(rightUnit, random);
                    }

                    if ((int)midUnit.m_unitPriority - (int)leftUnit.m_unitPriority > 1)
                    {
                        leftUnit.m_unitContent = "(" + leftUnit.m_unitContent + ")";
                        makeFlag = makeFlag || AddSeniorOp(rightUnit, random);
                    }

                    if (((int)midUnit.m_unitPriority % 2 == 0 &&
                         (int)midUnit.m_unitPriority - (int)rightUnit.m_unitPriority >= 0)
                        || ((int)midUnit.m_unitPriority % 2 != 0 &&
                            (int)midUnit.m_unitPriority - (int)rightUnit.m_unitPriority > 0))
                    {
                        rightUnit.m_unitContent = "(" + rightUnit.m_unitContent + ")";
                        makeFlag = makeFlag || AddSeniorOp(rightUnit, random);
                    }

                    unitStack.Push(
                        new CalUnit(
                            leftUnit.m_unitContent + midUnit.m_unitContent + rightUnit.m_unitContent,
                            midUnit.m_unitPriority));
                }
            }

            if (!makeFlag)
            {
                unitStack.Peek().m_unitContent =
                    "(" + unitStack.Peek().m_unitContent + ")";
                int index = random.Next(0, 3);
                unitStack.Peek().m_unitContent = g_operatorSenior[index] + unitStack.Peek().m_unitContent;
            }

            return new Question()
            {
                m_owner = null,
                m_answer = null,
                m_questionLevel = LevelType.ILLEGAL,
                m_stem = unitStack.Peek().m_unitContent
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calUnit"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static bool AddSeniorOp(CalUnit calUnit, Random random)
        {
            int randomJudge = random.Next(0, 2);
            if (randomJudge == 1)
            {
                int index = random.Next(0, 3);
                calUnit.m_unitContent = g_operatorSenior[index] + calUnit.m_unitContent;
                return true;
            }

            return false;
        }
        #endregion
    }
}
