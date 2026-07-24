function funcBranchMaster_Validation(ddlCircleOffice_Client, ddlBranchType_Client, txtSolID_Client, txtBranchName_Client, ddlType_Client) {

    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlBranchType = document.getElementById(ddlBranchType_Client);
    var txtSolID = document.getElementById(txtSolID_Client);
    var txtBranchName = document.getElementById(txtBranchName_Client);
    var ddlType = document.getElementById(ddlType_Client);

    if (ddlCircleOffice.value == "") {
        window.alert("please Select Circle Office");
        ddlCircleOffice.focus();
        return false;
    }

    if (txtSolID.value == "") {
        window.alert("please Enter Sol Id");
        txtSolID.focus();
        return false;
    }

    if (txtBranchName.value == "") {
        window.alert("please Enter Branch Name");
        txtBranchName.focus();
        return false;
    }
}

function funcCircleHead_Validation(ddlCircleOffice_Client, txtCircleHeadName_Client) {

    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var txtCircleHeadName = document.getElementById(txtCircleHeadName_Client);

    if (ddlCircleOffice.value == "") {
        window.alert("please Select Circle Office...!");
        ddlCircleOffice.focus();
        return false;
    }

    if (txtCircleHeadName.value == "") {
        window.alert("please Enter Circle Head Name...!");
        txtCircleHeadName.focus();
        return false;
    }
}

function funcUserCreation_Validation(TxtPF_Client, TxtName_Client, TxtEmail_Client, DDPOP_Client, DDLocation_Client) {
    var txtPF = document.getElementById(TxtPF_Client);
    var txtName = document.getElementById(TxtName_Client);
    var txtEmail = document.getElementById(TxtEmail_Client);
    var ddlPlaceOfPosting = document.getElementById(DDPOP_Client);
    var ddlLocation = document.getElementById(DDLocation_Client);

    if (txtPF.value == "") {
        window.alert("please Enter PF Number");
        txtPF.focus();
        return false;
    }

    if (txtName.value == "") {
        window.alert("please Enter Name");
        txtName.focus();
        return false;
    }

    if (txtEmail.value == "") {
        window.alert("please Enter Email Id");
        txtEmail.focus();
        return false;
    }
    else {
        funcEMailValidate(TxtEmail_Client);
    }

    if (ddlPlaceOfPosting.value == "") {
        window.alert("please Select Place of Posting");
        ddlPlaceOfPosting.focus();
        return false;
    }

    if (ddlLocation.value == "0") {
        window.alert("please Select Location");
        ddlLocation.focus();
        return false;
    }
}

function funcUserCreationSearch_Validation(TxtPF_Client) {
    debugger;
    var txtPF = document.getElementById(TxtPF_Client);

    if (txtPF.value == "") {
        window.alert("please Enter PF Number");
        txtPF.focus();
        return false;
    }
}

function funcSearch_Validation(txtElement_Client, displayMsg) {
    var txtElement = document.getElementById(txtElement_Client);

    if (txtElement.value == "") {
        window.alert(displayMsg);
        txtElement.focus();
        return false;
    }
}

function funcSearchMaster_Validation(txtElement1_Client, txtElement2_Client, displayMsg1, displayMsg2) {
    var txtElement1 = document.getElementById(txtElement1_Client);
    var txtElement2 = document.getElementById(txtElement2_Client);

    if (txtElement1.value == "0") {
        window.alert(displayMsg1);
        txtElement1.focus();
        return false;
    }

    if (txtElement2.value == "") {
        window.alert(displayMsg2);
        txtElement2.focus();
        return false;
    }
}

function funcStatusMaster_Validation(txtStatusCode_Client, ddlTable_Client, txtStatus_Client) {
    var ddlTable = document.getElementById(ddlTable_Client);
    var txtStatusCode = document.getElementById(txtStatusCode_Client);
    var txtStatus = document.getElementById(txtStatus_Client);

    if (ddlTable.value == "0") {
        window.alert("Select Table Name...!");
        ddlTable.focus();
        return false;
    }

    if (txtStatusCode.value == "") {
        window.alert("Enter Code...!");
        txtStatusCode.focus();
        return false;
    }

    if (txtStatus.value == "") {
        window.alert("Enter Name...!");
        txtStatus.focus();
        return false;
    }
}

