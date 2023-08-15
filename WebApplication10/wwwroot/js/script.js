new DataTable('#tablo1');
new DataTable('#tablo3');
new DataTable('#tablox');





    function combovalue(){
    var comboBox = document.getElementById("ComboBox");
    var selectedOptionValue = comboBox.value;
    var inp1 = document.getElementById("comboinput");
    inp1.value=selectedOptionValue;
}

function randevalue() {
    var comboBox = document.getElementById("myDropdown");
    var selectedOptionValue = comboBox.value;
    var inp1 = document.getElementById("hastaid");
    inp1.value = selectedOptionValue;
}
$(document).ready(function () {
    $('#tablo3').dataTable( "dom", "lfrtp" );
});


function kbl() {
    var lbl2 = document.getElementById("label3");
    var str = "Kabul Edildi";
    lbl2.value = str;
    str = "null";
}
function gnd() {

    var lbl2 = document.getElementById("label3");
    var str = "Gönderildi";
    lbl2.value = str;
    str = "null";
    
}
function iptal() {
    var lbl2 = document.getElementById("label3");
    var str = "İptal Edildi";
    lbl2.value = str;
    str = "null";
}

function success() {
    Swal.fire({
        icon: 'success',
        title: 'Başarılı!',
        text: 'İşlem başarıyla tamamlandı.',
    });
}
function failed() {
    Swal.fire({
        icon: 'error',
        title: 'Başarısız!',
        text: 'İşlem başarısız',
    });
}