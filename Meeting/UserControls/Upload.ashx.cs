using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using BusinessTier;
using System.Drawing;

namespace Site.UserControls
{
    /// <summary>
    /// 文件上传处理界面
    /// </summary>
    public class Upload1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {



            string action = context.Request.GetParam<string>("action", string.Empty);
            if (string.IsNullOrWhiteSpace(action))
            {
                action = context.Request.GetForm<string>("action", string.Empty);
            }
            Infrastructure.Log.SaveNote(string.Format("start UpFile {0}   {1}  ", context.Request.Url.ToString(), action));

            if (action == "del")
            {
                DeleteFile();
            }
            else if (action.ToLower() == "esb")
            {
                //UpFileByESB();
            }
            else
            {
                UpFile();
            }

            Infrastructure.Log.SaveNote(string.Format("end UpFile {0}   {1}  ", context.Request.Url.ToString(), action));
            //string err = string.Empty;
            //foreach (var item in context.Request.Form.AllKeys)
            //    err += (item + context.Request.Form.GetValues(item));

            //BusinessTier.Extension.EndRequestWriteText(context.Request.Url + " <br/> " + err + ex.Message);

        }

        private void UpFile()
        {
            HttpPostedFile file;
            string sourceFielName = string.Empty;
            var Server = HttpContext.Current.Server;
            try
            {
                //获取上传的文件数据
                file = HttpContext.Current.Request.Files["Filedata"];


                #region 上传的目录处理

                //上传的目录
                string uploadDir = HttpContext.Current.Request["Type"].ParseTo<string>("");
                switch (uploadDir)
                {
                    case "ContactPhoto":
                        uploadDir = "~/Uploadfile/ContactPhoto/";
                        break;
                    case "CreditImg":
                        uploadDir = "~/Uploadfile/CreditImg/";
                        break;
                    case "OnlineData":
                        uploadDir = "~/Uploadfile/OnlineData/";
                        break;
                    case "Contract":
                        uploadDir = "~/Uploadfile/Contract/";
                        break;
                    case "WebAccept":
                        uploadDir = "~/Uploadfile/WebAccept/";
                        break;
                    case "Credential":
                        uploadDir = "~/Uploadfile/Credential/" + System.DateTime.Now.ToString("yyyyMM") + "/";
                        break;
                    case "Bill":
                        uploadDir = "~/Uploadfile/Bill/";
                        break;
                    case "CS"://客服管理的上传推广方案
                        uploadDir = "~/Uploadfile/CS/";
                        break;
                    case "Customer"://客服管理的上传推广方案
                        uploadDir = "~/Uploadfile/Customer/";
                        break;
                    default:
                        uploadDir = "~/Uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
                        break;
                }

                //上传的目录
                //string uploadDir = "~/Uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
                //上传的路径
                //生成年月文件夹及日文件夹

                if (Directory.Exists(Server.MapPath(uploadDir)) == false)
                {
                    Directory.CreateDirectory(Server.MapPath(uploadDir));
                }

                string inventory = HttpContext.Current.Request["Inventory"].ParseTo<string>("Y");
                if (inventory == "Y")
                {
                    if (Directory.Exists(Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/")) == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/"));
                    }

                    uploadDir = uploadDir + System.DateTime.Now.ToString("dd") + "/";
                }

                #endregion

                #region 上文件文件名称处理

                string fileName = file.FileName;
                string fileType = Path.GetExtension(fileName).ToLower();
                sourceFielName = fileName;
                //由于不同浏览器取出的FileName不同（有的是文件绝对路径，有的是只有文件名），故要进行处理
                if (fileName.IndexOf(' ') > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf(' ') + 1);
                }
                else if (fileName.IndexOf('/') > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                }

                fileName = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");
                //文件关联的ID 
                string AssociateId = HttpContext.Current.Request["fileNameKey"].ParseTo<string>("");
                if (!string.IsNullOrEmpty(AssociateId))
                    fileName = AssociateId + "-" + fileName;

                #endregion

                string uploadPath = uploadDir + fileName + fileType;

                //string thumbnailPath = "";
                //保存文件
                file.SaveAs(Server.MapPath(uploadPath));

                #region 返回值处理

                XmlDocument doc = new XmlDocument();
                doc.AddOrSetNode("path", uploadPath.TrimStart('~'));
                //doc.AddNode("javascript", "alert(\"dd\")<>");
                doc.AddOrSetNode("fileName", sourceFielName);
                doc.AddOrSetNode("fileType", fileType.IsPictureUrl() ? "img" : "doc");
                XmlResponse(doc.InnerXml);

