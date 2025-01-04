var options = {
  chart: {
    height: 320,
    type: "area",
    stacked: !0,
    toolbar: { show: !1, autoSelected: "zoom" },
  },
  colors: ["rgba(42, 118, 244, 0.5)", "rgba(42, 118, 244, 0.5)"],
  dataLabels: { enabled: !1 },
  stroke: {
    curve: "smooth",
    width: [1.5, 1.5],
    dashArray: [0, 4],
    lineCap: "round",
  },
  grid: { padding: { left: 0, right: 0 }, strokeDashArray: 3 },
  markers: { size: 0, hover: { size: 0 } },
  series: [
    {
      name: "New Users",
      data: [0, 60, 20, 90, 45, 110, 55, 130, 44, 110, 75, 120],
    },
    {
      name: "Refer Users",
      data: [0, 45, 10, 75, 35, 94, 40, 115, 30, 105, 65, 110],
    },
  ],
  xaxis: {
    type: "month",
    categories: [
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec",
    ],
    axisBorder: { show: !0 },
    axisTicks: { show: !0 },
  },
  fill: {
    type: "gradient",
    gradient: {
      shadeIntensity: 1,
      opacityFrom: 0.4,
      opacityTo: 0.3,
      stops: [0, 90, 100],
    },
  },
  tooltip: { x: { format: "dd/MM/yy HH:mm" } },
  legend: { position: "top", horizontalAlign: "right" },
};
(chart = new ApexCharts(
  document.querySelector("#ana_dash_1"),
  options
)).render();
options = {
  chart: { height: 270, type: "donut" },
  plotOptions: { pie: { donut: { size: "85%" } } },
  dataLabels: { enabled: !1 },
  stroke: { show: !0, width: 2, colors: ["transparent"] },
  series: [50, 25],
  legend: {
    show: !0,
    position: "bottom",
    horizontalAlign: "center",
    verticalAlign: "middle",
    floating: !1,
    fontSize: "13px",
    offsetX: 0,
    offsetY: 0,
  },
  labels: ["Active", "Inactive"],
  colors: ["#039f5d", "#dddddd"],
  responsive: [
    {
      breakpoint: 600,
      options: {
        plotOptions: { donut: { customScale: 0.2 } },
        chart: { height: 240 },
        legend: { show: !1 },
      },
    },
  ],
  tooltip: {
    y: {
      formatter: function (o) {
        return o + " %";
      },
    },
  },
};
(chart = new ApexCharts(
  document.querySelector("#ana_device"),
  options
)).render();
var chart,
  colors = ["#98e7df", "#b8c4d0", "#bec7fa", "#ffe2a3", "#92e6f0"];
