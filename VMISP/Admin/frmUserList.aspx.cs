using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMISP.Admin
{
    public partial class frmUserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT * FROM [View_ListofVigilanceMISUsers] where RoleName=@RoleName";
            GridView1.DataBind();
            if (DropDownList1.SelectedValue == "ALL" || DropDownList1.SelectedValue == "")
            {
                SqlDataSource1.SelectCommand = "SELECT * FROM [View_ListofVigilanceMISUsers]";
                GridView1.DataBind();
            }

        }
    }
}