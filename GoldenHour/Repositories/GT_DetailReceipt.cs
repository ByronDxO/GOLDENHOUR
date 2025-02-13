using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class DetailReceiptRepository : RepositoryBase
    {
        public void InsertDetailReceipt(GT_DetailReceipt detail)
        {
            using (SqlConnection conn = GetConnection())
            {
                string sql = @"
                    INSERT INTO GT_DetailReceipt 
                        (dre_date, dre_stock, dre_total, fk_idProduct, fk_idReceipt)
                    VALUES 
                        (@date, @stock, @total, @fkProduct, @fkReceipt)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@date", detail.dre_date.HasValue ? (object)detail.dre_date.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@stock", detail.dre_stock.HasValue ? (object)detail.dre_stock.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@total", detail.dre_total.HasValue ? (object)detail.dre_total.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkProduct", detail.fk_idProduct.HasValue ? (object)detail.fk_idProduct.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkReceipt", detail.fk_idReceipt.HasValue ? (object)detail.fk_idReceipt.Value : DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