options = {
  series: [{ name: "Inflation", data: [4, 10.1, 6, 8, 9.1] }],
  chart: { height: 355, type: "bar", toolbar: { show: !1 } },
  plotOptions: {
    bar: {
      dataLabels: { position: "top" },
      columnWidth: "20",
      distributed: !0,
    },
  },
  dataLabels: {
    enabled: !0,
    formatter: function (o) {
      return o + "%";
    },
    offsetY: -20,
    style: { fontSize: "12px", colors: ["#000"] },
  },
  colors: colors,
  xaxis: {
    categories: ["Email", "Referral", "Organic", "Direct", "Campaign"],
    position: "top",
    axisBorder: { show: !1 },
    axisTicks: { show: !1 },
    crosshairs: {
      fill: {
        type: "gradient",
        gradient: {
          colorFrom: "#D8E3F0",
          colorTo: "#BED1E6",
          stops: [0, 100],
          opacityFrom: 0.4,
          opacityTo: 0.5,
        },
      },
    },
    tooltip: { enabled: !0 },
  },
  grid: { padding: { left: 0, right: 0 }, strokeDashArray: 3 },
  yaxis: {
    axisBorder: { show: !1 },
    axisTicks: { show: !1 },
    labels: {
      show: !1,
      formatter: function (o) {
        return o + "%";
      },
    },
  },
};
(chart = new ApexCharts(document.querySelector("#barchart"), options)).render();
var dash_spark_1 = {
    chart: { type: "area", height: 60, sparkline: { enabled: !0 } },
    stroke: { curve: "smooth", width: 3 },
    fill: {
      opacity: 1,
      gradient: {
        shade: "#2c77f4",
        type: "horizontal",
        shadeIntensity: 0.5,
        inverseColors: !0,
        opacityFrom: 0.1,
        opacityTo: 0.1,
        stops: [0, 80, 100],
        colorStops: [],
      },
    },
    series: [
      { data: [4, 8, 5, 10, 4, 16, 5, 11, 6, 11, 30, 10, 13, 4, 6, 3, 6] },
    ],
    yaxis: { min: 0 },
    colors: ["#2c77f4"],
    tooltip: { show: !1 },
  };
  new ApexCharts(document.querySelector("#dash_spark_1"), dash_spark_1).render();
  var dash_spark_2 = {
    chart: { type: "area", height: 60, sparkline: { enabled: !0 } },
    stroke: { curve: "smooth", width: 3 },
    fill: {
      opacity: 1,
      gradient: {
        shade: "#fdb5c8",
        type: "horizontal",
        shadeIntensity: 0.5,
        inverseColors: !0,
        opacityFrom: 0.1,
        opacityTo: 0.1,
        stops: [0, 80, 100],
        colorStops: [],
      },
    },
    series: [{ data: [4, 8, 5, 10, 4, 25, 5, 11, 6, 11, 5, 10, 3, 14, 6, 8, 6] }],
    yaxis: { min: 0 },
    colors: ["#fdb5c8"],
  };
  new ApexCharts(document.querySelector("#dash_spark_2"), dash_spark_2).render();
  var options = {
    series: [76],
    chart: { type: "radialBar", offsetY: -20, sparkline: { enabled: !0 } },
    plotOptions: {
      radialBar: {
        startAngle: -90,
        endAngle: 90,
        hollow: { size: "75%", position: "front" },
        track: {
          background: ["rgba(42, 118, 244, .18)"],
          strokeWidth: "80%",
          opacity: 0.5,
          margin: 5,
        },
        dataLabels: {
          name: { show: !1 },
          value: { offsetY: -2, fontSize: "20px" },
        },
      },
    },
    stroke: { lineCap: "butt" },
    colors: ["#2a76f4"],
    grid: { padding: { top: -10 } },
    labels: ["Average Results"],
  };
  (chart = new ApexCharts(document.querySelector("#ana_1"), options)).render();
  options = {
    chart: { height: 240, type: "donut" },
    plotOptions: { pie: { donut: { size: "85%" } } },
    dataLabels: { enabled: !1 },
    stroke: { show: !0, width: 2, colors: ["transparent"] },
    series: [65, 20, 10, 5],
    legend: {
      show: !1,
      position: "bottom",
      horizontalAlign: "center",
      verticalAlign: "middle",
      floating: !1,
      fontSize: "14px",
      offsetX: 0,
      offsetY: -13,
    },
    labels: ["Excellent", "Very Good", "Good", "Fair"],
    colors: ["#2a76f4", "#fdb5c8", "#67c8ff", "#c693ff"],
    responsive: [
      {
        breakpoint: 600,
        options: {
          plotOptions: { donut: { customScale: 0.2 } },
          chart: { height: 240 },
          legend: { show: !1 },
        },
      },
    ],
    tooltip: {
      y: {
        formatter: function (e) {
          return e + " %";
        },
      },
    },
  };
  (chart = new ApexCharts(
    document.querySelector("#ana_device"),
    options
  )).render();
  options = {
    chart: { height: 310, type: "donut" },
    plotOptions: { pie: { donut: { size: "85%" } } },
    dataLabels: { enabled: !1 },
    stroke: { show: !0, width: 2, colors: ["transparent"] },
    series: [70, 10, 20],
    legend: {
      show: !0,
      position: "bottom",
      horizontalAlign: "center",
      verticalAlign: "middle",
      floating: !1,
      fontSize: "13px",
      offsetX: 0,
      offsetY: 0,
    },
    labels: ["Verified", "Pending", "Rejected"],
    colors: ["rgba(3, 216, 127, .8)", "rgba(255, 184, 34, .8)", "rgba(245, 50, 92, .8)"],
    responsive: [
      {
        breakpoint: 600,
        options: {
          plotOptions: { donut: { customScale: 0.2 } },
          chart: { height: 240 },
          legend: { show: !1 },
        },
      },
    ],
    tooltip: {
      y: {
        formatter: function (e) {
          return e + " %";
        },
      },
    },
  };
  (chart = new ApexCharts(
    document.querySelector("#ana_device2"),
    options
  )).render();
  var chart;
  options = {
    chart: {
      type: "radialBar",
      height: 300,
      dropShadow: {
        enabled: !0,
        top: 5,
        left: 0,
        bottom: 0,
        right: 0,
        blur: 5,
        color: "#45404a2e",
        opacity: 0.35,
      },
    },
    plotOptions: {
      radialBar: {
        offsetY: -10,
        startAngle: 0,
        endAngle: 270,
        hollow: { margin: 5, size: "50%", background: "transparent" },
        track: { show: !1 },
        dataLabels: {
          name: { fontSize: "18px" },
          value: { fontSize: "16px", color: "#50649c" },
        },
      },
    },
    colors: ["#07b16a", "rgba(42, 118, 244, .5)"],
    stroke: { lineCap: "round" },
    series: [71, 100],
    labels: [ "Active", "Inactive"],
    legend: {
      show: !0,
      floating: !0,
      position: "left",
      offsetX: -10,
      offsetY: 0,
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          legend: {
            show: !0,
            floating: !0,
            position: "left",
            offsetX: 10,
            offsetY: 0,
          },
        },
      },
    ],
  };
  (chart = new ApexCharts(
    document.querySelector("#task_status"),
    options
  )).render();
  