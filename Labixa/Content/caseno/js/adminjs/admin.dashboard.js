// Create our number formatter.
var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'BIT',

    // These options are needed to round to whole numbers if that's what you want.
    //minimumFractionDigits: 0,
    //maximumFractionDigits: 0,
});
///////
//along update TotalBet
$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/affiliate/CalcTotalBet",
        async: true,
        success: function (data) {
            console.log(data);
            var parsedJSON = JSON.parse(data);
            console.log(parsedJSON.TotalBet);
            $("#span_ETH").html(" (1 ETH = " +(formatter.format(parsedJSON.TotalBet))+")");
            //  $("#systemBets").html(formatter.format(parsedJSON.SystemBet));

        },
        error: function (ex) {
            alert("Validate unsuccessful !!!");
            $('#button_exchangeETH').prop('disabled', false);
            document.getElementById("button_exchangeETH").innerText = "Exchange";
        }
    });
});

//along lấy lịch sử giao dịch
//$.ajax({
//    type: "POST",
//    url: "/affiliate/transactionHistory",
//    data: { type: "deposit" },
//    async: false,
//    success: function (data) {
//        if (data == null) {

//        } else {
//            $('#body_example1').children().remove();
//            JSON.parse(data).forEach(function (dat, index) {
//                var rows = "<tr  role='row' class='odd'>"
//                    + "<td>" + dat.Time.replace("T", " ").split(".")[0] + "</td>"
//                    + "<td>" + dat.Amount + "</td>"
//                    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + dat.TxHash + "' style='color: #f64554;'>View</a></td>"
//                    + "<td>" + dat.Status + "</td>"
//                    + "</tr>";
//                $('#body_example1').append(rows);
//            });
//        }
//        //console.log(parsedJSON.TotalBet);
//        //$("#totalBets").html(formatter.format(parsedJSON.TotalBet));
//        //$("#systemBets").html(formatter.format(parsedJSON.SystemBet));
//        //var rows = "<tr>"
//        //    + "<td>" + parsedJSON.Time + "</td>"
//        //    + "<td>" + parsedJSON.Amount + "</td>"
//        //    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + parsedJSON.TxHash + "' style='color: #f64554;'>View</a></td>"
//        //    + "<td>" + parsedJSON.Status + "</td>"
//        //    + "</tr>";
//        //$('#body_example1').append(rows);
//    },
//    error: function (ex) {
//    }
//});
//along hết
//along hết


var addr = "";
var username = "";
var maxWidthdrawETH = "";//giới hạn tối đa có thể rút
var maxWidthdrawVIP = "";
var ETHBet = "";
var VIPBet = "";
var modal = document.getElementById("myModal");
//Max withdraw ETH
$.ajax({
    type: "POST",
    url: "/callapiadmin/WidthdrawMaxETH",
    data: { username: _username },
    async: true,
    success: function (data) {
        document.getElementById('BalanceView').innerText = data + " ETH";
    }
});
//Max Withdraw USD
$.ajax({
    type: "POST",
    url: "/callapiadmin/WidthdrawMaxVIP",
    data: { username: _username },
    async: true,
    success: function (data) {
        document.getElementById('balanceUSDwithdraw').innerText = data + " BIT";
    }
});
//document.getElementById("level_user").innerText = "0";
$.ajax({
    type: "POST",
    url: "/home/GetAccountDashboardAsync",//type = 1
    data: { username: _username },
    async: false,//để true sẽ ảnh hưởng đến QR code
    success: function (data) {
        console.log("Data dashboard: " + data);
        //ETH
        $('#WidthdrawETH').append("<label id='label_widthdrawETH'>Amount Withdraw: </label><button class='btn btn-warning cls_btn' onclick='maxWidthETH()'>Max</button><input style='width:100%' id='amount-Widthdraw' value='' name='amount' type='number' min='" + data.minEth + "' max='" + data.eth + "'required>");
        $('#amount_exchangeEth').append("Amount Exchange: <button class='btn btn-danger cls_btn' onclick='maxExchangeETH()'>Max</button><input style='width:100%' id='amount-exchangeETH' name='amount' type='number' min='" + data.minExchange + "' max='" + data.eth + "'required>");
        //VIP
        $('#WidthdrawVIP').append("Amount Withdraw:<button class='btn btn-warning cls_btn' onclick='maxWidthVIP()'>Max</button> <input style='width:100%' id='amount-Widthdraw-vip' name='amount' type='number' min='" + data.minExchange + "' max='" + data.usd + "'required>");
        $('#amount_exchangeVip').append("Amount Exchange: <button class='btn btn-danger cls_btn' onclick='maxExchangeVIP()'>Max</button><input style='width:100%' id='amount-exchangeVip' name='amount' type='number' min='" + data.minExchange + "'max='" + data.usd + "'required>");
        //get id từ view và fill data vào
        document.getElementById('balanceETH').innerText = data.eth.toFixed(4);
        document.getElementById('balanceUSD').innerText = data.usd;
        document.getElementById('balanceUSDexchange').innerText = data.usd;
        document.getElementById('balanceUSDwithdraw').innerText = data.usd;
        document.getElementById('balanceUSD_bonus').innerText = data.bonus;
        document.getElementById('mybets').innerText = data.totalbet;
        document.getElementById('systemBets').innerText = data.systembet;
        document.getElementById('levelBets').innerText = data.level;
        //document.getElementById('total_affiliate').innerText = data.refferal;
        //available amount có thể rút (exchange k giới hạn số tiền rút)
        
        document.getElementById('Balance_exchangeEth').innerText = data.eth /*+ " ETH"*/;
        ///
        ETHBet = data.eth;
        VIPBet = data.usd;
        maxWidthdrawETH = data.maxEth;
        maxWidthdrawVIP = data.maxVip;
        var inputs = "<input type='text' id='MinETH' value='" + data.minEth + "'hidden />" +
            "<input type='text' id='MinETH_1' value='" + data.minVip + "' hidden />" +
            "<input type='text' id='RateETH' value='" + data.EthExchangeVipFee + "' hidden />" +
            "<input type='text' id='WidthdrawETHFee' value='" + data.withdrawfee + "' hidden />" +
            "<input type='text' id='WidthdrawVIPFee' value='" + data.withdrawvipfee + "' hidden />";
        $('#feetransaction').append(inputs);
    }
});

