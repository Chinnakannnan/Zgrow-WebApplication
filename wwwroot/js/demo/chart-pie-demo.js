// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example
var ctx = document.getElementById("myPieChart");
var myPieChart = new Chart(ctx, {
  type: 'doughnut',
  data: {
    labels: ["Issued", "Active", "Inactive"],
    datasets: [{
      data: [55, 30, 15],
      backgroundColor: ['rgb(18 107 114)', 'rgba(231, 74, 59, 1)', 'rgba(133, 135, 150,1)'],
      hoverBackgroundColor: ['rgba(18 107 114,.9)', 'rgba(231, 74, 59, .9)', 'rgba(133, 135, 150,.9)'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: false,
      caretPadding: 10,
    },
    legend: {
      display: true
    },title: {
      display: true,
      text: 'Card Counts'
    },
    cutoutPercentage: 80,
  },
});
