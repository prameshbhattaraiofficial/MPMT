// Create connection
const connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/usercount", {
    accessTokenfactory: () => decodeURIComponent(document.cookie)
})
    .withAutomaticReconnect()
    .build();

// Function to update notification count
function updateNotificationCount(value, message) {
    debugger;
    const decodeurl = decodeURIComponent('/admin/Admindashboard/GetAdminnotificationcount');
    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            console.log(res);
            const notificationcount = document.getElementById("AdminNotificationCount");
            //if (res === value) {
            //    notificationcount.innerText = value.toString();
            //    if (message) {
            //        new PNotify({
            //            title: 'Notification',
            //            text: message,
            //            type: 'success',
            //            animation: 'slide',
            //            delay: 2000,
            //            animation: {
            //                effect_in: 'fade',
            //                effect_out: 'slide',
            //            }
            //        });
            //    }
            //} else {
            //    notificationcount.innerText = res.toString(); // Assuming `res` is the new count
            //}

            notificationcount.innerText = res.toString();
                if (message) {
                    new PNotify({
                        title: 'Notification',
                        text: message,
                        type: 'success',
                        animation: 'slide',
                        delay: 10000,
                        animation: {
                            effect_in: 'fade',
                            effect_out: 'slide',
                        }
                    });
                }
        }
    });
}

// Handle updateTotalCount event
connectionUserCount.on("updateTotalCount", (value, message) => {
    debugger;
    updateNotificationCount(value, message);
});

// Invoke hub methods
async function getAdminCount() {
    debugger;
    try {
        const value = await connectionUserCount.invoke("CountChange");
        const notificationcount = document.getElementById("AdminNotificationCount");
        notificationcount.innerText = value.toString();
    } catch (error) {
        // Handle error if invoke fails
        console.error(error);
    }
}

function logoutsignalR() {
    connectionUserCount.send("LeaveGroupAsync");
}

// Start connection
async function startConnection() {
    try {
        await connectionUserCount.start();
        // Do something on start
        connectionUserCount.send("joinGroupAsync");
        // getAdminCount(); // You may call this function if needed immediately on start
    } catch (error) {
        console.error("SignalR connection failed to start:", error);
    }
}

function onUserLogin() {
    // Perform login actions...
    // Then start the SignalR connection
    startConnection();
}
startConnection(); // Initiate the connection












////create connection

//var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/usercount").build();
////connect to methods that hub invokes aka receiver notfications for hub
//connectionUserCount.on("updateTotalCount", (value, Message) => {
//    debugger
//    var decodeurl = decodeURIComponent('/admin/Admindashboard/GetAdminnotificationcount');
//    var newcount;
//    $.ajax({
//        type: "GET",
//        url: decodeurl,
//        success: function (res) {
//            console.log(res);
//            if (res == value) {
//                var notificationcount = document.getElementById("AdminNotificationCount");
//                notificationcount.innerText = value.toString();
//                if (Message != null) {
//                    new PNotify({
//                        title: 'Notification',
//                        text: Message,
//                        type: 'success',
//                        animation: 'slide',
//                        delay: 2000,


//                        animation: {
//                            effect_in: 'fade',
//                            effect_out: 'slide',

//                        }
//                    });
//                }
//            } else {
//                var notificationcount = document.getElementById("AdminNotificationCount");
//                notificationcount.innerText = newcount.toString();
//            }
//        }

//    });



//});

////invoke hub methods aka send notification to hub

//function getAdminCount() {
//    debugger
//    var value = connectionUserCount.invoke("CountChange");
//    var notificationcount = document.getElementById("AdminNotificationCount");
//    notificationcount.innerText = value.toString();

//}
//function logoutsignalR() {
//    connectionUserCount.send("LeaveGroupAsync");
//}


////start connection

//function fulfilled() {
//    //dosomething on start
//    connectionUserCount.send("joinGroupAsync");
//    //getAdminCount();
//}

//function rejected() {

//}

//connectionUserCount.start().then(fulfilled, rejected)

