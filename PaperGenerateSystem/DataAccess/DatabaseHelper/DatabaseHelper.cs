using PaperGenerateSystem.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AuctionBot.Code.SqliteTool;
using PaperGenerateSystem.Common.Resources.DataType;
using SqlSugar;

namespace PaperGenerateSystem.DataAccess.DatabaseHelper
{
    internal static class DatabaseHelper
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="sDBFilePath">数据库文件路径</param>
        public static void Init(string sDBFilePath)
        {
            Console.WriteLine($"获取数据库文件{sDBFilePath}");
            if (!File.Exists(sDBFilePath))
            {
                Console.WriteLine("未找到数据库文件，创建新的数据库文件");
                Directory.CreateDirectory(Path.GetDirectoryName(sDBFilePath));
                File.Create(sDBFilePath).Close();
            }
            else
            {
                Console.WriteLine("成功获取数据库文件，正在进入系统\r\n");
            }

            //创建数据库连接
            SqlSugarClient dbClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"DATA SOURCE={sDBFilePath}",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });


            if (!SugarUtils.TableExists<Account>(dbClient)) //彗酱数据库初始化
            {
                Console.WriteLine("未找到accounts数据表，创建一个新表");
                SugarUtils.CreateTable<Account>(dbClient);
                //插入初始账号
                List<Account> accounts = new List<Account>()
                {
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "张三1",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "张三2",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "张三3",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "李四1",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "李四2",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "李四3",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "王五1",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "王五2",
                        m_password = "123",
                        m_info = ""
                    },
                    new Account()
                    {
                        m_type = LevelType.PRIMARY,
                        m_username = "王五3",
                        m_password = "123",
                        m_info = ""
                    },
                };
                dbClient.Insertable<Account>(accounts.ToArray()).ExecuteCommand();
            }

            if (!SugarUtils.TableExists<Question>(dbClient)) //彗酱数据库初始化
            {
                Console.WriteLine("未找到questions数据表，创建一个新表");
                SugarUtils.CreateTable<Question>(dbClient);
            }
        }
    }
}
