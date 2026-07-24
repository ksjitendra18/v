using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

class CommonFunction
{
    public int convertToInt(TextBox objTextBox)
    {
        int intReturn = 0;
        if (objTextBox.Text == "")
        {
            intReturn = 0;
        }
        else
        {
            intReturn = Convert.ToInt32(objTextBox.Text.ToString());
        }

        return intReturn;
    }

    public int convertToInt(string strValue)
    {
        int intReturn = 0;
        if (strValue == "")
        {
            intReturn = 0;
        }
        else
        {
            intReturn = Convert.ToInt32(strValue);
        }

        return intReturn;
    }

    public int convertToIntToolTip(TextBox objTextBox)
    {
        int intReturn = 0;
        if (objTextBox.ToolTip == "")
        {
            intReturn = 0;
        }
        else
        {
            intReturn = Convert.ToInt32(objTextBox.ToolTip.ToString());
        }

        return intReturn;
    }

    public decimal convertToDecimal(TextBox objTextBox)
    {
        decimal decReturn = 0;
        if (objTextBox.Text == "")
        {
            decReturn = 0;
        }
        else
        {
            decReturn = Convert.ToDecimal(objTextBox.Text);
        }

        return decReturn;
    }

    public decimal convertToDecimal(string Amount)
    {
        decimal decReturn = 0;
        if (string.IsNullOrEmpty(Amount))
        {
            decReturn = 0;
        }
        else
        {
            decReturn = Convert.ToDecimal(Amount);
        }

        return decReturn;
    }

    public string convertToDateTime(TextBox objTextBox)
    {
        string inputDate = objTextBox.Text.Trim();

        return inputDate;

    }

    public string ddlSelectedText(DropDownList objDropDownList)
    {
        string strReturn = string.Empty;
        if (objDropDownList.SelectedItem.Text == "" || objDropDownList.SelectedItem.Text.ToUpper() == "SELECT" || objDropDownList.SelectedItem.Text.ToUpper() == "-SELECT")
        {
            strReturn = "";
        }
        else
        {
            strReturn = objDropDownList.SelectedItem.Text;
        }

        return strReturn;
    }

    public string ddlSelectedValue(DropDownList objDropDownList)
    {
        string strReturn = string.Empty;
        if (objDropDownList.Items.Count == 0)
        {

            strReturn = "";
        }
        else
        {
            if (objDropDownList.SelectedItem.Value == "" || objDropDownList.SelectedItem.Value.ToUpper() == "SELECT" || objDropDownList.SelectedItem.Value.ToUpper() == "0")
            {
                strReturn = "";
            }
            else
            {
                strReturn = objDropDownList.SelectedItem.Value;
            }
        }

        return strReturn;
    }

    public string ddlSelectedValue_Scale(DropDownList objDropDownList)
    {
        string strReturn = string.Empty;
        if (objDropDownList.SelectedItem.Value == "")
        {
            strReturn = "";
        }
        else
        {
            strReturn = objDropDownList.SelectedItem.Value;
        }

        return strReturn;
    }