function funcScaleMaster_Validation(txtScaleCode_Client, txtScale_Client) {
    var txtScaleCode = document.getElementById(txtScaleCode_Client);
    var txtScale = document.getElementById(txtScale_Client);

    if (txtScaleCode.value == "") {
        window.alert("Please Enter Code...!");
        txtScaleCode.focus();
        return false;
    }

    if (txtScale.value == "") {
        window.alert("Please Enter Name...!");
        txtScale.focus();
        return false;
    }
}

function funcNatureCaseMaster_Validation(txtCode_Client, ddlTable_Client, txtName_Client) {
    var ddlTable = document.getElementById(ddlTable_Client);
    var txtCode = document.getElementById(txtCode_Client);
    var txtName = document.getElementById(txtName_Client);

    if (ddlTable.value == "0") {
        window.alert("Select Form Name...!");
        ddlTable.focus();
        return false;
    }

    if (txtCode.value == "") {
        window.alert("Enter Nature Case Code...!");
        txtCode.focus();
        return false;
    }

    if (txtName.value == "") {
        window.alert("Enter Nature Case Name...!");
        txtName.focus();
        return false;
    }
}

function funcSourceRefMaster_Validation(txtCode_Client, ddlTable_Client, txtName_Client) {
    var ddlTable = document.getElementById(ddlTable_Client);
    var txtCode = document.getElementById(txtCode_Client);
    var txtName = document.getElementById(txtName_Client);

    if (ddlTable.value == "0") {
        window.alert("Select Form Name...!");
        ddlTable.focus();
        return false;
    }

    if (txtCode.value == "") {
        window.alert("Enter Source Ref Code...!");
        txtCode.focus();
        return false;
    }

    if (txtName.value == "") {
        window.alert("Enter Source Ref Name...!");
        txtName.focus();
        return false;
    }
}

function funchideUnhide(ddlColumnName_Client) {
    var ddlColumnName = document.getElementById(ddlColumnName_Client);
    var pnlText = document.getElementById("ctl00_cphBody_divText");
    var pnlDate = document.getElementById("ctl00_cphBody_divDate");
    var hidColumnDataType = document.getElementById("ctl00_cphBody_hidColumnDataType");

    var txtEnterValue = document.getElementById("ctl00_cphBody_txtEnterValue");
    var txtEnterDate = document.getElementById("ctl00_cphBody_txtEnterDate");

    txtEnterValue.value = "";
    txtEnterDate.value = "";

    var varValue = ddlColumnName.value.split(',');
    hidColumnDataType.value = varValue[1];

    if (varValue[1] == "datetime") {
        pnlDate.style.display = "block";
        pnlText.style.display = "none";
        return false;
    }
    else {
        pnlText.style.display = "block";
        pnlDate.style.display = "none";
        return false;
    }
}

function funchideUnhide_REPORT(ddlColumnName_Client, lblText_Client) {
    var ddlColumnName = document.getElementById(ddlColumnName_Client);
    var pnlText = document.getElementById("ctl00_cphBody_tdText");
    var pnlDate = document.getElementById("ctl00_cphBody_tdDate");
    var hidColumnDataType = document.getElementById("ctl00_cphBody_hidColumnDataType");
    var lblText = document.getElementById(lblText_Client);

    var txtEnterValue = document.getElementById("ctl00_cphBody_txtConditionValue_WHERE");
    var txtFromDate = document.getElementById("ctl00_cphBody_txtFromDate");
    var txtToDate = document.getElementById("ctl00_cphBody_txtToDate");

    txtEnterValue.value = "";
    txtFromDate.value = "";
    txtToDate.value = "";

    var varValue = ddlColumnName.value;
    hidColumnDataType.value = varValue;

    if (varValue.toUpperCase() == "DATE") {
        lblText.innerHTML = "Date :";
        pnlDate.style.display = "block";
        pnlText.style.display = "none";
        return false;
    }
    else {
        pnlText.style.display = "block";
        pnlDate.style.display = "none";
        lblText.innerHTML = "Value :";
        return false;
    }
}