//Start Đoạn script render QR code
var qrcode = new QRCode(document.getElementById("qrcode"), {
    width: 200,
    height: 200,
});
var qrcode2 = new QRCode(document.getElementById("qrcode2"), {
    width: 200,
    height: 200
});
function makeCode() {
    var elText = '';
    if (_addressCypto != null) {
        document.getElementById("addressETH").value = _addressCypto;
        elText = _addressCypto;
    }
    // console.log('(Session["addressETH"] as string)');
    qrcode.makeCode(elText);
};
//Generate QR user của form deposit vip
function makeCode2() {
    if (_username != "") {
        username = _username;
        document.getElementById("UserName").value = _username;
    }
    // console.log('(Session["addressETH"] as string)');
    qrcode2.makeCode(_username);
};
makeCode();
console.log("QR Code: " + makeCode());
makeCode2();
// Lấy text trước khi disabled
var add = document.getElementById("addressETH");
var urs = document.getElementById("UserName");
//Disabled input
//$('#addressETH').prop("disabled", true);
//$('#UserName').prop("disabled", true);
function myFunction() {
    /* Get the text field */
    var copyText = add;
    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /*For mobile devices*/

    /* Copy the text inside the text field */
    document.execCommand("copy");

    /* Alert the copied text */
    alert("Copied the text: " + copyText.value);
};
function myFunctionUserName() {
    /* Get the text field */
    var copyText = urs;
    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /*For mobile devices*/

    /* Copy the text inside the text field */
    document.execCommand("copy");

    /* Alert the copied text */
    alert("Copied the text: " + copyText.value);
};

