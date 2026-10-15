using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace huong_thai.admin
{
    public partial class about : System.Web.UI.Page
    {
        const string setting_ID = "gioi-thieu";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Code to execute on initial page load
                //tải nội dung about:

                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                string sqlQuery = $"SELECT * FROM settings where [key]='{setting_ID}'";
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt); // Đổ dữ liệu vào DataTable

                            foreach (DataRow row in dt.Rows)
                            {
                                // Lấy giá trị theo tên cột hoặc chỉ số cột
                                var key = row["key"].ToString();
                                var html = row["html"].ToString();

                                if (key == setting_ID)
                                {
                                    noi_dung.InnerHtml = html;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    //khi submit form
                    string html = hfHtmlContent.Value;

                    string key = setting_ID;
                    string sql = "sp_setting";

                    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@html", html);
                            cmd.Parameters.AddWithValue("@key", key);
                            cmd.ExecuteNonQuery();
                            //thoong baso ok
                            kq_update_about.InnerHtml = "<div class='alert alert-success'>Cập nhật thành công</div>";
                            noi_dung.InnerHtml = html;
                        }
                    }
                }
                catch (Exception ex)
                {
                    kq_update_about.InnerHtml = "<div class='alert alert-danger'>Cập nhật thất bại: " + ex.Message + "</div>";
                }
            }
        }
    }
}