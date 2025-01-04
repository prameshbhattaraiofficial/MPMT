//create connection

var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/usercount").build();
//connect to methods that hub invokes aka receiver notfications for hub
connectionUserCount.on("partnerupdateTotalCount", (value) => {
    console.log("atpartner", value);
    const decodeurl = decodeURIComponent('/Partner/Dashboard/GetPartnernotificationcount');
    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            var notificationcount = document.getElementById("partnerNotificationCount");
            notificationcount.innerText = res.toString();
        }
    });
    
});

//invoke hub methods aka send notification to hub

function PartnernotifyCount() {

    var value = connectionUserCount.invoke("PartnerCountChange");
    var notificationcount = document.getElementById("partnerNotificationCount");
    notificationcount.innerText = value.toString();

}
function logoutsignalR() {
    connectionUserCount.send("LeaveGroupAsync");
}


//start connection

function fulfilled() {
    //dosomething on start
    connectionUserCount.send("joinGroupAsync");
    //PartnernotifyCount();
}

function rejected() {

}

connectionUserCount.start().then(fulfilled, rejected)