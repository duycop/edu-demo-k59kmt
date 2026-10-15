miss H có sẵn file index.html
có chứa html,css,js khá ổn
nhưng nội dung tĩnh

muốn làm web động: quản trị đc nội dung

---
các bước làm:

1. có máy tính online 24/24 (ko cần thiết phải có ip tĩnh)
2. cài đặt cloudflared trên máy đó (windows os)
3. cài đặt iis(windows có sẵn) trên máy đó
4. cài đặt sql server
5. cài đặt wacs tại https://www.win-acme.com/

6. tạo thư mục, chứa file html
7. cấu hình trên iis để tạo website trỏ tới thư mục ở step6 với domain phù hợp
8. cấu hình cloudflare tunnel để trỏ domain truy cập qua đường hầm.

9. tạo db, table gv, nhập liệu demo cho table gv.
10. tạo project trên vs đỏ: 
11. cấu hình web.config để chứa chuỗi kết nối
12. tạo api.aspx : xoá sạch code html. giữ lại dòng 1. code c# viết ở page_load:
- truy vấn bảng gv
- tạo array json trả về client
13. viết code js để fetch tới api, nhận về json. update vào đúng chỗ trên trang. xem file code.js
14. sửa code js có sẵn (file hiong-thai.js) để gửi lên api các trường thông tin
15. tạo bảng web_form để lưu dữ liệu gửi lên
16. thêm code cho api để lưu dữ liệu js đẩy lên vào db
17. tạo bảng settings chứa key+html, bảng logSt để lưu log
18. tạo admin/about.aspx : lấy html từ db, hiển thị vào thẻ session runat server. (dễ)
19. cho thẻ sesion đc phép edit html trực tiếp. trước khi form submit thì code html đã edit này vào trường ẩn để POST lên
20. khi isPostBack thì lấy html này. gọi sp_setting để update vào table setting và insert vào bảng logSt.

21. lặp lại step 18,19,20 cho file adnin/pp.aspx để tạo trang cho admin edit phương pháp giảng dạy.

... to be continue... 