///end gender QR code
//Function Popup WidthdrawETH
function submit2fa_WidthdrawETH() {
    $("#button_WidthdrawETH").last().html("Withdraw");
    $('#button_WidthdrawETH').prop('enable', false);
    $('#address-Widthdraw').prop('enable', false);
    $('#2fa-Widthdraw').prop('enable', false);
    $('#amount-Widthdraw').prop('enable', false);
    $("#fee").attr("enable", false);
    modal.style.display = "block";
    $('.cl-image').show();
    //getCookie minutes
    var minutesLimited = getCookie("minuteLimited");
    var subMinute = 61;
    if (minutesLimited != "") {
        const diffInMilliseconds = Math.abs(new Date() - Date.parse(minutesLimited));
        subMinute = diffInMilliseconds / 1000;
    }
    if (subMinute > 60) {
        var delayInMilliseconds = 300; //1 second
        setTimeout(function () {
            var password = document.getElementById("password").value;
            //var amount = $("amount-Widthdraw").val();
            var amount = document.getElementById("amount-Widthdraw").value;
            var addressTo = $('#address-Widthdraw').val().trim();
            var code2fa = $('#2fa-Widthdraw').val();
            var check2FA_Number = /([0-9]{6})/;
            var resCheck = check2FA_Number.test(code2fa);
            console.log("Check valied: " + resCheck);
            console.log("2FA: " + code2fa);

            //submitWidthdrawETH(password, addressTo, amount);

            if (amount == "" || addressTo == "") {
                alert("Some fields are empty or invalid. Please input again !");
                $("#button_WidthdrawETH").last().html("Withdraw");
                $('#button_WidthdrawETH').prop('disabled', false);
                $('#address-Widthdraw').prop('disabled', false);
                $('#2fa-Widthdraw').prop('disabled', false);
                $('#amount-Widthdraw').prop('disabled', false);
                modal.style.display = "none";
            }
            else {
                if (_check2FA == 'True') {//kiểm tra xem 2fa đã được actived hay chưa
                    if (code2fa != "" || resCheck == false) {//Check input code và bắt đầu kiểm tra trước khi thực hiện giao dịch
                        $.ajax({
                            type: "POST",
                            url: "/callapiadmin/Validate_2fa",//kiểm tra validate khi có mã code
                            data: { userName: _username, pinCode: code2fa },
                            async: true,
                            success: function (data) {
                                //dis
                                if (data.status == "Success") {
                                    submitWidthdrawETH(password, addressTo, amount);
                                }
                                else {
                                    alert(data.Message);
                                    //window.location.reload();
                                    $('#button_WidthdrawETH').prop('disabled', false);
                                    $("#button_WidthdrawETH").last().html("Withdraw");
                                    $('#address-Widthdraw').prop('disabled', false);
                                    $('#2fa-Widthdraw').prop('disabled', false);
                                    $('#amount-Widthdraw').prop('disabled', false);
                                    $("#fee").attr("disabled", false);
                                    modal.style.display = "none";
                                }
                            },
                            error: function (ex) {
                                alert("Validate 2FA error !!!");
                                //window.location.reload();
                                $('#button_WidthdrawETH').prop('disabled', false);
                                $("#button_WidthdrawETH").last().html("Withdraw");
                                $('#address-Widthdraw').prop('disabled', false);
                                $('#2fa-Widthdraw').prop('disabled', false);
                                $('#amount-Widthdraw').prop('disabled', false);
                                $("#fee").attr("disabled", false);
                                modal.style.display = "none";
                            }
                        });
                    }
                    else {
                        alert("Validate 2FA unsuccessful. Please input 2FA!!!");
                        $("#button_WidthdrawETH").last().html("Withdraw");
                        $('#button_WidthdrawETH').prop('disabled', false);
                        $('#2fa-Widthdraw').prop('disabled', false);
                        $('#amount-Widthdraw').prop('disabled', false);
                        $('#address-Widthdraw').prop('disabled', false);
                        modal.style.display = "none";
                    }
                }
                //en
                else {
                    submitWidthdrawETH(password, addressTo, amount);
                }
            }
        }, delayInMilliseconds)
    }
    else {
        alert("Please wait make again transaction after 60 sec !!");
        $("#button_WidthdrawETH").last().html("Withdraw");
        $('#button_WidthdrawETH').prop('disabled', false);
        $('#address-Widthdraw').prop('disabled', false);
        $('#2fa-Widthdraw').prop('disabled', false);
        $('#amount-Widthdraw').prop('disabled', false);
        modal.style.display = "none";
    }
};
function submitWidthdrawETH(password, addressTo, amount) {
    //var amountMax = parseFloat($('#amount-Widthdraw').attr('max'));
    var x = document.getElementById("BalanceView").textContent;
    var balance = parseFloat(x);
    var amountMin = parseFloat($('#amount-Widthdraw').attr('min'));
    var amountInput = parseFloat(amount);
    console.log("input amount: " + amountInput);
    //console.log("Value max: " + amountMax);
    //var speed = document.getElementById("fee").checked;
    var feeETH = $('#WidthdrawETHFee').val();
    var feeTransactionETH = feeETH * amount;//tính phí giao dịch
    var amountReal = amount - feeTransactionETH;// số tiền thực mà player nhận được = số tiền rút - phí rút
    console.log("Transaction fee ETH: " + feeTransactionETH);
    console.log("Amount real: " + amountReal);
    //Tính balnace có thể rút (số tiển rút *(1 + fee))
    var balanceWidthETH = parseFloat((amountInput * (1 + parseFloat(feeETH)))).toFixed(4);
    console.log("Balanc có thể rút" + balanceWidthETH);
    $('#label_widthdrawETH').val("Amount Withdraw: " + balanceWidthETH);

    if (amountInput >= amountMin && /*amountInput <= maxWidthdrawETH &&*/ amountInput <= balance) {
        $.ajax({
            type: "POST",
            url: "/callapiadmin/SendWithdrawETH",//gọi api callApiadmin để thực hiện giao dịch
            data: { type: 3, username: _username, password: password, toAddress: addressTo, amount: amount, speed: 0, fee: feeTransactionETH },
            async: true,
            success: function (data) {
                alert(data);//message admin
                setCookie("minuteLimited", getMinute(), 1);
                window.location.reload();/* on.reload();*/
            },
            //console.log("Time stamp mail: " +@ViewBag.timeStamp);
            //sendMailTransaction(_email, _timestamp, amountReal, addressTo, feeTransactionETH, _username);//gọi ajax gửi mail thông báo đã giao dịch thành công


            error: function (ex) {
                alert("Transaction Unsuccessful !!!");
                $("#button_WidthdrawETH").last().html("Withdraw");
                $('#button_WidthdrawETH').prop('disabled', false);
                $('#address-Widthdraw').prop('disabled', false);
                $('#2fa-Widthdraw').prop('disabled', false);
                $('#amount-Widthdraw').prop('disabled', false);
                //document.getElementById("button_WidthdrawETH").innerText = "Widthdraw";
                modal.style.display = "none";
            }
        });
    }
    else if (amountInput > balance) {
        alert("Available Balance not is enough,Your balance can  withdrawn today"+ balance);
        //window.location.reload();
        $('#button_exchangeVIP').prop('disabled', false);
        $("#button_widthdrawVIP").last().html("Withdraw");
        $('#button_widthdrawVIP').prop('disabled', false);
        $('#2fa-widthdraw-vip').prop('disabled', false);
        $('#amount-Widthdraw-vip').prop('disabled', false);
        $('#address-widthdraw-vip').prop('disabled', false);
        modal.style.display = "none";
    }
    else {
        alert("Minimum Withdraw is " + amountMin + "ETH");
        $("#button_WidthdrawETH").last().html("Withdraw");
        $('#button_WidthdrawETH').prop('disabled', false);
        $('#address-Widthdraw').prop('disabled', false);
        $('#2fa-Widthdraw').prop('disabled', false);
        $('#amount-Widthdraw').prop('disabled', false);
        modal.style.display = "none";
    }
    //kiểm tra balance đã cộng phí có nhỏ hơn balance hay không
    //if (balanceWidthETH >= amountMax) {
    //    alert("Ethereum not enough balance. Please check again, thank you !!!");
    //    $("#button_WidthdrawETH").last().html("Widthdraw");
    //    $('#button_WidthdrawETH').prop('disabled', false);
    //    $('#address-Widthdraw').prop('disabled', false);
    //    $('#2fa-Widthdraw').prop('disabled', false);
    //    $('#amount-Widthdraw').prop('disabled', false);
    //}
    //else {
    //    if (amountInput >= amountMin && amountInput <= maxWidthdrawETH && amountInput <= amountMax) {
    //        $.ajax({
    //            type: "POST",
    //            url: "/callapiadmin/SendWithdrawETH",//gọi api callApiadmin để thực hiện giao dịch
    //            data: { type: 3, username: _username, password: password, toAddress: addressTo, amount: amount, speed: 0, fee: feeTransactionETH },
    //            async: true,
    //            success: function (data) {
    //                    alert(data);//message admin
    //                    window.location.reload();on.reload();
    //                },
    //                //console.log("Time stamp mail: " +@ViewBag.timeStamp);
    //                //sendMailTransaction(_email, _timestamp, amountReal, addressTo, feeTransactionETH, _username);//gọi ajax gửi mail thông báo đã giao dịch thành công
                   
                
    //            error: function (ex) {
    //                alert("Transaction Unsuccessful !!!");
    //                $("#button_WidthdrawETH").last().html("Widthdraw");
    //                $('#button_WidthdrawETH').prop('disabled', false);
    //                $('#address-Widthdraw').prop('disabled', false);
    //                $('#2fa-Widthdraw').prop('disabled', false);
    //                $('#amount-Widthdraw').prop('disabled', false);
    //                //document.getElementById("button_WidthdrawETH").innerText = "Widthdraw";
    //                modal.style.display = "none";
    //            }
    //        });
    //    }
    //    else {
    //        alert("Invalid Amount. Please Performed Again!!!");
    //        $("#button_WidthdrawETH").last().html("Widthdraw");
    //        $('#button_WidthdrawETH').prop('disabled', false);
    //        $('#address-Widthdraw').prop('disabled', false);
    //        $('#2fa-Widthdraw').prop('disabled', false);
    //        $('#amount-Widthdraw').prop('disabled', false);
    //        modal.style.display = "none";
    //    }
    //}
    //kiểm tra amount check trường hợp amount vượt quá số tiền hiện có
  
};
//Function Popup exchangeETH
function submit2fa_ExchangeETH() {
    $("#button_exchangeETH").html("Exchange");
    $('#button_exchangeETH').prop('enable', false);
    $('#2fa-exchange').prop('enable', false);
    var amount = document.getElementById("amount-exchangeETH").value;
    modal.style.display = "block";
    var minutesLimited = getCookie("minuteLimited");
    var subMinute = 61;
    if (minutesLimited != "") {
        const diffInMilliseconds = Math.abs(new Date() - Date.parse(minutesLimited));
        subMinute = diffInMilliseconds / 1000;
        console.log("Minute can transaction: " + subMinute);
    }
    if (subMinute > 60) {
        if (amount == "") {
            alert("Syntax ERROR. Please Input Again!!!")
            //alert("Valid Amount isn't empty. Please Input Again!!!")
            $("#button_exchangeETH").last().html("Exchange");
            $('#button_exchangeETH').prop('disabled', false);
            $('#2fa-exchange').prop('disabled', false);
            $('#amount-exchangeETH').prop('enable', false);
            modal.style.display = "none";
        }
        else {
            var delayInMilliseconds = 300; //1 second
            setTimeout(function () {
                var code2fa = document.getElementById("2fa-exchange").value;
                var check2FA_Number = /([0-9]{6})/;
                var resCheck = check2FA_Number.test(code2fa);
                console.log(code2fa);
                if (_check2FA == 'True') {
                    if (code2fa != "" && resCheck == true) {
                        $.ajax({
                            type: "POST",
                            url: "/callapiadmin/Validate_2fa",
                            data: { userName: _username, pinCode: code2fa },
                            async: true,
                            success: function (data) {
                                if (data.status == "Success") {
                                    $("#button_exchangeETH").last().html("Please waiting...");
                                    $('#button_exchangeETH').prop('disabled', false);
                                    $('#2fa-exchange').prop('disabled', false);
                                    $('#amount-exchangeETH').prop('disabled', false);
                                    submitExchangeETH(amount);
                                }
                                else {
                                    alert("Validate unsuccessful !!!");
                                    $("#button_exchangeETH").last().html("Exchange");
                                    $('#button_exchangeETH').prop('disabled', false);
                                    $('#2fa-exchange').prop('disabled', false);
                                    $('#amount-exchangeETH').prop('disabled', false);
                                    modal.style.display = "none";
                                }
                            },
                            error: function (ex) {
                                alert("Validate unsuccessful !!!");
                                $("#button_exchangeETH").last().html("Exchange");
                                $('#button_exchangeETH').prop('disabled', false);
                                $('#2fa-exchange').prop('disabled', false);
                                $('#amount-exchangeETH').prop('disabled', false);
                                modal.style.display = "none";
                            }
                        });
                    }
                    else {
                        alert("Validate 2FA unsuccessful. Please input 2FA!!!");
                        $('#2fa-exchange').prop('disabled', false);
                        $("#button_exchangeETH").last().html("Exchange");
                        $('#button_exchangeETH').prop('disabled', false);
                        modal.style.display = "none";
                    }
                }
                else {
                    $("#button_exchangeETH").last().html("Exchange");
                    $('#button_exchangeETH').prop('disabled', false);
                    $('#2fa-exchange').prop('disabled', false);
                    $('#amount-exchangeETH').prop('disabled', false);
                    submitExchangeETH(amount);
                }
                //your code to be executed after 1 second
            }, delayInMilliseconds);
        }
    }
    else {
        alert("Please wait make again transaction after 60 sec !!");
        $("#button_exchangeETH").html("Exchange");
        $('#button_exchangeETH').prop('disabled', false);
        $('#2fa-exchange').prop('disabled', false);
        modal.style.display = "none";
    }
};

