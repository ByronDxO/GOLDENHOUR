using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class ProductRepository : RepositoryBase
    {
        public int InsertProduct(GT_Product product)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                    INSERT INTO GT_Product 
                        (pro_name, pro_description, pro_status, pro_date, pro_total, pro_stock, pro_pathImage, fk_category)
                    VALUES 
                        (@pro_name, @pro_description, @pro_status, @pro_date, @pro_total, @pro_stock, @pro_pathImage, @fk_category)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pro_name", product.pro_name);
                    cmd.Parameters.AddWithValue("@pro_description", string.IsNullOrEmpty(product.pro_description) ? (object)DBNull.Value : product.pro_description);
                    cmd.Parameters.AddWithValue("@pro_status", product.pro_status.HasValue ? (object)product.pro_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pro_date", product.pro_date.HasValue ? (object)product.pro_date.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pro_total", product.pro_total.HasValue ? (object)product.pro_total.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pro_stock", product.pro_stock.HasValue ? (object)product.pro_stock.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pro_pathImage", string.IsNullOrEmpty(product.pro_pathImage) ? (object)DBNull.Value : product.pro_pathImage);
                    cmd.Parameters.AddWithValue("@fk_category", product.fk_category.HasValue ? (object)product.fk_category.Value : DBNull.Value);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return rowsAffected;
        }




        public IEnumerable<GT_Product> GetProductsByCategory(int categoryId)
        {
            var products = new List<GT_Product>();
            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                    SELECT pro_idProduct, pro_name, pro_description, pro_status, pro_date, pro_total, pro_stock, pro_pathImage, fk_category 
                    FROM GT_Product 
                    WHERE fk_category = @categoryId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new GT_Product
                            {
                                pro_idProduct = Convert.ToInt32(reader["pro_idProduct"]),
                                pro_name = reader["pro_name"].ToString(),
                                pro_description = reader["pro_description"] != DBNull.Value ? reader["pro_description"].ToString() : null,
                                pro_status = reader["pro_status"] != DBNull.Value ? (int?)Convert.ToInt32(reader["pro_status"]) : null,
                                pro_date = reader["pro_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["pro_date"]) : null,
                                pro_total = reader["pro_total"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["pro_total"]) : null,
                                pro_stock = reader["pro_stock"] != DBNull.Value ? (int?)Convert.ToInt32(reader["pro_stock"]) : null,
                                pro_pathImage = reader["pro_pathImage"].ToString(),
                                fk_category = reader["fk_category"] != DBNull.Value ? (int?)Convert.ToInt32(reader["fk_category"]) : null
                            };
                            products.Add(product);
                        }
                    }
                    conn.Close();
                }
            }
            return products;
        }

    }
}