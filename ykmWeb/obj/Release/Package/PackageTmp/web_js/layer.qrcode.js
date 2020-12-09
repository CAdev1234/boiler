function getQRcode() {
    var qrstr = window.location.href;
    $("#QRcode").qrcode({ width: 100, height: 100, text: qrstr });
}

function getQRcode(href) {
    $("#QRcode").qrcode({ width: 100, height: 100, text: href });
}