function submitExchangeETH(amount) {
    var amountInput = parseFloat(amount);
    var x = document.getElementById("Balance_exchangeEth").textContent;
    var balance = parseFloat(x);
    //var amountMax = parseFloat($('#amount-exchangeETH').attr('max'));Balance_exchangeEth
    var amountMin = parseFloat($('#amount-exchangeETH').attr('min'));
    //var balance = document.getElementById('balanceETH').value;
    

    //$.ajax({
    //    type: "POST",
    //    url: "/callapiadmin/SendExchange",
    //    data: { type: 1, username: _username, password: _addressPassword, amount: amount },
    //    async: true,
    //    success: function (data) {
    //        alert(data.Message);//message admin
    //        window.location.reload();
    //    },
    //    error: function (ex) {
    //        alert("Transaction Unsuccessful !");
    //        $("#button_exchangeETH").last().html("Exchange");
    //        $('#button_exchangeETH').prop('disabled', false);
    //        $('#2fa-exchange').prop('disabled', false);
    //        $('#amount-exchangeETH').prop('disabled', false);
    //        modal.style.display = "none";

    //    }
    //});
    if (amountInput >= amountMin && amountInput <= balance) {
        $.ajax({
            type: "POST",
            url: "/callapiadmin/SendExchange",
            data: { type: 1, username: _username, password: _addressPassword, amount: amount },
            async: true,
            success: function (data) {
                alert(data.Message);
                setCookie("minuteLimited", getMinute(), 1);
                window.location.reload();
            },
            error: function (ex) {
                alert("Transaction Unsuccessful !");
                $("#button_exchangeETH").last().html("Exchange");
                $('#button_exchangeETH').prop('disabled', false);
                $('#2fa-exchange').prop('disabled', false);
                $('#amount-exchangeETH').prop('disabled', false);
                modal.style.display = "none";

            }
        });
    }
    else if (amountInput > balance) {
        alert("Available Balance not is enough,Your balance can  withdrawn today " + balance + " ETH");
        //window.location.reload();
        $('#button_exchangeVIP').prop('disabled', false);
        $("#button_widthdrawVIP").last().html("Withdraw");
        $('#button_widthdrawVIP').prop('disabled', false);
        $('#2fa-widthdraw-vip').prop('disabled', false);
        $('#amount-Widthdraw-vip').prop('disabled', false);
        $('#address-widthdraw-vip').prop('disabled', false);
        modal.style.display = "none";
    }
    else {
        alert("Minimum exchange is " + amountMin +" ETH");
        $("#button_exchangeETH").last().html("Exchange");
        $('#button_exchangeETH').prop('disabled', false);
        $('#2fa-exchange').prop('disabled', false);
        $('#amount-exchangeETH').prop('disabled', false);
        modal.style.display = "none";
    }
};
//////////////////////////
//Function Popup exchange VIP
function submit2fa_ExchangeVIP() {
    
    $("#button_exchangeVIP").last().html("Please waiting...");
    $('#button_exchangeVIP').prop('enable', true);
    $('#2fa-exchange-vip').prop('enable', true);
    $('#amount-exchangeVip').prop('enable', true);
    modal.style.display = "block";
    var minutesLimited = getCookie("minuteLimited");
    var subMinute = 61;
    if (minutesLimited != "") {
        const diffInMilliseconds = Math.abs(new Date() - Date.parse(minutesLimited));
        subMinute = diffInMilliseconds / 1000;
    }
    if (subMinute > 60) {
        var amount = document.getElementById("amount-exchangeVip").value;
        if (amount === "") {
            alert("Syntax ERROR. Please Input Again!!!")
            $("#button_exchangeVIP").last().html("Exchange");
            $('#button_exchangeVIP').prop('disabled', false);
            $('#2fa-exchange-vip').prop('disabled', false);
            $('#amount-exchangeVip').prop('disabled', false);
            modal.style.display = "none";
        }
        else {
            var delayInMilliseconds = 300; //1 second
            setTimeout(function () {
                if (_check2FA == 'True') {
                    var code2fa = document.getElementById("2fa-exchange-vip").value;
                    var check2FA_Number = /([0-9]{6})/;
                    var resCheck = check2FA_Number.test(code2fa);
                    console.log(code2fa);
                    console.log(resCheck);
                    if (code2fa != "" && resCheck == true) {
                        console.log("valid code to check");
                        $.ajax({
                            type: "POST",
                            url: "/callapiadmin/Validate_2fa",
                            data: { userName: _username, pinCode: code2fa },
                            async: true,
                            success: function (data) {
                                if (data.status == "Success") {
                                    submitExchangeVIP(amount);
                                }
                                else {
                                    alert("Validate unsuccessful !!!");
                                    $("#button_exchangeVIP").last().html("Exchange");
                                    $('#button_exchangeVIP').prop('disabled', false);
                                    $('#2fa-exchange-vip').prop('disabled', false);
                                    $('#amount-exchangeVip').prop('disabled', false);
                                    modal.style.display = "none";
                                }
                            },
                            error: function (ex) {
                                alert("Validate unsuccessful !!!");
                                $("#button_exchangeVIP").last().html("Exchange");
                                $('#button_exchangeVIP').prop('disabled', false);
                                $('#2fa-exchange-vip').prop('disabled', false);
                                $('#amount-exchangeVip').prop('disabled', false);
                                modal.style.display = "none";
                            }
                        });
                    }
                    else {
                        alert("Validate 2FA unsuccessful. Please input 2FA!!!");
                        $("#button_exchangeVIP").last().html("Exchange");
                        $('#button_exchangeVIP').prop('disabled', false);
                        $('#2fa-exchange-vip').prop('disabled', false);
                        $('#amount-exchangeVip').prop('disabled', false);
                        modal.style.display = "none";
                    }
                }
                else {
                    $("#button_exchangeVIP").last().html("Please waiting...");
                    $('#button_exchangeVIP').prop('disabled', true);
                    $('#2fa-exchange-vip').prop('disabled', true);
                    $('#amount-exchangeVip').prop('disabled', true);
                    submitExchangeVIP(amount);
                }
            }, delayInMilliseconds);
        }
    }
    else {
        alert("Please wait make again transaction after 60 sec !!");
        $("#button_exchangeVIP").last().html("Exchange");
        $('#button_exchangeVIP').prop('disabled', false);
        $('#2fa-exchange-vip').prop('disabled', false);
        $('#amount-exchangeVip').prop('disabled', false);
        modal.style.display = "none";
    }

};
function submitExchangeVIP(amount) {
    var amountInput = parseFloat(amount);
    var amountMin = parseFloat($('#amount-exchangeVip').attr('min'));
    //var balanceusdx = (document.getElementById("Balance_exchangeVip").value).innerText;
    var x = document.getElementById("amount-exchangeVip").value;
    var balance = parseFloat(x);
    //document.getElementById("Balance_exchangeVip").innerHTML = balance;  

    //var balance = parseFloat($('#Balance_exchangeVip').va
    //var balance = $('#balanceUSD').value;
    //$.ajax({
    //    type: "POST",
    //    url: "/callapiadmin/SendExchange",
    //    data: { type: 2, username: _username, password: _addressPassword, amount: amount },
    //    async: true,
    //    success: function (data) {
    //        //console.log(data.listObj);
    //        //console.log(data.listBet);
    //        alert(data.Message);//message admin
    //        window.location.reload();
    //    },
    //    error: function (ex) {
    //        alert("Error Transaction. Please contact support !!!");
    //        $("#button_exchangeVIP").last().html("Exchange");
    //        $('#button_exchangeVIP').prop('disabled', false);
    //        $('#2fa-exchange-vip').prop('disabled', false);
    //        $('#amount-exchangeVip').prop('disabled', false);
    //        modal.style.display = "none";

    //    }
    //});
    if (amountInput >= amountMin && amountInput <= balance) {
        $.ajax({
            type: "POST",
            url: "/callapiadmin/SendExchange",
            data: { type: 2, username: _username, password: _addressPassword, amount: amount },
            async: true,
            success: function (data) {
                //console.log(data.listObj);
                //console.log(data.listBet);
                alert(data.Message);
                setCookie("minuteLimited", getMinute(), 1);
                window.location.reload();
            },
            error: function (ex) {
                alert("Error Transaction. Please contact support !!!");
                $("#button_exchangeVIP").last().html("Exchange");
                $('#button_exchangeVIP').prop('disabled', false);
                $('#2fa-exchange-vip').prop('disabled', false);
                $('#amount-exchangeVip').prop('disabled', false);
                modal.style.display = "none";

            }
        });
    }
    else if (amountInput > balance ) {
        alert("Available Balance not is enough,Your balance can  withdrawn today "+ balance +" BIT");
        $("#button_exchangeVIP").last().html("Exchange");
        $('#button_exchangeVIP').prop('disabled', false);
        $('#2fa-exchange-vip').prop('disabled', false);
        $('#amount-exchangeVip').prop('disabled', false);
        modal.style.display = "none";

    }
    else {
        alert("Minimum exchange is " + amountMin +" BIT");
        $("#button_exchangeVIP").last().html("Exchange");
        $('#button_exchangeVIP').prop('disabled', false);
        $('#2fa-exchange-vip').prop('disabled', false);
        $('#amount-exchangeVip').prop('disabled', false);
        modal.style.display = "none";
    }
};
//////////////////////////
//Function Popup widthdraw VIP
function submit2fa_widthdrawVIP() {
    //var code2fa = document.getElementById("2fa-widthdraw-vip").value;
    $("#button_widthdrawVIP").last().html("Please waiting...");
    $('#button_widthdrawVIP').prop('disabled', true);
    $('#2fa-widthdraw-vip').prop('disabled', true);
    $('#amount-Widthdraw-vip').prop('disabled', true);
    $('#address-widthdraw-vip').prop('disabled', true);
    modal.style.display = "block";
    var delayInMilliseconds = 300; //1 second
    var amount = document.getElementById("amount-Widthdraw-vip").value;
    //submitwidthdrawVIP(amount);
    var minutesLimited = getCookie("minuteLimited");
    var subMinute = 61;
    if (minutesLimited != "") {
        const diffInMilliseconds = Math.abs(new Date() - Date.parse(minutesLimited));
        subMinute = diffInMilliseconds / 1000;
    }
    if (subMinute > 60) {
        setTimeout(function () {
            //var amount = $("amount-Widthdraw").val();
            var addressTo = $('#address-widthdraw-vip').val().trim();
            var code2fa = $('#2fa-widthdraw-vip').val();
            var check2FA_Number = /([0-9]{6})/;
            var resCheck = check2FA_Number.test(code2fa);
            console.log("Check valied: " + resCheck);
            console.log("2FA: " + code2fa);
            // submitwidthdrawVIP(amount);
            if (amount == "" || addressTo == "") {
                alert("Some fields are empty or invalid. Please input again !");
                $("#button_widthdrawVIP").last().html("Withdraw");
                $('#button_widthdrawVIP').prop('disabled', false);
                $('#address-widthdraw-vip').prop('disabled', false);
                $('#2fa-widthdraw-vip').prop('disabled', false);
                $('#amount-Widthdraw-vip').prop('disabled', false);
                modal.style.display = "none";
            }
            else {
                if (_check2FA == 'True') {
                    if (code2fa != "" && resCheck == true) {
                        $.ajax({
                            type: "POST",
                            url: "/callapiadmin/Validate_2fa",
                            data: { userName: _username, pinCode: code2fa },
                            async: true,
                            success: function (data) {
                                if (data.status == "Success") {
                                    submitwidthdrawVIP(amount);
                                }
                                else {
                                    alert("Validate unsuccessful !!!");
                                    $('#button_exchangeVIP').prop('enable', false);
                                    $("#button_widthdrawVIP").last().html("Withdraw");
                                    $('#button_widthdrawVIP').prop('enable', false);
                                    $('#2fa-widthdraw-vip').prop('enable', false);
                                    $('#amount-Widthdraw-vip').prop('enable', false);
                                    $('#address-widthdraw-vip').prop('enable', false);
                                    modal.style.display = "none";
                                }
                            },
                            error: function (ex) {
                                alert("Validate unsuccessful !!!");
                                $('#button_exchangeVIP').prop('disabled', false);
                                $("#button_widthdrawVIP").last().html("Withdraw");
                                $('#button_widthdrawVIP').prop('disabled', false);
                                $('#2fa-widthdraw-vip').prop('disabled', false);
                                $('#amount-Widthdraw-vip').prop('disabled', false);
                                $('#address-widthdraw-vip').prop('disabled', false);
                                modal.style.display = "none";

                            }
                        });
                    }
                    else {
                        alert("Validate 2FA unsuccessful. Please input 2FA!!!");
                        $("#button_widthdrawVIP").last().html("Withdraw");
                        $('#button_widthdrawVIP').prop('disabled', false);
                        $('#2fa-widthdraw-vip').prop('disabled', false);
                        $('#amount-Widthdraw-vip').prop('disabled', false);
                        $('#address-widthdraw-vip').prop('disabled', false);
                        modal.style.display = "none";

                    }
                }
                else {
                    submitwidthdrawVIP(amount);
                }
            }

        }, delayInMilliseconds);
    }
    else {
        alert("Please wait make again transaction after 60 sec !!");
        $("#button_widthdrawVIP").last().html("Withdraw");
        $('#button_widthdrawVIP').prop('disabled', false);
        $('#address-widthdraw-vip').prop('disabled', false);
        $('#2fa-widthdraw-vip').prop('disabled', false);
        $('#amount-Widthdraw-vip').prop('disabled', false);
        modal.style.display = "none";
    }
}
function submitwidthdrawVIP(amount) {
    //var amountMax = document.getElementById('BalanceView_VIP').value;
    var x = document.getElementById("balanceUSDwithdraw").innerText;
    var balance = parseFloat(x);
    var amountMin = parseFloat($('#amount-Widthdraw-vip').attr('min'));
    var amountInput = parseFloat(amount);
    var _userNameTo = $('#address-widthdraw-vip').val();//tên người nhận
    var feeVIP = $('#WidthdrawVIPFee').val();//phí giao dịch vip
    var feeTransactionVIP = amount * feeVIP;//tiền phí giao dịch
    var amountRealVIP = amount - feeTransactionVIP;//số tiền sau khi giao dịch = số tiền rút - fee giao dịch
    console.log("Fee transaction VIP: " + feeTransactionVIP);
    console.log("Amount real VIP: " + amountRealVIP);
    var balanceWidthVIP = parseFloat((amountInput * (1 + parseFloat(feeVIP)))).toFixed(2);
    console.log("Balanc có thể rút" + balanceWidthVIP);
    //Gọi function check limited
    //var boolCheck = CheckLimit(_username);
    var bool = "";
    $.ajax({
        type: "POST",
        url: "/callapiadmin/CheckPlayerLimitedAsync",
        data: { username: _username },
        async: false,
        success: function (data) {
            bool = data;
            //alert(bool);
            console.log("Check nek: " + data);
        }
    });
    if (bool == "false") {
        //check amount VIP
        if (amountInput >= amountMin && amountInput <= balance /*&& amountInput <= maxWidthdrawVIP*/) {//amountMax là giới hạn last deposit
            $.ajax({
                type: "POST",
                url: "/callapiadmin/SendWidthdrawVIP",
                data: { type: 5, usernameFrom: _username, password: _addressPassword, UsernameToAddress: _userNameTo, amount: amount, fee: feeTransactionVIP, email: _email, id: _userId },
                async: true,
                success: function (data) {
                    alert(data);//message admin
                    setCookie("minuteLimited", getMinute(), 1);
                    window.location.reload();
                    //if (data.code == 1) {
                    //    alert(data);//message admin
                    //    $('#button_exchangeVIP').prop('disabled', false);
                    //    $("#button_widthdrawVIP").last().html("Widthdraw");
                    //    $('#button_widthdrawVIP').prop('disabled', false);
                    //    $('#2fa-widthdraw-vip').prop('disabled', false);
                    //    $('#amount-Widthdraw-vip').prop('disabled', false);
                    //    $('#address-widthdraw-vip').prop('disabled', false);
                    //    modal.style.display = "none";

                    //}
                    //else {
                    //    //console.log(data.listObj);
                    //    //console.log(data.listBet);
                    //    //console.log("Time stamp mail: " +@ViewBag.timeStamp)
                    //    //sendMailTransactionVIP(_email, _timestamp, amountRealVIP, _userNameTo, feeTransactionVIP);//gọi ajax gửi mail thông báo đã giao dịch thành công
                    //    alert(data.Message);//message admin
                    //    window.location.reload();
                    //}

                },
                error: function (ex) {
                    alert("Error transaction. Please contact support !");
                    $('#button_exchangeVIP').prop('disabled', false);
                    $("#button_widthdrawVIP").last().html("Withdraw");
                    $('#button_widthdrawVIP').prop('disabled', false);
                    $('#2fa-widthdraw-vip').prop('disabled', false);
                    $('#amount-Widthdraw-vip').prop('disabled', false);
                    $('#address-widthdraw-vip').prop('disabled', false);
                    modal.style.display = "none";

                }
            });
        }
        else if (amountInput > balance ){
            alert("Available Balance not is enough,Your balance can  withdrawn today"+ balance);
            //window.location.reload();
            $('#button_exchangeVIP').prop('disabled', false);
            $("#button_widthdrawVIP").last().html("Withdraw");
            $('#button_widthdrawVIP').prop('disabled', false);
            $('#2fa-widthdraw-vip').prop('disabled', false);
            $('#amount-Widthdraw-vip').prop('disabled', false);
            $('#address-widthdraw-vip').prop('disabled', false);
            modal.style.display = "none";
        }
        else {
            alert(" Minimum withdraw is " + amountMin +" BIT");
            //window.location.reload();
            $('#button_exchangeVIP').prop('disabled', false);
            $("#button_widthdrawVIP").last().html("Withdraw");
            $('#button_widthdrawVIP').prop('disabled', false);
            $('#2fa-widthdraw-vip').prop('disabled', false);
            $('#amount-Widthdraw-vip').prop('disabled', false);
            $('#address-widthdraw-vip').prop('disabled', false);
            modal.style.display = "none";
        }
    }
    else {
        $.ajax({
            type: "POST",
            url: "/callapiadmin/SendWidthdrawVIP",
            data: { type: 5, usernameFrom: _username, password: _addressPassword, UsernameToAddress: _userNameTo, amount: amount, fee: feeTransactionVIP, email: _email, id: _userId },
            async: true,
            success: function (data) {
                alert(data);//message admin
                setCookie("minuteLimited", getMinute(), 1);
                window.location.reload();
                //if (data.code == 1) {
                //    alert(data);//message admin
                //    $('#button_exchangeVIP').prop('disabled', false);
                //    $("#button_widthdrawVIP").last().html("Widthdraw");
                //    $('#button_widthdrawVIP').prop('disabled', false);
                //    $('#2fa-widthdraw-vip').prop('disabled', false);
                //    $('#amount-Widthdraw-vip').prop('disabled', false);
                //    $('#address-widthdraw-vip').prop('disabled', false);
                //    modal.style.display = "none";

                //}
                //else {
                //    //console.log(data.listObj);
                //    //console.log(data.listBet);
                //    //console.log("Time stamp mail: " +@ViewBag.timeStamp)
                //    //sendMailTransactionVIP(_email, _timestamp, amountRealVIP, _userNameTo, feeTransactionVIP);//gọi ajax gửi mail thông báo đã giao dịch thành công
                //    alert(data.Message);//message admin
                //    window.location.reload();
                //}

            },
            error: function (ex) {
                alert("Error transaction. Please contact support !");
                $('#button_exchangeVIP').prop('disabled', false);
                $("#button_widthdrawVIP").last().html("Withdraw");
                $('#button_widthdrawVIP').prop('disabled', false);
                $('#2fa-widthdraw-vip').prop('disabled', false);
                $('#amount-Widthdraw-vip').prop('disabled', false);
                $('#address-widthdraw-vip').prop('disabled', false);
                modal.style.display = "none";

            }
        });
    }
    //if (balanceWidthVIP > amountMax) {
    //    alert("VIP not enough balance. Please check again, thank you !!!");
    //    $('#button_exchangeVIP').prop('disabled', false);
    //    $("#button_widthdrawVIP").last().html("Widthdraw");
    //    $('#button_widthdrawVIP').prop('disabled', false);
    //    $('#2fa-widthdraw-vip').prop('disabled', false);
    //    $('#amount-Widthdraw-vip').prop('disabled', false);
    //    $('#address-widthdraw-vip').prop('disabled', false);
    //    modal.style.display = "none";

    //}
    //else {
    //    //check amount VIP
    //    if (amountInput >= amountMin && amountInput <= amountMax && amountInput <= maxWidthdrawVIP) {
    //        $.ajax({
    //            type: "POST",
    //            url: "/callapiadmin/SendWidthdrawVIP",
    //            data: { type: 5, usernameFrom: _username, password: _addressPassword, UsernameToAddress: _userNameTo, amount: amount, fee: feeTransactionVIP, email: _email, id: _userId },
    //            async: true,
    //            success: function (data) {
    //                alert(data);//message admin
    //                window.location.reload();
    //                //if (data.code == 1) {
    //                //    alert(data);//message admin
    //                //    $('#button_exchangeVIP').prop('disabled', false);
    //                //    $("#button_widthdrawVIP").last().html("Widthdraw");
    //                //    $('#button_widthdrawVIP').prop('disabled', false);
    //                //    $('#2fa-widthdraw-vip').prop('disabled', false);
    //                //    $('#amount-Widthdraw-vip').prop('disabled', false);
    //                //    $('#address-widthdraw-vip').prop('disabled', false);
    //                //    modal.style.display = "none";

    //                //}
    //                //else {
    //                //    //console.log(data.listObj);
    //                //    //console.log(data.listBet);
    //                //    //console.log("Time stamp mail: " +@ViewBag.timeStamp)
    //                //    //sendMailTransactionVIP(_email, _timestamp, amountRealVIP, _userNameTo, feeTransactionVIP);//gọi ajax gửi mail thông báo đã giao dịch thành công
    //                //    alert(data.Message);//message admin
    //                //    window.location.reload();
    //                //}

    //            },
    //            error: function (ex) {
    //                alert("Error transaction. Please contact support !");
    //                $('#button_exchangeVIP').prop('disabled', false);
    //                $("#button_widthdrawVIP").last().html("Exchange");
    //                $('#button_widthdrawVIP').prop('disabled', false);
    //                $('#2fa-widthdraw-vip').prop('disabled', false);
    //                $('#amount-Widthdraw-vip').prop('disabled', false);
    //                $('#address-widthdraw-vip').prop('disabled', false);
    //                modal.style.display = "none";

    //            }
    //        });
    //    }
    //    else {
    //        alert("Invalid Amount. Please Performed Again!!!");
    //        window.location.reload();
    //    }
    //}
 
};
//////////////////////////
//Conver Timestamp
function convert(timestamp) {

    var time = moment(timestamp).format("YYYY-MM-DD");
    return time;
    //document.getElementById('datetime').innerHTML = convdataTime;

};
//////////////////////////
////Send mail transaction
//function sendMailTransaction(email, timeStamp, amount, addressTo, feeTransaction, username) {
//    $.ajax({
//        type: "GET",
//        url: "http://mailapi.avataclub.com/WebCallMail/MailTransactionETH",
//        data: { id: _userId, email: email, timestamp: timeStamp, Amount: amount, AddressTo: addressTo, FeeTransaction: feeTransaction, username: username },//chưa check do thiếu trường
//        success: function (data) {
//            if (data.Status == 1) {
//                console.log(data.Message);
//            }
//            else {
//                console.log(data.Message);
//            }
//        },
//        error: function (ex) {
//            console.log("Gửi mail thất bại. Lỗi exception");
//        }
//    });
//}
//////////////////////////
////Send mail transaction VIP
//function sendMailTransactionVIP(email, timeStamp, amount, addressTo, feeTransaction) {
//    $.ajax({
//        type: "GET",
//        url: "http://mailapi.avataclub.com/WebCallMail/MailTransactionVIP",
//        data: { id: _userId, email: email, timestamp: timeStamp, Amount: amount, AddressTo: addressTo, FeeTransaction: feeTransaction, username: _username },//chưa check do thiếu trường
//        success: function (data) {
//            if (data.Status == 1) {
//                console.log(data.Message);
//            }
//            else {
//                console.log(data.Message);
//            }
//        },
//        error: function (ex) {
//            console.log("Gửi mail thất bại. Lỗi exception");
//        }
//    });
//}
    //////////////////////////