                #endregion
            }
            catch (Exception ex)
            {
                BusinessTier.Extension.EndRequestWriteText(ex.Message);
            }
            finally
            {

            }
        }

        protected void DeleteFile()
        {
            string path = HttpContext.Current.Request.GetForm<string>("path", string.Empty);
            path = path.TrimStart('.');
            path = HttpRuntime.AppDomainAppPath + path;
            XmlDocument doc = new XmlDocument();

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    doc.AddOrSetNode("status", "0");
                }
                catch (Exception ex)
                {
                    ErrXml(doc, ex.Message);
                    XmlResponse(doc.InnerXml);
                }
            }
            else
            {
                ErrXml(doc, "路径不存在!");
            }

            XmlResponse(doc.InnerXml);
        }


        //protected void UpFileByESB()
        //{

        //    Result resultInfo = new Result();
        //    resultInfo.resultCode = -1;

        //    string sourceFielName = string.Empty;
        //    string uploadDir = HttpContext.Current.Request.GetForm<string>("type", string.Empty);
        //    string Filedata = HttpContext.Current.Request.GetForm<string>("filedata", string.Empty);
        //    string fileNameKey = HttpContext.Current.Request.GetForm<string>("fileNameKey", string.Empty);
        //    string fileName = HttpContext.Current.Request.GetForm<string>("fileName", string.Empty);
        //    string errString = string.Empty;
        //    string fileUrl = string.Empty;
        //    try
        //    {

        //        Infrastructure.Log.SaveNote(string.Format("start UpFileByESB {0}   {1}  ", uploadDir, fileNameKey));

        //        if (string.IsNullOrEmpty(Filedata))
        //        {
        //            throw new Exception("Filedata为空！");
        //        }
        //        if (string.IsNullOrEmpty(fileName))
        //        {
        //            throw new Exception("fileName为空！");
        //        }

        //        string fileType = Path.GetExtension(fileName).ToLower();


        //        switch (uploadDir)
        //        {
        //            case "Customer"://客服管理的上传推广方案
        //                uploadDir = "/Uploadfile/Customer/";
        //                break;
        //            default:
        //                uploadDir = "/Uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
        //                break;
        //        }

        //        fileUrl = uploadDir;
        //        if (Directory.Exists(HttpContext.Current.Server.MapPath(uploadDir)) == false)
        //            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadDir));
        //        uploadDir = HttpContext.Current.Server.MapPath(uploadDir);

        //        fileName = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");
        //        if (!string.IsNullOrEmpty(fileNameKey))
        //            fileName = fileNameKey + "-" + fileName;
        //        uploadDir = uploadDir + fileName + fileType;
        //        fileUrl = fileUrl + fileName + fileType;

        //        if (fileUrl.IsPictureUrl())
        //        {
        //            BinaryToImgFile(uploadDir, Filedata);
        //        }
        //        else
        //        {
        //            BinaryToFile(uploadDir, Filedata);
        //        }

        //        errString = "成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        errString = ex.Message;
        //    }
        //    finally
        //    {
        //        resultInfo.resultMessage = errString;
        //        resultInfo.returnValue = fileUrl;
        //        resultInfo.resultCode = 0;
        //        Infrastructure.Log.SaveNote(string.Format("end UpFileByESB {0}   {1}  {2} ", fileUrl, fileNameKey, uploadDir));
        //        BusinessTier.Extension.EndRequestWriteText(resultInfo.ToJson());
        //    }


        //}
        /// <summary>
        /// 将文本转成文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="binary"></param>
        public void BinaryToFile(string path, string binary)
        {

            //利用新传来的路径实例化一个FileStream对像  
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                {
                    //实例化一个用于写的BinaryWriter  
                    bw.Write(System.Convert.FromBase64String(binary));

                }
            }
        }

        /// <summary>
        /// 将文本转成图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="binary"></param>
        public void BinaryToImgFile(string path, string binary)
        {
            MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(binary));
            var img = System.Drawing.Image.FromStream(ms);
            img.Save(path);
        }

        private void ErrXml(XmlDocument doc, string msg)
        {
            doc.AddOrSetNode("status", "err");
            doc.AddOrSetNode("messges", msg);
        }

        protected void XmlResponse(string data)
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(data);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}