    public void ddlSetData(DropDownList objDropDownList, string strData, bool bValue)
    {

        if (strData == "0" || strData == "")
        {
            objDropDownList.SelectedIndex = 0;
        }
        else
        {
            ListItem li;
            for (int i = 0; i < objDropDownList.Items.Count; i++)
            {
                li = objDropDownList.Items[i];
                if (li.Text == strData)
                {
                    objDropDownList.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public void ddlSetDataValue(DropDownList objDropDownList, string strData)
    {

        if (strData == "0" || strData == "")
        {
            objDropDownList.SelectedIndex = 0;
        }
        else
        {
            ListItem li;
            for (int i = 0; i < objDropDownList.Items.Count; i++)
            {
                li = objDropDownList.Items[i];
                if (li.Value == strData)
                {
                    objDropDownList.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public void ddlSetDataValue_Scale(DropDownList objDropDownList, string strData)
    {

        if (strData == "")
        {
            objDropDownList.SelectedIndex = 0;
        }
        else
        {
            ListItem li;
            for (int i = 0; i < objDropDownList.Items.Count; i++)
            {
                li = objDropDownList.Items[i];
                if (li.Value == strData)
                {
                    objDropDownList.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public void bindDropdownList(DropDownList objDropDownList, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            objDropDownList.DataSource = dt;
            objDropDownList.DataTextField = "NAME";
            objDropDownList.DataValueField = "CODE";
            objDropDownList.DataBind();
        }
        //else
        //{
        //    objDropDownList.Items.Insert(0, new ListItem("Select", "0"));
        //}

        objDropDownList.Items.Insert(0, new ListItem("Select", "0"));
    }

    public void bindDropdownList_SELECT(DropDownList objDropDownList, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            objDropDownList.DataSource = dt;
            objDropDownList.DataTextField = "NAME";
            objDropDownList.DataValueField = "CODE";
            objDropDownList.DataBind();
        }
        //else
        //{
        //    objDropDownList.Items.Insert(0, new ListItem("Select", "Select"));
        //}

        objDropDownList.Items.Insert(0, new ListItem("Select", "Select"));
    }

    public void disableControlsTextBox(TextBox objTextBox)
    {
        objTextBox.Attributes.Add("readonly", "readonly");
    }

    public void enableControlsTextBox(TextBox objTextBox)
    {
        objTextBox.Attributes.Add("readonly", "false");
    }

    public void disableControlsDropDownList(DropDownList objDropDownList)
    {
        objDropDownList.Attributes.Add("disabled", "disabled");
    }

    public void disableControlsCheckBox(CheckBox objCheckBox)
    {
        objCheckBox.Attributes.Add("onclick", "disabled");
    }

    public string chkSelected(CheckBox objCheckBox)
    {
        string strReturn = string.Empty;
        if (objCheckBox.Checked == true)
        {
            strReturn = "Y";
        }
        else
        {
            strReturn = "N";
        }

        return strReturn;
    }

    public void chkSetData(CheckBox objCheckBox, string strData)
    {
        if (strData.ToUpper() == "Y")
        {
            objCheckBox.Checked = true;
        }
        else
        {
            objCheckBox.Checked = false;
        }
    }

    public void removeTextBoxFirstComma(TextBox objTextBox)
    {
        objTextBox.Text = objTextBox.Text.Trim().TrimStart(',');
    }

    public string removeStringLastComma(String objString)
    {
        objString = objString.TrimEnd(',');
        return objString;
    }

    public string removeStringLastPipe(String objString)
    {
        objString = objString.TrimEnd('|');
        return objString;
    }

    public string funcGetUserIP()
    {
        string strResult = string.Empty;
        HttpContext objHttpContext = HttpContext.Current;
        strResult = (objHttpContext.Request.UserHostAddress != null) ? objHttpContext.Request.UserHostAddress : string.Empty;
        return strResult;
    }

    public void DisableAllControls(Control ctrl)
    {
        foreach (Control c in ctrl.Controls)
        {
            DisableAllControls(c);
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = false;
            }

            else if (c is TextBox)
            {
                ((TextBox)(c)).Enabled = false;
            }

            else if (c is CheckBox)
            {
                ((CheckBox)(c)).Enabled = false;
            }

            if (c is Button)
            {
                ((Button)(c)).Enabled = false;
            }

            if (c is Calendar)
            {
                ((Calendar)(c)).Enabled = false;
            }
        }
    }

    public void EnableAllControls(Control ctrl)
    {
        foreach (Control c in ctrl.Controls)
        {
            EnableAllControls(c);
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = true;
            }
            else if (c is TextBox)
            {
                ((TextBox)(c)).Enabled = true;
            }
            else if (c is CheckBox)
            {
                ((CheckBox)(c)).Enabled = true;
            }
            else if (c is Button)
            {
                ((Button)(c)).Enabled = true;
            }

        }
    }

    public void funcDisciplinaryAuthority(DropDownList objDropDownList, string ROLE, string TYPE)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        dt.Clear();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spDisciplinaryAuthority_Ddl]";

            cmd.Parameters.AddWithValue("@p_ROLE", ROLE);
            cmd.Parameters.AddWithValue("@p_TYPE", TYPE);
            cmd.CommandTimeout = 0;
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                bindDropdownList(objDropDownList, dt);
            }
        }

        catch (Exception es)
        {
            VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        }

        finally
        {
            con.Close();
            sda.Dispose();
            con.Dispose();
        }
    }

    public void funcMasterEmail_Get(TextBox objTextBox, string SOLID, string FORM)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        dt.Clear();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spEmailMaster_Get]";

            cmd.Parameters.AddWithValue("@p_SOLID", SOLID);

            cmd.CommandTimeout = 0;
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (FORM.Equals("EMAIL_MASTER"))
                {
                    objTextBox.Text = Convert.ToString(dt.Rows[0]["EMAILID"]);
                    objTextBox.ToolTip = Convert.ToString(dt.Rows[0]["UNIQUEID"]);
                }
                else if (FORM.Equals("VIG_IAC"))
                {
                    objTextBox.Text = Convert.ToString(dt.Rows[0]["EMAILID"]);
                    objTextBox.ToolTip = "";
                }
            }
            else
            {
                objTextBox.Text = "";
                objTextBox.ToolTip = "";
            }
        }

        catch (Exception es)
        {
            VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(es);
        }

        finally
        {
            con.Close();
            sda.Dispose();
            con.Dispose();
        }
    }

