using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessTier;
using System.IO;
using System.Threading;
using System.Xml;

namespace Site.UserControls
{
    public partial class Upload : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpPostedFile file;
            try
            {
                //获取上传的文件数据
                file = Request.Files["Filedata"];
                //上传的目录
                string uploadDir = Request["Type"].ParseTo<string>("");
                switch (uploadDir)
                {
                    //case "AD":
                    //    uploadDir = Config.ADPath;
                    //    break;
                    //case "Product":
                    //    uploadDir = Config.ProductPath;
                    //    break;
                    //case "News":
                    //    uploadDir = Config.NewsPath;
                    //    break;
                    //case "Photo":
                    //    uploadDir = Config.PhotosPath;
                    //    break;
                    //case "MyCara":
                    //    uploadDir = Config.MyCaraPath;
                    //    break;
                    //case "DownImg":
                    //    uploadDir = Config.DownImgPath;
                    //    break;
                    //case "DownFile":
                    //    uploadDir = Config.DownFilePath;
                    //    break;
                    default:
                        uploadDir = "~/Uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
                        break;
                }

                string fileName = file.FileName;
                string fileType = Path.GetExtension(fileName).ToLower();
                //由于不同浏览器取出的FileName不同（有的是文件绝对路径，有的是只有文件名），故要进行处理
                if (fileName.IndexOf(' ') > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf(' ') + 1);
                }
                else if (fileName.IndexOf('/') > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                }
                //上传的目录
                //string uploadDir = "~/Uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
                //上传的路径
                //生成年月文件夹及日文件夹
                if (Directory.Exists(Server.MapPath(uploadDir)) == false)
                {
                    Directory.CreateDirectory(Server.MapPath(uploadDir));
                }

                if (Directory.Exists(Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath(uploadDir + System.DateTime.Now.ToString("dd") + "/"));
                }

                uploadDir = uploadDir + System.DateTime.Now.ToString("dd") + "/";

                fileName = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");

                string uploadPath = uploadDir + fileName + fileType;

                //string thumbnailPath = "";
                //保存文件
                file.SaveAs(Server.MapPath(uploadPath));

                //Thread.Sleep(1000);

                //生成缩略图
                //if (SessionMgr.IsThumbnail)
                //{
                //    thumbnailPath = uploadDir + "s_" + fileName + fileType;
                //    ImageThumbnail img = new ImageThumbnail(Server.MapPath(uploadPath));
                //    img.ReducedImage(SessionMgr.ThumbnailWidth, SessionMgr.ThumbnailHeight, Server.MapPath(thumbnailPath));
                //    SessionMgr.ThumbnailPath = thumbnailPath;

                //    img.DisImage();
                //}
                //SessionMgr.UploadPath = uploadPath;

                //if (SessionMgr.UploadParentid > 0)
                //{
                //    ImgRow row = new ImgRow();
                //    row.ParentId = SessionMgr.UploadParentid;
                //    row.Path = uploadPath;
                //    row.Sort = 0;
                //    row.ThumbnailPath = thumbnailPath;
                //    row.CreateTime = DateTime.Now;

                //    ImgManager.Create(row);
                //}

                //清空Session
                //SessionMgr.UploadFileType = "";
                //SessionMgr.IsThumbnail = false;
                //SessionMgr.ThumbnailWidth = 0;
                //SessionMgr.ThumbnailHeight = 0;


                //Response.Write("1");
                //Alert("成功！");
                XmlDocument doc = new XmlDocument();
                doc.AddOrSetNode("imgUrl", uploadPath);
                // doc.AddNode("javascript", "alert('dd')");
                HttpContext.Current.Response.ContentType = "text/xml";
                HttpContext.Current.Response.Clear();
                
                //BusinessTier.Extension.EndRequestWriteText(doc.InnerText);
            }
            catch (Exception ex)
            {

                BusinessTier.Extension.EndRequestWriteText(ex.Message);
                //Response.Write("0");
            }
            finally
            {

            }
        }
    }
}