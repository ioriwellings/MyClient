using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace LBKJClient.dao
{
    class deleteInvalidDataDao
    {
        private static object obj = new object();
        private string dbPath = "Data Source =new.baw";
        public void deleteInvalidData()
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = null;
            try
            {
                conn = new SQLiteConnection(dbPath);
                conn.SetPassword(frmLogin.dataPassw);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                String sql1 = " delete from lb_base_data_home where warnState != '1' and warnState != '3'  and warningistrue != '2' and warningistrue != '3'  and houseinterval < 1  and carinterval < 1";

                cmd = new SQLiteCommand(sql1, conn);
                lock (obj)
                        {
                            cmd.ExecuteNonQuery();
                        }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                //MessageBox.Show(ex.Message);
                //throw new Exception(ex.Message);
            }
            finally
            {
                //reader.Close();
                cmd.Dispose();//释放命令执行器
                conn.Close();//断开数据库连接
                conn.Dispose();//释放连接器

            }

        }

    }
}
