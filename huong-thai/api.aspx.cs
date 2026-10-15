using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;

namespace huong_thai
{
    public partial class api : System.Web.UI.Page
    {
        //get gv
        void get_gv()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string sqlQuery = "SELECT * FROM GV order by stt";
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable
                    }
                }
            }
            //dt => json
            string json = JsonConvert.SerializeObject(dt);
            Response.ContentType = "application/json";
            Response.Write(json);
        }

        void get_setting()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string sqlQuery = "SELECT * FROM settings";
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt); // Đổ dữ liệu vào DataTable
                    }
                }
            }
            //dt => json
            string json = JsonConvert.SerializeObject(dt);
            Response.ContentType = "application/json";
            Response.Write(json);
        }

        class thong_bao
        {
            public int ok;
            public string msg;
        }
        void bao_loi(string action)
        {
            thong_bao tb = new thong_bao();
            tb.ok = 0;
            tb.msg = $"Không hiểu action {action}";
            string json = JsonConvert.SerializeObject(tb); //chuyển đổi đối tượng tb => chuỗi txt json format
            Response.ContentType = "application/json";
            Response.Write(json);
        }
        void save_web_form()
        {
            thong_bao tb = new thong_bao();
            try
            {
                // Lấy dữ liệu từ form gửi lên theo phương thức POST
                string hoten = Request.Form["hoten"];
                string sdt = Request.Form["sdt"];
                string nhucau = Request.Form["nhucau"];
                string khoi = Request.Form["khoi"];
                string mon = Request.Form["mon"];
                // tạo sql insert
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string sqlAdd="INSERT INTO web_form (hoten, sdt, more) VALUES (@hoten, @sdt, @more)";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlAdd, conn))
                    {
                        cmd.Parameters.Add("@hoten", SqlDbType.NVarChar,50).Value = hoten;
                        cmd.Parameters.Add("@sdt", SqlDbType.NVarChar, 50).Value = sdt;
                        cmd.Parameters.Add("@more", SqlDbType.NVarChar, 500).Value = $"Nhu cầu: {nhucau}. Môn: {mon}";
                        int n = cmd.ExecuteNonQuery();
                        if (n == 1)
                        {
                            tb.ok = 1;
                            tb.msg = "Ngon lành cành đào";
                        }
                        else
                        {
                            tb.ok = 0;
                            tb.msg = "Có gì đó sai sai";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                tb.ok = 0;
                tb.msg = ex.Message;
            }
            string json = JsonConvert.SerializeObject(tb);
            Response.ContentType = "application/json";
            Response.Write(json);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //token bot huowngthai: 8522227346:AAGdcBZJYBnhIGdQtuw1XnRaj2GId-lUhvM
            //lấy biến truyền lên dạng GET, tên: action, giá trị: tuỳ
            //telegram of group id: -5013469989
            string action = Request.QueryString["action"];  //lấy biến gửi lên bởi method GET
            switch (action)
            {
                case "gv":
                    get_gv();
                    break;
                case "setting":
                    get_setting();
                    break;
                case "bome":
                    save_web_form();
                    break;
                default:
                    bao_loi(action);                    
                    break;
            }
            
        }
    }
}