function funcCustomizedReport_Validation(ddlColumnName_Client, ddlTableName_Client) {
    var ddlColumnName = document.getElementById(ddlColumnName_Client);
    var ddlTableName = document.getElementById(ddlTableName_Client);
    var txtColumnName_WHERE = document.getElementById("ctl00_cphBody_txtColumnName_WHERE");
    var ddlCondition_WHERE = document.getElementById("ctl00_cphBody_ddlCondition_WHERE");
    var txtConditionValue_WHERE = document.getElementById("ctl00_cphBody_txtConditionValue_WHERE");
    var txtFromDate = document.getElementById("ctl00_cphBody_txtFromDate");
    var txtToDate = document.getElementById("ctl00_cphBody_txtToDate");
    var CHK = document.getElementById("ctl00_cphBody_chkColumnName");

    var counter = 0;
    var atLeast = 1;

    if (ddlTableName.value == "0") {
        window.alert("Please Select Form Name...!");
        ddlTableName.focus();
        return false;
    }

    if (ddlColumnName.value.toUpperCase() != "SELECT") {

        if (txtColumnName_WHERE.value == "") {
            window.alert("Please Enter Column Name...!");
            txtColumnName_WHERE.focus();
            return false;
        }

        if (ddlCondition_WHERE.value == "") {
            window.alert("Please Select Condition...!");
            ddlCondition_WHERE.focus();
            return false;
        }

        if (ddlColumnName.value.toUpperCase() == "TEXT") {
            if (txtConditionValue_WHERE.value == "") {
                window.alert("Please Enter Column Value...!");
                txtConditionValue_WHERE.focus();
                return false;
            }
        }
        else if (ddlColumnName.value.toUpperCase() == "DATE") {
            if (txtFromDate.value == "") {
                window.alert("Please Enter From Date...!");
                txtFromDate.focus();
                return false;
            }
            if (txtToDate.value == "") {
                window.alert("Please Enter To Date...!");
                txtToDate.focus();
                return false;
            }
        }
    }

    if (CHK != null) {
        var checkbox = CHK.getElementsByTagName("input");
        for (var i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked) {
                counter++;
            }
        }
        if (atLeast > counter) {
            alert("Please select atleast " + atLeast + " item(s)");
            return false;
        }
        return true;
    }

}

function funchideUnhideControls(ddlTableName_Client) {
    var ddlTableName = document.getElementById(ddlTableName_Client);
    var hidTableName = document.getElementById("ctl00_cphBody_hidTableName");
    var txtPFNo = document.getElementById("ctl00_cphBody_txtPFNo");
    var txtCaseNo = document.getElementById("ctl00_cphBody_txtCaseNo");
    var tdPFCaption = document.getElementById("ctl00_cphBody_tdPFCaption");
    var tdCaseCaption = document.getElementById("ctl00_cphBody_tdCaseCaption");


    txtPFNo.value = "";
    txtCaseNo.value = "";

    hidTableName.value = ddlTableName.value;
    if (hidTableName.value != 0) {

        if (hidTableName.value.toUpperCase() == "SR") {
            tdPFCaption.style.display = "none";
            //tdPFControl.style.display = "none";
            tdCaseCaption.style.display = "none";
            //tdCaseControl.style.display = "none";

            return false;
        }
        else if (hidTableName.value.toUpperCase() == "RRB" || hidTableName.value.toUpperCase() == "VIGILANCE" || hidTableName.value.toUpperCase() == "IAC") {
            tdPFCaption.style.display = "block";
            //tdPFControl.style.display = "block";
            tdCaseCaption.style.display = "none";
            //tdCaseControl.style.display = "none";

            return false;
        }
        else {
            tdPFCaption.style.display = "none";
            //tdPFControl.style.display = "none";
            tdCaseCaption.style.display = "block";
            //tdCaseControl.style.display = "block";

            return false;
        }
    }
}

function funcUpload_Validation(ddlTable_Client) {

    var ddlTableName = document.getElementById(ddlTable_Client);

    if (ddlTableName.value == "0") {
        window.alert("please Select upload for Table");
        ddlTableName.focus();
        return false;
    }
}

function funcEMailValidate(txtEmail_Client) {
    var txtEmaill = document.getElementById(txtEmail_Client);
    var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var userinput = txtEmaill.value;

    if (!pattern.test(userinput)) {
        window.alert("not a valid e-mail address");
        function_to_call_when_oked_or_closed();
        return false;
    }
}

