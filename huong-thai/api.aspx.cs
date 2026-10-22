using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

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
                string sqlAdd = "INSERT INTO web_form (hoten, sdt, more) VALUES (@hoten, @sdt, @more)";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlAdd, conn))
                    {
                        cmd.Parameters.Add("@hoten", SqlDbType.NVarChar, 50).Value = hoten;
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
            catch (Exception ex)
            {
                tb.ok = 0;
                tb.msg = ex.Message;
            }
            string json = JsonConvert.SerializeObject(tb);
            Response.ContentType = "application/json";
            Response.Write(json);
        }
        void Login()
        {
            thong_bao tb = new thong_bao();
            string json;
            int thu_sai = -1;
            const int LOCK_TIME_SECONDS = 30; // Thời gian khóa (30 giây)

            try
            {
                thu_sai = this.Session["thu_sai"] == null ? 0 : (int)this.Session["thu_sai"];

                // 1. Kiểm tra nếu đã thử sai >= 5 lần
                if (thu_sai >= 5)
                {
                    // Kiểm tra an toàn nếu thiếu session thời gian thì reset luôn
                    if (this.Session["thu_sai_time"] == null)
                    {
                        this.Session["thu_sai"] = 0;
                        thu_sai = 0;
                    }
                    else
                    {
                        DateTime lockTime = (DateTime)this.Session["thu_sai_time"];
                        double elapsedSeconds = (DateTime.Now - lockTime).TotalSeconds;

                        if (elapsedSeconds < LOCK_TIME_SECONDS)
                        {
                            int remainingSeconds = (int)(LOCK_TIME_SECONDS - elapsedSeconds);
                            tb.ok = 0;
                            tb.msg = $"Bạn đã thử sai quá 5 lần, vui lòng thử lại sau {remainingSeconds} giây.";
                            json = JsonConvert.SerializeObject(tb);
                            Response.ContentType = "application/json";
                            Response.Write(json);
                            return;
                        }
                        else
                        {
                            // Hết thời gian khóa, reset lại
                            this.Session["thu_sai"] = 0;
                            thu_sai = 0;
                        }
                    }
                }

                string uid = this.Request["uid"];
                string hash = this.Request["hash"];

                string SQL = "SELECT uid, hoten FROM users WHERE uid = @uid AND LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST(ISNULL(@uid,'') + ISNULL(@salt,'') + ISNULL(pwd,'') AS VARCHAR(500))), 2)) = @hash";
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        string salt = this.Session["salt"] as string;
                        cmd.Parameters.Add("@uid", SqlDbType.VarChar, 50).Value = uid;
                        cmd.Parameters.Add("@salt", SqlDbType.VarChar, 50).Value = salt;
                        cmd.Parameters.Add("@hash", SqlDbType.VarChar, 250).Value = hash;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    tb.ok = 1;
                    tb.msg = JsonConvert.SerializeObject(dt);
                    this.Session["login_ok"] = "ok";
                    this.Session["thu_sai"] = 0;
                    this.Session["thu_sai_time"] = null;
                }
                else
                {
                    this.Session["login_ok"] = "nono";
                    thu_sai++;
                    this.Session["thu_sai"] = thu_sai;
                    tb.ok = 0;
                    tb.msg = "Sai thông tin đăng nhập";

                    // Nếu vừa chạm mốc đúng 5 lần sai, lưu lại mốc thời gian
                    if (thu_sai == 5)
                    {
                        this.Session["thu_sai_time"] = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Session["login_ok"] = "nono";
                tb.ok = 0;
                tb.msg = "ERROR: " + ex.Message;
            }

            json = JsonConvert.SerializeObject(tb);
            Response.ContentType = "application/json";
            Response.Write(json);
        }
        class LOGIN_RESPON
        {
            public string name;
            public string data;
        }
        void gen_salt()
        {
            //tạo ra 1 chuỗi ngẫu nhiên 32 ký tự, gồm chữ và số
            string salt = Guid.NewGuid().ToString("N"); //tạo ra chuỗi ngẫu nhiên 32 ký tự
            //lưu chuỗi này vào session["salt"]
            this.Session["salt"] = salt;
            LOGIN_RESPON rp = new LOGIN_RESPON();
            rp.name = "salt";
            rp.data = salt;
            //gửi về client dạng json: {name: "salt", data:"chuỗi ngẫu nhiên"}
            string json = JsonConvert.SerializeObject(rp);
            Response.ContentType = "application/json";
            Response.Write(json);
            //client gửi lên uid, hash_sha256(uid+salt+pwd) => server check trong db xem có đúng ko, nếu đúng thì trả về ok=1, msg="ok", nếu sai thì ok=0, msg="Sai thông tin đăng nhập"
        }

        void gen_captcha()
        {
            lib_captcha.CaptchaGenerator generator = new lib_captcha.CaptchaGenerator();
            string txt;
            Bitmap bm = generator.GenerateCaptchaImage(out txt);
            this.Session["captcha"] = txt;
            string b64 = lib_captcha.CaptchaGenerator.BitmapToBase64Jpg(bm);
            LOGIN_RESPON rp = new LOGIN_RESPON();
            rp.name = "captcha";
            rp.data = b64;
            string json = JsonConvert.SerializeObject(rp);
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
                case "xin_muoi":
                    gen_salt();
                    break;
                case "login":
                    Login();
                    break;
                case "gen_captcha":
                    gen_captcha();
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