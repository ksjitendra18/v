//For numeric only
function IsNumeric(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

//For numeric decimal only
function isNumbericDecimal(evt, element) {
    var varElement = document.getElementById(element).value;
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode != 45 || varElement.indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
            (charCode != 46 || varElement.indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
            (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//date Validate
function validate(isDate) {
    isValid = true;
    tmp = isDate.split("/");
    isLeap = tmp[2] % 4;
    if (!/[1-9]{1}/.test(tmp[0]) || /[1-9]{2}/.test(tmp[0]) && !/10|11|12/.test(tmp[0])) { isValid = false }
    else if ((/4|6|9|11/.test(tmp[0])) && tmp[1] > 30) { isValid = false }
    else if ((/1|3|5|7|8|10|12/.test(tmp[0])) && tmp[1] > 31) { isValid = false }
    else if ((tmp[0] == 2 && isLeap == 0) && tmp[1] > 29) { isValid = false }
    else if ((tmp[0] == 2 && isLeap != 0) && tmp[1] > 28) { isValid = false }
    else if (tmp[2].length != 4 || (!/^19|20/.test(tmp[2]))) { isValid = false }
    if (isNaN(Date.parse(isDate)) || !isValid) { alert("Invalid Date"); return false }
    else { return true }
}

function checkDate(isDate_Client) {
    var isDate = document.getElementById(isDate_Client);
    tmp = isDate.value;
    if (tmp != "") {
        tmp = tmp.split("/");
        refDate = tmp[1] + "/" + tmp[0] + "/" + tmp[2];
        if (validate(refDate)) { alert('Valid Date') }
        else { isDate.value = ""; isDate.focus() }
    }
}

function parseDate(str) {
    var m = str.match(/^(\d{1,2})-(\d{1,2})-(\d{4})$/);
    return (m) ? new Date(m[3], m[2] - 1, m[1]) : null;
}

function disableControlsDropDownList(DropDownList) {

    $('#DropDownList').attr("disabled", true);
}

function showModalPopup(txtForm_Client, lblModal_Client, txtModal_Client, lblModalText_Client) {

    var txtForm = document.getElementById(txtForm_Client);
    var lblModal = document.getElementById(lblModal_Client);
    var txtModal = document.getElementById(txtModal_Client);
    txtModal.value = "";
    lblModal.value = "";

    lblModal.innerHTML = lblModalText_Client;
    txtModal.value = txtForm.value;

    //$("#ctl00_cphBody_tabMain_tabEntry_pnlModal").show();
}

function showModalPopup_SR(txtForm_Client, txtTo_Client, cond_Client) {

    var txtForm = document.getElementById(txtForm_Client);
    var txtTo = document.getElementById(txtTo_Client);

    var cond = cond_Client;

    if (cond == "OPEN") {
        //txtTo.value = "";
        txtTo.value = txtForm.value;
        txtTo.focus();
    }
    else {
        txtForm.value = txtTo.value;
        txtForm.focus();
    }
}

function showModal() {
    debugger;
    $find('ctl00_cphBody_tabMain_tabEntry_pnlModalSR_STATUS').show();
}