//Start max ETH
function maxWidthETH() {
    modal.style.display = "block";
    $.ajax({
        type: "POST",
        url: "/callapiadmin/WidthdrawMaxETH",
        data: { username: _username },
        async: true,
        success: function (data) {
            $('#amount-Widthdraw').val(data);
            modal.style.display = "none";
        }
    });
};
function maxExchangeETH() {
    $('#amount-exchangeETH').val(ETHBet);
};
//Start max VIP
function maxWidthVIP() {
    modal.style.display = "block";
    $.ajax({
        type: "POST",
        url: "/callapiadmin/WidthdrawMaxVIP",
        data: { username: _username },
        async: true,
        success: function (data) {
            $('#amount-Widthdraw-vip').val(data);
            modal.style.display = "none";
        }
    });
}; function maxExchangeVIP() {
    $('#amount-exchangeVip').val(VIPBet);
};
//SetCookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
};
//GetCookie
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
};
//Get Minute when transaction
function getMinute() {
    //var d = new Date();
    //var n = d.getMinutes();//get phút hiện tại sau khi thực hiện giao dịch thành công
    //return n;
    var n = new Date();
    return n;
};
//Check cho phép user được vượt giới hạn
function CheckLimit(userName) {
    var bool = "";
    $.ajax({
        type: "POST",
        url: "/callapiadmin/CheckPlayerLimitedAsync",
        data: { username: userName },
        async: true,
        success: function (data) {
            bool = data;
            console.log("Check nek: " + data);
        }
    });
    return bool;
};