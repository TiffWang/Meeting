<%@ Page Title="主页" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Meeting._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>ERP</title>
    <link href="css/shell.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="script/jquery.erp.shell.js"></script>
    <script type="text/javascript">
        $(function () {
            $('body').erp_shell();
            $('.about').click(function () {
                $.copyright('V1.1.10 Build 20140416-1305');
            });
        });
    </script>
</head>
<body>
    <div class="erp_shell">
        <!--- header start --->
        <div class="header">
            <div class="user">
                员工编号：01000，管理员<label class="side">|</label>今天是2014年02月08日 星期六<label class="side">|</label>签到时间：2014-4-8
                08:42:12
            </div>
            <ul class="button">
                <li class="about"><a href="javascript:void(0)">关于ERP</a></li>
                <li class="info"><a href="">个人信息</a></li>
                <li class="help"><a href="">寻求帮助</a></li>
                <li class="exit"><a href="">注销</a></li>
            </ul>
        </div>
        <!--- header end --->
        <!--- navigation start --->
        <div class="navigation">
            <div class="logo">
            </div>
            <div class="content" id="navigation">
                <a href="javascript:void(0)" class="prev"></a><a href="javascript:void(0)" class="next">
                </a>
                <div class="container">
                    <ul>
                        <li jqlink="MeetingList.aspx">会议管理</li>
                    </ul>
                </div>
                <div class="submenu">
                    <div>
                        <ul style="display: none">
                            <li jqlink="MeetingList.aspx">会议记录</li>
                            <li jqlink="我的任务.html">我的任务</li>
                            <li jqlink="我的线索.html">我的线索</li>
                            <li jqlink="我的跟进线索.html">我的跟进线索</li>
                            <li jqlink="搜索线索.html">搜索线索</li>
                            <li jqlink="添加商机.html">添加商机</li>
                            <li jqlink="我的商机.html">我的商机</li>
                            <li jqlink="我的跟进商机.html">我的跟进商机</li>
                            <li jqlink="搜索商机.html">搜索商机</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!--- navigation end --->
        <!--- main start --->
        <div class="main">
            <div class="shadow">
            </div>
            <div id="iframe_list">
            </div>
            <ul id="systems_list">
                <li class="callcentent"><a href="">呼叫中心</a></li>
            </ul>
        </div>
        <!--- main end --->
        <!--- footer start --->
        <div class="footer">
            <div id="slide_button">
                <a href="javascript:void(0)" class="prev"></a><a href="javascript:void(0)" class="next">
                </a>
            </div>
            <div class="container">
                <ul id="tab_button_list">
                </ul>
            </div>
            <div id="tab_more_botton">
                <a href="javascript:void(0)" class="button"></a>
                <div class="layer">
                    <div>
                        <ul>
                        </ul>
                    </div>
                    <ol>
                        <a href="javascript:void(0)" class="prev"></a><a href="javascript:void(0)" class="next">
                        </a>
                    </ol>
                </div>
            </div>
            <ul id="shortcut_button">
                <li class="news"><a href="javascript:void(0)"></a>
                    <ol class="remind">
                        <span class="left"></span><span class="center">2</span> <span class="right"></span>
                    </ol>
                </li>
                <li class="hnjing"><a href="javascript:void(0)"></a></li>
                <li class="system"><a href="javascript:void(0)"></a></li>
            </ul>
        </div>
        <!--- footer end --->
    </div>
</body>
</html>