function funcValidation_Complaint(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNo.value == "") {
        window.alert("Please enter Complaint Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funchideUnhide_ComplaintUpdate(ddlColumnName_Client, lblText_Client) {
    var ddlColumnName = document.getElementById(ddlColumnName_Client);
    var lblText = document.getElementById(lblText_Client);

    var tdCircle = document.getElementById("ctl00_cphBody_tdCircle");
    var tdSentTo = document.getElementById("ctl00_cphBody_tdSentTo");
    var ddlCircleOffice = document.getElementById("ctl00_cphBody_ddlCircleOffice");
    var txtSentTo = document.getElementById("ctl00_cphBody_txtSentTo");
    var hidColumnDataType = document.getElementById("ctl00_cphBody_hidColumnDataType");

    ddlCircleOffice.selectedIndex = 0;
    txtSentTo.value = "";
    var varValue = ddlColumnName.value;
    hidColumnDataType.value = varValue;

    if (varValue.toUpperCase() == "CIRCLE") {
        lblText.innerHTML = "Circle Office :";
        tdCircle.style.display = "block";
        tdSentTo.style.display = "none";
        return false;
    }
    else if (varValue.toUpperCase() == "SENTTO") {
        tdSentTo.style.display = "block";
        tdCircle.style.display = "none";
        lblText.innerHTML = "Sent To :";
        return false;
    }
    else {
        tdSentTo.style.display = "none";
        tdCircle.style.display = "none";
        lblText.innerHTML = "";
        return false;
    }
}

function funcValidation_ComplaintUpdate(txtRNo_Client, ddlField_Client, ddlCircle_Client, txtSentoTo_Client) {

    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlField = document.getElementById(ddlField_Client);
    var ddlCircle = document.getElementById(ddlCircle_Client);
    var txtSentoTo = document.getElementById(txtSentoTo_Client);

    if (txtRNo.value == "") {
        window.alert("Please enter Complaint Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlField.value.toUpperCase() == "SELECT") {
        window.alert("Please Select Field name...!")
        txtRNo.focus();
        return false;
    }

    if (ddlField.value.toUpperCase() == "CIRCLE") {
        if (ddlCircle.value == 0) {
            window.alert("Please Select Circle name...!")
            ddlCircle.focus();
            return false;
        }
    }
    else if (ddlField.value.toUpperCase() == "SENTTO") {
        if (txtSentoTo.value == "") {
            window.alert("Please enter Sent To...!")
            txtSentoTo.focus();
            return false;
        }
    }

}

function funcValidation_IAC(txtIACNo_Client, txtDAView_Client, txtIACView_Client, txtCVOView_Client, ddlStatusCode_Client, chkClosureDate_Client, ddlCircleOffice_Client) {
    var txtIACNo = document.getElementById(txtIACNo_Client);
    var txtDAView = document.getElementById(txtDAView_Client);
    var txtIACView = document.getElementById(txtIACView_Client);
    var txtCVOView = document.getElementById(txtCVOView_Client);
    var ddlStatusCode = document.getElementById(ddlStatusCode_Client);
    var chkClosureDate = document.getElementById(chkClosureDate_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtIACNo.value == "") {
        window.alert("Please Enter IAC Number...!")
        txtIACNo.focus();
        return false;
    }

    if (chkClosureDate.checked == true) {
        if (ddlStatusCode.value != 15) {
            window.alert("Please Select Status Code 15...!")
            ddlStatusCode.focus();
            return false;
        }
    }

    if (ddlStatusCode.value == 15) {
        if (chkClosureDate.checked == false) {
            window.alert("Please check closure date...!")
            chkClosureDate.focus();
            return false;
        }
    }

    if ((chkClosureDate.checked == true) && (ddlStatusCode.value == 15)) {

        if (txtDAView.value == "") {
            window.alert("Please Enter DA View...!")
            txtDAView.focus();
            return false;
        }

        if (txtIACView.value == "") {
            window.alert("Please Enter IAC View...!")
            txtIACView.focus();
            return false;
        }

        if (txtCVOView.value == "") {
            window.alert("Please Enter CVO View...!")
            txtCVOView.focus();
            return false;
        }
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funcValidation_IACUpdate(txtIACNo_Client, txtDA_Client) {
    var txtIACNo = document.getElementById(txtIACNo_Client);
    var txtDA = document.getElementById(txtDA_Client);

    if (txtIACNo.value == "") {
        window.alert("Please enter IAC Number...!")
        txtIACNo.focus();
        return false;
    }

    if (txtDA.value == "") {
        window.alert("Please enter DA...!")
        txtDA.focus();
        return false;
    }
}

function funcValidation_MISC(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNo.value == "") {
        window.alert("Please enter MISC Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}



function funcValidation_OperationalRef(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");


    if (txtRNo.value == "") {
        window.alert("Please enter Operational Ref Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funcValidation_RRB(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");


    if (txtRNo.value == "") {
        window.alert("Please enter RRB Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funcValidation_RTI(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNo.value == "") {
        window.alert("Please enter RTI Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funcValidation_SR(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNo.value == "") {
        window.alert("Please enter SR Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }


}

function funcValidation_Sanction(txtRCNO_Client, ddlSanction_Client, txtRefusedDate_Client, txtCVCDate_Client) {

    var txtRCNO = document.getElementById(txtRCNO_Client);
    var ddlSanction = document.getElementById(ddlSanction_Client);
    var txtRefusedDate = document.getElementById(txtRefusedDate_Client);
    var txtCVCDate = document.getElementById(txtCVCDate_Client);

    if (txtRCNO.value == "") {
        window.alert("Please enter RC Number...!")
        txtRCNO.focus();
        return false;
    }

    //if (ddlSanction.value != 0) {
    //    if (ddlSanction.value == "UNDERPROCESS") {
    //        if (txtCVCDate.value == "") {
    //            window.alert("Please enter CVC Date...!")
    //            txtCVCDate.focus();
    //            return false;
    //        }
    //    }
    //    else {
    //        if (txtRefusedDate.value == "") {
    //            window.alert("Please enter sanction Refused Date...!")
    //            txtRefusedDate.focus();
    //            return false;
    //        }
    //    }
    //}
}

function funcValidation_Vigilance(txtRNO_Client, txtNAME_Client, txtPFNO_Client, ddlDACOZOHO_Client, chkRNODATE_Client, ddlSCALE_Client, ddlSTATUSCODE_Client, txtORDDADate_Client, ddlPenaltyType_Client, txtNAPUNDA_Client, ddlRegister_Client, ddlCircleOffice_Client, ddlPenaltyProceedings_Client) {
    var txtRNO = document.getElementById(txtRNO_Client);
    var txtNAME = document.getElementById(txtNAME_Client);
    var txtPFNO = document.getElementById(txtPFNO_Client);
    var ddlDACOZOHO = document.getElementById(ddlDACOZOHO_Client);
    var chkRNODATE = document.getElementById(chkRNODATE_Client);
    var ddlSCALE = document.getElementById(ddlSCALE_Client);
    var ddlSTATUSCODE = document.getElementById(ddlSTATUSCODE_Client);
    var txtORDDADate = document.getElementById(txtORDDADate_Client);
    var ddlPenaltyType = document.getElementById(ddlPenaltyType_Client);
    var txtNAPUNDA = document.getElementById(txtNAPUNDA_Client);
    var ddlRegister = document.getElementById(ddlRegister_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlPenaltyProceedings = document.getElementById(ddlPenaltyProceedings_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");


    if (txtRNO.value == "") {
        window.alert("Please Enter R Number...!");
        txtRNO.focus();
        return false;
    }

    if (txtNAME.value == "") {
        window.alert("Please Enter Name...!");
        txtNAME.focus();
        return false;
    }

    if (ddlSTATUSCODE.value == "0") {
        window.alert("Please Select Status Code of Vigilance...!");
        ddlSTATUSCODE.focus();
        return false;
    }

    if (ddlRegister.value == "0") {
        window.alert("Please Select Register...!");
        ddlRegister.focus();
        return false;
    }

    if (ddlSCALE.value == "Select") {
        window.alert("Please Select Scale...!");
        ddlSCALE.focus();
        return false;
    }

    if (chkRNODATE.checked == false) {
        window.alert("Please check box for RNO Date...!");
        chkRNODATE.focus();
        return false;
    }

    if (txtPFNO.value == "") {
        window.alert("Please Enter PF Number...!");
        txtPFNO.focus();
        return false;
    }

    if (ddlDACOZOHO.value == "0") {
        window.alert("Please Select DA_CO/ZO/HO...!");
        ddlDACOZOHO.focus();
        return false;
    }

    if (ddlSTATUSCODE.value == "15") {
        if (txtNAPUNDA.value == "") {
            window.alert("Please Enter NA Pun Da...!");
            txtNAPUNDA.focus();
            return false;
        }
        if (ddlPenaltyType.value == "0") {
            window.alert("Please Select Penlaty Type...!");
            ddlPenaltyType.focus();
            return false;
        }
        //if (ddlPenaltyProceedings.value == "0") {
        //    window.alert("Please Select Penlaty Proceedings...!");
        //    ddlPenaltyProceedings.focus();
        //    return false;
        //}
    }
    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funcValidation_VigilanceTemp(txtRNO_Client, txtNAME_Client, txtPFNO_Client, chkRNODATE_Client, ddlSCALE_Client, ddlSTATUSCODE_Client, txtNAPUNDA_Client, ddlRegister_Client, ddlCircleOffice_Client) {
    var txtRNO = document.getElementById(txtRNO_Client);
    var txtNAME = document.getElementById(txtNAME_Client);
    var txtPFNO = document.getElementById(txtPFNO_Client);
    var chkRNODATE = document.getElementById(chkRNODATE_Client);
    var ddlSCALE = document.getElementById(ddlSCALE_Client);
    var ddlSTATUSCODE = document.getElementById(ddlSTATUSCODE_Client);
    var txtNAPUNDA = document.getElementById(txtNAPUNDA_Client);
    var ddlRegister = document.getElementById(ddlRegister_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNO.value == "") {
        window.alert("Please Enter R Number...!");
        txtRNO.focus();
        return false;
    }

    if (chkRNODATE.checked == false) {
        window.alert("Please check box for RNO Date...!");
        chkRNODATE.focus();
        return false;
    }

    if (txtNAME.value == "") {
        window.alert("Please Enter Name...!");
        txtNAME.focus();
        return false;
    }

    if (ddlSTATUSCODE.value == "0") {
        window.alert("Please Select Status Code...!");
        ddlSTATUSCODE.focus();
        return false;
    }


    if (ddlSCALE.value == "Select") {
        window.alert("Please Select Scale...!");
        ddlSCALE.focus();
        return false;
    }

    if (txtPFNO.value == "") {
        window.alert("Please Enter PF Number...!");
        txtPFNO.focus();
        return false;
    }

    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }
}

function funchideUnhide_VigilanceUpdate(ddlColumnName_Client, lblText_Client) {
    var ddlColumnName = document.getElementById(ddlColumnName_Client);
    var lblText = document.getElementById(lblText_Client);

    var tdBASICPAY = document.getElementById("ctl00_cphBody_tdBASICPAY");
    var tdDACOZOHO = document.getElementById("ctl00_cphBody_tdDACOZOHO");
    var tdREGISTER = document.getElementById("ctl00_cphBody_tdREGISTER");
    var tdPENALTYPROCEEDING = document.getElementById("ctl00_cphBody_tdPENALTYPROCEEDING");
    var txtBASICPAY = document.getElementById("ctl00_cphBody_txtBASICPAY");
    var ddlDACOZOHO = document.getElementById("ctl00_cphBody_ddlDACOZOHO");
    var ddlRegister = document.getElementById("ctl00_cphBody_ddlRegister");
    var ddlPenaltyProceeding = document.getElementById("ctl00_cphBody_ddlPenaltyProceeding");
    var hidColumnDataType = document.getElementById("ctl00_cphBody_hidColumnDataType");

    txtBASICPAY.value = "";
    ddlDACOZOHO.selectedIndex = 0;
    ddlRegister.selectedIndex = 0;
    ddlPenaltyProceeding.selectedIndex = 0;
    var varValue = ddlColumnName.value;
    hidColumnDataType.value = varValue;

    if (varValue.toUpperCase() == "BASICPAY") {
        lblText.innerHTML = "Basic Pay :";
        tdBASICPAY.style.display = "block";
        tdDACOZOHO.style.display = "none";
        tdREGISTER.style.display = "none";
        return false;
    }
    else if (varValue.toUpperCase() == "DA_CO_ZO_HO") {
        tdBASICPAY.style.display = "none";
        tdREGISTER.style.display = "none";
        tdPENALTYPROCEEDING.style.display = "none";
        tdDACOZOHO.style.display = "block";
        lblText.innerHTML = "DA_CO/ZO/HO :";
        return false;
    }
    else if (varValue.toUpperCase() == "REGISTER") {
        tdDACOZOHO.style.display = "none";
        tdBASICPAY.style.display = "none";
        tdPENALTYPROCEEDING.style.display = "none";
        tdREGISTER.style.display = "block";
        lblText.innerHTML = "Register :";
        return false;
    }
    else if (varValue.toUpperCase() == "PENALTYPROCEEDING") {
        tdDACOZOHO.style.display = "none";
        tdBASICPAY.style.display = "none";
        tdREGISTER.style.display = "none";
        tdPENALTYPROCEEDING.style.display = "block";
        lblText.innerHTML = "Penalty Proceeding :";
        return false;
    }
    else {
        tdDACOZOHO.style.display = "none";
        tdBASICPAY.style.display = "none";
        tdREGISTER.style.display = "none";
        tdPENALTYPROCEEDING.style.display = "none";
        lblText.innerHTML = "";
        return false;
    }
}

function funcValidation_VigilanceUpdate(txtRNo_Client, ddlField_Client, txtBasciPay_Client, ddlDACOZOHO_Client, ddlRegister_Client, ddlPenaltyProceeding_Client) {
    debugger;
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlField = document.getElementById(ddlField_Client);
    var txtBasciPay = document.getElementById(txtBasciPay_Client);
    var ddlDACOZOHO = document.getElementById(ddlDACOZOHO_Client);
    var ddlRegister = document.getElementById(ddlRegister_Client);
    var ddlPenaltyProceeding = document.getElementById(ddlPenaltyProceeding_Client);

    if (txtRNo.value == "") {
        window.alert("Please enter Vigilance Number...!")
        txtRNo.focus();
        return false;
    }

    if (ddlField.value.toUpperCase() == "SELECT") {
        window.alert("Please Select Field name...!")
        txtRNo.focus();
        return false;
    }

    if (ddlField.value.toUpperCase() == "BASICPAY") {
        if (txtBasciPay.value == "") {
            window.alert("Please enter Basic pay...!")
            txtBasciPay.focus();
            return false;
        }
    }
    else if (ddlField.value.toUpperCase() == "DA_CO_ZO_HO") {
        if (ddlDACOZOHO.value == "0") {
            window.alert("Please select DA_CO/ZO/HO...!")
            ddlDACOZOHO.focus();
            return false;
        }
    }
    else if (ddlField.value.toUpperCase() == "REGISTER") {
        if (ddlRegister.value == "0") {
            window.alert("Please select Register...!")
            ddlRegister.focus();
            return false;
        }
    }
    else if (ddlField.value.toUpperCase() == "PENALTYPROCEEDING") {
        if (ddlPenaltyProceeding.value == "0") {
            window.alert("Please select Penalty Proceeding...!")
            ddlPenaltyProceeding.focus();
            return false;
        }
    }

}

function funcValidation_WB(txtRNo_Client, ddlCircleOffice_Client) {
    var txtRNo = document.getElementById(txtRNo_Client);
    var ddlCircleOffice = document.getElementById(ddlCircleOffice_Client);
    var ddlZoneNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlZoneNew");
    var ddlCircleNew = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircleNew");

    if (txtRNo.value == "") {
        window.alert("Please enter R Number...!")
        txtRNo.focus();
        return false;
    }
    if (ddlZoneNew.value == "0") {
        window.alert("Please select New Zone...!")
        ddlZoneNew.focus();
        return false;
    }

    if (ddlCircleNew.value == "0") {
        window.alert("Please select New Circle...!")
        ddlCircleNew.focus();
        return false;
    }

}

function funcValidation_SanctionForInvestigation() {
    var txtSINumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtSINumber");
    var txtRCNumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRCNumber");
    var txtRCDate = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRCDate");
    var txtReportDate = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtReportDate");
    var txtPFNumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtPFNumber");
    var txtName = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtName");
    var txtDesignation = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtDesignation");
    var ddlCircle = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircle");
    var ddlBranch = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlBranch");
    var ddlDA = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlDA");
    var txtRemarks = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRemarks");
    var txtDealingOfficerRemarks = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtDealingOfficerRemarks");
    var hidUserRole = document.getElementById("ctl00_cphBody_hidUserRole");

    if (txtSINumber.value == "") {
        window.alert("Please enter SI Number...!")
        txtSINumber.focus();
        return false;
    }

    if (txtRCNumber.value == "") {
        window.alert("Please enter RC Number...!")
        txtRCNumber.focus();
        return false;
    }

    if (txtRCDate.value == "") {
        window.alert("Please enter RC Date...!")
        txtRCDate.focus();
        return false;
    }

    if (txtReportDate.value == "") {
        window.alert("Please enter Date of Report Received...!")
        txtReportDate.focus();
        return false;
    }

    if (txtPFNumber.value == "") {
        window.alert("Please enter PF Number...!")
        txtPFNumber.focus();
        return false;
    }

    if (txtName.value == "") {
        window.alert("Please enter Name...!")
        txtName.focus();
        return false;
    }

    if (txtDesignation.value == "") {
        window.alert("Please enter Designation...!")
        txtDesignation.focus();
        return false;
    }

    if (ddlCircle.value == "0") {
        window.alert("Please select Circle...!")
        ddlCircle.focus();
        return false;
    }

    if (ddlBranch.value == "0") {
        window.alert("Please select Branch...!")
        ddlBranch.focus();
        return false;
    }

    if (ddlDA.value == "0") {
        window.alert("Please select DA...!")
        ddlDA.focus();
        return false;
    }


    if (txtRemarks.value == "") {
        window.alert("Please enter MIS User Remarks...!")
        txtRemarks.focus();
        return false;
    }

    if (hidUserRole.value == "VMIS_DESKUSER") {
        if (txtDealingOfficerRemarks.value == "") {
            window.alert("Please enter Desk User Dealing Officer Remarks...!")
            txtDealingOfficerRemarks.focus();
            return false;
        }
    }

}

function funcValidation_SanctionForProsecution() {
    var txtSPNumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtSPNumber");
    var txtRCNumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRCNumber");
    var txtRCDate = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRCDate");
    var txtReportDate = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtReportDate");
    var txtPFNumber = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtPFNumber");
    var txtName = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtName");
    var txtDesignation = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtDesignation");
    var ddlCircle = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlCircle");
    var ddlBranch = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlBranch");
    var ddlDA = document.getElementById("ctl00_cphBody_tabMain_tabEntry_ddlDA");
    var txtRemarks = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtRemarks");
    var txtDealingOfficerRemarks = document.getElementById("ctl00_cphBody_tabMain_tabEntry_txtDealingOfficerRemarks");

    var hidUserRole = document.getElementById("ctl00_cphBody_hidUserRole");

    if (txtSPNumber.value == "") {
        window.alert("Please enter SP Number...!")
        txtSPNumber.focus();
        return false;
    }

    if (txtRCNumber.value == "") {
        window.alert("Please enter RC Number...!")
        txtRCNumber.focus();
        return false;
    }

    if (txtRCDate.value == "") {
        window.alert("Please enter RC Date...!")
        txtRCDate.focus();
        return false;
    }

    if (txtReportDate.value == "") {
        window.alert("Please enter Date of Report Received...!")
        txtReportDate.focus();
        return false;
    }

    if (txtPFNumber.value == "") {
        window.alert("Please enter PF Number...!")
        txtPFNumber.focus();
        return false;
    }

    if (txtName.value == "") {
        window.alert("Please enter Name...!")
        txtName.focus();
        return false;
    }

    if (txtDesignation.value == "") {
        window.alert("Please enter Designation...!")
        txtDesignation.focus();
        return false;
    }

    if (ddlCircle.value == "0") {
        window.alert("Please select Circle...!")
        ddlCircle.focus();
        return false;
    }

    if (ddlBranch.value == "0") {
        window.alert("Please select Branch...!")
        ddlBranch.focus();
        return false;
    }

    if (ddlDA.value == "0") {
        window.alert("Please select DA...!")
        ddlDA.focus();
        return false;
    }

    if (txtRemarks.value == "") {
        window.alert("Please enter MIS User Remarks...!")
        txtRemarks.focus();
        return false;
    }

    if (hidUserRole.value == "VMIS_DESKUSER") {
        if (txtDealingOfficerRemarks.value == "") {
            window.alert("Please enter Desk User Dealing Officer Remarks...!")
            txtDealingOfficerRemarks.focus();
            return false;
        }
    }
}



