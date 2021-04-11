//deposit ETH
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "deposit" },
    async: true,
    success: function (data) {
        console.log("hel");
        console.log(JSON.parse(data));
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#deposit-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (key) {
                console.log("aaa" + key, data.historycurrency[key].Username);
                console.log("aaa");
                console.log(dat);

                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + data.historycurrency[key].Id + "</td>"
                    + "<td>" + data.historycurrency[key].Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + data.historycurrency[key].Amount + " ETH</td>"
                    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + dat.TxHash + "' style='color: #f64554;'>View</a></td>"
                    + "<td>" + data.historycurrency[key].Status + "</td>"
                    + "</tr>";
                $('#deposit-table').append(rows);
            });
        }
    },
    error: function (ex) {
    }
});
//withdraw ETH
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "withdraw" },
    async: false,
    success: function (data) {
        console.log("hel");
        console.log(JSON.parse(data));
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#withdraw-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (dat, index) {
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + dat.Id + "</td>"
                    + "<td>" + dat.Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + dat.Address + "</td>"
                    + "<td>" + dat.Amount + " ETH</td>"
                    + "<td>" + dat.Fee + "</td>"
                    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + dat.TxnHash + "' style='color: #f64554;'>View</a></td>"
                    + "<td>" + dat.Status + "</td>"
                    + "</tr>";
                $('#withdraw-table').append(rows);
            });
        }
    },
    error: function (ex) {
    }
});

//Exchange ETH
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "exchangeETH" },
    async: false,
    success: function (data) {
        console.log("hel");
        console.log(JSON.parse(data));
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#exchangeETH-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (dat, index) {
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + dat.Id + "</td>"
                    + "<td>" + dat.Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + dat.From + " ETH</td>"
                    + "<td>" + dat.To + " USD</td>"
                    + "<td>" + dat.Status + "</td>"
                    + "</tr>";
                $('#exchangeETH-table').append(rows);
            });
        }
        //console.log(parsedJSON.TotalBet);
        //$("#totalBets").html(formatter.format(parsedJSON.TotalBet));
        //$("#systemBets").html(formatter.format(parsedJSON.SystemBet));
        //var rows = "<tr>"
        //    + "<td>" + parsedJSON.Time + "</td>"
        //    + "<td>" + parsedJSON.Amount + "</td>"
        //    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + parsedJSON.TxHash + "' style='color: #f64554;'>View</a></td>"
        //    + "<td>" + parsedJSON.Status + "</td>"
        //    + "</tr>";
        //$('#body_example1').append(rows);
    },
    error: function (ex) {
    }
});
//Exchange VIP
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "exchangeVIP" },
    async: false,
    success: function (data) {
        console.log("exchangeVIP");
        console.log(JSON.parse(data));
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#ExchangeVIP-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (dat, index) {
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + dat.Id + "</td>"
                    + "<td>" + dat.Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + dat.From + " USD</td>"
                    + "<td>" + dat.To + " ETH</td>"
                    + "<td>" + dat.Status + "</td>"
                    + "</tr>";
                $('#ExchangeVIP-table').append(rows);
            });
        }
    },
    error: function (ex) {
    }
});
//deposit VIP
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "depositVIP" },
    async: false,
    success: function (data) {
        console.log(JSON.parse(data));
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#depositVIP-table').children().remove();
            Object.keys(data.).forEach(function (key) {
                console.log("aaa" + key, data.Username);
                console.log("depositVIP");
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + data.Id + "</td>"
                    + "<td>" + data.Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + data.Amount + " USD</td>"
                    + "<td>" + data.Status + "</td>"
                    + "</tr>";
                $('#depositVIP-table').append(rows);
            });
        }
        //console.log(parsedJSON.TotalBet);
        //$("#totalBets").html(formatter.format(parsedJSON.TotalBet));
        //$("#systemBets").html(formatter.format(parsedJSON.SystemBet));
        //var rows = "<tr>"
        //    + "<td>" + parsedJSON.Time + "</td>"
        //    + "<td>" + parsedJSON.Amount + "</td>"
        //    + "<td>" + "<a target='_blank' href='https://etherscan.io/tx/" + parsedJSON.TxHash + "' style='color: #f64554;'>View</a></td>"
        //    + "<td>" + parsedJSON.Status + "</td>"
        //    + "</tr>";
        //$('#body_example1').append(rows);
    },
    error: function (ex) {
    }
});

////Withdraw VIP
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "withdrawVIP" },
    async: false,
    success: function (data) {
        console.log("withdrawVIP");
        console.log(JSON.parse(data));
        console.log(data);
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#withdrawVIP-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (key) {
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + data.historycurrency[key].historycurrency[key].Id + "</td>"
                    + "<td>" + data.historycurrency[key].Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + data.historycurrency[key].Address + "</td>"
                    + "<td>" + data.historycurrency[key].Amount + " USD</td>"
                    + "<td>" + data.historycurrency[key].Fee + "</td>"
                    + "<td>" + data.historycurrency[key].Status + "</td>"
                    + "</tr>";
                $('#withdrawVIP-table').append(rows);
            });
        }
    },
    error: function (ex) {
    }
});
////Commission
$.ajax({
    type: "POST",
    url: "/affiliate/transactionHistory",
    data: { type: "commission" },
    async: false,
    success: function (data) {
        console.log("commission");
        console.log(JSON.parse(data));
        console.log(data);
        //var parsedJSON = JSON.parse(JSON.parse(data));
        if (data == null) {

        } else {
            $('#Commission-table').children().remove();
            Object.keys(data.historycurrency).forEach(function (key) {
                var rows = "<tr  role='row' class='odd'>"
                    + "<td>" + data.historycurrency[key].Id + "</td>"
                    + "<td>" + data.historycurrency[key].Time.replace("T", " ").split(".")[0] + "</td>"
                    + "<td>" + data.historycurrency[key].Amount + " USD</td>"
                    + "<td>" + data.historycurrency[key].Status + "</td>"
                    + "</tr>";
                $('#Commission-table').append(rows);
            });
        }
    },
    error: function (ex) {
    }
});