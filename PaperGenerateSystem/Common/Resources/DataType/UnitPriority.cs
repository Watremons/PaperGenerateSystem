namespace PaperGenerateSystem.Common.Resources.DataType
{
    internal enum UnitPriority
    {
        /// <summary>
        /// 加法优先级
        /// </summary>
        PLUS = 1,
        /// <summary>
        /// 减法优先级
        /// </summary>
        SUB = 2,
        /// <summary>
        /// 乘法优先级
        /// </summary>
        MUL = 5,
        /// <summary>
        /// 除法优先级
        /// </summary>
        DIV = 6,
        /// <summary>
        /// 数字优先级
        /// </summary>
        NUM = 9
    }
}
