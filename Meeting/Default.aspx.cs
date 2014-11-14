using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessTier;

namespace Meeting
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //InviteManager.CreateList(InviteManager.ReaderData("C:\\erp.xls", ""));
            Response.Redirect("SignIn.aspx");
        }
    }
}