    public void funcZoneCircleMaster(DropDownList objDropDownList, string SOLID)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spZoneCircle_Ddl]";

            cmd.Parameters.AddWithValue("@p_SOLID", SOLID);
            cmd.CommandTimeout = 0;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                bindDropdownList(objDropDownList, dt);
            }
        }
        catch (Exception ex)
        {
            VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
        }
        finally
        {
            cmd.Dispose();
            con.Dispose();
            con.Close();
        }
    }

    public DataTable funcGetBranchName(string SOLID)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[spBranchName_Get]";

            cmd.Parameters.AddWithValue("@p_SOLID", SOLID);
            cmd.CommandTimeout = 0;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
        }
        catch (Exception ex)
        {
            VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex);
        }
        finally
        {
            cmd.Dispose();
            con.Dispose();
            con.Close();
        }

        return dt;
    }

    public Boolean funcCheckUserRights(string FORMNAME)
    {
        string USERROLE = Convert.ToString(HttpContext.Current.Session["ROLE"]);
        Boolean Result = false;
        if (USERROLE.Equals("VMIS_SUPERUSER"))
        {
            Result = true;
        }

        // changes for checker
        if (USERROLE.Equals("VMIS_CHECKER"))
        {
            Result = true;
        }

        else if (USERROLE.Equals("VMIS_ADMIN"))
        {
            if (FORMNAME.Equals("BRANCH_MASTER") || FORMNAME.Equals("CIRCLE_MASTER") || FORMNAME.Equals("EMAIL_MASTER") ||
                FORMNAME.Equals("ZONE_CHIEF_MANAGER") || FORMNAME.Equals("LODI_DISABLE") || FORMNAME.Equals("DASHBOARD"))
            {
                Result = true;
            }
        }

        else if (USERROLE.Equals("VMIS_MISUSER"))
        {
            if (FORMNAME.Equals("BRANCH_MASTER") || FORMNAME.Equals("DASHBOARD"))
            {
                Result = true;
            }
        }

        else if (USERROLE.Equals("VMIS_DESKUSER"))
        {
            if (FORMNAME.Equals("BRANCH_MASTER") || FORMNAME.Equals("DASHBOARD"))
            {
                Result = true;
            }
        }

        else if (USERROLE.Equals("VMIS_VIEWUSER"))
        {
            if (FORMNAME.Equals("BRANCH_MASTER") || FORMNAME.Equals("DASHBOARD"))
            {
                Result = true;
            }
        }

        return Result;
    }

}