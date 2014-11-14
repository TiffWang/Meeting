<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Site.SignIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>竞网科技ERP系统</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/ico" href="/Images/favicon.ico" />
    <script src="Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var timerID = null;
        var timerRunning = false;
        function stopclock() {
            if (timerRunning)
                clearTimeout(timerID);
            timerRunning = false;
        }
        function startclock() {
            stopclock();
            showtime();
        }
        function showtime() {
            var now = new Date();
            var hours = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds();
            var timeValue = "";
            timeValue += (hours);
            timeValue += ((minutes < 10) ? ":0" : ":") + minutes; //timeValue += ((seconds < 10) ? ":0" : ":") + seconds
            $("#theTime").text(timeValue);
            timerID = setTimeout("showtime()", 1000);
            timerRunning = true;
        }

        $(function () {
            document.onkeydown = function (e) {
                var ev = document.all ? window.event : e;
                if (ev.keyCode == 13) {

                    $('#btnLogin').click();

                }
            };
        }); 
    </script>
</head>
<body onload="startclock();" class="login_bg">
    <div class="login_01">
        <div class="login_menu">
            <a href="#">录单贴示</a> | <a href="#">工单贴示</a> | <a href="#">忘记密码</a> | <a href="#">速度测试</a></div>
    </div>
    <div class="login_02">
        <div class="login_time">
            <div class="login_timefont">
                <span id="theTime" class="font01"></span>
                <script type="text/javascript">
                    today = new Date();
                    function initArray() {
                        this.length = initArray.arguments.length;
                        for (var i = 0; i < this.length; i++)
                            this[i + 1] = initArray.arguments[i];
                    }
                    var d = new initArray("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
                    document.write("<span>", d[today.getDay() + 1], "</span>", "<span>", today.getFullYear(), ".", today.getMonth() + 1, ".", today.getDate(), "", "</span>"); 
                </script>
            </div>
            <div class="login_tianqi">
               <%-- <iframe width="300" scrolling="no" height="50" frameborder="0" allowtransparency="true"
                    src="http://i.tianqi.com/index.php?c=code&id=12&color=%23FFFFFF&icon=1&num=2">
                </iframe>--%>
            </div>
        </div>
    </div>
    <div class="login_03">
        <form id="form1" runat="server">
        <div class="logininput">
            <asp:TextBox ID="txtLoginName" MaxLength="15" runat="server" autofocus></asp:TextBox><asp:TextBox
                CssClass="mima" ID="txtPassword" MaxLength="12" TextMode="Password" runat="server"></asp:TextBox><%--<input
                    class="yanzheng" name="" type="text" />
            <span class="loginyzm">
                <img src="Images/loginyz_08.jpg" align="top" /></span>--%>&nbsp;&nbsp;<span><a href="javascript:void(0);"
                    onclick="$('#btnLogin').click();" runat="server"><img src="Images/loginbtn_05.jpg" /></a></span></div>
        <asp:Button runat="server" ID="btnLogin" Text="登入" OnClick="btnLogin_Click" Style="display: none;" />
        </form>
    </div>
    <div class="login_04">
        湖南竞网科技有限公司版权所有 Copyright &copy; 2013.Hnjing.com.All Rights Reserved.Tel:400-0731-777
        技术支持：竞网科技</div>
</body>
</html>
