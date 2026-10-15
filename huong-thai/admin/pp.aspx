<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pp.aspx.cs" Inherits="huong_thai.admin.pp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin - Cập nhật Phương Pháp</title>
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        brand: {
                            50: '#f0fdf4',
                            100: '#dcfce7',
                            500: '#22c55e',
                            600: '#16a34a',
                            700: '#15803d',
                        },
                        accent: {
                            blue: '#0284c7'
                        }
                    }
                }
            }
        }
    </script>
    <!-- FontAwesome & Google Fonts -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet">

    <link href="../huong-thai.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" >
        <asp:HiddenField ID="hfHtmlContent" runat="server" />
        <section contenteditable="true" id="gioi-thieu" class="page-section py-16 bg-white" style="display: block;">
            <div id="aboutContent" runat="server"></div>
        </section>
        <hr />
        <div style="align-content: center; margin: 10px; text-align: center;">
            <asp:Button 
                ID="btnUpdate_about" 
                runat="server" Text="Cập nhật" 
                />
        </div>
        <div id="kq_update_about" runat="server">ket qua cap nhat</div>
    </form>

<script type="text/javascript">
    document.getElementById('<%= form1.ClientID %>').onsubmit = function () {
        document.getElementById('<%= hfHtmlContent.ClientID %>').value = document.getElementById('gioi-thieu').innerHTML;
    };
</script>
</body>
</html>